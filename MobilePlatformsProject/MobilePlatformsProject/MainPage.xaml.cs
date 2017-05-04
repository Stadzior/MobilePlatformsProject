using System;
using System.Collections.Generic;
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
        private int _counter;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnRun_Click(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(async () =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    if (!_tokenSource.IsCancellationRequested)
                    {
                        _counter++;
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => textBoxCounter.Text = _counter.ToString());
                        await Task.Delay(1000);
                    }
                }
            },_tokenSource.Token);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _tokenSource.Cancel();
        }

        private void btnHello_Click(object sender, RoutedEventArgs e)
        {
            textBoxHello.Text = $"Hello {_counter} time!";
        }
    }
}
