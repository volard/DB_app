using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Models;
public class Medicine
{
    [Key]
    public int id_medicine  { get; private set; }

    [Required]
    public string Name         { get; set; }

    [Required]
    public string Type         { get; set; }

    public override string ToString() => $"Medicine '{Name}' under '{Type}' type";
}
