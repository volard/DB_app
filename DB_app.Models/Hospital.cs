using DB_app.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace DB_app.Entities;

public class Hospital
{

    #region Constructors

    public Hospital
        (
            string surename_main_doctor,
            string name_main_doctor,
            string middlename_main_doctor,
            List<Address> addresses
        )
    {
        Surename_main_doctor    = surename_main_doctor;
        Name_main_doctor        = name_main_doctor;
        Middlename_main_doctor  = middlename_main_doctor;
        Addresses               = addresses;
    }

    public Hospital
        (
            int    id,
            string surename_main_doctor,
            string name_main_doctor,
            string middlename_main_doctor
        )
    {
        Id                     = id;
        Surename_main_doctor   = surename_main_doctor;
        Name_main_doctor       = name_main_doctor;
        Middlename_main_doctor = middlename_main_doctor;
    }

    public Hospital
        (
            int id,
            string surename_main_doctor,
            string name_main_doctor,
            string middlename_main_doctor,
            List<Address> addresses
        ) : this
        (
            surename_main_doctor,
            name_main_doctor,
            middlename_main_doctor,
            addresses
        )
    {
        Id = id;
    }

    public Hospital() { }

    #endregion

    #region Properties

    [Required]
    [Key]
    public int           Id                        { get; set; }

    [Required]
    public string        Surename_main_doctor        { get; set; }

    [Required]
    public string        Name_main_doctor            { get; set; }

    [Required]
    public string        Middlename_main_doctor      { get; set; }

    [Required]
    public bool          IsActive                  { get; set; } = true;


    public List<HospitalLocation> Locations { get; set; } = new();

    [NotMapped]
    public List<Address> Addresses
    {
        get
        {
            var _data = new List<Address>();
            foreach (var item in Locations) { _data.Add(item.Address); }
            return _data;
        }
        set
        {
            var _data = new List<HospitalLocation>();
            foreach (var item in value) { _data.Add(new HospitalLocation(item)); }
            Locations = _data;
        }
    }

    public void AddAddress(Address address) 
    {   
        if (address == null)
        {
            throw new ArgumentNullException(nameof(address));
        }
        Locations.Add(new HospitalLocation(address));
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

    public override string ToString()
        => $"Hospital #{Id} - {Surename_main_doctor} {Name_main_doctor} {Middlename_main_doctor} maindoctor";

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not Hospital)
        {
            return false;
        }

        return 
           (
            (
                ((Hospital)obj).Surename_main_doctor == Surename_main_doctor && 
                ((Hospital)obj).Name_main_doctor == Name_main_doctor &&
                ((Hospital)obj).Middlename_main_doctor == Middlename_main_doctor
            ) && ((Hospital)obj).IsActive == IsActive) ||
            ((Hospital)obj).Id == Id ;
    }
}


public class HospitalLocation
{
    public HospitalLocation(Address address) { Address = address; }

    public HospitalLocation() { }

    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public Address Address { get; set; }
}
