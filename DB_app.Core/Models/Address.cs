using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Core.Models;
internal class Address
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Street{ get; set;}

    [Required]
    public string Building{ get; set;}
}
