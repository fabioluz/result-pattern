
using Example.Books;
using Example.Books.Create;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMvcCore()
    .AddJsonOptions(options =>
        options
            .JsonSerializerOptions
            .PropertyNameCaseInsensitive = true
    );

builder.Services
    .AddSingleton<IBooksRepository, MockBookRepository>()
    .AddSingleton<BooksService>();

var app = builder.Build();

app.MapPost("/books", Handlers.CreateBook);
app.Run();

public class MockBookRepository : IBooksRepository
{
    public Task<Book?> GetByID(BookID bookID)
    {
        throw new NotImplementedException();
    }

    public Task<Book> Insert(ValidCreateBook book)
    {
        var bookID = new BookID();
        return Task.FromResult(new Book(bookID, book.Name, book.Author, book.Year));
    }
}