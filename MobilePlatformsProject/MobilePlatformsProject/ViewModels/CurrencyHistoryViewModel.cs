using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MobilePlatformsProject.Interfaces;
using MobilePlatformsProject.Models;
using System;
using System.Windows.Input;
using Windows.UI.Xaml.Input;

namespace MobilePlatformsProject.ViewModels
{
    public class CurrencyHistoryViewModel : ViewModelBase, IRegisterCommands, INavigatableViewModel
    {
        private INavigationService _navigationService;

        public DateTimeOffset MaxDateTimeOffset => DateTimeOffset.Now;
        public DateTimeOffset MinDateTimeOffset => DateTimeOffset.Parse("2002-02-02");

        private Currency _currency;
        public Currency Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                Set(() => Currency, ref _currency, value);
            }
        }

        public ICommand BackCommand { get; set; }
        public ICommand ManipulationStartedCommand { get; set; }
        public ICommand ManipulationCompletedCommand { get; set; }

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
        }

        public void OnNavigateTo(object parameter = null) 
            => Currency = (Currency)parameter;
    }
}
