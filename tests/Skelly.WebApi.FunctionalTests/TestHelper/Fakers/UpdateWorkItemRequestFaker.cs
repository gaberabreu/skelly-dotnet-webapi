using Skelly.WebApi.Presentation.Requests;

namespace Skelly.WebApi.FunctionalTests.TestHelper.Fakers;

public class UpdateWorkItemRequestFaker : Faker<UpdateWorkItemRequest>
{
    public UpdateWorkItemRequestFaker()
    {
        RuleFor(e => e.Title, f => f.Lorem.Sentence(1));
    }
}
