using System.ComponentModel.DataAnnotations;

namespace DB_app.Models;

public class PharmacyLocation
{
    public PharmacyLocation(Address address) { Address = address; }

    public PharmacyLocation() { }

    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public Pharmacy Pharmacy { get; set; }

    [Required]
    public Address Address { get; set; }
}