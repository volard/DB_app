using CommunityToolkit.Mvvm.Messaging.Messages;
using DB_app.Entities;
using DB_app.ViewModels;

namespace DB_app.Services.Messages;
public class AddRecordMessage<T> : ValueChangedMessage<T>
{
    public AddRecordMessage(T value) : base(value)
    {
    }
}

public class ShowRecordDetailsMessage<T> : ValueChangedMessage<T>
{
    public ShowRecordDetailsMessage(T value) : base(value)
    {
    }
}