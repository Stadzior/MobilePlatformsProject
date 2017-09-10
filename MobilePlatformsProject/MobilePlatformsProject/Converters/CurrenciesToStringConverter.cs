using MobilePlatformsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MobilePlatformsProject.Converters
{
    public class CurrenciesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var sourceList = (IEnumerable<Currency>)value;
            char separator = parameter == null ? ';' : (char)parameter;
            var targetStringBuilder = new StringBuilder();
            foreach (var item in sourceList)
            {
                targetStringBuilder.Append(item.Name).Append(separator);
            }

            targetStringBuilder.Remove(targetStringBuilder.Length - 1, 1);

            return targetStringBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
