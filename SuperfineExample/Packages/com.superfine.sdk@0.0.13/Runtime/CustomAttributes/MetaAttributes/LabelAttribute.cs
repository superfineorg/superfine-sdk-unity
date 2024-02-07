using System;

namespace Superfine.Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LabelAttribute : MetaAttribute
    {
        public string Label { get; private set; }

        public bool Bold { get; private set; }

        public LabelAttribute(string label, bool bold = false)
        {
            Label = label;
            Bold = bold;
        }
    }
}
