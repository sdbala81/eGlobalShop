using ElementLogiq.SharedKernel;

using MediatR;

namespace ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;
