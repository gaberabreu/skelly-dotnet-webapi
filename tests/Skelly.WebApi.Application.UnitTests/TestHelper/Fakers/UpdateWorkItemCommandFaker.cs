using Skelly.WebApi.Application.WorkItemAggregate.Update;

namespace Skelly.WebApi.Application.UnitTests.TestHelper.Fakers;

public class UpdateWorkItemCommandFaker : Faker<UpdateWorkItemCommand>
{
    public UpdateWorkItemCommandFaker()
    {
        RuleFor(e => e.Id, f => f.Random.Guid());
        RuleFor(e => e.Title, f => f.Lorem.Sentences(1));

        CustomInstantiator(f => new UpdateWorkItemCommand(
            f.Random.Guid(),
            f.Lorem.Sentences(1)
        ));
    }
}
