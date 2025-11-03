using Basket.API.Data.Repositories;
using BuildingBlocks.CQRS;

namespace Basket.API.Features.Baskets.Queries.GetBasketByUserName;

/// <summary>
/// Handles the retrieval of a shopping basket associated with a specific username.
/// Implements the <see cref="IQueryHandler{TQuery, TResponse}"/> interface to process
/// <see cref="GetBasketByUserNameQuery"/> and return a <see cref="GetBasketByUserNameQueryResult"/>.
/// </summary>
public class GetBasketByUserNameQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketByUserNameQuery, GetBasketByUserNameQueryResult>
{
    /// <summary>
    /// Handles the execution of a query to retrieve the shopping basket associated with a specified username.
    /// </summary>
    /// <param name="request">The query request containing the username for which the basket is to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the query, which includes the shopping basket details.</returns>
    public async Task<GetBasketByUserNameQueryResult> Handle(GetBasketByUserNameQuery request,
        CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketByUserNameAsync(request.UserName, cancellationToken)
           .ConfigureAwait(false);

        // Ensure BasePrice is set for all items (for backward compatibility)
        foreach (var item in basket.Items)
        {
            if (item.BasePrice == 0 && item.Price > 0)
            {
                item.BasePrice = item.Price;
            }
        }

       return new GetBasketByUserNameQueryResult(basket);
    }
}