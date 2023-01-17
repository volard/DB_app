﻿using System.ComponentModel.DataAnnotations;

namespace DB_app.Entities;

/// <summary>
/// Represents an order item (product + quantity).
/// </summary>
public class OrderItem
{
    #region Constructors

    public OrderItem
        (
            Order   representingOrder, 
            Product product, 
            int     quantity
        )
    {
        RepresentingOrder = representingOrder;
        Product           = product;
        Quantity          = quantity;
    }

    public OrderItem
        (
            int     id,
            Order   representingOrder,
            Product product,
            int     quantity
        ) : this
            (
                representingOrder,
                product,
                quantity
            )
    {
        Id = id;
    }

    public OrderItem() { }

    #endregion

    #region Properties

    [Key]
    [Required]
    public int     Id                 { get; set; }

    [Required]
    public Order   RepresentingOrder  { get; set; }

    [Required]
    public Product Product            { get; set; }

    [Required]
    public int     Quantity           { get; set; }

    [Required]
    public double  Price              { get; set; }

    #endregion

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not OrderItem)
        {
            return false;
        }

        return
            ((OrderItem)obj).Id == Id;
    }
}
