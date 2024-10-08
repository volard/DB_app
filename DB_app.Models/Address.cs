﻿using System.ComponentModel.DataAnnotations;


namespace DB_app.Models;

public class Address
{

    #region Constructors

    public Address
        (
            string? city, 
            string? street, 
            string? building
        ) 
    {
        City     = city;
        Street   = street;
        Building = building;
    }

    public Address
        (
            int     id,
            string? city,
            string? street,
            string? building
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

    [Required]
    public string?  City         { get; set; }

    [Required]
    public string?   Street     { get; set; }

    [Required]
    public string?   Building   { get; set; }

    #endregion

    public override string ToString() => $"{City}; {Street}; {Building}";

    protected bool Equals(Address other)
    {
        return string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase) && 
               string.Equals(Street, other.Street, StringComparison.OrdinalIgnoreCase) && 
               string.Equals(Building, other.Building, StringComparison.OrdinalIgnoreCase);
    }

}