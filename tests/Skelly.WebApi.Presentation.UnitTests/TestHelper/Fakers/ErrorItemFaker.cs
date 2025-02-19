namespace Skelly.WebApi.Presentation.UnitTests.TestHelper.Fakers;

public class ErrorItemFaker : Faker<ErrorItem>
{
    public ErrorItemFaker()
    {
        RuleFor(e => e.Field, f => f.Lorem.Word());
        RuleFor(e => e.Code, f => f.Random.AlphaNumeric(5));
        RuleFor(e => e.Message, f => f.Lorem.Sentence());

        CustomInstantiator(f => new ErrorItem(
            f.Random.Word(),
            f.Random.AlphaNumeric(5),
            f.Lorem.Sentence()
        ));
    }
}