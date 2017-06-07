using MobilePlatformsProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MobilePlatformsProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public ObservableCollection<string> DownloadedFilesNames { get; }
        public ObservableCollection<Currency> Currencies { get; }
        public MainPage()
        {
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

            this.InitializeComponent();
        }
    }
}
