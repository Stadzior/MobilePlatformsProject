using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MobilePlatformsProject.Interfaces;
using MobilePlatformsProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MobilePlatformsProject.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IRegisterCommands
    {
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly INavigationService _navigationService;
        
        public ObservableCollection<string> DownloadedFilesNames { get; set; }
        public ObservableCollection<Currency> Currencies { get; set; }

        private string _selectedFileName;
        public string SelectedFileName
        {
            get => _selectedFileName;
            set
            {
                _selectedFileName = value;
                Set(() => SelectedFileName, ref _selectedFileName, value);
            }
        }

        public ICommand NavigateToCurrencyHistoryCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand LoadDataFromFileCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService)
        {
            SelectedFileName = "None";
            _navigationService = navigationService;
            DownloadedFilesNames = new ObservableCollection<string>(
            Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json", SearchOption.TopDirectoryOnly)
                .Select(f => f.Substring(f.LastIndexOf(@"\")))
                .Select(f => f.Replace(@"\", ""))
                );

            Currencies = new ObservableCollection<Currency>();
            Currencies.Add(new Currency() { Name = "Dolar amerykański", Code = "USD", ExchangeRate = 3.7241 });
            Currencies.Add(new Currency() { Name = "Dolar kanadyjski", Code = "CAD", ExchangeRate = 2.7703 });
            Currencies.Add(new Currency() { Name = "euro", Code = "EUR", ExchangeRate = 4.1943 });
            Currencies.Add(new Currency() { Name = "forint ", Code = "HUF", ExchangeRate = 0.013593 });

            RegisterCommands();
        }

        public void RegisterCommands()
        {
            NavigateToCurrencyHistoryCommand = new RelayCommand<ItemClickEventArgs>(e => _navigationService.NavigateTo("CurrencyHistory", e.ClickedItem));
            LoadDataFromFileCommand = new RelayCommand(() =>
            {
                var i = SelectedFileName;
            });
            BackCommand = new RelayCommand(() =>
            {
                _navigationService.GoBack();
            });
        }
    }
}
