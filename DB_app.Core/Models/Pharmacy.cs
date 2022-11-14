using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Core.Models;
internal class Pharmacy
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int INN { get; set;}

    [Required]
    public int OGRN {get; set;}

    public List<Address> Addresses { get; set; } = new List<Address>();
}
