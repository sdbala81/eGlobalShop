using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Customers.Application.GetById;

namespace ElementLogiq.eGlobalShop.Customers.Application.GetAll;

public record GetAllCustomersQuery : IQuery<IReadOnlyList<GetCustomerResponse>>;
