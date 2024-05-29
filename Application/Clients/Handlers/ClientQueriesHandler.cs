using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Clients.Dtos;
using Application.Clients.Queries;
using Application.Coachs;
using Core.EntityFramework.Features.SearchPagination;
using Core.EntityFramework.Features.SearchPagination.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Clients.Handlers
{
    internal class ClientQueriesHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, IClientMapper clientMapper, IClientService clientService) :
        IRequestHandler<GetClientsListQuery, PagedResult<ClientListViewModel>>, IRequestHandler<GetClientQuery, ClientViewModel>
    {
        public async Task<PagedResult<ClientListViewModel>> Handle(GetClientsListQuery request, CancellationToken cancellationToken)
        {
            var clientQuery = dbContext.Clients
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Surname)
                .ApplySearch(request, x => x.Name, x => x.Surname);

            var clientList = await clientQuery
                .ApplyPagination(request)
                .ToListAsync(cancellationToken);

            var result = clientList.Select(clientMapper.MapToListViewModel);
            return result.AsPagedResult(request, await clientQuery.CountAsync(cancellationToken));
        }

        public async Task<ClientViewModel> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientAsync(request.ClientId.ToString(), true, cancellationToken);

            return clientMapper.MapToViewModel(client);
        }
    }
}
