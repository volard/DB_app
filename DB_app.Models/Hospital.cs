using System.ComponentModel.DataAnnotations;

namespace DB_app.Models;
public class Hospital
{
    public Hospital(
        string surename_main_doctor, 
        string name_main_doctor, 
        string middlename_main_doctor,
        string inn,
        string ogrn)
    {
        Surename_main_doctor    = surename_main_doctor;
        Name_main_doctor        = name_main_doctor;
        Middlename_main_doctor  = middlename_main_doctor;
        INN                     = inn;
        OGRN                    = ogrn;
    }

    public Hospital() { }

    [Required]
    [Key]
    public int Id                  { get; set; }

    [Required]
    public string Surename_main_doctor      { get; set; }

    [Required]
    public string Name_main_doctor          { get; set; }

    [Required]
    public string Middlename_main_doctor    { get; set; }

    [Required]
    public string INN                       { get; set; }

    [Required]
    public string OGRN                      { get; set; }

    public List<Address> Addresses          { get; set; } = new List<Address>();

    public override string ToString() 
        => $"Hospital managed by {Surename_main_doctor} {Name_main_doctor} {Middlename_main_doctor} maindoctor";
}
