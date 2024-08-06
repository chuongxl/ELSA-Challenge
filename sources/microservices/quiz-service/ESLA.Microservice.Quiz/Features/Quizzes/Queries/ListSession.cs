using System.Collections;
using ESLA.Microservice.Quiz.Core.Response;
using ESLA.Microservice.Quiz.Features.Quizzes.ValueObjects;
using ItSchool.Application.Core.Abstraction.Message;
using MediatR;

namespace ESLA.Microservice.Quiz.Features.Quizzes.Queries
{
    public static class ListSession
    {
        public class Query(Ulid quizId) : IQuery<OperationResult<List<QuizSession>>>
        {
            public Ulid QuizId { get; set; } = quizId;
        }

        public class Handler : IQueryHandler<Query, OperationResult<List<QuizSession>>>
        {
            public Task<OperationResult<List<QuizSession>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var allSessionOfAQuiz = MockData.QuizSessions.Where(s => s.Key == request.QuizId)
                .Select(x => x.Value).ToList();

                return Task.FromResult(OperationResult.Ok(allSessionOfAQuiz));
            }
        }
    }
}