using Microsoft.UI.Dispatching;
using CommunityToolkit.WinUI;
using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Controls;
using DocumentFormat.OpenXml.Bibliography;
using Windows.System;

namespace DB_app.Helpers;

public static class CollectionsHelper
{

    public static bool IsDifferent<T>(List<T> lhs, List<T> rhs)
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
    
    /// <summary>
    /// Load data from DataSource using <see cref="getData"/> function and update UI using
    /// provided <see cref="dispatcherQueue"/>
    /// </summary>
    /// <param name="targetCollection">Collection in which data will be load</param>
    /// <param name="dispatcherQueue">Local dispatcher queue to access UI related stuff asynchronous</param>
    /// <param name="getData">function to get data</param>
    /// <typeparam name="T">Type of objects will be retrieved from DataSource</typeparam>
    public static async void LoadCollectionAsync<T>(
        ObservableCollection<T>   targetCollection,
        Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue, 
        Func<Task<IEnumerable<T>>> getData)
    {
        IEnumerable<T>? items = await getData();

        await dispatcherQueue.EnqueueAsync(() =>
        {
            targetCollection.Clear();
            foreach (T item in items)
            {
                targetCollection.Add(item);
            }

        });
    }

    public static async void RichLoadCollectionAsync<T>(
        ObservableCollection<T> targetCollection,
        Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue,
        Func<Task<IEnumerable<T>>> getData,
        Func<bool> getter)
    {
        await dispatcherQueue.EnqueueAsync(() =>
        {
            bool loading = getter();
            loading = true;
            targetCollection.Clear();
        });

        IEnumerable<T>? items = await Task.Run(getData);

        await dispatcherQueue.EnqueueAsync(() =>
        {
            targetCollection.Clear();
            foreach (T item in items)
            {
                targetCollection.Add(item);
            }
            bool loading = getter();
            loading = false;

        });
    }

}
