using ESLA.Microservice.Quiz.Features.Quizzes.ValueObjects;

public static class MockData
{
    public static List<QuizModel> Quizzes = new List<QuizModel>
            {
                new QuizModel(Ulid.NewUlid(), "Quiz 1"),
                 new QuizModel(Ulid.NewUlid(), "Quiz 2"),
                 new QuizModel(Ulid.NewUlid(), "Quiz 3"),
            };

    public static IDictionary<Ulid, QuizSession> QuizSessions = new Dictionary<Ulid, QuizSession>();
}