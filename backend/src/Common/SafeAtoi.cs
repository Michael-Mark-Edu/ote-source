namespace OTE.Common;

public class SafeAtoiError
{
    public string BodyMessage { get; set; }
    public SafeAtoiError(string bodyMessage) => BodyMessage = bodyMessage;
}

public static class SafeAtoi
{
    public static Result<int, SafeAtoiError> Parse(string str)
    {
        try
        {
            return Result<int, SafeAtoiError>.NewOk(int.Parse(str));
        }
        catch (ArgumentNullException)
        {
            return Result<int, SafeAtoiError>.NewError(new SafeAtoiError("Expected 32-bit signed integer at end of url, instead got null."));
        }
        catch (FormatException)
        {
            return Result<int, SafeAtoiError>.NewError(new SafeAtoiError($"Expected 32-bit signed integer at end of url, instead got {str}."));
        }
        catch (OverflowException)
        {
            return Result<int, SafeAtoiError>.NewError(new SafeAtoiError($"{str} is too big/small and causes an overflow."));
        }
    }
}
