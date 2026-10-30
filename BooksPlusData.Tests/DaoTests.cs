using BooksPlusData.Data;
using BooksPlusData.Models;
using Microsoft.Data.SqlClient;

namespace BooksPlusData.Tests;

// These tests run your DAOs against your own BooksPlus database, so SQL Server
// must be running and BooksPlus created (see the course's sample databases).
// You don't need to change this file.
public static class TestDatabase
{
    // The BOOKSPLUS_CONNECTION environment variable, if set, points the tests
    // at another database; otherwise they use your local BooksPlus.
    public static string ConnectionString =>
        Environment.GetEnvironmentVariable("BOOKSPLUS_CONNECTION")
        ?? "Server=localhost;Database=BooksPlus;Integrated Security=true;TrustServerCertificate=true;";
}

[TestClass]
public class CategoryDaoTests
{
    private readonly CategoryDao dao = new CategoryDao(TestDatabase.ConnectionString);

    [TestMethod]
    public void GetAll_ReturnsEveryCategory_ByName()
    {
        List<Category> categories = dao.GetAll();

        Assert.HasCount(7, categories, "BooksPlus has 7 categories.");
        Assert.AreEqual("Business", categories[0].CategoryName, "Business comes first alphabetically.");
    }

    [TestMethod]
    [DataRow(1, "Software Development")]
    [DataRow(3, "Data Science")]
    [DataRow(7, "Business")]
    public void GetById_ReturnsTheCategory(int categoryId, string expected)
    {
        Category? category = dao.GetById(categoryId);

        Assert.IsNotNull(category, $"Category {categoryId} exists.");
        Assert.AreEqual(categoryId, category.CategoryId);
        Assert.AreEqual(expected, category.CategoryName);
    }

    [TestMethod]
    public void GetById_ReturnsNull_WhenThereIsNoSuchCategory()
    {
        Assert.IsNull(dao.GetById(999), "There's no category 999, so GetById returns null.");
    }

    [TestMethod]
    public void Add_InsertsTheCategory_AndReturnsItsNewId()
    {
        int newId = dao.Add(new Category { CategoryName = "Test Category", Description = "Added by a test" });

        try
        {
            Assert.IsGreaterThan(7, newId, "A new category gets a new id, after the 7 that exist.");
            Category? added = dao.GetById(newId);
            Assert.IsNotNull(added, "The new category can be read back by its id.");
            Assert.AreEqual("Test Category", added.CategoryName);
            Assert.AreEqual("Added by a test", added.Description);
        }
        finally
        {
            // Remove the test's category, so the database is as it was.
            using SqlConnection connection = new SqlConnection(TestDatabase.ConnectionString);
            connection.Open();
            using SqlCommand command = new SqlCommand("DELETE FROM Category WHERE CategoryId = @Id", connection);
            command.Parameters.AddWithValue("@Id", newId);
            command.ExecuteNonQuery();
        }
    }

    [TestMethod]
    public void Rename_GivesTheCategoryItsNewName()
    {
        int newId = dao.Add(new Category { CategoryName = "Test Category", Description = null });

        try
        {
            bool renamed = dao.Rename(newId, "Renamed Category");

            Assert.IsTrue(renamed, "Rename returns true when it renamed a category.");
            Assert.AreEqual("Renamed Category", dao.GetById(newId)?.CategoryName);
        }
        finally
        {
            using SqlConnection connection = new SqlConnection(TestDatabase.ConnectionString);
            connection.Open();
            using SqlCommand command = new SqlCommand("DELETE FROM Category WHERE CategoryId = @Id", connection);
            command.Parameters.AddWithValue("@Id", newId);
            command.ExecuteNonQuery();
        }
    }

    [TestMethod]
    public void Rename_ReturnsFalse_WhenThereIsNoSuchCategory()
    {
        Assert.IsFalse(dao.Rename(999, "Nothing"), "There's no category 999, so nothing was renamed.");
    }
}

[TestClass]
public class BookDaoTests
{
    private readonly BookDao dao = new BookDao(TestDatabase.ConnectionString);

    [TestMethod]
    [DataRow(5, "SQL Server 2012 Programming")]
    [DataRow(21, "Fast Track to MDX")]
    [DataRow(60, "Pro C# 7")]
    public void GetById_ReturnsTheBook(int bookId, string expected)
    {
        Book? book = dao.GetById(bookId);

        Assert.IsNotNull(book, $"Book {bookId} exists.");
        Assert.AreEqual(bookId, book.BookId);
        Assert.AreEqual(expected, book.Title);
    }

    [TestMethod]
    public void GetById_MapsEveryColumn_IncludingNulls()
    {
        Book? book = dao.GetById(21);

        Assert.IsNotNull(book);
        Assert.AreEqual(36.58m, book.Price, "Book 21 costs $36.58.");
        Assert.AreEqual(new DateTime(2016, 6, 11), book.PublishDate);
        Assert.IsNull(book.Summary, "Book 21 has no summary: the column is NULL, so the property is null.");
    }

    [TestMethod]
    public void GetById_ReturnsNull_WhenThereIsNoSuchBook()
    {
        Assert.IsNull(dao.GetById(9999));
    }

    [TestMethod]
    [DataRow(1, 46)]
    [DataRow(2, 24)]
    [DataRow(7, 11)]
    public void GetByCategory_ReturnsEveryBookInTheCategory(int categoryId, int expected)
    {
        List<Book> books = dao.GetByCategory(categoryId);

        Assert.HasCount(expected, books, $"Category {categoryId} has {expected} books.");
        Assert.IsTrue(books.All(b => b.CategoryId == categoryId), "Every book returned is in that category.");
    }

    [TestMethod]
    [DataRow("Program", 7)]
    [DataRow("Admin", 2)]
    [DataRow("VB", 1)]
    [DataRow("Knitting", 0)]
    public void SearchByTitle_FindsTheTextAnywhereInTheTitle(string text, int expected)
    {
        List<Book> books = dao.SearchByTitle(text);

        Assert.HasCount(expected, books, $"{expected} titles contain \"{text}\".");
    }

    [TestMethod]
    public void SearchByTitle_IsSafeFromSqlInjection()
    {
        List<Book> books = dao.SearchByTitle("x' OR 1 = 1 --");

        Assert.HasCount(0, books, "The search text must be a parameter: as plain text it matches no title.");
    }

    [TestMethod]
    public void GetById_TrimsTheIsbn()
    {
        Book? book = dao.GetById(21);

        Assert.IsNotNull(book);
        Assert.AreEqual("1846281741", book.Isbn, "ISBN is padded with spaces in the table; the model holds it without them.");
    }

    [TestMethod]
    public void GetByIds_ReturnsTheBooksWithThoseIds_ByTitle()
    {
        List<Book> books = dao.GetByIds(new List<int> { 5, 21, 9999 });

        Assert.HasCount(2, books, "Books 5 and 21 exist; there's no book 9999.");
        Assert.AreEqual("Fast Track to MDX", books[0].Title, "The books are in order of title.");
        Assert.AreEqual("SQL Server 2012 Programming", books[1].Title);
    }
}

[TestClass]
public class CustomerDaoTests
{
    private readonly CustomerDao dao = new CustomerDao(TestDatabase.ConnectionString);

    [TestMethod]
    public void GetById_ReturnsTheCustomer()
    {
        Customer? customer = dao.GetById(1);

        Assert.IsNotNull(customer, "Customer 1 exists.");
        Assert.AreEqual("Wanda", customer.FirstName);
        Assert.AreEqual("Paget", customer.LastName);
        Assert.AreEqual("wanda.paget@mail.com", customer.Email);
    }

    [TestMethod]
    public void GetById_ReturnsNull_WhenThereIsNoSuchCustomer()
    {
        Assert.IsNull(dao.GetById(9999));
    }

    [TestMethod]
    [DataRow("Chicago", 10)]
    [DataRow("Topeka", 9)]
    [DataRow("Nowhere", 0)]
    public void GetByCity_ReturnsEveryCustomerInTheCity(string city, int expected)
    {
        List<Customer> customers = dao.GetByCity(city);

        Assert.HasCount(expected, customers, $"{expected} customers live in {city}.");
    }

    [TestMethod]
    public void GetByLastNameStart_ReturnsMatchingCustomers_InOrder()
    {
        List<Customer> customers = dao.GetByLastNameStart("W");

        Assert.HasCount(27, customers, "27 customers' last names start with W.");
        Assert.IsTrue(customers.All(c => c.LastName.StartsWith('W')), "Every last name starts with W.");
        for (int i = 1; i < customers.Count; i++)
        {
            Assert.IsLessThanOrEqualTo(0, string.Compare(customers[i - 1].LastName, customers[i].LastName, StringComparison.Ordinal),
                "The customers are in order of last name.");
        }
    }
}
