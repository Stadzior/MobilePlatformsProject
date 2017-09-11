using MobilePlatformsProject.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MobilePlatformsProject.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CurrencyHistoryPage : Page
    {
        public CurrencyHistoryPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (DataContext is INavigatableViewModel navigatableViewModel)
            {
                navigatableViewModel.OnNavigateTo(e.Parameter);
                dateFromPicker.Date = (DateTimeOffset)e.Parameter.GetType().GetProperty("DateFrom").GetValue(e.Parameter);
                dateToPicker.Date = (DateTimeOffset)e.Parameter.GetType().GetProperty("DateTo").GetValue(e.Parameter);
            }
        }
    }
}
