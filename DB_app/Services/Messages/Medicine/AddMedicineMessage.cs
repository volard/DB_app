using CommunityToolkit.Mvvm.Messaging.Messages;
using DB_app.ViewModels;

namespace DB_app.Services.Messages;

public class AddMedicineMessage : ValueChangedMessage<MedicineWrapper>
{
    public AddMedicineMessage(MedicineWrapper value) : base(value)
    {
    }
}
