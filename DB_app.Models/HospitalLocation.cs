using System.ComponentModel.DataAnnotations;

namespace DB_app.Models;

public class HospitalLocation : IEquatable<HospitalLocation>
{
    public HospitalLocation(Address address)
    { Address = address; }

    public HospitalLocation()
    { }

    [Key, Required]
    public int Id { get; set; }

    [Required]
    public Hospital Hospital { get; set; }

    [Required]
    public Address Address { get; set; }

    public bool Equals(HospitalLocation? other)
    {
        return Equals(other?.Address, Address);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as HospitalLocation);
    }
}