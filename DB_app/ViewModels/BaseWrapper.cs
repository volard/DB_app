using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.ViewModels;

public abstract partial class BaseWrapper<T> : ObservableValidator
{
    protected BaseWrapper(T? wrappedObj)
    {
        if (wrappedObj == null)
        {
            IsNew = true;
        }
        else { WrappedObjectData = wrappedObj; }
    }

    public T WrappedObjectData { get; set; }

    public override string ToString() =>
        $"wrapped data is {WrappedObjectData}";

    protected T? _backupData;

    /// <summary>
    /// Indicates about changes that is not synced with UI DataGrid
    /// </summary>
    [ObservableProperty] protected bool isModified = false;

    /// <summary>
    /// Indicate edit mode
    /// </summary>
    [ObservableProperty] protected bool isInEdit = false;

    /// <summary>
    /// Indicates whether its a new object
    /// </summary>
    [ObservableProperty] protected bool _isNew = false;


    public void BeginEdit()
    {
        this.IsInEdit = true;
        OnPropertyChanged(nameof(IsInEdit));
        Backup();
    }

    public void CancelEdit()
    {
        IsModified = false;
        IsInEdit = false;
    }

    public void EndEdit()
    {
        IsInEdit = false;
    }


    public void Backup() =>
        _backupData = WrappedObjectData;
}
