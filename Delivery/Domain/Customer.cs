using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Customers.Domain;

public class Customer : Entity
{
    public Guid Id { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string ZipCode { get; set; }
}
