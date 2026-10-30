namespace BooksPlusData.Models;

// One row of the Customer table.
public class Customer
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
}
