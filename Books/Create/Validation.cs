using System.ComponentModel.DataAnnotations;
using Example.Result;

namespace Example.Books.Create;

// This represents a validated input that can be used to create a book.
// It must be created from a CreateBook input object.
// It must be used in the Repository interface for creating a book.
// The goal is to guarantee that only validate information gets to the database.
// We should not trust on "if (obj.IsValid)" because this can be easily forgotten in the implementation
// and silently passed through code review. 
public record ValidCreateBook
{
    [Required(ErrorMessage = CreateBookErrors.NameIsRequired)]
    public required string Name { get; init; }

    [Required(ErrorMessage = CreateBookErrors.AuthorIsRequired)]
    public required string Author { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = CreateBookErrors.AuthorIsRequired)]
    public required int Year { get; init; }

    // ValidCreateBook can only be created using the FromInput().
    private ValidCreateBook() { }

    // That's the only way to create a ValidCreateBook.
    // The implementer will have to make sure the information was properly validated.
    // The only way to check if the validation succeeded, is by pattern matching the Result.
    public static Result<ValidCreateBook> FromInput(CreateBook input)
    {
        var createBook = new ValidCreateBook
        {
            Name = input.Name,
            Author = input.Author,
            Year = input.Year,
        };

        var context = new ValidationContext(createBook);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(createBook, context, results, true);
        if (isValid)
        {
            return new Success<ValidCreateBook>(createBook);
        }

        var errors = results.Select(x => CreateBookErrors.CreateError(x.ErrorMessage ?? ""));
        return new Failure<ValidCreateBook>(errors);
    }
}

// This represents the error codes generated during the validation phase.
// We should not use human-readable messages for errors because we don't know which language the UI is using.
// We should always return error codes and the UI is responsible for transalating those codes into a human-readable language.
// Since Data Annotations don't support codes out-of-the-box, we are using the ErrorMessage as a placeholder 
public static class CreateBookErrors
{
    public const string NameIsRequired = "name_is_required";

    public const string AuthorIsRequired = "author_is_required";

    public const string YearIsRequired = "year_is_required";

    // This generate a human-readable message within the error code.
    // This is useful when the UI has no idea how to translate it, or in cases the translation was not yet implemented.
    public static Error CreateError(string errorCode)
    {
        var errorMessage = errorCode switch
        {
            NameIsRequired => "Name is required.",
            AuthorIsRequired => "Author is required.",
            YearIsRequired => "Year is required.",
            _ => "Unknown error."
        };

        return new Error(errorCode, errorMessage);
    }
}