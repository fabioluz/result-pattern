namespace Example.Books.Create;

// This represents the input for creating a new book.
// It an unstruted source of information and should only be used as a interface to the outside world.
// It should not take any validation rules. Validaiton will be done creating a ValidCreateBook.
public record CreateBook(string Name, string Author, int Year);

