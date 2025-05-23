using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Inventory.Application.Data;
using ElementLogiq.eGlobalShop.Inventory.Domain;
using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Categories.Create;

public class CreateCategoryCommandHandler(IInventoryDbContext inventoryDbContext) : ICommandHandler<CreateCategoryCommand, byte>
{
    public async Task<Result<byte>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        inventoryDbContext.Categories.Add(category);

        await inventoryDbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return category.Id;
    }
}
