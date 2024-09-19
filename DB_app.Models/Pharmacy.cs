using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DB_app.Models;

public class Pharmacy
{
    #region Constructors

    public Pharmacy
        (
            string        name,
            List<Address> addresses
        )
    {
        Name      = name;
        List<PharmacyLocation> data = new List<PharmacyLocation>();
        foreach (Address item in addresses) { data.Add(new PharmacyLocation(item)); }
        Locations = data;
    }

    public Pharmacy
        (
            int id,
            string name
        )
    {
        Name = name;
        Id = id;
    }

    public Pharmacy
        (
            int           id,
            string        name,
            List<Address> addresses
        ) : this
            (
                name,
                addresses
            )
    {
        Id = id;
    }

    public Pharmacy() { }

    #endregion

    #region Properties

    [Required]
    [Key]
    public int           Id         { get; set; }

    [Required]
    public string        Name       { get; set; }

    [Required]
    public bool          IsActive   { get; set; } = true;

    public List<PharmacyLocation> Locations { get; set; } = new();

    #endregion

    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not Pharmacy pharmacy)
        {
            return false;
        }

        return
            (pharmacy.Name == Name && pharmacy.IsActive == IsActive) ||
            pharmacy.Id == Id;
    }
}


