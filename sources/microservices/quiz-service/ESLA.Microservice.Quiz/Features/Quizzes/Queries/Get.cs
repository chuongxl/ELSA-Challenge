using ESLA.Microservice.Quiz.Core.Response;
using ESLA.Microservice.Quiz.Features.Quizzes.ValueObjects;
using ItSchool.Application.Core.Abstraction.Message;

namespace ESLA.Microservice.Quiz.Features.Quizzes.Queries
{
    public static class Get
    {
        public class Query(Ulid Id) : IQuery<OperationResult<QuizModel>>
        {
            public Ulid Id { get; set; } = Id;
        }

        public class Handler : IQueryHandler<Query, OperationResult<QuizModel>>
        {
            public Task<OperationResult<QuizModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                var quiz = MockData.Quizzes.FirstOrDefault(x => x.Id == request.Id);
                if (quiz is not null)
                {
                    return Task.FromResult(OperationResult.Ok(quiz));
                }

                return Task.FromResult(new OperationResult<QuizModel>(OperationResult.NotFound()));
            }
        }
    }
}