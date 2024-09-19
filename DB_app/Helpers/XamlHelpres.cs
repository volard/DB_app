using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
namespace DB_app.Helpers;

static public class XamlHelpres
{
    /// <summary>
    ///  Finds the DataGridRow that was clicked I traverse the visual tree
    ///  Thanks to https://stackoverflow.com/questions/70429745/how-to-know-when-a-datagridrow-is-clicked
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="childElement"></param>
    /// <returns></returns>
    public static T? FindParent<T>(DependencyObject childElement) where T : Control
    {
        DependencyObject currentElement = childElement;

        while (currentElement != null)
        {
            if (currentElement is T matchingElement)
            {
                return matchingElement;
            }

            currentElement = VisualTreeHelper.GetParent(currentElement);
        }

        return null;
    }
}
