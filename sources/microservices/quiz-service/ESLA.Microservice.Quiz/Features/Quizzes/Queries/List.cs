using System.Collections;
using ESLA.Microservice.Quiz.Core.Response;
using ESLA.Microservice.Quiz.Features.Quizzes.ValueObjects;
using ItSchool.Application.Core.Abstraction.Message;
using MediatR;

namespace ESLA.Microservice.Quiz.Features.Quizzes.Queries
{
    public static class List
    {
        public class Query : IQuery<OperationResult<List<QuizModel>>>
        {

        }

        public class Handler : IQueryHandler<Query, OperationResult<List<QuizModel>>>
        {
            public Task<OperationResult<List<QuizModel>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(OperationResult.Ok(MockData.Quizzes));
            }
        }
    }
}