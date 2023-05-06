using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DB_app.Models;

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
        var _data = new List<HospitalLocation>();
        foreach (var item in addresses) { _data.Add(new HospitalLocation(item)); }
        Locations = _data;
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

    [Required, Key]
    public int           Id                          { get; set; }

    [Required, NotNull]
    public string        Surename_main_doctor        { get; set; }

    [Required, NotNull]
    public string        Name_main_doctor            { get; set; }

    [Required, NotNull]
    public string        Middlename_main_doctor      { get; set; }

    [Required, NotNull]
    public bool          IsActive                    { get; set; } = true;


    public List<HospitalLocation> Locations { get; set; } = new();

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

    [Key, Required]
    public int Id { get; set; }

    [Required, NotNull]
    public Hospital Hospital { get; set; }

    [Required, NotNull]
    public Address Address { get; set; }
}
