using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DB_app.Models;
public class Order
{
    [Required]
    [Key]
    public int Id                   { get; set; }

    [Required]
    public Hospital HospitalCustomer { get; set; }

    [Required]
    public Address ShippingAddress { get; set; }

    [Required]
    public Pharmacy PharmacySeller { get; set; }

    [Required]
    public DateTime DatePlaced { get; set; } = DateTime.Now;

    public List<OrderItem> Items { get; set; } = new();

    public override string ToString() => $"Order # {Id} - {Items.Count} medicines to {ShippingAddress}";
}
