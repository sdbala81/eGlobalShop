using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Customers.Application.GetById;

public record GetCustomerByIdQuery(Guid CustomerId) : IQuery<GetCustomerResponse>;
