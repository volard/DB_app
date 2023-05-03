using DB_app.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        Addresses = addresses;
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

    [NotMapped]
    public List<Address> Addresses
    {
        get
        {
            var _data = new List<Address>();
            foreach (var item in Locations) { _data.Add(item.Address);  }
            return _data;
        }
        set
        {
            var _data = new List<PharmacyLocation>();
            foreach (var item in value) { _data.Add(new PharmacyLocation(item));  }
            Locations = _data;
        }
    }

    public void AddAddress(Address address)
    {
        if (address == null)
        {
            throw new ArgumentNullException(nameof(address));
        }
        Locations.Add(new PharmacyLocation(address));
    }

    public void RemoveAddress(Address address)
    {
        if (address == null)
        {
            throw new ArgumentNullException(nameof(address));
        }
        var locationToRemove = Locations.FirstOrDefault(a => a.Address == address);
        if (locationToRemove == null)
        {
            throw new InvalidOperationException("Address doesn't exist in locations collection");
        }
        Locations.Remove(locationToRemove);
    }


    #endregion

    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not Pharmacy)
        {
            return false;
        }

        return
            (((Pharmacy)obj).Name == Name && ((Pharmacy)obj).IsActive == IsActive) ||
            ((Pharmacy)obj).Id == Id;
    }
}


public class PharmacyLocation
{
    public PharmacyLocation(Address address) { Address = address; }

    public PharmacyLocation() { }

    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public Address Address { get; set; }
}