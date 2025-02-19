using Skelly.WebApi.Presentation.Requests;

namespace Skelly.WebApi.Presentation.UnitTests.TestHelper.Fakers;

public class ListWorkItemsRequestFaker : Faker<ListWorkItemsRequest>
{
    public ListWorkItemsRequestFaker()
    {
        RuleFor(e => e.PageNumber, f => f.Random.Int(1, 100));
        RuleFor(e => e.PageSize, f => f.Random.Int(1, 100));
    }
}