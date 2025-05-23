using ElementLogiq.SharedKernel;

using MediatR;

namespace ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
