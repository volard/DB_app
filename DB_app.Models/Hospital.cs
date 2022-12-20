using System.ComponentModel.DataAnnotations;

namespace DB_app.Models;
public class Hospital
{
    [Required]
    [Key]
    public int id_hospital { get; set; }

    [Required]
    public string Surename_main_doctor{get; set;}

    [Required]
    public string Name_main_doctor { get; set;}

    [Required]
    public string Middlename_main_doctor{ get; set;}

    [Required]
    public int INN { get; set; }

    [Required]
    public int OGRN{get; set;}

    public List<Address> Addresses { get; set; } = new List<Address>();
}
