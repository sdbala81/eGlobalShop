namespace ElementLogiq.eGlobalShop.Shipping.Domain.Entities;

public class ShippingAddress
{
    public string HouseNo { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }

    public static ShippingAddress Create(
        string houseNo,
        string street,
        string city,
        string state,
        string postalCode,
        string country)
    {
        return new ShippingAddress
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
