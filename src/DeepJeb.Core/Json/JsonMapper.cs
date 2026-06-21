using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DeepJeb.Core.Json
{
    /// <summary>
    /// Typed JSON serializer/deserializer using reflection.
    /// Replaces Newtonsoft.Json for DeepJeb's typed object serialization needs.
    ///
    /// Usage:
    ///   string json = JsonMapper.ToJson(myObject);
    ///   MyType obj = JsonMapper.FromJson&lt;MyType&gt;(json);
    ///
    /// Supports: primitives, string, DateTime, enums, List&lt;T&gt;,
    /// and nested objects. Property names are lowercased first character.
    /// </summary>
    public static class JsonMapper
    {
        /// <summary>Serialize an object to JSON string.</summary>
        public static string ToJson(object obj)
        {
            if (obj == null) return "null";
            var dict = ObjectToDict(obj);
            return MiniJson.Serialize(dict);
        }

        /// <summary>Deserialize a JSON string to a typed object.</summary>
        public static T FromJson<T>(string json) where T : new()
        {
            if (string.IsNullOrEmpty(json) || json == "null") return default;
            object parsed = MiniJson.Deserialize(json);
            if (parsed is Dictionary<string, object> dict)
                return (T)DictToObject(dict, typeof(T));
            return default;
        }

        /// <summary>Deserialize to a list of typed objects.</summary>
        public static List<T> FromJsonList<T>(string json) where T : new()
        {
            if (string.IsNullOrEmpty(json) || json == "null") return new List<T>();
            object parsed = MiniJson.Deserialize(json);
            if (parsed is List<object> list)
            {
                var result = new List<T>(list.Count);
                foreach (var item in list)
                {
                    if (item is Dictionary<string, object> d)
                        result.Add((T)DictToObject(d, typeof(T)));
                    else
                        result.Add((T)ConvertValue(item, typeof(T)));
                }
                return result;
            }
            return new List<T>();
        }

        /// <summary>Parse JSON to a raw object tree (Dict/List/primitive).</summary>
        public static object Parse(string json)
        {
            return MiniJson.Deserialize(json);
        }

        /// <summary>Serialize a raw object tree to JSON string.</summary>
        public static string Stringify(object tree)
        {
            return MiniJson.Serialize(tree);
        }

        // ---- Object → Dictionary ----

        private static Dictionary<string, object> ObjectToDict(object obj)
        {
            if (obj == null) return null;
            var dict = new Dictionary<string, object>();
            var type = obj.GetType();

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead) continue;

                string jsonName = GetJsonPropertyName(prop);
                object value = prop.GetValue(obj, null);
                dict[jsonName] = ValueToJsonValue(value);
            }

            return dict;
        }

        private static object ValueToJsonValue(object value)
        {
            if (value == null) return null;

            Type t = value.GetType();

            // Primitives
            if (t == typeof(string) || t == typeof(bool) ||
                t == typeof(int) || t == typeof(long) ||
                t == typeof(float) || t == typeof(double))
                return value;

            // DateTime → ISO 8601 string
            if (t == typeof(DateTime))
                return ((DateTime)value).ToString("o");

            // Enum → string
            if (t.IsEnum)
                return value.ToString();

            // List → List<object>
            if (value is IList list)
            {
                var jsonList = new List<object>();
                foreach (var item in list)
                    jsonList.Add(ValueToJsonValue(item));
                return jsonList;
            }

            // Nested object → Dictionary
            return ObjectToDict(value);
        }

        // ---- Dictionary → Object ----

        private static object DictToObject(Dictionary<string, object> dict, Type type)
        {
            if (dict == null) return null;
            object obj = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanWrite) continue;

                string jsonName = GetJsonPropertyName(prop);

                // Try matching JSON field name
                if (!dict.TryGetValue(jsonName, out object rawValue))
                {
                    // Also try case-insensitive match as fallback
                    string match = null;
                    foreach (var key in dict.Keys)
                    {
                        if (string.Equals(key, jsonName, StringComparison.OrdinalIgnoreCase))
                        {
                            match = key;
                            break;
                        }
                    }
                    if (match != null)
                        rawValue = dict[match];
                    else
                        continue;
                }

                object converted = ConvertValue(rawValue, prop.PropertyType);
                prop.SetValue(obj, converted, null);
            }

            return obj;
        }

        private static object ConvertValue(object rawValue, Type targetType)
        {
            if (rawValue == null)
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;

            Type rawType = rawValue.GetType();

            // Same type or compatible
            if (targetType.IsAssignableFrom(rawType))
                return rawValue;

            // String → enum
            if (targetType.IsEnum && rawValue is string enumStr)
            {
                try { return Enum.Parse(targetType, enumStr, ignoreCase: true); }
                catch { return Activator.CreateInstance(targetType); }
            }

            // String → DateTime
            if (targetType == typeof(DateTime) && rawValue is string dateStr)
            {
                if (DateTime.TryParse(dateStr, out DateTime dt))
                    return dt;
                return DateTime.MinValue;
            }

            // Nested object: dict → typed object
            if (rawValue is Dictionary<string, object> nestedDict && !targetType.IsPrimitive &&
                targetType != typeof(string) && targetType != typeof(object))
            {
                return DictToObject(nestedDict, targetType);
            }

            // List handling
            if (typeof(IList).IsAssignableFrom(targetType) && rawValue is List<object> rawList)
            {
                Type elementType = targetType.IsGenericType
                    ? targetType.GetGenericArguments()[0]
                    : typeof(object);

                var typedList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

                foreach (var item in rawList)
                {
                    if (item is Dictionary<string, object> itemDict && !elementType.IsPrimitive &&
                        elementType != typeof(string))
                    {
                        typedList.Add(DictToObject(itemDict, elementType));
                    }
                    else
                    {
                        typedList.Add(ConvertValue(item, elementType));
                    }
                }
                return typedList;
            }

            // Numeric conversions
            try
            {
                if (targetType == typeof(int)) return Convert.ToInt32(rawValue);
                if (targetType == typeof(long)) return Convert.ToInt64(rawValue);
                if (targetType == typeof(float)) return Convert.ToSingle(rawValue);
                if (targetType == typeof(double)) return Convert.ToDouble(rawValue);
                if (targetType == typeof(bool)) return Convert.ToBoolean(rawValue);
                if (targetType == typeof(string)) return rawValue.ToString();
            }
            catch { }

            return rawValue;
        }

        /// <summary>
        /// Get the JSON field name for a property. Uses [JsonProperty("name")]
        /// if present, otherwise falls back to the C# property name.
        /// </summary>
        private static string GetJsonPropertyName(PropertyInfo prop)
        {
            // Default convention: lowercase first letter of C# property name.
            // This matches the JSON camelCase convention used by most APIs.
            string propName = prop.Name;
            if (propName.Length == 1)
                return propName.ToLowerInvariant();
            return char.ToLowerInvariant(propName[0]) + propName.Substring(1);
        }
    }
}
