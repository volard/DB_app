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
    public int id_medicine  { get; set; }

    [Required]
    public String Name         { get; set; }

    [Required]
    public String Type         { get; set; }
}
