﻿using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Coachs.Commands;
using Core.Exceptions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Coachs.Handlers
{
    internal class CoachCommandsHandlers(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, ICoachMapper coachMapper, 
        ICoachService coachService, IClientService clientService) :
        IRequestHandler<CreateCoachCommand, CreatedOrUpdatedEntityViewModel<Guid>>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
        {
            var coachWithSameId = await coachService.GetCoachAync(contextAccessor.IdentityUserId, cancellationToken);

            if (coachWithSameId != null)
            {
                throw new BusinessLogicException(
                    $"Тренер с идентификатором \"{coachWithSameId.ExternalId}\" уже существует!");
            }

            var coachToCreate = coachMapper.MapToEntity((contextAccessor.UserName, contextAccessor.UserSurname, Guid.Parse(contextAccessor.IdentityUserId)));
            var createdCoach = await dbContext.AddAsync(coachToCreate, cancellationToken);
            await dbContext.SaveChangesAsync();

            var clientWithSameId = await clientService.GetClientAsync(contextAccessor.IdentityUserId, false, cancellationToken);

            if (clientWithSameId != null)
            {
                dbContext.Remove(clientWithSameId);
                await dbContext.SaveChangesAsync();
            }

            return new CreatedOrUpdatedEntityViewModel(createdCoach.Entity.Id);
        }
    }
}
