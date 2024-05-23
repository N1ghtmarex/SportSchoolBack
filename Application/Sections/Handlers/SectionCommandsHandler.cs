using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Sections.Commands;
using Core.Exceptions;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Sections.Handlers
{
    internal class SectionCommandsHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, ISectionMapper sectionMapper, ICoachService coachService,
        ISportService sportService, IRoomService roomService, IClientService clientService, ISectionService sectionService) :
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

            var coach = await coachService.GetCoachAync(contextAccessor.IdentityUserId, cancellationToken);

            var sectionWithSameName = await dbContext.Sections
                .Where(x => x.Name == request.Body.Name)
                .SingleOrDefaultAsync(cancellationToken);

            if (sectionWithSameName != null) 
            {
                throw new BusinessLogicException(
                    $"Секция с названием \"{sectionWithSameName.Name}\" уже существует!");
            }

            var sport = await sportService.GetSportAsync(request.Body.SportId, cancellationToken);

            var room = await roomService.GetRoomAsync(request.Body.RoomId, cancellationToken);

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
            var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, true, cancellationToken);

            var section = await sectionService.GetSectionAsync(request.Body.SectionId, true, true, true, true, cancellationToken);

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
            var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, true, cancellationToken);

            if (client == null)
            {
                throw new ObjectNotFoundException(
                    $"Пользователь с внешним идентификатором {contextAccessor.IdentityUserId} не найден!");
            }

            var section = await sectionService.GetSectionAsync(request.Body.SectionId, true, true, true, true, cancellationToken);
            
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

            var coach = await coachService.GetCoachAync(contextAccessor.IdentityUserId, cancellationToken);

            var section = await sectionService.GetSectionAsync(request.SectionId, true, true, true, true, cancellationToken);

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
