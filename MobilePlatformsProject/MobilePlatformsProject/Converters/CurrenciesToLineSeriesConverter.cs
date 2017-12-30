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
            var randomizer = new Random();
            foreach (var currency in currencies)
            {
                var randomColorCoords = new[] { System.Convert.ToByte(randomizer.Next(0, 255)), System.Convert.ToByte(randomizer.Next(0, 255)), System.Convert.ToByte(randomizer.Next(0, 255)) };
                var randomColor = new Windows.UI.Color
                {
                    A = 0xFF,
                    R = randomColorCoords[0],
                    G = randomColorCoords[1],
                    B = randomColorCoords[2]
                };

                result.Add(new LineSeries
                {
                    ItemsSource = currency.Rates,
                    XBindingPath = "Date",
                    YBindingPath = "Value",
                    Label = currency.Code,
                    Interior = new SolidColorBrush(randomColor),
                    ShowTooltip = true
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
