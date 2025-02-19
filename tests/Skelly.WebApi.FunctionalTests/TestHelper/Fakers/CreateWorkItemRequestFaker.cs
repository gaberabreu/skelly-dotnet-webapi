using Skelly.WebApi.Presentation.Requests;

namespace Skelly.WebApi.FunctionalTests.TestHelper.Fakers;

public class CreateWorkItemRequestFaker : Faker<CreateWorkItemRequest>
{
    public CreateWorkItemRequestFaker()
    {
        RuleFor(e => e.Title, f => f.Lorem.Sentence(1));
    }
}
