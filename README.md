# Result Pattern

This project demonstrates how to use pattern matching and stronger typing to avoid the unsafe `if (isValid)` calls.

## The Problem

A common way of validating inputs, objects and business logic in imperative languages is by using if conditions to check validation results. Then, early returning the method if necessary. E.g.

```
obj.Validate();
if (!obj.IsValid)
{
    return obj.Errors;
}
```

```
if (!obj.IsValid())
{
    return obj.Errors;
}
```

The problem with this approach is that a programmer can easily forget to call `if (obj.IsValid)`, thus allowing unvalidated information to proceed in the flow. This can also easily pass through code reviews, where people are usually focused on finding problems in the code they are currently reviewing rather than looking for missing code in the pull request.

You may argue that this would be the programmer's or reviewer's fault because inputs should always be validated before anything else. However, sometimes that is not obvious. In a large codebase, a programmer who has just started working on it may not know that they have to call if (obj.SomethingIsNotTrue). Sometimes, it is not even clear where that information is coming from, which makes it harder to determine which validations or adjustments must be performed before saving that information in the database or sending it to another system.

## Solution

Use a class to represent the validated information. For example, consider the following input model:

```
public record CreateBook(string Name, string Author, int Year);
```

Use another class to represent the input after validation:

```
public record ValidCreateBook(string Name, string Author, int Year);
```

Use a class to represent the Result of an operation:

```
public abstract class Result<T>
{
}

public class Success<T>(T value) : Result<T>
{
    public T Value { get; init; } = value;
}

public class Failure<T>(IEnumerable<Error> error) : Result<T>
{
    public IEnumerable<Error> Errors { get; init; } = error;
    
    public Result<TR> Cast<TR>()
    {
        return new Failure<TR>(Errors);
    }
}

public readonly record struct Error(string Code, string Message);
```

Instead of checking for a `Bool` flag for validation, always use `Result<T>` to force the consumer to pattern match against `Success` or `Failure`. 

In the `ValidCreateBook` hide the default constructor and use a smart-constructor to enforce input validation. You can see an example at https://github.com/fabioluz/result-pattern/blob/main/Books/Create/Validation.cs

Now, you can use the Result interface to check if the `ValidCreateBook` exists. E.g.

```
if (result is Failure<BookOutput> failure)
{
    return Results.UnprocessableEntity(failure.Errors);
}

if (result is Success<BookOutput> success)
{
    return Results.Created($"/books/{success.Value.ID}", success.Value);
}
```

If you forget to match against the `Success` or `Failure` you won't be able to access the result `ValidCreateBook`. 

