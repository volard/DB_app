using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DB_app.Models;

public class Medicine
{

    #region Constructors
    public Medicine (string name, string type)
    {
        Name = name;
        Type = type;
    }

    public Medicine( int id, string name,  string type)
        : this(name,  type)
    {
        Id = id;
    }

    public Medicine() { }

    #endregion

    [Required, Key]
    public int    Id           { get; set; }

    [Required, NotNull]
    public string Name         { get; set; }

    [Required, NotNull]
    public string Type         { get; set; }

    public override string ToString() => $"{Name} - {Type}";

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not Medicine)
        {
            return false;
        }

        return
            ((Medicine)obj).Name == Name ||
            ((Medicine)obj).Id == Id;
    }
}
