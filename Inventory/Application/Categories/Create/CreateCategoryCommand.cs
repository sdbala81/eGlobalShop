using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Categories.Create;

public sealed class CreateCategoryCommand : ICommand<byte>
{
    public string Name { get; init; }

    public string Description { get; init; }
}
