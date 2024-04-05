using Example.Books.Create;
using Example.Result;

namespace Example.Books;

public static class Handlers
{
    public static async Task<IResult> CreateBook(BooksService service, CreateBook input)
    {
        var result = await service.CreateBook(input);

        if (result is Failure<BookOutput> failure)
        {
            return Results.UnprocessableEntity(failure.Errors);
        }
        
        if (result is Success<BookOutput> success)
        {
            return Results.Created($"/books/{success.Value.ID}", success.Value);
        }

        // Result is neither Failure nor Success. This should never happen.
        throw new NotImplementedException();
    }
}