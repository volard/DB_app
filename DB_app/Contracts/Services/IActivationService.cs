namespace DB_app.Contracts.Services;


/// <summary>
/// The ActivationService is in charge of handling the application's initialization and activation.
/// </summary>
public interface IActivationService
{


    /// <summary>
    /// The entry point for the application lifecycle event `OnLaunched`
    /// </summary>
    /// <param newName="activationArgs"></param>
    /// <returns></returns>
    Task ActivateAsync(object activationArgs);
}
