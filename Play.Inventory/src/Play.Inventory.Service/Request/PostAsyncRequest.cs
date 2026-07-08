using System;
using MediatR;

namespace Play.Inventory.Service.Request
{
    public class PostAsyncRequest:IRequest<Boolean>
    {
        public GrantItemsDto grantItemsDto {get; set; }
    }
}
