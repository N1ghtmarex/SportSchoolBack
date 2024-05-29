using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Clients;
using Domain;

namespace Application.Users.Handlers
{
    internal class ClientCommandsHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, IClientMapper clientMapper, IClientService clientService)
    {
    }
}
