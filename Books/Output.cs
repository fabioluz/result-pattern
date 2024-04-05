namespace Example.Books;

// This is the BookOutput (some people would call it "view model", which is doesn't apply in this case since we don't any "view" involved).
// This should be a record constructed from the main Book entity.
// It should not contain any business logic or validation rules.
// This can be reused as the reusult of operations such as "Create Book, Update Book, or List Books"
public record BookOutput(string ID, string Name, string Author, int Year)
{
    public static BookOutput FromBook(Book book)
    {
        return new BookOutput(
            ID: book.ID,
            Name: book.Name,
            Author: book.Author,
            Year: book.Year
        );
    }
}