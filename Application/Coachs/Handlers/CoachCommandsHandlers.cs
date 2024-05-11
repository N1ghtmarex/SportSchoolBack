﻿using Abstractions.CommonModels;
using Application.Coachs.Commands;
using Core.Exceptions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Coachs.Handlers
{
    internal class CoachCommandsHandlers(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, ICoachMapper coachMapper) :
        IRequestHandler<CreateCoachCommand, CreatedOrUpdatedEntityViewModel<Guid>>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
        {
            var coachWithSameId = await dbContext.Coachs
                .Where(x => x.ExternalId == Guid.Parse(contextAccessor.IdentityUserId))
                .SingleOrDefaultAsync(cancellationToken);

            if (coachWithSameId != null)
            {
                throw new BusinessLogicException(
                    $"Тренер с идентификатором \"{coachWithSameId.ExternalId}\" уже существует!");
            }

            var clientWithSameId = await dbContext.Clients
                .Where(x => x.ExternalId == Guid.Parse(contextAccessor.IdentityUserId))
                .SingleOrDefaultAsync(cancellationToken);

            var coachToCreate = coachMapper.MapToEntity((contextAccessor.UserName, contextAccessor.UserSurname, Guid.Parse(contextAccessor.IdentityUserId)));
            var createdCoach = await dbContext.AddAsync(coachToCreate, cancellationToken);
            await dbContext.SaveChangesAsync();

            if (clientWithSameId != null)
            {
                dbContext.Remove(clientWithSameId);
                await dbContext.SaveChangesAsync();
            }

            return new CreatedOrUpdatedEntityViewModel(createdCoach.Entity.Id);
        }
    }
}