using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DB_app.Services.Messages;

public class AddRecordMessage    <T> : ValueChangedMessage<T> { public AddRecordMessage(T value) : base(value){} }
public class DeleteRecordMessage <T> : ValueChangedMessage<T> { public DeleteRecordMessage(T value) : base(value) {} }