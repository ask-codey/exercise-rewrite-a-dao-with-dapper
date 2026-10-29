using BooksPlusData.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BooksPlusData.Data;

public class BookDao
{
    // The columns of Book, named as the Book model's properties. ISBN is a fixed-width
    // column, padded with spaces, so RTRIM takes them off.
    private const string Columns = "BookId, BookTypeId, CategoryId, PublisherId, Title, PublishDate, Summary, Price, RTRIM(ISBN) AS Isbn, UnitsInStock";

    private readonly string connectionString;

    public BookDao(string connectionString)
    {
        this.connectionString = connectionString;
    }

    // The book with this id, or null if there isn't one.
    public Book? GetById(int bookId)
    {
        throw new NotImplementedException();
    }

    // Every book in the category, in alphabetical order of title.
    public List<Book> GetByCategory(int categoryId)
    {
        throw new NotImplementedException();
    }

    // Every book whose title contains the text, anywhere, in alphabetical
    // order of title. SearchByTitle("Program") finds "SQL Server 2012 Programming".
    public List<Book> SearchByTitle(string text)
    {
        throw new NotImplementedException();
    }

    // The books with these ids, in alphabetical order of title. Ids with no book are skipped.
    public List<Book> GetByIds(List<int> bookIds)
    {
        throw new NotImplementedException();
    }
}
