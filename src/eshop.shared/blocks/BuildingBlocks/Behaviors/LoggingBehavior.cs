using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

/// <summary>
/// Represents a pipeline behavior that logs incoming MediatR requests and their responses.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var uniqueId = Guid.NewGuid().ToString();
        var timer = Stopwatch.StartNew();

        using (logger.BeginScope(new Dictionary<string, object>
               {
                   ["CorrelationId"] = uniqueId,
                   ["RequestName"] = requestName,
                   ["Utilisateur"] = "test@gmail.com"
               }))
        {
            using (logger.BeginScope("Détails Requête: {@RequestDetails}", new
                   {
                       Timestamp = DateTimeOffset.UtcNow,
                       RequestType = requestName,
                       RequestData = request
                   }))
            {
                logger.LogInformation(
                    "Début du traitement de la requête {RequestName} [{UniqueId}]",
                    requestName,
                    uniqueId);

                var response = await next(cancellationToken);
                timer.Stop();

                using (logger.BeginScope("Détails Réponse: {@ResponseDetails}", new
                       {
                           DuréeExécution = timer.Elapsed,
                           ResponseData = response
                       }))
                {
                    if(timer.Elapsed.TotalSeconds > 3)
                        logger.LogWarning("Performance dégradée - La requête {RequestName} [{UniqueId}] a pris plus de {TimeTaken} secondes",
                            requestName,
                            uniqueId,
                            timer.Elapsed.TotalSeconds);
                    
                    logger.LogInformation(
                        "Fin du traitement de la requête {RequestName} [{UniqueId}]. Durée: {ElapsedMilliseconds}ms",
                        requestName,
                        uniqueId,
                        timer.ElapsedMilliseconds);

                    return response;
                }
            }
        }
     
    }
}