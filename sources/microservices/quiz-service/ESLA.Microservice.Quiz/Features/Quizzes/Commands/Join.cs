using ESLA.Microservice.Quiz.Core.Response;
using ESLA.Microservice.Quiz.Features.Quizzes.ValueObjects;
using ItSchool.Application.Core.Abstraction.Message;

namespace ESLA.Microservice.Quiz.Features.Quizzes.Commands
{
    public static class Join
    {
        public class Command(Ulid quizId) : ICommand<OperationResult<QuizSession>>
        {
            public Ulid QuizId { get; set; } = quizId;
            public Ulid? SessionId { get; set; }
        }

        public class Handler : ICommandHandler<Command, OperationResult<QuizSession>>
        {
            public Task<OperationResult<QuizSession>> Handle(Command request, CancellationToken cancellationToken)
            {
                var quiz = MockData.Quizzes.FirstOrDefault(x => x.Id == request.QuizId);
                if (quiz is null)
                {
                    return Task.FromResult(new OperationResult<QuizSession>(OperationResult.NotFound()));
                }
                /*
                * This is a new feature that allows a user to join a quiz session.
                * If the session is not found, a new session is created.
                * For the real implemetation, the session should be stored in a real time database.
                * And this handle just only call the AWS EventBridge to send the request to AWS Kinesis stream.
                * The AWS Kinesis stream will handle the request and store the session in the real time database.
                * And then the AWS Kinesis stream will send the response back to the client via Notification service.
                */

                QuizSession? session = null;

                if (request.SessionId.HasValue)
                {

                    session = MockData.QuizSessions.Where(x => x.Key == request.SessionId)
                   .Select(x => x.Value).FirstOrDefault();
                }

                const int maxSessionPerQuiz = 45;

                if (session is null)
                {
                    var newSession = new QuizSession(Ulid.NewUlid(),
                    request.QuizId, DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(maxSessionPerQuiz));

                    MockData.QuizSessions.Add(newSession.Id, newSession);
                    return Task.FromResult(OperationResult.Ok(newSession));
                    /*
                    * Need to add the user to the session.
                    */
                }

                return Task.FromResult(OperationResult.Ok(session));
            }
        }
    }
}