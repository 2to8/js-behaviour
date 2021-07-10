using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace SQLiteNetExtensions.Extensions.TextBlob.Serializers
{
    public class TupleConverter<T1, T2> : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture,
            object value) {
            var elements = Convert.ToString(value)
                .Trim('(')
                .Trim(')')
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return (Int32.Parse(elements.First()), Int32.Parse(elements.Last()));
        }
    }

    public class JsonBlobSerializer : ITextBlobSerializer
    {
        public string Serialize(object element) {
            TypeDescriptor.AddAttributes(typeof((int, int)),
                new TypeConverterAttribute(typeof(TupleConverter<int, int>)));

            return JsonConvert.SerializeObject(element);
        }

        public object Deserialize(string text, Type type) {
            Debug.Log(text);

            //Debug.Log(type.Name);
            try {
                TypeDescriptor.AddAttributes(typeof((int, int)),
                    new TypeConverterAttribute(typeof(TupleConverter<int, int>)));

                //var json = JsonConvert.SerializeObject(dictionary);
                return JsonConvert.DeserializeObject<Dictionary<(int, int), string>>(text);
            } catch(Exception e) {
                return JsonConvert.DeserializeObject(text, type);
            }
        }
    }
}
