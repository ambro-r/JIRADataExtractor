using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace JIRADataExtractor.Converters
{
    class NestedJSONConverter<T> : JsonConverter where T : new()
    {

        private Dictionary<string, string> CustomElements;
        public NestedJSONConverter(Dictionary<string, string> customFields) {
            CustomElements = customFields;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var root = JObject.Load(reader);
            var result = new T();

            foreach (var prop in result.GetType().GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            {
                string propName = string.Empty;
                //filter out non-Json attributes
                var attr = prop.GetCustomAttributes(false).Where(a => a.GetType() == typeof(JsonPropertyAttribute)).FirstOrDefault();
                if (attr != null)
                {
                    propName = ((JsonPropertyAttribute)attr).PropertyName;
                }
                //If no JsonPropertyAttribute existed, or no PropertyName was set,
                //still attempt to deserialize the class member
                if (string.IsNullOrWhiteSpace(propName))
                {
                    propName = prop.Name;
                }
                //split by the delimiter, and traverse recursevly according to the path
                var elements = propName.Split('/');
                object propValue = null;
                JToken jToken = null;
                for (var i = 0; i < elements.Length; i++)
                {
                    string element = elements[i];
                    if (element.StartsWith("custom.element."))
                    {
                        Log.Debug("Custom element {element} found in JsonProperty.", element);
                        element = element.Substring("custom.element.".Length).ToLower();
                        Log.Verbose("{0}Custom elements dictonary contains element {element}: {containsKey}.", "".PadRight(i), element, CustomElements.ContainsKey(element));
                        element = CustomElements.ContainsKey(element) ? CustomElements[element] : "";
                    }
                    Log.Verbose("{0}Getting {element} from jSON.", "".PadRight(i) ,element);
                    jToken = jToken == null ? root[element] : jToken[element];

                    if (jToken == null)
                    {
                        Log.Debug("No token found for element {element} at position {i} in the tree, will not process this branch any further.", element, i);
                        break;
                    }
                    else
                    {
                        if (jToken is JValue)
                        {
                            propValue = ((JValue)jToken).Value;
                        } else if (jToken is JArray)
                        {
                            propValue = ((JArray)jToken).Select(ja => (string)ja).ToArray();        
                        }
                    }
                }

                if (propValue != null)
                {
                    // workaround for numeric values being automatically created as Int64 (long) objects.
                    if (prop.PropertyType == typeof(Int32))
                    {
                        prop.SetValue(result, Convert.ToInt32(propValue));
                    }
                    else if (prop.PropertyType == typeof(Int64))
                    {
                        prop.SetValue(result, Convert.ToInt64(propValue));
                    } else if(prop.PropertyType == typeof(DateTimeOffset))
                    {
                        prop.SetValue(result, Convert.ToDateTime(propValue));
                    }
                    else
                    {
                        prop.SetValue(result, propValue);
                    }
                }
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
