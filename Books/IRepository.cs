using Example.Books.Create;

namespace Example.Books;

public interface IBooksRepository
{
    // When inserting a book, we should always take a ValidCreteBook as an argument.
    // This guarantees the input was validated, and only validated books are being inserted.
    public Task<Book> Insert(ValidCreateBook book);

    // When getting a book, we are forced to pass a BookID.
    // This guarantees we will never pass UserID or String or Int by mistake.
    public Task<Book?> GetByID(BookID bookID);
}