using Example.Result;

namespace Example.Books.Create;

public partial class BooksService(IBooksRepository booksRepository)
{
    public async Task<Result<BookOutput>> CreateBook(CreateBook input)
    {
        var validationResult = ValidCreateBook.FromInput(input);

        if (validationResult is Failure<ValidCreateBook> failure)
        {
            return failure.Cast<BookOutput>();
        }

        if (validationResult is Success<ValidCreateBook> success)
        {
            var book = await booksRepository.Insert(success.Value);
            var output = BookOutput.FromBook(book);
            return new Success<BookOutput>(output);
        }

        throw new NotImplementedException();
    }
}