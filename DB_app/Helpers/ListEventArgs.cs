using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Helpers;

public class ListEventArgs : EventArgs
{
    public List<string> Data { get; set; }
    public ListEventArgs(List<string> data)
    {
        Data = data;
    }
}
