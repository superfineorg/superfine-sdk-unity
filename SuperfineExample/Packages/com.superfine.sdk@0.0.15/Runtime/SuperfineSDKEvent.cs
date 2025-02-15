using System.Collections.Generic;
using System.Text;

namespace Superfine.Unity
{
    public class SuperfineSDKEvent
    {
        public enum ValueType
        {
            NONE = 0,
            INT = 1,
            STRING = 2,
            MAP = 3,
            JSON = 4
        }

        public string eventName;

        public ValueType valueType = ValueType.NONE;
        public object value = null;
        
        public EventFlag eventFlag = EventFlag.NONE;

        private bool hasRevenue = false;

        public double revenue = 0.0;
        public string currency = string.Empty;

        public SuperfineSDKEvent(string eventName)
        {
            this.eventName = eventName;

            valueType = ValueType.NONE;
            value = null;

            eventFlag = EventFlag.NONE;

            hasRevenue = false;
        }

        public void SetValue(int value)
        {
            valueType = ValueType.INT;
            this.value = value;
        }

        public void SetValue(string value)
        {
            valueType = ValueType.STRING;
            this.value = value;
        }

	    public void SetValue(Dictionary<string, string> value)
        {
            valueType = ValueType.MAP;
            this.value = value;
        }

	    public void SetValue(SimpleJSON.JSONObject value)
        {
            valueType = ValueType.JSON;
            this.value = value;
        }

        public void SetRevenue(double revenue, string currency)
        {
            hasRevenue = true;

            this.revenue = revenue;
            this.currency = currency;
        }

        public bool HasRevenue()
        {
            return hasRevenue;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(eventName);

            if (value != null)
            {
                stringBuilder.Append(", data = ");
                stringBuilder.Append(value.ToString());
            }

            if (eventFlag != EventFlag.NONE)
            {
                stringBuilder.Append(", eventFlag = ");
                stringBuilder.Append(eventFlag);
            }

            if (hasRevenue)
            {
                stringBuilder.Append(", revenue = ");
                stringBuilder.Append(string.Format("{0} {1}", revenue, currency));
            }

            return stringBuilder.ToString();
        }
    }
}