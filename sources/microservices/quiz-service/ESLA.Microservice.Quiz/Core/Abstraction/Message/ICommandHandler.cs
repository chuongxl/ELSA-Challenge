using MediatR;

namespace ItSchool.Application.Core.Abstraction.Message
{
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
       where TCommand : ICommand<TResponse>
    {
    }
}