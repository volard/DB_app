using System.ComponentModel.DataAnnotations;

namespace DB_app.Models;

public class Pharmacy
{
    #region Constructors

    public Pharmacy
        (
            string        name,
            string        inn,
            string        ogrn,
            List<Address> addresses
        )
    {
        Name      = name;
        INN       = inn;
        OGRN      = ogrn;
        Addresses = addresses;
    }

    public Pharmacy
        (
            int           id,
            string        name,
            string        inn,
            string        ogrn,
            List<Address> addresses
        ) : this
            (
                name,
                inn,
                ogrn,
                addresses
            )
    {
        Id = id;
    }

    public Pharmacy() { }

    #endregion

    #region Properties

    [Required]
    [Key]
    public int           Id         { get; set; }

    [Required]
    public string        Name       { get; set; }

    [Required]
    public string        INN        { get; set; }

    [Required]
    public string        OGRN       { get; set; }

    public List<Address> Addresses  { get; set; } = new();

    #endregion

    public override string ToString() => $"Pharmacy '{Name}'";
}
