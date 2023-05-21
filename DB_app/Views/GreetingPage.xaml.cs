using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace DB_app.Views;

public sealed partial class GreetingPage : Page
{
    public GreetingViewModel ViewModel { get; }

    public GreetingPage()
    {
        ViewModel = App.GetService<GreetingViewModel>();
        InitializeComponent();


        M1Text = 
            "## Характеристика данных\n" +
            "Имеются `лекарства`, `аптеки` и `больницы`.\n\n" +
                "- `Лекарство` характеризуется *названием* и *видом* (таблетки, капсулы, ампулы и т.д.).\n" +
                "- `Аптека` — *номером*, *адресом*, *названием*.\n" +
                "- `Больница` — *номером*, *адресом*, *ФИО главного врача*.\n";

        M2Text = 
            "## Взаимоссвязь данных\n" +
            "В `аптеках` имеются `лекарства` по определенной *цене за упаковку* и в определенном *количестве упаковок*. `Больницам` требуются `лекарства` в определенном *количестве упаковок*.";

        M3Text =
            "## Выходные документы\n\n" +
                "- Список `лекарств`, имеющихся в определенной `аптеке`, упорядоченный по их *названиям*.\n" +
                "- Список `лекарств`, купленных определенной `больницей`, упорядоченный по *виду*, с указанием общего *количества упаковок* каждого *вида*.";
    }
    
    public readonly string M1Text;
    public readonly string M2Text;
    public readonly string M3Text;
}
