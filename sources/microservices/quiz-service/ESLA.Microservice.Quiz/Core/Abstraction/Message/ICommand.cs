using MediatR;

namespace ItSchool.Application.Core.Abstraction.Message
{
    public interface ICommand<out TResponse> : IRequest<TResponse>, IRequestValidator<TResponse>
    {
    }
}