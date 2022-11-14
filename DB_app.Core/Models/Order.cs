using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Core.Models;
internal class Order
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int IdHospital { get; set; }

    [Required]
    public int IdPharmacy { get; set; }

    public List<Product> Products { get; set; } = new List<Product>();
}
