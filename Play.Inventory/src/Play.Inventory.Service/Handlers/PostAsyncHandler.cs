using System;
using System.Threading;
using MediatR;
using Play.Common;
using Play.Inventory.Entities;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Request;
using Play.Inventory.Service.Services;

namespace Play.Inventory.Service.Handlers
{
    public class PostAsyncHandler : IRequestHandler<PostAsyncRequest, Boolean>
    {
        private readonly IRepository<InventoryItem> inventoryItemsRepository;
        private readonly IRepository<CatalogItem> catalogItemsRepository;
        private readonly IValidationService _validationService;

        public PostAsyncHandler(IRepository<InventoryItem> inventoryItemsRepository, IRepository<CatalogItem> catalogItemsRepository, IValidationService validationService)
        {
            this.inventoryItemsRepository = inventoryItemsRepository;
            this.catalogItemsRepository = catalogItemsRepository;
            _validationService = validationService;
        }

        public async System.Threading.Tasks.Task<Boolean> Handle(PostAsyncRequest request, CancellationToken cancellationToken)
        {
            var inventoryItem = await inventoryItemsRepository.GetAsync(
                item => item.UserId == request.grantItemsDto.UserId && item.CatalogItemId == request.grantItemsDto.CatalogItemId);


            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = request.grantItemsDto.CatalogItemId,
                    UserId = request.grantItemsDto.UserId,
                    Quantity = request.grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await inventoryItemsRepository.CreateAsync(inventoryItem);
                return true;
            }
            else if (inventoryItem != null)
            {
                inventoryItem.Quantity += request.grantItemsDto.Quantity;
                await inventoryItemsRepository.UpdateAsync(inventoryItem);
                return true;
            }
            else
                return false;
        }
    }
}
