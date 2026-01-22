namespace OTE.Common;

/// <summary>Functional-style Result type. Can either be something (ok) or an error.</summary>
/// <typeparam name="TValue">The value type of the `Result`.</typeparam>
/// <typeparam name="TError">The error type of the `Result`.</typeparam>
public sealed class Result<TValue, TError>
    where TValue : class?
    where TError : class?
{
    /// <summary>Exception thrown when calling `Unwrap()` on an error-state `Result`.</summary>
    public class BadUnwrapException : Exception
    {
        public BadUnwrapException() : base() { }
    }

    /// <summary>The `TValue` of the `Result` as a nullable type.</summary>
    public TValue? Value { get; private set; }

    /// <summary>The `TError` of the `Result` as a nullable type.</summary>
    public TError? Error { get; private set; }

    /// <summary>Flag that determines if the `Result` is in an ok or error state.</summary>
    public bool Ok { get; private set; }

    /// <summary>Returns a `Result` type in an ok state.</summary>
    /// <param name="value">The value to store in the `Result`.</param>
    public Result(TValue value)
    {
        Value = value;
        Error = null;
        Ok = true;
    }

    /// <summary>Returns a `Result` type in an error state.</summary>
    /// <param name="error">The error to store in the `Result`.</param>
    public Result(TError error)
    {
        Value = null;
        Error = error;
        Ok = false;
    }

    /// <summary>Force-get the value of the `Result`.</summary>
    /// <returns>The stored value.</returns>
    /// <exception cref="BadUnwrapException">Thrown if the `Result` is in an error state.</exception>
    public TValue Unwrap()
    {
        if (!Ok)
            throw new BadUnwrapException();

        return Value!;
    }

    /// <summary>Force-get the error of the `Result`.</summary>
    /// <returns>The stored error.</returns>
    /// <exception cref="BadUnwrapException">Thrown if the `Result` is ok.</exception>
    public TError UnwrapError()
    {
        if (Ok)
            throw new BadUnwrapException();

        return Error!;
    }

    /// <summary>
    /// Monadic bind-function. Applies a function to the stored value, potentially changing the value type.
    ///
    /// If the `Result` is ok, then the function is called on the `Result`'s
    /// value and its return value is returned by this function. If the `Result`
    /// is in an error state, then the error is passed to the new `Result`
    /// object.
    /// </summary>
    /// <typeparam name="TNewValue">The value type of the returned `Result`.</typeparam>
    /// <param name="func">The function to apply if the `Result` is ok.</param>
    /// <returns>A new `Result` object, potentially with a different type signature.</returns>
    public Result<TNewValue, TError> Bind<TNewValue>(Func<TValue, Result<TNewValue, TError>> func)
        where TNewValue : class
    {
        if (Ok)
            return func(Value!);
        else
            return new Result<TNewValue, TError>(Error!);
    }

    /// <summary>Sets the `Result` to be ok and have a new value.</summary>
    /// <param name="value">The new value for the `Result` to store.</param>
    public void SetOk(TValue value)
    {
        Value = value;
        Error = null;
        Ok = true;
    }

    /// <summary>Sets the `Result` to be an error and have a new value.</summary>
    /// <param name="value">The new error for the `Result` to store.</param>
    public void SetError(TError error)
    {
        Value = null;
        Error = error;
        Ok = false;
    }
}
