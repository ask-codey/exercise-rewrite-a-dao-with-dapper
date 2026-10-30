namespace BooksPlusData.Models;

// One row of the Book table. The nullable properties (with ?) are the
// columns that allow NULL.
public class Book
{
    public int BookId { get; set; }
    public int BookTypeId { get; set; }
    public int CategoryId { get; set; }
    public int? PublisherId { get; set; }
    public string Title { get; set; } = "";
    public DateTime PublishDate { get; set; }
    public string? Summary { get; set; }
    public decimal? Price { get; set; }
    public string? Isbn { get; set; }
    public int? UnitsInStock { get; set; }
}
