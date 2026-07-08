using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Play.Catalog.Service;
using Play.Catalog.Services.Cache;
using Play.Catalog.Services.Dtos;
using Play.Catalog.Services.Entities;
using Play.Catalog.Services.Request;
using Play.Catalog.Services.Services;
using Play.Common;

namespace Play.Catalog.Services.Handlers
{
    public class GetAsyncHandler : IRequestHandler<GetAsyncRequest, IEnumerable<ItemDto>>
    {
        private readonly IRepository<Item> _itemsRepository;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IItemCache itemcache;

        public GetAsyncHandler(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint,
        IItemCache itemcache)
        {
            _itemsRepository = itemsRepository;
            this.publishEndpoint = publishEndpoint;
            this.itemcache = itemcache;
        }

        public async Task<IEnumerable<ItemDto>> Handle(GetAsyncRequest request, CancellationToken cancellationToken)
        {
            return await itemcache.GetCachedItems();
        }
    }
}
