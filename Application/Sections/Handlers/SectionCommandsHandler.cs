using Abstractions.CommonModels;
using Application.Sections.Commands;
using Core.Exceptions;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Sections.Handlers
{
    internal class SectionCommandsHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, ISectionMapper sectionMapper) :
        IRequestHandler<CreateSectionCommand, CreatedOrUpdatedEntityViewModel<Guid>>, IRequestHandler<AddClientToSectionCommand>, IRequestHandler<RemoveClientFromSectionCommand>,
        IRequestHandler<DeleteSectionCommand>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
        {
            var directory = @"C:\Users\andre\source\repos\SportSchool\images\sections";

            if (!contextAccessor.UserRoles.Contains("Coach"))
            {
                throw new ForbiddenException(
                    "Только тренер может добавлять секции!");
            }

            var coach = await dbContext.Coachs
                .Where(x => x.ExternalId == Guid.Parse(contextAccessor.IdentityUserId))
                .SingleOrDefaultAsync(cancellationToken);

            if (coach == null)
            {
                throw new ObjectNotFoundException(
                    $"Тренер с внешним идентификатором \"{contextAccessor.IdentityUserId}\" не найден!");
            }

            var sectionWithSameName = await dbContext.Sections
                .Where(x => x.Name == request.Body.Name)
                .SingleOrDefaultAsync(cancellationToken);

            if (sectionWithSameName != null) 
            {
                throw new BusinessLogicException(
                    $"Секция с названием \"{sectionWithSameName.Name}\" уже существует!");
            }

            var sport = await dbContext.Sports
                .Where(x => x.Id == request.Body.SportId)
                .SingleOrDefaultAsync(cancellationToken);

            if (sport == null)
            {
                throw new ObjectNotFoundException(
                    $"Вид спорта с идентификатором \"{request.Body.SportId}\" не найден!");
            }

            var room = await dbContext.Rooms
                .Where(x => x.Id == request.Body.RoomId)
                .SingleOrDefaultAsync(cancellationToken);

            if (room == null)
            {
                throw new ObjectNotFoundException(
                    $"Зал с идентификатором \"{request.Body.RoomId}\" не найден!");
            }

            var sectionToCreate = sectionMapper.MapToEntity((request.Body, coach.Id));
            var createdSection = await dbContext.AddAsync(sectionToCreate, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var imagesDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.ToString() ?? string.Empty, "SportSchool", "wwwroot", "sections");
            var filePath = Path.Combine(imagesDirectory, $"{createdSection.Entity.Id.ToString()}.jpeg");
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                request.Body.Image.CopyTo(fileStream);
            }

            return new CreatedOrUpdatedEntityViewModel(createdSection.Entity.Id);
        }

        public async Task Handle(AddClientToSectionCommand request, CancellationToken cancellationToken)
        {
            var client = await dbContext.Clients
                .Where(x => x.ExternalId == Guid.Parse(contextAccessor.IdentityUserId))
                .Include(x => x.Section)
                .SingleOrDefaultAsync(cancellationToken);

            if (client == null)
            {
                throw new ObjectNotFoundException(
                    $"Пользователь с внешним идентификатором {contextAccessor.IdentityUserId} не найден!");
            }

            var section = await dbContext.Sections
                .Where(x => x.Id == request.Body.SectionId)
                .Include(x => x.Sport)
                .Include(x => x.Room)
                .Include(x => x.Coach)
                .SingleOrDefaultAsync(cancellationToken);

            if (section == null) 
            {
                throw new ObjectNotFoundException(
                    $"Секция с идентификатором {request.Body.SectionId} не найдена!");
            }

            if (client.Section == null)
            {
                client.Section = new List<Section> { section };
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            else if (client.Section.Contains(section))
            {
                throw new BusinessLogicException(
                    $"Пользователь с идентификатором {client.Id} уже состоит в секции с идентификатором {section.Id}!");
            }

            client.Section.Add(section);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Handle(RemoveClientFromSectionCommand request, CancellationToken cancellationToken)
        {
            var client = await dbContext.Clients
                .Where(x => x.ExternalId == Guid.Parse(contextAccessor.IdentityUserId))
                .Include(x => x.Section)
                .SingleOrDefaultAsync(cancellationToken);

            if (client == null)
            {
                throw new ObjectNotFoundException(
                    $"Пользователь с внешним идентификатором {contextAccessor.IdentityUserId} не найден!");
            }

            var section = await dbContext.Sections
                .Where(x => x.Id == request.Body.SectionId)
                .Include(x => x.Sport)
                .Include(x => x.Room)
                .Include(x => x.Coach)
                .SingleOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                throw new ObjectNotFoundException(
                    $"Секция с идентификатором {request.Body.SectionId} не найдена!");
            }
            
            if (client.Section == null) 
            {
                throw new BusinessLogicException(
                    $"Пользователь с идентификатором {client.Id} еще не состоит ни в одной секции!");
            }
            if (!client.Section.Contains(section))
            {
                throw new BusinessLogicException(
                    $"Пользователь с идентификатором {client.Id} не состоит в секции с идентификатором {section.Id}!");
            }

            client.Section.Remove(section);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
        {
            if (!contextAccessor.UserRoles.Contains("Coach"))
            {
                throw new ForbiddenException(
                    "Только тренер может добавлять секции!");
            }

            var coach = await dbContext.Coachs
                .Where(x => x.ExternalId == Guid.Parse(contextAccessor.IdentityUserId))
                .SingleOrDefaultAsync(cancellationToken);

            if (coach == null)
            {
                throw new ObjectNotFoundException(
                    $"Тренер с внешним идентификатором \"{contextAccessor.IdentityUserId}\" не найден!");
            }

            var section = await dbContext.Sections
                .Where(x => x.Id == request.SectionId)
                .Include(x => x.Sport)
                .Include(x => x.Room)
                .Include(x => x.Coach)
                .SingleOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                throw new ObjectNotFoundException(
                    $"Секция с идентификатором {request.SectionId} не найдена!");
            }

            if (coach.Id != section.CoachId)
            {
                throw new ForbiddenException(
                    "Вы не являетесь тренером данной секции!");
            }

            dbContext.Remove(section);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
