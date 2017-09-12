using MobilePlatformsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePlatformsProject.Rest
{
    public static class NbpApiRequests
    {
        private static RequestHelper _requestHelper = new RequestHelper("http://api.nbp.pl/api/exchangerates");

        public static IEnumerable<Currency> GetActualRates()
        {
            var _requestHelper.GetAsync<IEnumerable<>
        }

        public static ExchangeRatesSeries GetRatesForDate(DateTimeOffset date)
        { 
            _requestHelper.GetAsync<>
        }
    }
}
