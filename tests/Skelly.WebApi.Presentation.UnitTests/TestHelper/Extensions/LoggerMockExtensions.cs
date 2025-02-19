namespace Skelly.WebApi.Presentation.UnitTests.TestHelper.Extensions;
public static class LoggerMockExtensions
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> logger, LogLevel level, string message, Times times)
    {
        logger.Verify(l => l.Log(
            level,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains(message)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()), times);
    }

    public static void VerifyLog<T>(this Mock<ILogger<T>> logger, LogLevel level, string message, Func<Times> times)
    {
        logger.VerifyLog(level, message, times());
    }
}
