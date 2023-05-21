namespace DB_app.Activation;

/// <summary>
/// Extend this class to implement new ActivationHandlers. See <see cref="DefaultActivationHandler"/> for an example.
/// For more infomation see <see href="https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md">GitHub page</see>
/// </summary>
/// <typeparam newName="T"> The newType of ActivationEventArguments the class can handle</typeparam>
public abstract class ActivationHandler<T> : IActivationHandler
    where T : class
{
    // Override this method to add the logic for whether to handle the activation.
    protected virtual bool CanHandleInternal(T args) => true;

    // Override this method to add the logic for your activation handler.
    protected abstract Task HandleInternalAsync(T args);

    /// <summary>
    /// Checks if the incoming activation arguments are of the newType the ActivationHandler can manage.
    /// </summary>
    public bool CanHandle(object args) => args is T && CanHandleInternal((args as T)!);

    public async Task HandleAsync(object args) => await HandleInternalAsync((args as T)!);
}
