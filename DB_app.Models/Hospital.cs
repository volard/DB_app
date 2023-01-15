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
            string inn,
            string ogrn,
            List<Address> addresses
        )
    {
        MainDoctorSurename    = surename_main_doctor;
        MainDoctorName        = name_main_doctor;
        MainDoctorMiddlename  = middlename_main_doctor;
        INN                   = inn;
        OGRN                  = ogrn;
        Addresses             = addresses;
    }

    public Hospital
        (
            int id,
            string surename_main_doctor,
            string name_main_doctor,
            string middlename_main_doctor,
            string inn,
            string ogrn,
            List<Address> addresses
        ) : this
        (
            surename_main_doctor,
            name_main_doctor,
            middlename_main_doctor,
            inn,
            ogrn,
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
    public string        MainDoctorSurename        { get; set; }

    [Required]
    public string        MainDoctorName            { get; set; }

    [Required]
    public string        MainDoctorMiddlename      { get; set; }

    [Required]
    public string        INN                       { get; set; }

    [Required]
    public string        OGRN                      { get; set; }

    public List<Address> Addresses                 { get; set; } = new List<Address>();

    #endregion

    public override string ToString()
        => $"Hospital managed by {MainDoctorSurename} {MainDoctorName} {MainDoctorMiddlename} maindoctor";
}
