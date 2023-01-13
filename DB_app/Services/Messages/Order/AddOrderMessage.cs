using CommunityToolkit.Mvvm.Messaging.Messages;
using DB_app.ViewModels;

namespace DB_app.Services.Messages;

public class AddOrderMessage : ValueChangedMessage<OrderWrapper>
{
    public AddOrderMessage(OrderWrapper value) : base(value)
    {
    }
}
