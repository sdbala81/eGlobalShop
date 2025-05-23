using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Customers.Domain;

public static class CustomerErrors
{
    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Customers.NotFoundByEmail",
        "The customer with the specified email was not found");

    public static readonly Error EmailNotUnique = Error.Conflict("Customers.EmailNotUnique", "The provided email is not unique");

    public static Error NotFound(Guid customerId)
    {
        return Error.NotFound("Customers.NotFound", $"The customer with the Id = '{customerId}' was not found");
    }
}
