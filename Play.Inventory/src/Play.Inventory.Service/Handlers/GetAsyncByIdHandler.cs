using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Play.Common;
using Play.Inventory.Entities;
using Play.Inventory.Exception;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Request;
using Play.Inventory.Service.Services;

namespace Play.Inventory.Service.Handlers
{
    public class GetAsyncByIdHandler : IRequestHandler<GetAsyncRequest, IEnumerable<InventoryItemDto>>
    {
        private readonly IRepository<InventoryItem> inventoryItemsRepository;
        private readonly IRepository<CatalogItem> catalogItemsRepository;
        private readonly IValidationService _validationService;

        public GetAsyncByIdHandler(IRepository<InventoryItem> inventoryItemsRepository, IRepository<CatalogItem> catalogItemsRepository, IValidationService validationService)
        {
            this.inventoryItemsRepository = inventoryItemsRepository;
            this.catalogItemsRepository = catalogItemsRepository;
            _validationService = validationService;
        }

        public async Task<IEnumerable<InventoryItemDto>> Handle(GetAsyncRequest request, CancellationToken cancellationToken)
        {
            _validationService.Validate(request);
            var inventoryItemEntities = await inventoryItemsRepository.GetAllAsync(item => item.UserId == request.id);
            if (inventoryItemEntities.Count() == 0)
                throw new DefaultException("UserId not exists");

            var itemIds = inventoryItemEntities.Select(item => item.CatalogItemId);
            var catalogItemEntities = await catalogItemsRepository.GetAllAsync(item => itemIds.Contains(item.Id));
            if (catalogItemEntities.Count() == 0)
                throw new DefaultException("CatalogItemId not exists");

            return catalogItemEntities.Select(catalogItem =>
            {
                var inventoryItem = inventoryItemEntities.SingleOrDefault(i => i.CatalogItemId == catalogItem.Id);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });
        }
    }
}
