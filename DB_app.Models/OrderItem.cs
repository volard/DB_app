using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DB_app.Models;

/// <summary>
/// Represents an order item (product + quantity).
/// </summary>
public class OrderItem
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public Order RepresentingOrder { get; set; }

    [Required]
    public Product UnderlyingProduct { get; set; }

    public int Quantity { get; set; } = 1;
}
