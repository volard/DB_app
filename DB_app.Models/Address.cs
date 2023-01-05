
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DB_app.Models;

public class Address
{
    public Address(string city, string street, string building) {
        City = city;
        Street = street;
        Building = building;
    }

    public Address() { }

    [Required]
    [Key]
    public int id_address   { get; set; }

    [Required]
    public string City      { get; set; }

    [Required]
    public string Street    { get; set; }

    [Required]
    public string Building  { get; set; }

    public override string ToString() => $"Address : '{City}' city, '{Street}' streen,  {Building} building";
}

