using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MobilePlatformsProject.Interfaces;
using MobilePlatformsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MobilePlatformsProject.ViewModels
{
    public class CurrencyHistoryViewModel : ViewModelBase, IRegisterCommands, INavigatableViewModel
    {
        private INavigationService _navigationService;

        public DateTime CurrentDateTime => DateTime.Now;

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
        }

        public void OnNavigateTo(object parameter = null) 
            => Currency = (Currency)parameter;
    }
}
