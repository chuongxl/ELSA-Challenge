using Carter;
using ESLA.Microservice.Quiz.Core.Extensions;
using ESLA.Microservice.Quiz.Features.Quizzes.Commands;
using ESLA.Microservice.Quiz.Features.Quizzes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ESLA.Microservice.Quiz.Features.Quizzes.EndPoints
{
    public class QuizEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/quiz", async (IMediator sender) =>
            {
                var result = await sender.Send(new List.Query());
                return result.ToResult();
            }).WithOpenApi();

            app.MapGet("/api/quiz/{id}", async ([FromRoute] Ulid id, IMediator sender) =>
           {
               var result = await sender.Send(new Get.Query(id));
               return result.ToResult();
           }).WithOpenApi();

            app.MapGet("/api/quiz/{id}/session", async ([FromRoute] Ulid id, IMediator sender) =>
               {
                   var result = await sender.Send(new ListSession.Query(id));
                   return result.ToResult();
               }).WithOpenApi();

            app.MapPost("/api/quiz/{id}/join/", async ([FromRoute] Ulid id, IMediator sender) =>
              {
                  var result = await sender.Send(new Join.Command(id));
                  return result.ToResult();
              }).WithOpenApi();

            app.MapPost("/api/quiz/{id}/join/{sessionId}", async ([FromRoute] Ulid id,
            [FromRoute] Ulid sessionId, IMediator sender) =>
           {
               var result = await sender.Send(new Join.Command(id) { SessionId = sessionId });
               return result.ToResult();
           }).WithOpenApi();

        }
    }
}
