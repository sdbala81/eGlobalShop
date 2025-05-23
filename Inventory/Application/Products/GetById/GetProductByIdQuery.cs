using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Inventory.Application.Products.GetAll;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.GetById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductResponse>;
