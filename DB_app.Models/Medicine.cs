using System.ComponentModel.DataAnnotations;

namespace DB_app.Models;

public class Medicine
{
    public Medicine
        (
            string name, 
            string type
        )
    {
        Name = name;
        Type = type;
    }

    public Medicine
        (
            int id, 
            string name, 
            string type
        )
        : this
            (
                name, 
                type
            )
    {
        Id = id;
    }

    public Medicine() { }

    [Key]
    public int    Id           { get; set; }

    [Required]
    public string Name         { get; set; }

    [Required]
    public string Type         { get; set; }

    public override string ToString() => $"Medicine '{Name}' under '{Type}' type";
}
