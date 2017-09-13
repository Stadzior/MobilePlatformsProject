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

        public static async Task<List<Currency>> GetActualRates()
            => await _requestHelper.GetAsync<List<Currency>>("/api/exchangerates/tables/A/", new CurrenciesConverter());

        public static async Task<List<Currency>> GetRatesForDate(DateTimeOffset date)
            => await _requestHelper.GetAsync<List<Currency>>($"/api/exchangerates/tables/A/{date.ToString("yyyy-MM-dd")}/", new CurrenciesConverter());

        internal static async Task<List<Rate>> GetRatesForCurrency(string code, DateTimeOffset dateFrom, DateTimeOffset dateTo)
            => await _requestHelper.GetAsync<List<Rate>>($"/api/exchangerates/rates/A/{code}/{dateFrom.ToString("yyyy-MM-dd")}/{dateTo.ToString("yyyy-MM-dd")}/", new RatesConverter());
    }
}
