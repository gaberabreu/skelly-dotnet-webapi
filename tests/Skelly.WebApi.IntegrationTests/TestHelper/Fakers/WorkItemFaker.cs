using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.IntegrationTests.TestHelper.Fakers;

public class WorkItemFaker : Faker<WorkItem>
{
    public WorkItemFaker()
    {
        RuleFor(e => e.Title, f => f.Lorem.Sentences(1));

        CustomInstantiator(f => new WorkItem(
            f.Lorem.Sentences(1)
        ));
    }
}
