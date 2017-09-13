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
    class CurrenciesConverter : JsonCreationConverter<List<Currency>>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        protected override List<Currency> Create(Type objectType, JToken jToken)
        {
            var ratesTokens = jToken.First.Value<JArray>("rates");
            var result = new List<Currency>();
            foreach (var token in ratesTokens)
            {
                result.Add(new Currency
                {
                    Name = token.Value<string>("currency"),
                    Code = token.Value<string>("code"),
                    ExchangeRate = token.Value<double>("mid")
                });
            }

            return result;
        }
    }
}
