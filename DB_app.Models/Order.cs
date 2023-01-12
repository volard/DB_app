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
    public int Id     { get; set; }

    [Required]
    public Hospital BuyingHospital { get; set; }

    [Required]
    public Pharmacy SellingPharmacy { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}
