using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Inventory.Domain;

public static class InventoryErrors
{
    public static readonly Error NotFoundByProductId = Error.NotFound(
        "Products.NotFoundById",
        "The products with the specified id was not found");

    public static Error NotFound(Guid productId)
    {
        return Error.NotFound("Products.NotFound", $"The product with the Id = '{productId}' was not found");
    }
}
