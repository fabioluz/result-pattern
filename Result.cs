namespace Example.Result;

// Abstract Result class to be pattern matched.
public abstract class Result<T>
{
}

// Success Result, meaning that what we tried to do worked.
public class Success<T>(T value) : Result<T>
{
    public T Value { get; init; } = value;
}

// This represents an error (either validation error or business logic error) from the backend.
// We should always return a Code and Message. The Code should be used by the frontend to translate the error
// to a human-readable language. In case they don't have that implemented, they can fallback to Message.
public readonly record struct Error(string Code, string Message);

// Failure Result, meaning that the operation didn't work, and we have a collection of errors
// to describe why.
public class Failure<T>(IEnumerable<Error> error) : Result<T>
{
     public IEnumerable<Error> Errors { get; init; } = error;
    
    public Result<TR> Cast<TR>()
    {
        return new Failure<TR>(Errors);
    }

}
