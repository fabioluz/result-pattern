namespace Example.Books;

// This is a specialized type for representing BookID.
// It should be used in all business logic that involves a book ID. E.g. GetBookById.
// The idea is that we never pass strings or int representing ids, so in this way we will never pass a BookID "by mistake"
// when we function was expecting a UserID or something.
public readonly record struct BookID
{
    private readonly string value;

    public BookID()
    {
        value = Guid.NewGuid().ToString();
    }

    public BookID(string id)
    {
        value = id;
    }

    public override string ToString()
    {
        return value;
    }

    public static implicit operator string(BookID bookId)
    {
        return bookId.value;
    }

    public static explicit operator BookID(string id)
    {
        return new BookID(id);
    }
}

// This is the main Book entity. It represents an existing book and should be always be used
// in the business logic when a Book is necessary.
// It is database agnostic and should not be used as database representation.
public record Book(BookID ID, string Name, string Author, int Year);