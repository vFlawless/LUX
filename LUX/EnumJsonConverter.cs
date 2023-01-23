using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUX
{
    public class EnumJsonConverter<TEnum> : JsonConverter where TEnum : struct, Enum
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TEnum);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string enumValue = reader.Value.ToString();
                if (Enum.TryParse<TEnum>(enumValue, true, out TEnum result))
                {
                    return result;
                }
            }

            throw new JsonSerializationException($"Failed to deserialize {typeof(TEnum)} value.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }

}
