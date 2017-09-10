using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using MobilePlatformsProject.ViewModels;
using MobilePlatformsProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePlatformsProject
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary> 
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            var nav = new NavigationService();
            nav.Configure(MainPageKey, typeof(MainPage));
            nav.Configure(CurrencyHistoryKey, typeof(CurrencyHistoryPage));
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
            }
            else
            {
                // Create run time view services and models
            }

            //Register your services used here
            SimpleIoc.Default.Unregister<INavigationService>();
            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<CurrencyHistoryViewModel>();          
        }


        public const string MainPageKey = "MainPage";
        // <summary>
        // Gets the FirstPage view model.
        // </summary>
        // <value>
        // The FirstPage view model.
        // </value>
        public MainPageViewModel MainPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainPageViewModel>();
            }
        }

        public const string CurrencyHistoryKey = "CurrencyHistory";
        // <summary>
        // Gets the SecondPage view model.
        // </summary>
        // <value>
        // The SecondPage view model.
        // </value>
        public CurrencyHistoryViewModel CurrencyHistoryViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CurrencyHistoryViewModel>();
            }
        }

        // <summary>
        // The cleanup.
        // </summary>
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
