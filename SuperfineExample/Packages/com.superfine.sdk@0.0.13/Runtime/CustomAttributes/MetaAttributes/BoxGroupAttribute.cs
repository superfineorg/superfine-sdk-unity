using System;

namespace Superfine.Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BoxGroupAttribute : MetaAttribute, IGroupAttribute
    {
        public string Name { get; private set; }

        public int Indent { get; private set; }

        public BoxGroupAttribute(string name = "", int indent = 0)
        {
            Name = name;
            Indent = indent;
        }
    }
}