using CommunityToolkit.Mvvm.Messaging.Messages;
using DB_app.ViewModels;

namespace DB_app.Services.Messages;

public class ShowOrderDetailsMessage : ValueChangedMessage<OrderWrapper>
{
    public ShowOrderDetailsMessage(OrderWrapper value) : base(value)
    {
    }
}