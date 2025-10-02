using System.Collections.ObjectModel;

using ElementLogiq.eGlobalShop.Inventory.Domain;

namespace ElementLogiq.eGlobalShop.Inventory.Infrastructure.Database;

public static class InventorySeeder
{
    public static readonly Collection<Category> Categories =
    [
        new()
        {
            Id = 1,
            Name = "Electronics",
            Description = "Devices and gadgets including phones, laptops, and accessories."
        },
        new()
        {
            Id = 2,
            Name = "Books",
            Description = "Printed and digital books across various genres."
        },
        new()
        {
            Id = 3,
            Name = "Clothing",
            Description = "Men's, women's, and children's apparel."
        },
        new()
        {
            Id = 4,
            Name = "Home & Kitchen",
            Description = "Appliances, cookware, and home essentials."
        },
        new()
        {
            Id = 5,
            Name = "Sports & Outdoors",
            Description = "Equipment and gear for sports and outdoor activities."
        },
        new()
        {
            Id = 6,
            Name = "Toys & Games",
            Description = "Toys, games, and puzzles for all ages."
        },
        new()
        {
            Id = 7,
            Name = "Beauty & Personal Care",
            Description = "Cosmetics, skincare, and personal hygiene products."
        },
        new()
        {
            Id = 8,
            Name = "Automotive",
            Description = "Car accessories, tools, and automotive parts."
        },
        new()
        {
            Id = 9,
            Name = "Health",
            Description = "Health products, supplements, and medical supplies."
        },
        new()
        {
            Id = 10,
            Name = "Office Supplies",
            Description = "Stationery, office equipment, and organization tools."
        }
    ];

    public static readonly Collection<Product> Products =
    [
        new()
        {
            Id = new Guid("e2c1f5b8-1c7a-4b5e-9c1a-1b2e3f4a5b6c"),
            Name = "Smartphone X1",
            Description = "Latest 5G smartphone with high-resolution display.",
            Price = 699.99m,
            Quantity = 500,
            CategoryId = 1
        },
        new()
        {
            Id = new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"),
            Name = "Wireless Earbuds",
            Description = "Noise-cancelling wireless earbuds with long battery life.",
            Price = 129.99m,
            Quantity = 500,
            CategoryId = 1
        },
        new()
        {
            Id = new Guid("f6e5d4c3-b2a1-4c5d-8e9f-0a1b2c3d4e5f"),
            Name = "Laptop Pro 15",
            Description = "High-performance laptop for professionals.",
            Price = 1499.99m,
            Quantity = 500,
            CategoryId = 1
        },
        new()
        {
            Id = new Guid("b7c8d9e0-1a2b-4c3d-9e8f-7a6b5c4d3e2f"),
            Name = "Bluetooth Speaker",
            Description = "Portable speaker with deep bass and waterproof design.",
            Price = 89.99m,
            Quantity = 500,
            CategoryId = 1
        },
        new()
        {
            Id = new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"),
            Name = "Smartwatch S2",
            Description = "Fitness tracking smartwatch with heart rate monitor.",
            Price = 199.99m,
            Quantity = 500,
            CategoryId = 1
        },
        new()
        {
            Id = new Guid("d4e5f6a7-b8c9-4d0e-9f1a-2b3c4d5e6f7a"),
            Name = "Mystery Novel",
            Description = "A thrilling mystery novel by a bestselling author.",
            Price = 14.99m,
            Quantity = 500,
            CategoryId = 2
        },
        new()
        {
            Id = new Guid("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8a9b"),
            Name = "Science Textbook",
            Description = "Comprehensive science textbook for high school students.",
            Price = 49.99m,
            Quantity = 500,
            CategoryId = 2
        },
        new()
        {
            Id = new Guid("f7a8b9c0-d1e2-4f3a-5b6c-7d8e9f0a1b2c"),
            Name = "Cookbook Deluxe",
            Description = "Delicious recipes from around the world.",
            Price = 24.99m,
            Quantity = 500,
            CategoryId = 2
        },
        new()
        {
            Id = new Guid("a9b0c1d2-e3f4-4a5b-6c7d-8e9f0a1b2c3d"),
            Name = "Children's Storybook",
            Description = "Illustrated storybook for young readers.",
            Price = 9.99m,
            Quantity = 500,
            CategoryId = 2
        },
        new()
        {
            Id = new Guid("b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e"),
            Name = "Business Guide",
            Description = "Essential guide for starting a small business.",
            Price = 19.99m,
            Quantity = 500,
            CategoryId = 2
        },
        new()
        {
            Id = new Guid("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f"),
            Name = "Men's T-Shirt",
            Description = "100% cotton classic fit t-shirt.",
            Price = 12.99m,
            Quantity = 500,
            CategoryId = 3
        },
        new()
        {
            Id = new Guid("d5e6f7a8-b9c0-4d1e-2f3a-4b5c6d7e8f9a"),
            Name = "Women's Jeans",
            Description = "Slim fit stretch denim jeans.",
            Price = 39.99m,
            Quantity = 500,
            CategoryId = 3
        },
        new()
        {
            Id = new Guid("e7f8a9b0-c1d2-4e3f-4a5b-6c7d8e9f0a1b"),
            Name = "Children's Jacket",
            Description = "Warm and waterproof jacket for kids.",
            Price = 29.99m,
            Quantity = 500,
            CategoryId = 3
        },
        new()
        {
            Id = new Guid("f9a0b1c2-d3e4-4f5a-6b7c-8d9e0f1a2b3c"),
            Name = "Summer Dress",
            Description = "Lightweight floral summer dress.",
            Price = 24.99m,
            Quantity = 500,
            CategoryId = 3
        },
        new()
        {
            Id = new Guid("a2b3c4d5-e6f7-4a8b-9c0d-1e2f3a4b5c6d"),
            Name = "Sports Hoodie",
            Description = "Comfortable hoodie for workouts and casual wear.",
            Price = 34.99m,
            Quantity = 500,
            CategoryId = 3
        },
        new()
        {
            Id = new Guid("b4c5d6e7-f8a9-4b0c-1d2e-3f4a5b6c7d8e"),
            Name = "Blender Pro",
            Description = "High-speed blender for smoothies and soups.",
            Price = 59.99m,
            Quantity = 500,
            CategoryId = 4
        },
        new()
        {
            Id = new Guid("c6d7e8f9-a0b1-4c2d-3e4f-5a6b7c8d9e0f"),
            Name = "Nonstick Cookware Set",
            Description = "10-piece nonstick cookware set for all your cooking needs.",
            Price = 89.99m,
            Quantity = 500,
            CategoryId = 4
        },
        new()
        {
            Id = new Guid("d8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a"),
            Name = "Vacuum Cleaner",
            Description = "Bagless vacuum cleaner with HEPA filter.",
            Price = 129.99m,
            Quantity = 500,
            CategoryId = 4
        },
        new()
        {
            Id = new Guid("e0f1a2b3-c4d5-4e6f-7a8b-9c0d1e2f3a4b"),
            Name = "Coffee Maker",
            Description = "Programmable coffee maker with thermal carafe.",
            Price = 79.99m,
            Quantity = 500,
            CategoryId = 4
        },
        new()
        {
            Id = new Guid("f2a3b4c5-d6e7-4f8a-9b0c-1d2e3f4a5b6c"),
            Name = "Air Fryer",
            Description = "Oil-less air fryer for healthy cooking.",
            Price = 99.99m,
            Quantity = 500,
            CategoryId = 4
        },
        new()
        {
            Id = new Guid("a4b5c6d7-e8f9-4a0b-1c2d-3e4f5a6b7c8d"),
            Name = "Yoga Mat",
            Description = "Non-slip yoga mat for all types of exercise.",
            Price = 19.99m,
            Quantity = 500,
            CategoryId = 5
        },
        new()
        {
            Id = new Guid("b6c7d8e9-f0a1-4b2c-3d4e-5f6a7b8c9d0e"),
            Name = "Mountain Bike",
            Description = "Durable mountain bike with 21-speed gears.",
            Price = 299.99m,
            Quantity = 500,
            CategoryId = 5
        },
        new()
        {
            Id = new Guid("c8d9e0f1-a2b3-4c5d-6e7f-8a9b0c1d2e3f"),
            Name = "Tennis Racket",
            Description = "Lightweight tennis racket for beginners and pros.",
            Price = 49.99m,
            Quantity = 500,
            CategoryId = 5
        },
        new()
        {
            Id = new Guid("d0e1f2a3-b4c5-4d6e-7f8a-9b0c1d2e3f4a"),
            Name = "Camping Tent",
            Description = "4-person waterproof camping tent.",
            Price = 119.99m,
            Quantity = 500,
            CategoryId = 5
        },
        new()
        {
            Id = new Guid("e2f3a4b5-c6d7-4e8f-9a0b-1c2d3e4f5a6b"),
            Name = "Dumbbell Set",
            Description = "Adjustable dumbbell set for strength training.",
            Price = 59.99m,
            Quantity = 500,
            CategoryId = 5
        },
        new()
        {
            Id = new Guid("f4a5b6c7-d8e9-4f0a-1b2c-3d4e5f6a7b8c"),
            Name = "Building Blocks",
            Description = "Creative building blocks set for kids.",
            Price = 29.99m,
            Quantity = 500,
            CategoryId = 6
        },
        new()
        {
            Id = new Guid("a6b7c8d9-e0f1-4a2b-3c4d-5e6f7a8b9c0d"),
            Name = "Puzzle Game",
            Description = "Challenging puzzle game for all ages.",
            Price = 14.99m,
            Quantity = 500,
            CategoryId = 6
        },
        new()
        {
            Id = new Guid("b8c9d0e1-f2a3-4b4c-5d6e-7f8a9b0c1d2e"),
            Name = "Remote Control Car",
            Description = "High-speed remote control car with rechargeable battery.",
            Price = 39.99m,
            Quantity = 500,
            CategoryId = 6
        },
        new()
        {
            Id = new Guid("c0d1e2f3-a4b5-4c6d-7e8f-9a0b1c2d3e4f"),
            Name = "Board Game Classic",
            Description = "Classic board game for family fun.",
            Price = 24.99m,
            Quantity = 500,
            CategoryId = 6
        },
        new()
        {
            Id = new Guid("d2e3f4a5-b6c7-4d8e-9f0a-1b2c3d4e5f6a"),
            Name = "Plush Toy",
            Description = "Soft and cuddly plush toy.",
            Price = 12.99m,
            Quantity = 500,
            CategoryId = 6
        },
        new()
        {
            Id = new Guid("e4f5a6b7-c8d9-4e0f-1a2b-3c4d5e6f7a8b"),
            Name = "Face Moisturizer",
            Description = "Hydrating face moisturizer for all skin types.",
            Price = 19.99m,
            Quantity = 500,
            CategoryId = 7
        },
        new()
        {
            Id = new Guid("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8a9b0c"),
            Name = "Shampoo & Conditioner",
            Description = "Nourishing shampoo and conditioner set.",
            Price = 15.99m,
            Quantity = 500,
            CategoryId = 7
        },
        new()
        {
            Id = new Guid("a8b9c0d1-e2f3-4a4b-5c6d-7e8f9a0b1c2d"),
            Name = "Lipstick Set",
            Description = "Long-lasting lipstick set in assorted colors.",
            Price = 24.99m,
            Quantity = 500,
            CategoryId = 7
        },
        new()
        {
            Id = new Guid("b0c1d2e3-f4a5-4b6c-7d8e-9f0a1b2c3d4e"),
            Name = "Electric Toothbrush",
            Description = "Rechargeable electric toothbrush with multiple modes.",
            Price = 39.99m,
            Quantity = 500,
            CategoryId = 7
        },
        new()
        {
            Id = new Guid("c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f"),
            Name = "Sunscreen Lotion",
            Description = "SPF 50+ sunscreen lotion for sensitive skin.",
            Price = 14.99m,
            Quantity = 500,
            CategoryId = 7
        },
        new()
        {
            Id = new Guid("d4e5f6a7-b8c9-4d0e-9f1a-2b3c4d5e6f7b"),
            Name = "Car Vacuum Cleaner",
            Description = "Portable vacuum cleaner for car interiors.",
            Price = 34.99m,
            Quantity = 500,
            CategoryId = 8
        },
        new()
        {
            Id = new Guid("e6f7a8b9-c0d1-4e2f-3a4b-5c6d7e8f9a0b"),
            Name = "Dash Cam",
            Description = "Full HD dash cam with night vision.",
            Price = 79.99m,
            Quantity = 500,
            CategoryId = 8
        },
        new()
        {
            Id = new Guid("f8a9b0c1-d2e3-4f4a-5b6c-7d8e9f0a1b2c"),
            Name = "Car Phone Mount",
            Description = "Adjustable phone mount for car dashboards.",
            Price = 19.99m,
            Quantity = 500,
            CategoryId = 8
        },
        new()
        {
            Id = new Guid("a0b1c2d3-e4f5-4a6b-7c8d-9e0f1a2b3c4d"),
            Name = "Tire Inflator",
            Description = "Portable tire inflator with digital gauge.",
            Price = 49.99m,
            Quantity = 500,
            CategoryId = 8
        },
        new()
        {
            Id = new Guid("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e"),
            Name = "Car Cover",
            Description = "All-weather car cover for sedans.",
            Price = 59.99m,
            Quantity = 500,
            CategoryId = 8
        },
        new()
        {
            Id = new Guid("c4d5e6f7-a8b9-4c0d-1e2f-3a4b5c6d7e8f"),
            Name = "Vitamin C Tablets",
            Description = "Immune support vitamin C supplement.",
            Price = 11.99m,
            Quantity = 500,
            CategoryId = 9
        },
        new()
        {
            Id = new Guid("d6e7f8a9-b0c1-4d2e-3f4a-5b6c7d8e9f0a"),
            Name = "First Aid Kit",
            Description = "Comprehensive first aid kit for emergencies.",
            Price = 24.99m,
            Quantity = 500,
            CategoryId = 9
        },
        new()
        {
            Id = new Guid("e8f9a0b1-c2d3-4e4f-5a6b-7c8d9e0f1a2b"),
            Name = "Digital Thermometer",
            Description = "Fast and accurate digital thermometer.",
            Price = 9.99m,
            Quantity = 500,
            CategoryId = 9
        },
        new()
        {
            Id = new Guid("f0a1b2c3-d4e5-4f6a-7b8c-9d0e1f2a3b4c"),
            Name = "Blood Pressure Monitor",
            Description = "Automatic blood pressure monitor with large display.",
            Price = 39.99m,
            Quantity = 500,
            CategoryId = 9
        },
        new()
        {
            Id = new Guid("a2b3c4d5-e6f7-4a8b-9c0d-1e2f3a4b5c6e"),
            Name = "Hand Sanitizer",
            Description = "Antibacterial hand sanitizer gel.",
            Price = 4.99m,
            Quantity = 500,
            CategoryId = 9
        },
        new()
        {
            Id = new Guid("b4c5d6e7-f8a9-4b0c-1d2e-3f4a5b6c7d8f"),
            Name = "Notebook Pack",
            Description = "Pack of 5 ruled notebooks for school or office.",
            Price = 9.99m,
            Quantity = 500,
            CategoryId = 10
        },
        new()
        {
            Id = new Guid("c6d7e8f9-a0b1-4c2d-3e4f-5a6b7c8d9e1f"),
            Name = "Ballpoint Pens",
            Description = "Set of 20 smooth-writing ballpoint pens.",
            Price = 7.99m,
            Quantity = 500,
            CategoryId = 10
        },
        new()
        {
            Id = new Guid("d8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3b"),
            Name = "Desk Organizer",
            Description = "Multi-compartment desk organizer for supplies.",
            Price = 14.99m,
            Quantity = 500,
            CategoryId = 10
        },
        new()
        {
            Id = new Guid("e0f1a2b3-c4d5-4e6f-7a8b-9c0d1e2f3a4c"),
            Name = "Office Chair",
            Description = "Ergonomic office chair with lumbar support.",
            Price = 129.99m,
            Quantity = 500,
            CategoryId = 10
        },
        new()
        {
            Id = new Guid("f2a3b4c5-d6e7-4f8a-9b0c-1d2e3f4a5b6d"),
            Name = "Stapler Set",
            Description = "Heavy-duty stapler with extra staples.",
            Price = 12.99m,
            Quantity = 500,
            CategoryId = 10
        }
    ];
}
