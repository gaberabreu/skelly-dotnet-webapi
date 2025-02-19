using Skelly.WebApi.Application.WorkItemAggregate.Delete;

namespace Skelly.WebApi.Application.UnitTests.TestHelper.Fakers;

public class DeleteWorkItemCommandFaker : Faker<DeleteWorkItemCommand>
{
    public DeleteWorkItemCommandFaker()
    {
        RuleFor(e => e.Id, f => f.Random.Guid());

        CustomInstantiator(f => new DeleteWorkItemCommand(
            f.Random.Guid()
        ));
    }
}
