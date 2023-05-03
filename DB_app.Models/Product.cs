using System.ComponentModel.DataAnnotations;

namespace DB_app.Models;

public class Product
{
    #region Constructors

    public Product
        (
            Medicine medicine, 
            Pharmacy pharmacy, 
            double   price,
            int      quantity
        )
    {
        Medicine = medicine;
        Pharmacy = pharmacy;
        Price    = price;
        Quantity = quantity;
    }

    public Product() { }

    public Product
        (
            int id,
            Medicine medicine,
            Pharmacy pharmacy,
            double   price,
            int      quantity
        ) : this 
            (
                medicine, 
                pharmacy, 
                price, 
                quantity
            )
    {
        Id = id;
    }

    #endregion

    #region Properties

    [Required]
    [Key]
    public int      Id        { get; set; }

    [Required]
    public Medicine Medicine  { get; set; }

    [Required]
    public Pharmacy Pharmacy  { get; set; }

    [Required]
    public double   Price     { get; set; }

    [Required]
    public int      Quantity { get; set; } = 1;

    #endregion

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not Product)
        {
            return false;
        }

        return
            (
                ((Product)obj).Medicine == Medicine &&
                ((Product)obj).Pharmacy == Pharmacy
            ) || 
            ((Product)obj).Id == Id;
    }
}