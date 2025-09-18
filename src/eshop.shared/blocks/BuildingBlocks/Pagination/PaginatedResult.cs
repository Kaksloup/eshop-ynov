namespace BuildingBlocks.Pagination;

/// <summary>
/// Represents a paginated result for a collection of data, providing
/// the relevant pagination information such as page index, page size,
/// total count of items, and the data for the specific page.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entities within the paginated result. Must be a reference type.
/// </typeparam>
public class PaginatedResult<TEntity>(int pageIndex, int pageSize, long totalCount, IEnumerable<TEntity> data)
    where TEntity : class
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long TotalCount { get; } = totalCount;
    public IEnumerable<TEntity> Data { get; } = data;
}