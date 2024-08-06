namespace ESLA.Microservice.Quiz.Features.Quizzes.ValueObjects
{
    public record QuizSession(Ulid Id, Ulid QuizId, DateTime StartedAt, DateTime? FinishedAt);
}