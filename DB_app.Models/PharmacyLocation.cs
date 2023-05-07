using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DB_app.Models;

public class PharmacyLocation
{
    public PharmacyLocation(Address address) { Address = address; }

    public PharmacyLocation() { }

    [Key]
    [Required]
    public int Id { get; set; }

    [Required, NotNull]
    public Pharmacy Pharmacy { get; set; }

    [Required, NotNull]
    public Address Address { get; set; }
}