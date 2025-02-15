using System;

namespace Superfine.Unity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : ShowIfAttributeBase
    {
        public ShowIfAttribute(string condition)
            : base(condition)
        {
            Inverted = false;
        }

        public ShowIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = false;
        }

        public ShowIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = false;
        }

        public ShowIfAttribute(string enumName, EConditionOperator conditionOperator, params object[] enumObjects)
            : base(enumName, conditionOperator, ConvertEnumArray(enumObjects))
        {
            Inverted = false;
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
