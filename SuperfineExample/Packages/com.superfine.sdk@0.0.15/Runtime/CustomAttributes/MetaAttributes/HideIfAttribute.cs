using System;

namespace Superfine.Unity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HideIfAttribute : ShowIfAttributeBase
    {
        public HideIfAttribute(string condition)
            : base(condition)
        {
            Inverted = true;
        }

        public HideIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = true;
        }

        public HideIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = true;
        }

        public HideIfAttribute(string enumName, EConditionOperator conditionOperator, params object[] enumObjects)
            : base(enumName, conditionOperator, ConvertEnumArray(enumObjects))
        {
            Inverted = true;
        }

        private static Enum[] ConvertEnumArray(object[] enumObjects)
        {
            int numEnumValues = enumObjects.Length;

            Enum[] enumValues = new Enum[numEnumValues];
            for (int i = 0; i < numEnumValues; ++i)
            {
                enumValues[i] = enumObjects[i] as Enum;
            }

            return enumValues;
        }
    }
}
