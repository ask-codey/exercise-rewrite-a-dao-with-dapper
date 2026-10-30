using BooksPlusData.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BooksPlusData.Data;

public class CustomerDao
{
    private readonly string connectionString;

    public CustomerDao(string connectionString)
    {
        this.connectionString = connectionString;
    }

    // The customer with this id, or null if there isn't one.
    public Customer? GetById(int customerId)
    {
        throw new NotImplementedException();
    }

    // Every customer in the city, in order of last name, then first name.
    public List<Customer> GetByCity(string city)
    {
        throw new NotImplementedException();
    }

    // Every customer whose last name starts with the letters given,
    // in order of last name, then first name.
    public List<Customer> GetByLastNameStart(string letters)
    {
        throw new NotImplementedException();
    }
}
