using Amazon.Lambda.Core;

namespace OTE.Data.Test;

public class MockLambdaLogger : ILambdaLogger
{
    void ILambdaLogger.Log(string level, string message)
    {
        Console.WriteLine($"MockLambdaLogger [{level}]: {message}");
    }

    void ILambdaLogger.Log(LogLevel level, string message)
    {
        Console.WriteLine($"MockLambdaLogger [{level}]: {message}");
    }

    void ILambdaLogger.Log(string message)
    {
        Console.WriteLine($"MockLambdaLogger [nolevel]: {message}");
    }

    void ILambdaLogger.LogLine(string message)
    {
        Console.WriteLine($"MockLambdaLogger [line]: {message}");
    }
}
