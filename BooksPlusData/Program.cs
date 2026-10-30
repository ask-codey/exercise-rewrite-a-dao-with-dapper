using BooksPlusData.Data;
using BooksPlusData.Models;
using Microsoft.Extensions.Configuration;

// Read the connection string from appsettings.json
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
string connectionString = configuration.GetConnectionString("BooksPlus")!;

// Try your DAOs here. The tests in BooksPlusData.Tests check every method.
CategoryDao categoryDao = new CategoryDao(connectionString);

Console.WriteLine("BooksPlus categories");
Console.WriteLine("--------------------");
foreach (Category category in categoryDao.GetAll())
{
    Console.WriteLine($"{category.CategoryId}. {category.CategoryName}");
}
