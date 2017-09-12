using MobilePlatformsProject.Converters.Json.Base;
using MobilePlatformsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MobilePlatformsProject.Converters.Json
{
    class CurrenciesConverter : JsonCreationConverter<Currency[]>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        protected override Currency[] Create(Type objectType, JObject jObject)
        {
            var ratesTokens = jObject
                .GetValue("rates")
                .Children();

            var result = new Currency[ratesTokens.Count()];
            for (int i = 0; i < result.Length; i++)
            {
                var token = ratesTokens.ElementAt(i);
                result[i] = new Currency
                {
                    Name = token.Value<string>("currency"),
                    Code = token.Value<string>("code"),
                    ExchangeRate = token.Value<double>("mid")
                };
            }

            return result;
        }
    }
}
