using System.Collections.Generic;

namespace Superfine.Unity
{
    public class SuperfineSDKThirdPartySharingSettings
    {
        private Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, Dictionary<string, bool>> flags = new Dictionary<string, Dictionary<string, bool>>();

        public Dictionary<string, Dictionary<string, string>> GetValues()
        {
            return values;
        }

        public Dictionary<string, Dictionary<string, bool>> GetFlags()
        {
            return flags;
        }

        public void AddValue(string partnerName, string key, string value)
        {
            if (!values.TryGetValue(partnerName, out Dictionary<string, string> dict))
            {
                dict = new Dictionary<string, string>();
                values.Add(partnerName, dict);
            }

            dict[key] = value;
        }

        public void AddFlag(string partnerName, string key, bool value)
        {
            if (!flags.TryGetValue(partnerName, out Dictionary<string, bool> dict))
            {
                dict = new Dictionary<string, bool>();
                flags.Add(partnerName, dict);
            }

            dict[key] = value;
        }
    }
}