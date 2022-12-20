using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Models;
public class Order
{
    [Required]
    [Key]
    public int id_order     { get; set; }

    [Required]
    public int id_hospital { get; set; }

    [Required]
    public int id_pharmacy { get; set; }

    public List<Product> Products { get; set; } = new List<Product>();
}
