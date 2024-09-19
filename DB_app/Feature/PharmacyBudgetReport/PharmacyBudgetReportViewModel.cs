using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace DB_app.ViewModels;

public partial class PharmacyBudgetReportViewModel: ObservableRecipient
{

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();


    public ObservableCollection<BudgetPharmacyItem> Source = new();

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isLoading = false;

    public async Task LoadReport()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsLoading = true;
        });


        //IEnumerable<Product>? items = await Task.Run(_repositoryControllerService.Products.GetAsync);

        List<BudgetPharmacyItem> data = await Task.Run(async () =>
        {
            List<BudgetPharmacyItem> output = new();
            IEnumerable<Pharmacy> pharmacies = await _repositoryControllerService.Pharmacies.GetAsync();

            foreach (var pharmacy in pharmacies)
            {
                var budget = await _repositoryControllerService.Pharmacies.GetPharmacyBudget(pharmacy.Id);
                BudgetPharmacyItem temp = new(pharmacy, budget);
                output.Add(temp);
            }

            return output;
        });



        await _dispatcherQueue.EnqueueAsync(() =>
        {
            Source.Clear();
            foreach (var item in data)
            {
                Source.Add(item);
            }

            IsLoading = false;
        });
    }



    
}

public class BudgetPharmacyItem
    {
        public Pharmacy mPharmacy { get; set; }
        public double Budget { get; set; }

        public BudgetPharmacyItem(Pharmacy mPharmacy, double budget)
        {
            this.mPharmacy = mPharmacy;
            Budget = budget;
        }
    }
