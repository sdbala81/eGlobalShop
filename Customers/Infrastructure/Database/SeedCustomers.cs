using ElementLogiq.eGlobalShop.Customers.Domain;

// Assume that the Customer model is defined in the following namespace

namespace ElementLogiq.eGlobalShop.Customers.Infrastructure.Database;

public static class SeedCustomers
{
    public static readonly IReadOnlyCollection<Customer> Customers =
    [
        new()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "555-1234",
            DateOfBirth = new DateOnly(1985, 06, 15), // updated from DateTime to DateOnly
            Address = "123 Main St",
            City = "Springfield",
            State = "IL",
            ZipCode = "62701"
        },
        new()
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            PhoneNumber = "555-5678",
            DateOfBirth = new DateOnly(1990, 09, 22), // updated from DateTime to DateOnly
            Address = "456 Oak Ave",
            City = "Greenville",
            State = "TX",
            ZipCode = "75401"
        },
        new()
        {
            FirstName = "Michael",
            LastName = "Johnson",
            Email = "michael.johnson@example.com",
            PhoneNumber = "555-2345",
            DateOfBirth = new DateOnly(1978, 03, 12), // updated from DateTime to DateOnly
            Address = "789 Pine Rd",
            City = "Fairview",
            State = "CA",
            ZipCode = "90210"
        },
        new()
        {
            FirstName = "Emily",
            LastName = "Davis",
            Email = "emily.davis@example.com",
            PhoneNumber = "555-3456",
            DateOfBirth = new DateOnly(1982, 11, 30), // updated from DateTime to DateOnly
            Address = "321 Maple St",
            City = "Riverside",
            State = "FL",
            ZipCode = "32099"
        },
        new()
        {
            FirstName = "William",
            LastName = "Brown",
            Email = "william.brown@example.com",
            PhoneNumber = "555-4567",
            DateOfBirth = new DateOnly(1995, 07, 18), // updated from DateTime to DateOnly
            Address = "654 Cedar Ave",
            City = "Hilltown",
            State = "NY",
            ZipCode = "10001"
        },
        new()
        {
            FirstName = "Olivia",
            LastName = "Wilson",
            Email = "olivia.wilson@example.com",
            PhoneNumber = "555-5679",
            DateOfBirth = new DateOnly(1988, 02, 25), // updated from DateTime to DateOnly
            Address = "987 Birch Blvd",
            City = "Lakeview",
            State = "WA",
            ZipCode = "98001"
        },
        new()
        {
            FirstName = "James",
            LastName = "Martinez",
            Email = "james.martinez@example.com",
            PhoneNumber = "555-6789",
            DateOfBirth = new DateOnly(1975, 10, 05), // updated from DateTime to DateOnly
            Address = "159 Spruce Dr",
            City = "Brookfield",
            State = "OH",
            ZipCode = "44403"
        },
        new()
        {
            FirstName = "Sophia",
            LastName = "Garcia",
            Email = "sophia.garcia@example.com",
            PhoneNumber = "555-7890",
            DateOfBirth = new DateOnly(1992, 04, 14), // updated from DateTime to DateOnly
            Address = "753 Willow Ln",
            City = "Meadowbrook",
            State = "CO",
            ZipCode = "80014"
        },
        new()
        {
            FirstName = "Benjamin",
            LastName = "Rodriguez",
            Email = "benjamin.rodriguez@example.com",
            PhoneNumber = "555-8901",
            DateOfBirth = new DateOnly(1980, 12, 01), // updated from DateTime to DateOnly
            Address = "852 Aspen Ct",
            City = "Oakwood",
            State = "GA",
            ZipCode = "30566"
        },
        new()
        {
            FirstName = "Ava",
            LastName = "Lee",
            Email = "ava.lee@example.com",
            PhoneNumber = "555-9012",
            DateOfBirth = new DateOnly(1998, 08, 09), // updated from DateTime to DateOnly
            Address = "951 Poplar St",
            City = "Sunnyvale",
            State = "CA",
            ZipCode = "94086"
        },
        new()
        {
            FirstName = "Logan",
            LastName = "Walker",
            Email = "logan.walker@example.com",
            PhoneNumber = "555-0123",
            DateOfBirth = new DateOnly(1983, 05, 21), // updated from DateTime to DateOnly
            Address = "246 Elm St",
            City = "Westfield",
            State = "IN",
            ZipCode = "46074"
        },
        new()
        {
            FirstName = "Mia",
            LastName = "Hall",
            Email = "mia.hall@example.com",
            PhoneNumber = "555-1235",
            DateOfBirth = new DateOnly(1991, 01, 17), // updated from DateTime to DateOnly
            Address = "357 Chestnut Ave",
            City = "Easton",
            State = "PA",
            ZipCode = "18042"
        },
        new()
        {
            FirstName = "Alexander",
            LastName = "Allen",
            Email = "alexander.allen@example.com",
            PhoneNumber = "555-2346",
            DateOfBirth = new DateOnly(1979, 09, 28), // updated from DateTime to DateOnly
            Address = "468 Walnut Dr",
            City = "Centerville",
            State = "OH",
            ZipCode = "45459"
        },
        new()
        {
            FirstName = "Charlotte",
            LastName = "Young",
            Email = "charlotte.young@example.com",
            PhoneNumber = "555-3457",
            DateOfBirth = new DateOnly(1987, 03, 03), // updated from DateTime to DateOnly
            Address = "579 Hickory Ln",
            City = "Franklin",
            State = "TN",
            ZipCode = "37064"
        },
        new()
        {
            FirstName = "Daniel",
            LastName = "Hernandez",
            Email = "daniel.hernandez@example.com",
            PhoneNumber = "555-4568",
            DateOfBirth = new DateOnly(1984, 06, 11), // updated from DateTime to DateOnly
            Address = "680 Magnolia St",
            City = "Georgetown",
            State = "KY",
            ZipCode = "40324"
        },
        new()
        {
            FirstName = "Amelia",
            LastName = "King",
            Email = "amelia.king@example.com",
            PhoneNumber = "555-5670",
            DateOfBirth = new DateOnly(1993, 12, 19), // updated from DateTime to DateOnly
            Address = "791 Sycamore Rd",
            City = "Madison",
            State = "WI",
            ZipCode = "53703"
        },
        new()
        {
            FirstName = "Matthew",
            LastName = "Wright",
            Email = "matthew.wright@example.com",
            PhoneNumber = "555-6780",
            DateOfBirth = new DateOnly(1986, 08, 23), // updated from DateTime to DateOnly
            Address = "802 Redwood Ave",
            City = "Lexington",
            State = "KY",
            ZipCode = "40508"
        },
        new()
        {
            FirstName = "Harper",
            LastName = "Lopez",
            Email = "harper.lopez@example.com",
            PhoneNumber = "555-7891",
            DateOfBirth = new DateOnly(1997, 02, 02), // updated from DateTime to DateOnly
            Address = "913 Palm St",
            City = "Bayside",
            State = "NY",
            ZipCode = "11360"
        },
        new()
        {
            FirstName = "David",
            LastName = "Scott",
            Email = "david.scott@example.com",
            PhoneNumber = "555-8902",
            DateOfBirth = new DateOnly(1977, 11, 07), // updated from DateTime to DateOnly
            Address = "124 Cypress Blvd",
            City = "Rockville",
            State = "MD",
            ZipCode = "20850"
        },
        new()
        {
            FirstName = "Ella",
            LastName = "Green",
            Email = "ella.green@example.com",
            PhoneNumber = "555-9013",
            DateOfBirth = new DateOnly(1994, 04, 27), // updated from DateTime to DateOnly
            Address = "235 Dogwood Dr",
            City = "Mapleton",
            State = "MN",
            ZipCode = "56065"
        }
    ];
}
