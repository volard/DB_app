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
    public int id_product
    {
        get; set; 
    }
    [Required]
    public int id_medicine
    {
        get; set; 
    }
    [Required]
    public int id_pharmacy
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
