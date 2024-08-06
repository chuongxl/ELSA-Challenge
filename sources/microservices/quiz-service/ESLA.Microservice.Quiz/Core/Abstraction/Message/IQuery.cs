using MediatR;

namespace ItSchool.Application.Core.Abstraction.Message
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}