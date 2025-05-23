namespace ElementLogiq.eGlobalShop.Billing.Domain.Entities;

public class BillingAddress
{
    public string HouseNo { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }

    public static BillingAddress Create(
        string houseNo,
        string street,
        string city,
        string state,
        string postalCode,
        string country)
    {
        return new BillingAddress
        {
            HouseNo = houseNo,
            Street = street,
            City = city,
            State = state,
            PostalCode = postalCode,
            Country = country
        };
    }

    public override string ToString()
    {
        return $"{HouseNo}, {Street}, {City}, {State}, {PostalCode}, {Country}";
    }
}
