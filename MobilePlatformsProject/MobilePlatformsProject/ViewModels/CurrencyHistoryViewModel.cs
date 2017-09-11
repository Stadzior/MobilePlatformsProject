using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MobilePlatformsProject.Interfaces;
using MobilePlatformsProject.Models;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MobilePlatformsProject.ViewModels
{
    public class CurrencyHistoryViewModel : ViewModelBase, IRegisterCommands, INavigatableViewModel
    {
        private INavigationService _navigationService;

        private DateTimeOffset? _dateFrom;
        public DateTimeOffset? DateFrom
        {
            get => _dateFrom ?? MinDateTimeOffset;
            set
            {
                _dateFrom = value;
                Set(() => DateFrom, ref _dateFrom, value);
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["CurrencyHistoryDateFrom"] = DateFrom;
            }
        }

        private DateTimeOffset? _dateTo;
        public DateTimeOffset? DateTo
        {
            get => _dateTo ?? MaxDateTimeOffset;
            set
            {
                _dateTo = value;
                Set(() => DateTo, ref _dateTo, value);
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["CurrencyHistoryDateTo"] = DateTo;
            }
        }

        public DateTimeOffset MaxDateTimeOffset => DateTimeOffset.Now;
        public DateTimeOffset MinDateTimeOffset => DateTimeOffset.Parse("2002-02-02");

        public ObservableCollection<Currency> SelectedCurrencies { get; set; }

        public ICommand BackCommand { get; set; }
        public ICommand ManipulationStartedCommand { get; set; }
        public ICommand ManipulationCompletedCommand { get; set; }
        public ICommand DateFromChangedCommand { get; set; }
        public ICommand DateToChangedCommand { get; set; }
        public ICommand SaveCommand { get; set; }

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
            });

            ManipulationStartedCommand = new RelayCommand<ManipulationStartedRoutedEventArgs>(e => _fingerPosition = e.Position);
            ManipulationCompletedCommand = new RelayCommand<ManipulationCompletedRoutedEventArgs>(e =>
            {
                //if (_fingerPosition.X < e.Position.X)
                    //causes win32 unhandled exception _navigationService.NavigateTo("MainPage");
            });

            DateFromChangedCommand = new RelayCommand<CalendarDatePickerDateChangedEventArgs>(e => DateFrom = e.NewDate);
            DateToChangedCommand = new RelayCommand<CalendarDatePickerDateChangedEventArgs>(e => DateTo = e.NewDate);
            SaveCommand = new RelayCommand<SfChart>(chart =>
            {
                chart.Save("dummy.jpg", Windows.ApplicationModel.Package.Current.InstalledLocation);
            });
        }

        public void OnNavigateTo(object parameter = null)
        {
            SelectedCurrencies = new ObservableCollection<Currency>((IEnumerable<Currency>)parameter);
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["lastOpenedPage"] = "CurrencyHistory";
            //Windows.Storage.ApplicationData.Current.LocalSettings.Values["SelectedCurrencies"] = SelectedCurrencies;
        }
    }
}
