using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace DB_app.Entities;

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
        ) : this 
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

    [Required]
    [Key]
    public int      Id        { get; set; }

    [Required]
    public string   City      { get; set; }

    [Required]
    public string   Street    { get; set; }

    [Required]
    public string   Building  { get; set; }

    #endregion

    public override string ToString() => $"{City}; {Street}; {Building}";

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not Address)
        {
            return false;
        }

        return ((Address)obj).City == City && ((Address)obj).Street == Street && ((Address)obj).Building == Building;
    }
}