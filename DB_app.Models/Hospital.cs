using System.ComponentModel.DataAnnotations;

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
        List<HospitalLocation> data = new List<HospitalLocation>();
        foreach (Address item in addresses) { data.Add(new HospitalLocation(item)); }
        Locations = data;
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

    [Required]
    public string        Surename_main_doctor        { get; set; }

    [Required]
    public string        Name_main_doctor            { get; set; }

    [Required]
    public string        Middlename_main_doctor      { get; set; }

    [Required]
    public bool          IsActive                    { get; set; } = true;


    public List<HospitalLocation> Locations { get; set; } = new List<HospitalLocation>();

    #endregion

    public override string ToString()
        => $"{Id} - {Surename_main_doctor} {Name_main_doctor} {Middlename_main_doctor}";

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not Hospital hospital)
        {
            return false;
        }

        return 
           (
            (
                hospital.Surename_main_doctor == Surename_main_doctor && 
                hospital.Name_main_doctor == Name_main_doctor &&
                hospital.Middlename_main_doctor == Middlename_main_doctor
            ) && hospital.IsActive == IsActive) ||
            hospital.Id == Id ;
    }
}


