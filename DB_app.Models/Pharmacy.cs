using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Models;
public class Pharmacy
{
    [Required]
    [Key]
    public int id_pharmacy { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int INN { get; set;}

    [Required]
    public int OGRN {get; set;}

    public List<Address> Addresses { get; set; } = new List<Address>();
}
