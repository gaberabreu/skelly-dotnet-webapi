using Skelly.WebApi.Application.WorkItemAggregate.Create;

namespace Skelly.WebApi.Application.UnitTests.TestHelper.Fakers;

public class CreateWorkItemCommandFaker : Faker<CreateWorkItemCommand>
{
    public CreateWorkItemCommandFaker()
    {
        RuleFor(e => e.Title, f => f.Lorem.Sentences(1));

        CustomInstantiator(f => new CreateWorkItemCommand(
            f.Lorem.Sentences(1)
        ));
    }
}