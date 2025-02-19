namespace Skelly.WebApi.FunctionalTests.TestHelper.Statics;

public class StaticPagedList<T>
{
    public IEnumerable<T> Items { get; set; } = [];

    public int PageNumber { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}