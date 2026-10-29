# Practice: Rewrite a DAO with Dapper

Practice code for the lesson **Practice: Rewrite a DAO with Dapper** in the C# Full-Stack Bootcamp on [ask-codey.com](https://www.ask-codey.com).

These are the BooksPlus DAOs from *Practice: Connecting an App*, with their method bodies taken out. You'll write each one again, this time with **Dapper**, and add two new methods. The tests are the same as before, plus tests for the new methods.

## Before you start

The tests run your code against your own BooksPlus database, so:

1. SQL Server must be running on your computer.
2. The BooksPlus database must exist. If you haven't created it yet, run `BooksPlus.sql` from the course's sample databases in SSMS.

The connection string is in `BooksPlusData/appsettings.json`. The project already has the Dapper package.

## What's here

- `BooksPlusData/Models`: `Category`, `Book` and `Customer`, unchanged.
- `BooksPlusData/Data`: the DAOs. `CategoryDao.GetAll()` is finished with Dapper, as an example to follow. Every other method throws `NotImplementedException` for now.
- `BooksPlusData.Tests`: tests for every DAO method. You don't need to change these.

## What to do

Write each method in `CategoryDao`, `BookDao` and `CustomerDao` with Dapper. Two are new:

- `CategoryDao.Rename(categoryId, newName)` renames a category, and returns whether it found one to rename.
- `BookDao.GetByIds(bookIds)` returns the books whose ids are in the list, in order of title.

Run the tests with **Test > Run All Tests** (or `dotnet test`). At first most of them fail: each method you finish turns its tests green.

## Tips

- Keep the SQL from your ADO.NET version. Dapper replaces the code around it, not the query.
- `Query<T>` for many rows, `QuerySingleOrDefault<T>` for one row or none, `QuerySingle<int>` for the new id, `Execute` for an `UPDATE`.
- Pass parameters as an anonymous object whose property names match the `@` names: `new { BookId = bookId }`.
- Dapper maps columns to properties by name. The `ISBN` column is padded with spaces: `RTRIM(ISBN) AS Isbn` trims it and names it to match the `Isbn` property.
- For `GetByIds`, write `WHERE BookId IN @BookIds`, with no parentheses: Dapper expands the list for you.
