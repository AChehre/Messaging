using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Messaging.Infrastructure.Common.Extensions
{
    public static class SerializationExtension
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static Stream ToJsonStream(this object obj)
        {
            var byteArray = Encoding.UTF8.GetBytes(obj.ToJson());
            return new MemoryStream(byteArray);
        }

        public static Stream SerializeToJsonStream(this object obj, Encoding encoding)
        {
            var byteArray = encoding.GetBytes(obj.ToJson());
            return new MemoryStream(byteArray);
        }

        public static TObject DeserializeFromJson<TObject>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<TObject>(jsonString);
        }

        public static TObject DeserializeFromJsonStream<TObject>(this Stream jsonStream)
        {
            var reader = new StreamReader(jsonStream);
            var jsonString = reader.ReadToEnd();
            return jsonString.DeserializeFromJson<TObject>();
        }

       
    }
}