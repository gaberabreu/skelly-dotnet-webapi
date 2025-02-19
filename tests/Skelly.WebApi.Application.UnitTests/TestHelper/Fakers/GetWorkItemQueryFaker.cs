using Skelly.WebApi.Application.WorkItemAggregate.Get;

namespace Skelly.WebApi.Application.UnitTests.TestHelper.Fakers;

public class GetWorkItemQueryFaker : Faker<GetWorkItemQuery>
{
    public GetWorkItemQueryFaker()
    {
        RuleFor(e => e.Id, f => f.Random.Guid());

        CustomInstantiator(f => new GetWorkItemQuery(
            f.Random.Guid()
        ));
    }
}
