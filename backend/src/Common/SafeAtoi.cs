namespace OTE.Common;

/// <summary>Static class for performing a `Result`-encapsulated string-to-integer conversion.</summary>
public static class SafeAtoi
{
    /// <summary>Turns a `string` into an `int`.</summary>
    /// <param name="str">The `string` to parse.</param>
    /// <returns>A result containing the parsed `int`, or an error string.</returns>
    public static Result<int, string> Parse(string str)
    {
        try
        {
            return Result<int, string>.NewOk(int.Parse(str));
        }
        catch (ArgumentNullException)
        {
            return Result<int, string>.NewError("Expected 32-bit signed integer at end of url, instead got null.");
        }
        catch (FormatException)
        {
            return Result<int, string>.NewError($"Expected 32-bit signed integer at end of url, instead got {str}.");
        }
        catch (OverflowException)
        {
            return Result<int, string>.NewError($"{str} is too big/small and causes an overflow.");
        }
    }
}
