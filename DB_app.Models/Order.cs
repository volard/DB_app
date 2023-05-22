using System.ComponentModel.DataAnnotations;

namespace DB_app.Models;

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
            Pharmacy pharmacySeller,
            DateTime placed
        ) : this
            (
                hospitalCustomer,
                shippingAddress,
                pharmacySeller
            )
    {
        Id = id;
        DatePlaced = placed; 
    }


    public Order
    (
        int id,
        Hospital hospitalCustomer,
        Address shippingAddress,
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


    public Order
    (
        int id,
        Hospital hospitalCustomer,
        Address shippingAddress,
        Pharmacy pharmacySeller,
        List<OrderItem> orderItems) : this
            (
                id,
                hospitalCustomer,
                shippingAddress,
                pharmacySeller
            )
    {
        Items = orderItems;
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

    public List<OrderItem> Items               { get; set; } = new List<OrderItem>();

    #endregion

   
    public override string ToString() => $"Order # {Id} - {Items.Count} medicines to {ShippingAddress}";

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not Order order)
        {
            return false;
        }

        return
            order.Id == Id;
    }
}
