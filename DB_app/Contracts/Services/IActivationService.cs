namespace DB_app.Contracts.Services;

public interface IActivationService
{
    /**
     * The entry point for the application lifecycle event `OnLaunched`
     */
    Task ActivateAsync(object activationArgs);
}
