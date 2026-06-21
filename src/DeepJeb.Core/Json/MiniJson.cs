using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace DeepJeb.Core.Json
{
    /// <summary>
    /// Minimal JSON parser/serializer. No external dependencies.
    /// Parses JSON → Dictionary&lt;string,object&gt; | List&lt;object&gt; | primitive.
    /// Serializes the same trees back to JSON string.
    ///
    /// Based on MiniJSON by Calvin Rien (public domain), extended for DeepJeb.
    /// </summary>
    public static class MiniJson
    {
        // ---- Deserialize ----

        public static object Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
            int index = 0;
            SkipWhitespace(json, ref index);
            if (index >= json.Length) return null;
            bool success = ParseValue(json, ref index, out object result);
            return success ? result : null;
        }

        // ---- Serialize ----

        public static string Serialize(object obj)
        {
            var sb = new StringBuilder();
            SerializeValue(obj, sb);
            return sb.ToString();
        }

        // ---- Parser internals ----

        private static bool ParseValue(string json, ref int index, out object result)
        {
            SkipWhitespace(json, ref index);
            if (index >= json.Length) { result = null; return false; }

            char c = json[index];

            switch (c)
            {
                case '"': return ParseString(json, ref index, out result);
                case '{': return ParseObject(json, ref index, out result);
                case '[': return ParseArray(json, ref index, out result);
                case 't': result = true;  index += 4; return true;
                case 'f': result = false; index += 5; return true;
                case 'n': result = null;  index += 4; return true;
                default:
                    if (c == '-' || char.IsDigit(c))
                        return ParseNumber(json, ref index, out result);
                    result = null;
                    return false;
            }
        }

        private static bool ParseString(string json, ref int index, out object result)
        {
            var sb = new StringBuilder();
            index++; // skip opening "
            while (index < json.Length)
            {
                char c = json[index];
                if (c == '"') { index++; result = sb.ToString(); return true; }
                if (c == '\\')
                {
                    index++;
                    if (index >= json.Length) break;
                    switch (json[index])
                    {
                        case '"': sb.Append('"'); break;
                        case '\\': sb.Append('\\'); break;
                        case '/': sb.Append('/'); break;
                        case 'b': sb.Append('\b'); break;
                        case 'f': sb.Append('\f'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        case 'u':
                            if (index + 5 <= json.Length)
                            {
                                string hex = json.Substring(index + 1, 4);
                                try { sb.Append((char)int.Parse(hex, NumberStyles.HexNumber)); }
                                catch { sb.Append('?'); }
                                index += 4;
                            }
                            break;
                    }
                    index++;
                }
                else { sb.Append(c); index++; }
            }
            result = sb.ToString();
            return true;
        }

        private static bool ParseNumber(string json, ref int index, out object result)
        {
            int start = index;
            if (json[index] == '-') index++;
            while (index < json.Length && char.IsDigit(json[index])) index++;
            bool isFloat = false;
            if (index < json.Length && json[index] == '.')
            {
                isFloat = true;
                index++;
                while (index < json.Length && char.IsDigit(json[index])) index++;
            }
            if (index < json.Length && (json[index] == 'e' || json[index] == 'E'))
            {
                isFloat = true;
                index++;
                if (index < json.Length && (json[index] == '+' || json[index] == '-')) index++;
                while (index < json.Length && char.IsDigit(json[index])) index++;
            }

            string numStr = json.Substring(start, index - start);

            if (isFloat)
            {
                if (double.TryParse(numStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double d))
                {
                    // Store as double if it fits, otherwise keep as string
                    if (d >= int.MinValue && d <= int.MaxValue && d == Math.Floor(d))
                    {
                        result = (int)d;
                    }
                    else
                    {
                        // Store in a wrapper so we can distinguish int from float on re-serialize
                        result = d;
                    }
                    return true;
                }
            }
            else
            {
                if (long.TryParse(numStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out long l))
                {
                    if (l >= int.MinValue && l <= int.MaxValue)
                        result = (int)l;
                    else
                        result = l;
                    return true;
                }
            }

            result = 0;
            return false;
        }

        private static bool ParseObject(string json, ref int index, out object result)
        {
            var dict = new Dictionary<string, object>();
            index++; // skip {
            SkipWhitespace(json, ref index);

            if (index < json.Length && json[index] == '}')
            {
                index++;
                result = dict;
                return true;
            }

            while (true)
            {
                SkipWhitespace(json, ref index);
                if (!ParseString(json, ref index, out object keyObj)) { result = null; return false; }
                string key = (string)keyObj;

                SkipWhitespace(json, ref index);
                if (index >= json.Length || json[index] != ':') { result = null; return false; }
                index++; // skip :

                if (!ParseValue(json, ref index, out object value)) { result = null; return false; }
                dict[key] = value;

                SkipWhitespace(json, ref index);
                if (index >= json.Length) break;
                if (json[index] == '}') { index++; break; }
                if (json[index] != ',') { result = null; return false; }
                index++; // skip ,
            }

            result = dict;
            return true;
        }

        private static bool ParseArray(string json, ref int index, out object result)
        {
            var list = new List<object>();
            index++; // skip [
            SkipWhitespace(json, ref index);

            if (index < json.Length && json[index] == ']')
            {
                index++;
                result = list;
                return true;
            }

            while (true)
            {
                if (!ParseValue(json, ref index, out object value)) { result = null; return false; }
                list.Add(value);

                SkipWhitespace(json, ref index);
                if (index >= json.Length) break;
                if (json[index] == ']') { index++; break; }
                if (json[index] != ',') { result = null; return false; }
                index++; // skip ,
            }

            result = list;
            return true;
        }

        private static void SkipWhitespace(string json, ref int index)
        {
            while (index < json.Length && char.IsWhiteSpace(json[index]))
                index++;
        }

        // ---- Serializer internals ----

        private static void SerializeValue(object obj, StringBuilder sb)
        {
            if (obj == null)
            {
                sb.Append("null");
                return;
            }

            if (obj is string s)
            {
                SerializeString(s, sb);
                return;
            }

            if (obj is bool b)
            {
                sb.Append(b ? "true" : "false");
                return;
            }

            if (obj is int i)
            {
                sb.Append(i);
                return;
            }

            if (obj is long l)
            {
                sb.Append(l);
                return;
            }

            if (obj is double d)
            {
                sb.Append(d.ToString("R", CultureInfo.InvariantCulture));
                return;
            }

            if (obj is float f)
            {
                sb.Append(f.ToString("R", CultureInfo.InvariantCulture));
                return;
            }

            if (obj is IDictionary dict)
            {
                SerializeDictionary(dict, sb);
                return;
            }

            if (obj is IList list)
            {
                SerializeList(list, sb);
                return;
            }

            // Fallback: use ToString and treat as string
            SerializeString(obj.ToString(), sb);
        }

        private static void SerializeString(string str, StringBuilder sb)
        {
            sb.Append('"');
            foreach (char c in str)
            {
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < 32)
                            sb.AppendFormat("\\u{0:X4}", (int)c);
                        else
                            sb.Append(c);
                        break;
                }
            }
            sb.Append('"');
        }

        private static void SerializeDictionary(IDictionary dict, StringBuilder sb)
        {
            sb.Append('{');
            bool first = true;
            foreach (DictionaryEntry kvp in dict)
            {
                if (!first) sb.Append(',');
                first = false;
                SerializeString(kvp.Key.ToString(), sb);
                sb.Append(':');
                SerializeValue(kvp.Value, sb);
            }
            sb.Append('}');
        }

        private static void SerializeList(IList list, StringBuilder sb)
        {
            sb.Append('[');
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0) sb.Append(',');
                SerializeValue(list[i], sb);
            }
            sb.Append(']');
        }
    }
}
