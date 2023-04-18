namespace Words.BusinessAccess.Models;

public class PaginationResult<T>
{
    public int TotalCount { get; init; }
    public IEnumerable<T> Value { get; set; }
}