using DB_app.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;

namespace DB_app.Views;

public sealed partial class GreetingPage : Page
{
    public GreetingViewModel ViewModel { get; }

    public GreetingPage()
    {
        ViewModel = App.GetService<GreetingViewModel>();
        InitializeComponent();


        M1text = 
            "## Характеристика данных\n" +
            "Имеются `лекарства`, `аптеки` и `больницы`.\n\n" +
                "- `Лекарство` характеризуется *названием* и *видом* (таблетки, капсулы, ампулы и т.д.).\n" +
                "- `Аптека` — *номером*, *адресом*, *названием*.\n" +
                "- `Больница` — *номером*, *адресом*, *ФИО главного врача*.\n";

        M2text = 
            "## Взаимоссвязь данных\n" +
            "В `аптеках` имеются `лекарства` по определенной *цене за упаковку* и в определенном *количестве упаковок*. `Больницам` требуются `лекарства` в определенном *количестве упаковок*.";

        M3text =
            "## Выходные документы\n\n" +
                "- Список `лекарств`, имеющихся в определенной `аптеке`, упорядоченный по их *названиям*.\n" +
                "- Список `лекарств`, купленных определенной `больницей`, упорядоченный по *виду*, с указанием общего *количества упаковок* каждого *вида*.";
    }
    
    public string M1text;
    public string M2text;
    public string M3text;
}
