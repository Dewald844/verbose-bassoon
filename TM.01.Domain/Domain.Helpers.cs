namespace TaskManager.Domain;

using System.Data.Common;
using System.Text.RegularExpressions;

public record Error(string Code, string Message);

public class Errors
{
    public static readonly Error None = new("", "");
    public static readonly Error NullValue = new("Error.NullValue", "A null value was provided.");
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Errors.None)
            throw new InvalidOperationException("Success result cannot have an error.");
        if (!isSuccess && error == Errors.None)
            throw new InvalidOperationException("Failure result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new (true, Errors.None);
    public static Result Failure(Error error) => new (false, error);
}

public class Result<T> : Result
{
    private readonly T _value;

    private Result(T value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value
    {
        get
        {
            if (!IsSuccess)
                throw new InvalidOperationException("Cannot access the value of a failure result.");
            return _value;
        }
    }

    public static Result<T> Success(T value) => new(value, true, Errors.None);
    public static new Result<T> Failure(Error error) => new Result<T>(default!, false, error);
}

public class EmailValidator
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }
}
