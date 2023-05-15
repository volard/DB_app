using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DB_app.Models;

public class HospitalLocation : IEquatable<HospitalLocation>
{
    public HospitalLocation(Address address)
    { Address = address; }

    public HospitalLocation()
    { }

    [Key, Required]
    public int Id { get; set; }

    [Required, NotNull]
    public Hospital Hospital { get; set; }

    [Required, NotNull]
    public Address Address { get; set; }

    public bool Equals(HospitalLocation? other)
    {
        return other?.Address == Address;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as HospitalLocation);
    }
}