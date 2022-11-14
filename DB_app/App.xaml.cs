using DB_app.Activation;
using DB_app.Contracts.Services;
using DB_app.Core.Contracts.Services;
using DB_app.Core.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Services;
using DB_app.ViewModels;
using DB_app.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Windows.Services.Maps;

namespace DB_app;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    /// </summary>
    public IHost Host { get; }

    /// <summary>
    /// Get registered service othervise throws an ArgumentException.
    /// </summary>
    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    /// <summary>
    /// Gets main App Window
    /// </summary>
    public static WindowEx MainWindow { get; } = new MainWindow();

    public static IDataAccessService? DataAccessServiceObject;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();

        var Builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();

        Host = Builder.UseContentRoot(AppContext.BaseDirectory).
        // Action<HostBuilderContext, IServiceCollection>
        ConfigureServices((context, services) =>
        {
            // --------------------------------
            // Default Activation Handler
            // --------------------------------
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();


            // --------------------------------
            // Services
            // --------------------------------
            //register a class as the interface in the DI IServiceCollection container
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();


            // --------------------------------
            // Repository Services
            // --------------------------------



            services.AddSingleton<IFileService, FileService>();


            // --------------------------------
            // Views and ViewModels
            // --------------------------------
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<HospitalReportGridViewModel>();
            services.AddTransient<HospitalReportGridPage>();

            services.AddTransient<PharmacyReportGridViewModel>();
            services.AddTransient<PharmacyReportGridPage>();

            services.AddTransient<ProductsGridViewModel>();
            services.AddTransient<ProductsGridPage>();

            services.AddTransient<PharmaciesGridViewModel>();
            services.AddTransient<PharmaciesGridPage>();

            services.AddTransient<OrdersGridViewModel>();
            services.AddTransient<OrdersGridPage>();

            services.AddTransient<MedicinesGridViewModel>();
            services.AddTransient<MedicinesGridPage>();

            services.AddTransient<HospitalsGridViewModel>();
            services.AddTransient<HospitalsGridPage>();

            services.AddTransient<GreetingViewModel>();
            services.AddTransient<GreetingPage>();

            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();


            // --------------------------------
            // Configuration
            // --------------------------------
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));

            var connectionString = context.Configuration.GetValue<string>("ConnectionStrings:Default");

            var optionsBuilder = new DbContextOptionsBuilder<DataAccessService>();

            var options = optionsBuilder.UseNpgsql(connectionString).Options;

            services.AddSingleton<IDataAccessService>(new DataAccessService(options));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.
    /// </summary>
    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
