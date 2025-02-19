using Skelly.WebApi.Application.WorkItemAggregate;

namespace Skelly.WebApi.Presentation.UnitTests.TestHelper.Fakers;

public class WorkItemDtoFaker : Faker<WorkItemDto>
{
    public WorkItemDtoFaker()
    {
        RuleFor(e => e.Id, f => f.Random.Guid());
        RuleFor(e => e.Title, f => f.Lorem.Sentences(1));

        CustomInstantiator(f => new WorkItemDto(
            f.Random.Guid(),
            f.Lorem.Sentences(1)
        ));
    }
}