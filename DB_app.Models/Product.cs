using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Models;
public class Product
{
    [Required]
    [Key]
    public int Id   { get; set; }

    [Required]
    public Medicine Medicine  { get; set; }

    [Required]
    public Pharmacy Pharmacy  { get; set; }

    [Required]
    public double Price       { get; set; }

    [Required]
    public int Quantity { get; set; } = 1;
}
