using CommunityToolkit.Mvvm.ComponentModel;
namespace DB_app.Views.Components;

public sealed partial class OrderItemDialog
{
    public readonly int Min = 0;
    public readonly int Max;
    private readonly int _initial;
    public OrderItemDialogViewModel ViewModel = new OrderItemDialogViewModel();
    public int Difference => ViewModel.Current - _initial;
    
    
    public OrderItemDialog(int max)
    {
        InitializeComponent();
        Max = max;
        _initial = Min;
        ViewModel.Current = _initial;
    }

    public OrderItemDialog(int max, int current)
    {
        InitializeComponent();
        Max = max;
        ViewModel.Current = current;
        _initial = current;
    }
}

public partial class OrderItemDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private int _current;
}