using System.Net;
using ESLA.Microservice.Quiz.Core.Response;


namespace ESLA.Microservice.Quiz.Core.Extensions;
public static class OperationResultExtension
{
    public static IResult ToResult(this OperationResult operationResult)
    {
        return Results.StatusCode((int)operationResult.StatusCode);
    }

    public static IResult ToResult<T>(this OperationResult<T> operationResult)
    {
        return operationResult.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(operationResult.Value),
            HttpStatusCode.Created => Results.Created(string.Empty, value: operationResult.Value),
            HttpStatusCode.Accepted => Results.Accepted(string.Empty, value: operationResult.Value),
            HttpStatusCode.BadRequest => Results.BadRequest(operationResult.Errors),
            _ => Results.StatusCode((int)operationResult.StatusCode)
        };
    }
}