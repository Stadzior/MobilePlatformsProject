using MobilePlatformsProject.Converters.Json.Base;
using MobilePlatformsProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePlatformsProject.Converters.Json
{
    public class RatesConverter : JsonCreationConverter<List<Rate>>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        protected override List<Rate> Create(Type objectType, JToken jToken)
        {
            var ratesTokens = jToken.Value<JArray>("rates");
            var result = new List<Rate>();
            foreach (var token in ratesTokens)
            {
                result.Add(new Rate
                {
                    Date = DateTimeOffset.Parse(token.Value<string>("effectiveDate")),
                    Value = token.Value<double>("mid")
                });
            }

            return result;
        }
    }
}
