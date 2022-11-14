using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Core.Models;
internal class Product
{
    [Required]
    public int Id
    {
        get; set; 
    }
    [Required]
    public int IdMedicine
    {
        get; set; 
    }
    [Required]
    public int IdPharmacy
    {
        get; set;
    }
    [Required]
    public double Price
    {
        get; set;
    }
    [Required]
    public int Amount
    {
        get; set;
    }
}
