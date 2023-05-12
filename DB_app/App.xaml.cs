using DB_app.Activation;
using DB_app.Contracts.Services;
using DB_app.Core.Contracts.Services;
using DB_app.Core.Services;
using DB_app.Models;
using DB_app.Repository.Services;
using DB_app.Services;
using DB_app.ViewModels;
using DB_app.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using DB_app.Repository.SQL;
using Windows.ApplicationModel;
using Windows.Storage;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;


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

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();

        var Builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();


        Host = Builder.UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // --------------------------------
            #region Default Activation Handler
            // --------------------------------
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            #endregion


            // --------------------------------
            #region Services
            // --------------------------------
            //register a class as the interface in the DI IServiceCollection container
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

            #endregion


            // --------------------------------
            #region Repository Services
            // --------------------------------

            services.AddSingleton<IFileService, FileService>();

            #endregion


            // --------------------------------
            #region Views and ViewModels
            // --------------------------------

            // === Medicine
            services.AddTransient<MedicineDetailsPage>();
            services.AddTransient<MedicinesGridViewModel>();
            services.AddTransient<MedicinesGridPage>();
            services.AddTransient<MedicineDetailsViewModel>();


            // === Order
            services.AddTransient<OrdersGridViewModel>();
            services.AddTransient<OrdersGridPage>();

            services.AddTransient<OrderDetailsViewModel>();
            services.AddTransient<OrderDetailsPage>();


            // === Pharmacy
            services.AddTransient<PharmacyReportGridViewModel>();
            services.AddTransient<PharmacyReportGridPage>();

            services.AddTransient<PharmacyDetailsViewModel>();
            services.AddTransient<PharmacyDetailsPage>();

            services.AddTransient<PharmaciesGridViewModel>();
            services.AddTransient<PharmaciesGridPage>();


            // === Product
            services.AddTransient<ProductsGridViewModel>();
            services.AddTransient<ProductsGridPage>();
            
            services.AddTransient<ProductDetailsPage>();
            services.AddTransient<ProductDetailsViewModel>();


            // === Address
            services.AddTransient<AddressesGridViewModel>();
            services.AddTransient<AddressesGridPage>();

            services.AddTransient<AddressDetailsViewModel>();
            services.AddTransient<AddressDetailsPage>();


            // === Hospital
            services.AddTransient<HospitalReportGridViewModel>();
            services.AddTransient<HospitalReportGridPage>();

            services.AddTransient<HospitalsGridViewModel>();
            services.AddTransient<HospitalsGridPage>();

            services.AddTransient<HospitalDetailsViewModel>();
            services.AddTransient<HospitalDetailsPage>();


            // === Misc
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>(); 

            services.AddTransient<GreetingViewModel>();
            services.AddTransient<GreetingPage>();

            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            #endregion


            // --------------------------------
            #region Configuration
            // --------------------------------
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));


            var connectionString    = context.Configuration.GetValue<string>("ConnectionStrings:Default");
            var optionsBuilder      = new DbContextOptionsBuilder<SQLContext>();
            string demoDatabasePath = Package.Current.InstalledLocation.Path   + @"\Assets\SQLiteDatabase.db";
            string databasePath     = ApplicationData.Current.LocalFolder.Path + @"\SQLiteDatabase.db";
            
            // C:\Users\volard\AppData\Local\Packages\143B4C51-0F44-4D70-BB38-F638E2F61F1B_7fv57v02n40z6\LocalState

            if (!File.Exists(databasePath) && File.Exists(demoDatabasePath))
            {
                File.Copy(demoDatabasePath, databasePath);
            }

            //var options = optionsBuilder.UseNpgsql(connectionString).Options;
            //optionsBuilder.EnableSensitiveDataLogging();
            //var options = optionsBuilder.EnableSensitiveDataLogging().UseSqlite("Data Source=" + databasePath).Options;
            var options = optionsBuilder.EnableSensitiveDataLogging().UseSqlite("Data Source=Database.db").Options;

            services.AddSingleton<IRepositoryControllerService>(new SQLControllerService(options));

            #endregion
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    /// <summary>
    /// Deals with XAML unhandled exceptions that can be handled by app code, 
    /// as forwarded from a native-level Windows Runtime error. Apps can mark the occurrence as handled in event data.
    /// </summary>
    /// <remarks>
    /// To get more details: <seealso href="https://learn.microsoft.com/en-us/uwp/api/windows.ui.xaml.application.unhandledexception?view=winrt-22621"/>
    /// </remarks>
    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        Debug.WriteLine("\n========= Microsoft.UI.Xaml.UnhandledException occured =========");
        Debug.WriteLine(e.Message);
        Debug.WriteLine("================================================================\n");
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.
    /// </summary>
    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await GetService<IActivationService>().ActivateAsync(args);
    }
}
