using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DB_app.Models;

public class Address
{

    #region Constructors

    public Address
        (
            string city, 
            string street, 
            string building
        ) 
    {
        City     = city;
        Street   = street;
        Building = building;
    }

    public Address
        (
            int    id,
            string city,
            string street,
            string building
        ) : 
        this 
        (
            city,
            street,
            building
        )
    {
        Id = id;
    }


    public Address() { }

    #endregion

    #region Properties

    [Key, Required]
    public int      Id          { get; set; }

    [Required, NotNull]
    public string? City         { get; set; }

    [Required, NotNull]
    public string?   Street     { get; set; }

    [Required, NotNull]
    public string?   Building   { get; set; }

    #endregion

    public override string ToString() => $"{City}; {Street}; {Building}";

    public override bool Equals(object? obj)
    {
        if (obj is not Address other) return false;

        return other.City == City && other.Street == Street && other.Building == Building;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}