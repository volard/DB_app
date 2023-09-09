using DB_app.Models;
using DB_app.Services;
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

        //var lang = App.GetService<LocalizationService>().GetCurrentLanguageItem();
        LanguageItem lang = null;

        if (lang == null || lang.Tag == "en-US")
        {
            M1Text =
                "## Data characterization\n" +
                "There are `medicines', `pharmacies' and `hospitals'.\n\n" +
                "- `Medicine' is characterized by its *name* and *species* (tablets, capsules, ampoules, etc.).\n" +
                "- `Pharmacy' - by *number*, *address*, *name*.\n" +
                "- `Hospital` - by *number*, *address*, *name of chief physician*.\n";

            M2Text =
                "## Data interconnectivity\n" +
                "Pharmacies have `medicines' at a certain *price per pack* and in a certain *number of packs*. Hospitals require `medicines' at a certain *price per pack* and in a certain *number of packs*.";

            M3Text =
                "## Output documents\n\n" +
                "- A list of `medicines' available in a particular `pharmacy', organized by their *names*.\n" +
                "- A list of `medicines' purchased by a particular `hospital', organized by *species*, showing the total *number of packages* of each *species*.";
        }
        else if (lang.Tag == "ru-RU")
        {
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

        
    }
    
    public readonly string M1Text;
    public readonly string M2Text;
    public readonly string M3Text;
}
