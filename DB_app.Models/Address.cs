
using System.ComponentModel.DataAnnotations;

namespace DB_app.Models;

public class Address
{
    [Required]
    public int id_address
    {
        get; set;
    }

    [Required]
    public string City
    {
        get; set;
    }

    [Required]
    public string Street
    {
        get; set;
    }

    [Required]
    public string Building
    {
        get; set;
    }
}

