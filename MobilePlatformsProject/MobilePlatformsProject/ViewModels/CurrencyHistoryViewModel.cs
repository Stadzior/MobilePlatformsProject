using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MobilePlatformsProject.Interfaces;
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
    public class CurrencyHistoryViewModel : ViewModelBase, IRegisterCommands
    {
        private INavigationService _navigationService;

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
    }
}
