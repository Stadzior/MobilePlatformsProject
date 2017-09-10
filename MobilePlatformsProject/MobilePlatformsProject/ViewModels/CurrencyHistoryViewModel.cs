using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        public ICommand BackCommand { get; set; }
        public CurrencyHistoryViewModel()
        {
            RegisterCommands();
        }

        public void RegisterCommands()
        {
            BackCommand = new RelayCommand(() =>
            {
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame == null)
                    return;

                if (rootFrame.CanGoBack)
                {
                    rootFrame.GoBack();
                }
            });
        }
    }
}
