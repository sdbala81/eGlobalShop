using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.GetAll;

public record GetAllProductsQuery : IQuery<IReadOnlyList<GetProductResponse>>;
