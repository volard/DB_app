using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DB_app.Models;

public class HospitalLocation
{
    public HospitalLocation(Address address) { Address = address; }

    public HospitalLocation() { }

    [Key, Required]
    public int Id { get; set; }

    [Required, NotNull]
    public Hospital Hospital { get; set; }

    [Required, NotNull]
    public Address Address { get; set; }
}

