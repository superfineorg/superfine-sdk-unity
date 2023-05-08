using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Superfine.Tracking
{ 
    public class MethodArguments
    {
        private Dictionary<string, object> arguments = new Dictionary<string, object>();

        public MethodArguments() : this(new Dictionary<string, object>())
        {
        }

        public MethodArguments(MethodArguments methodArgs) : this(methodArgs.arguments)
        {
        }

        private MethodArguments(Dictionary<string, object> arguments)
        {
            this.arguments = arguments;
        }

        public void AddPrimative<T>(string argumentName, T value) where T : struct
        {
            arguments[argumentName] = value;
        }

        public void AddNullablePrimitive<T>(string argumentName, T? nullable) where T : struct
        {
            if (nullable != null && nullable.HasValue)
            {
                arguments[argumentName] = nullable.Value;
            }
        }

        public void AddString(string argumentName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                arguments[argumentName] = value;
            }
        }

        public void AddCommaSeparatedList(string argumentName, IEnumerable<string> value)
        {
            if (value != null)
            {
                arguments[argumentName] = string.Join(",", value.ToArray());
            }
        }

        public void AddDictionary(string argumentName, IDictionary<string, object> dict)
        {
            if (dict != null)
            {
                arguments[argumentName] = ToStringDict(dict);
            }
        }

        public void AddList<T>(string argumentName, IEnumerable<T> list)
        {
            if (list != null)
            {
                arguments[argumentName] = list;
            }
        }

        public void AddUri(string argumentName, Uri uri)
        {
            if (uri != null && !string.IsNullOrEmpty(uri.AbsoluteUri))
            {
                arguments[argumentName] = uri.ToString();
            }
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(arguments);
        }

        private static Dictionary<string, string> ToStringDict(IDictionary<string, object> dict)
        {
            if (dict == null)
            {
                return null;
            }

            var newDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> kvp in dict)
            {
                newDict[kvp.Key] = kvp.Value.ToString();
            }

            return newDict;
        }
    }
}
