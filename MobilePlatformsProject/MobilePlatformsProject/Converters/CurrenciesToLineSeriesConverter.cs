using MobilePlatformsProject.Models;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MobilePlatformsProject.Converters
{
    public class CurrenciesToLineSeriesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IEnumerable<Currency> currencies = (IEnumerable<Currency>)value;
            var result = new ChartSeriesCollection();
            foreach (var currency in currencies)
            {
                result.Add(new LineSeries
                {
                    ItemsSource = currency.Rates,
                    XBindingPath = "Date",
                    YBindingPath = "Value",
                    Label = "temp label",
                    Interior = new SolidColorBrush(Windows.UI.Colors.Red)
                });
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
