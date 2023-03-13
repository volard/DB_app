using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB_app.Entities;

public class Order
{
    #region Constructors

    public Order
        (
            Hospital hospitalCustomer, 
            Address  shippingAddress, 
            Pharmacy pharmacySeller
        )
    {
        HospitalCustomer = hospitalCustomer;
        ShippingAddress  = shippingAddress;
    }

    public Order
        (
            int id,
            Hospital hospitalCustomer,
            Address  shippingAddress,
            Pharmacy pharmacySeller
        ) : this
            (
                hospitalCustomer,
                shippingAddress,
                pharmacySeller
            )
    {
        Id = id;
    }

    public Order() { }

    #endregion

    #region Properties

    [Required]
    [Key]
    public int             Id                  { get; set; }

    [Required]
    public Hospital        HospitalCustomer    { get; set; }

    [Required]
    public Address         ShippingAddress     { get; set; }

    [Required]
    public DateTime        DatePlaced          { get; set; } = DateTime.Now;

    public List<OrderItem> Items               { get; set; } = new();

    #endregion

   
    public override string ToString() => $"Order # {Id} - {Items.Count} medicines to {ShippingAddress}";

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not Order)
        {
            return false;
        }

        return
            ((Order)obj).Id == Id;
    }
}
