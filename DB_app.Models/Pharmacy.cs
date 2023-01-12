using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Models;
public class Pharmacy
{
    public Pharmacy(
        string name,
        string inn,
        string ogrn)
    {
        Name = name;
        INN = inn;
        OGRN = ogrn;
    }

    public Pharmacy() { }


    [Required]
    [Key]
    public int id_pharmacy          { get; set; }

    [Required]
    public string Name              { get; set; }

    [Required]
    public string INN               { get; set;}

    [Required]
    public string OGRN              { get; set; }

    public List<Address> Addresses { get; set; } = new();

    public override string ToString() => $"Pharmacy '{Name}'";
}
