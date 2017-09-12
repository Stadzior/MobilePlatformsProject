using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePlatformsProject
{
    public static class JsonExtensions
    {
        public static bool FieldExists(this JObject jObject, string fieldName)
            => jObject[fieldName] != null;
    }
}
