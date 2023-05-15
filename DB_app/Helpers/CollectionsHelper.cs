namespace DB_app.Helpers;

public class CollectionsHelper
{

    static public bool IsDifferent<T>(List<T> lhs, List<T> rhs)
    {
        if (lhs.Count != rhs.Count) return true;
        for (int i = 0; i < lhs.Count; i++)
        {
            if (lhs.ElementAt(i) == null && rhs.ElementAt(i) == null) continue;

            if (
                    (lhs.ElementAt(i) == null && rhs.ElementAt(i) != null) ||
                    (lhs.ElementAt(i) != null && rhs.ElementAt(i) == null)
               ) return true;

            if (!lhs.ElementAt(i)!.Equals(rhs.ElementAt(i))) return true;
        }
        return false;
    }
}
