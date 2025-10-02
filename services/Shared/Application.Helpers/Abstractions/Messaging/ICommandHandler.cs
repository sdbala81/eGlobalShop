using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

public interface ICommandHandler<in TCommand> : MediatR.IRequestHandler<TCommand, Result> where TCommand : ICommand;

public interface ICommandHandler
    <in TCommand, TResponse> : MediatR.IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>;
