using BooksPlusData.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BooksPlusData.Data;

public class CategoryDao
{
    private readonly string connectionString;

    public CategoryDao(string connectionString)
    {
        this.connectionString = connectionString;
    }

    // An example to follow: every category, in alphabetical order of name.
    public List<Category> GetAll()
    {
        string sql = """
            SELECT CategoryId, CategoryName, Description
            FROM Category
            ORDER BY CategoryName;
            """;

        using SqlConnection connection = new SqlConnection(connectionString);
        return connection.Query<Category>(sql).ToList();
    }

    // The category with this id, or null if there isn't one.
    public Category? GetById(int categoryId)
    {
        throw new NotImplementedException();
    }

    // Inserts a new category and returns the CategoryId it was given.
    // (The table also has a ModifiedDate column, which the database fills in itself.)
    public int Add(Category category)
    {
        throw new NotImplementedException();
    }

    // Gives the category a new name. Returns true if the category was renamed,
    // false if there's no category with that id.
    public bool Rename(int categoryId, string newName)
    {
        throw new NotImplementedException();
    }
}
