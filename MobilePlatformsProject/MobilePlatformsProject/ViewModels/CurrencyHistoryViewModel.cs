using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MobilePlatformsProject.Interfaces;
using MobilePlatformsProject.Models;
using MobilePlatformsProject.Rest;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace MobilePlatformsProject.ViewModels
{
    public class CurrencyHistoryViewModel : ViewModelBase, IRegisterCommands, INavigatableViewModel, ILoadable
    {
        private INavigationService _navigationService;

        private DateTimeOffset? _dateFrom;
        public DateTimeOffset? DateFrom
        {
            get => _dateFrom;
            set
            {
                Set(() => DateFrom, ref _dateFrom, value);
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["CurrencyHistoryDateFrom"] = DateFrom;
                Task.Factory.StartNew(async () =>
                {
                    await DownloadDataAsync(SelectedCurrencies, DateFrom, DateTo);
                });
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(() => IsLoading, ref _isLoading, value);
        }

        private DateTimeOffset? _dateTo;
        public DateTimeOffset? DateTo
        {
            get => _dateTo;
            set
            {
                Set(() => DateTo, ref _dateTo, value);
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["CurrencyHistoryDateTo"] = DateTo;
                Task.Factory.StartNew(async () =>
                {
                    await DownloadDataAsync(SelectedCurrencies, DateFrom, DateTo);
                });
            }
        }

        public DateTimeOffset MaxDateTimeOffset => DateTimeOffset.Now;
        public DateTimeOffset MinDateTimeOffset => DateTimeOffset.Parse("2002-02-02");

        private ObservableCollection<Currency> _selectedCurrencies;
        public ObservableCollection<Currency> SelectedCurrencies
        {
            get => _selectedCurrencies;
            set => Set(() => SelectedCurrencies, ref _selectedCurrencies, value);
        }

        public ICommand BackCommand { get; set; }
        public ICommand ManipulationStartedCommand { get; set; }
        public ICommand ManipulationCompletedCommand { get; set; }
        public ICommand DateFromChangedCommand { get; set; }
        public ICommand DateToChangedCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        private Windows.Foundation.Point _fingerPosition;

        public CurrencyHistoryViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            RegisterCommands();
        }

        public void RegisterCommands()
        {
            BackCommand = new RelayCommand(() =>
            {
                _navigationService.GoBack();
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["lastOpenedPage"] = "MainPage";
            });

            ManipulationStartedCommand = new RelayCommand<ManipulationStartedRoutedEventArgs>(e => _fingerPosition = e.Position);
            ManipulationCompletedCommand = new RelayCommand<ManipulationCompletedRoutedEventArgs>(e =>
            {
                //if (_fingerPosition.X < e.Position.X)
                //causes win32 unhandled exception _navigationService.NavigateTo("MainPage");
            });

            DateFromChangedCommand = new RelayCommand<CalendarDatePickerDateChangedEventArgs>(e => DateFrom = e.NewDate);
            DateToChangedCommand = new RelayCommand<CalendarDatePickerDateChangedEventArgs>(e => DateTo = e.NewDate);

            SaveCommand = new RelayCommand<UIElement>(async element =>
            {
                await element.Save();
            });

            RefreshCommand = new RelayCommand(async () =>
            {
                await DownloadDataAsync(SelectedCurrencies, DateFrom, DateTo);                
            });
        }

        private async Task DownloadDataAsync(IEnumerable<Currency> selectedCurrencies, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
        {
            IsLoading = true;

            try
            { 
                foreach (var currency in selectedCurrencies)
                {
                    List<Rate> grabbedRates = await NbpApiRequests.GetRatesForCurrency(currency.Code,
                        DateFrom ?? (DateTo != null ? DateTo.Value.AddDays(-10) : MaxDateTimeOffset.AddDays(-10)),
                        DateTo ?? (DateFrom != null ? DateFrom.Value.AddDays(10) : DateTimeOffset.Now));
                    if (currency.Rates == null)
                        currency.Rates = new ObservableCollection<Rate>();
                    else
                        currency.Rates.Clear();

                    foreach (var rate in grabbedRates.Where(x => x != null))
                        currency.Rates.Add(rate);
                }
            }   
            catch (Exception e)
            {
                await new MessageDialog(e.Message).ShowAsync();
            }

            SelectedCurrencies = new ObservableCollection<Currency>(SelectedCurrencies);

            IsLoading = false;
        }

        public void OnNavigateTo(object parameter = null)
        {
            SelectedCurrencies = new ObservableCollection<Currency>((IEnumerable<Currency>)parameter.GetType().GetProperty("SelectedCurrencies").GetValue(parameter));
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["lastOpenedPage"] = "CurrencyHistory";
            //Windows.Storage.ApplicationData.Current.LocalSettings.Values["SelectedCurrencies"] = SelectedCurrencies;
        }
    }
}
