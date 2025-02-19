using Skelly.WebApi.Application.WorkItemAggregate.List;

namespace Skelly.WebApi.Application.UnitTests.TestHelper.Fakers;

public class ListWorkItemsQueryFaker : Faker<ListWorkItemsQuery>
{
    public ListWorkItemsQueryFaker()
    {
        RuleFor(e => e.PageNumber, f => f.Random.Int(1, 100));
        RuleFor(e => e.PageSize, f => f.Random.Int(1, 100));

        CustomInstantiator(f => new ListWorkItemsQuery(
            f.Random.Int(1, 100),
            f.Random.Int(1, 100)
        ));
    }
}
