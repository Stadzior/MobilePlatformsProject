﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MobilePlatformsProject.Interfaces;
using MobilePlatformsProject.Models;
using MobilePlatformsProject.Rest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MobilePlatformsProject.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IRegisterCommands, INavigatableViewModel, ILoadable
    {
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly INavigationService _navigationService;
        
        public ObservableCollection<string> DownloadedFilesNames { get; set; }
        public ObservableCollection<Currency> Currencies { get; set; }
        public ObservableCollection<Currency> SelectedCurrencies { get; set; }
        //TODO Windows.Storage && REST
        private string _selectedFileName;
        public string SelectedFileName
        {
            get => _selectedFileName;
            set
            {
                Set(() => SelectedFileName, ref _selectedFileName, value);
            }
        }

        private DateTimeOffset? _date;
        public DateTimeOffset? Date
        {
            get => _date;
            set
            {
                Set(() => Date, ref _date, value);
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["MainPageDate"] = Date;
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(() => IsLoading, ref _isLoading, value);
        }

        public DateTimeOffset MaxDateTimeOffset => DateTimeOffset.Now;
        public DateTimeOffset MinDateTimeOffset => DateTimeOffset.Parse("2002-02-02");

        public ICommand NavigateToCurrencyHistoryCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand LoadDataFromFileCommand { get; set; }
        public ICommand SelectedCurrenciesChangedCommand { get; set; }
        public ICommand OpenPaneCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DateChangedCommand { get; set; }
        public ICommand DownloadDataCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Date = DateTimeOffset.Now;

            DownloadedFilesNames = new ObservableCollection<string>(
            Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json", SearchOption.TopDirectoryOnly)
                .Select(f => f.Substring(f.LastIndexOf(@"\")))
                .Select(f => f.Replace(@"\", ""))
                );

            Currencies = new ObservableCollection<Currency>();

            RegisterCommands();

            RetrieveAppState();
        }

        private void RetrieveAppState()
        {
            var localStorage = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localStorage.Values["LastOpenedPage"].ToString() == "MainPage")
            {
                //TODO Do loading lastly loaded file 
                var lastLoadedFileName = localStorage.Values["lastLoadedFile"];
            }
            else
            {
                _navigationService.NavigateTo(
                    "CurrencyHistory",
                    new
                    {
                        SelectedCurrencies = SelectedCurrencies,
                        DateFrom = localStorage.Values["CurrencyHistoryDateFrom"],
                        DateTo = localStorage.Values["CurrencyHistoryDateTo"],
                    });
            }
        }

        public void RegisterCommands()
        {
            NavigateToCurrencyHistoryCommand = new RelayCommand(() => _navigationService.NavigateTo(
                "CurrencyHistory",
                new
                {
                    SelectedCurrencies = SelectedCurrencies,
                    DateFrom = DateTimeOffset.Now.AddDays(-10),
                    DateTo = DateTimeOffset.Now
                }),() => SelectedCurrencies != null && SelectedCurrencies.Any());
            LoadDataFromFileCommand = new RelayCommand(() =>
            {
                var i = SelectedFileName;
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["lastLoadedFile"] = SelectedFileName;
            });
            BackCommand = new RelayCommand(() =>
            {
                _navigationService.GoBack();
            });
            SelectedCurrenciesChangedCommand = new RelayCommand<SelectionChangedEventArgs>(e =>
            {
                const int maximumSelectionCount = 5; 

                if (SelectedCurrencies == null)
                    SelectedCurrencies = new ObservableCollection<Currency>();

                foreach (var item in e.RemovedItems.Cast<Currency>())
                {
                    if (SelectedCurrencies.Contains(item))
                        SelectedCurrencies.Remove(item);
                }

                foreach (var item in e.AddedItems.Cast<Currency>())
                {
                    if (!SelectedCurrencies.Contains(item) && SelectedCurrencies.Count < maximumSelectionCount)
                        SelectedCurrencies.Add(item);
                }
            });
            DateChangedCommand = new RelayCommand<CalendarDatePickerDateChangedEventArgs>(e => Date = e.NewDate);
            DownloadDataCommand = new RelayCommand(async () => await DownloadDataAsync(Date));
        }

        private async Task DownloadDataAsync(DateTimeOffset? date)
        {
            List<Currency> grabbedCurrencies;
            IsLoading = true;
            try
            {
                if (date == null)
                    grabbedCurrencies = await NbpApiRequests.GetActualRates();
                else
                    grabbedCurrencies = await NbpApiRequests.GetRatesForDate(date ?? DateTimeOffset.Now);

                Currencies.Clear();
                foreach (var currency in grabbedCurrencies?.Where(x => x != null))
                    Currencies.Add(currency);
            }
            catch (ArgumentException e)
            {
                await new MessageDialog(e.Message).ShowAsync();
            }
            catch (Exception)
            {
                await new MessageDialog("Error occured while getting data.").ShowAsync();
            }

            IsLoading = false;
        }

        public void OnNavigateTo(object parameter = null)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["lastOpenedPage"] = "MainPage";
        }
    }
}
