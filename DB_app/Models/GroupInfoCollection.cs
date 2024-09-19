using System.Collections.ObjectModel;

namespace DB_app.Models;

public class GroupInfoCollection<T> : ObservableCollection<T>
{
    public object Key { get; set; }

    public new IEnumerator<T> GetEnumerator()
    {
        return (IEnumerator<T>)base.GetEnumerator();
    }
}