using MobilePlatformsProject.Converters.Json;
using MobilePlatformsProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobilePlatformsProject.Rest
{
    public static class NbpApiRequests
    {
        private static RequestHelper _requestHelper = new RequestHelper("http://api.nbp.pl");

        public static async Task<IEnumerable<Currency>> GetActualRates()
            => await _requestHelper.GetAsync<IEnumerable<Currency>>("/api/exchangerates/tables/A/", new CurrenciesConverter());

        public static async Task<IEnumerable<Currency>> GetRatesForDate(DateTimeOffset date)
            => await _requestHelper.GetAsync<IEnumerable<Currency>>($"/api/exchangerates/tables/A/{date.ToString("yyyy-MM-dd")}/", new CurrenciesConverter());
    }
}
