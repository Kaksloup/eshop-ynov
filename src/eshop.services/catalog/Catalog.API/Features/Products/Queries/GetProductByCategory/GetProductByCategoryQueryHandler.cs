using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;
using BuildingBlocks.Pagination;

namespace Catalog.API.Features.Products.Queries.GetProductByCategory;

/// <summary>
/// Handles the execution of the <see cref="GetProductByCategoryQuery"/> and retrieves the corresponding
/// product data from the data store.
/// </summary>
/// <remarks>
/// This class interacts with the database session to load the product category identified by
/// the specified <see cref="string"/> in the query. If the product does not exist,
/// a <see cref="ProductNotFoundException"/> is thrown. It utilizes a logger to log
/// the status and outcome of the operation.
/// </remarks>
public class GetProductByCategoryQueryHandler(IDocumentSession documentSession) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryQueryResult>
{
    /// <summary>
    /// Handles the execution of the GetProductByCategoryQuery and retrieves the associated product data.
    /// </summary>
    /// <param name="request">The query containing the identifier for the product to be retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the query, including the product data.</returns>
    /// <exception cref="CategoryNotFoundException">Thrown when no products are found for the specified category.</exception>
    public async Task<GetProductByCategoryQueryResult> Handle(GetProductByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        // Validate pagination inputs
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

        var baseQuery = documentSession.Query<Product>()
            .Where(x => x.Categories.Any(c => c == request.Category));
        
        var totalCount = await baseQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
            throw new CategoryNotFoundException(request.Category);

        // Retrieve page
        var products = await baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var paginated = new PaginatedResult<Product>(pageNumber, pageSize, totalCount, products);

        return new GetProductByCategoryQueryResult(paginated);
    }
}