using System.Xml.Schema;
using UnityEngine;

namespace Superfine
{
    [System.Serializable]
    public struct SuperfineNullable<T> where T : struct
    {
        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new System.InvalidOperationException("Serializable nullable object must have a value.");
                }

                return value;
            }
        }

        public bool HasValue { get { return hasValue; } }

        [SerializeField]
        private T value;

        [SerializeField]
        private bool hasValue;

        public SuperfineNullable(bool hasValue, T value)
        {
            this.value = value;
            this.hasValue = hasValue;
        }

        private SuperfineNullable(T value)
        {
            this.value = value;
            hasValue = true;
        }

        public void SetValue(T value)
        {
            this.value = value;
            hasValue = true;
        }

        public static implicit operator SuperfineNullable<T>(T value)
        {
            return new SuperfineNullable<T>(value);
        }

        public static implicit operator SuperfineNullable<T>(System.Nullable<T> value)
        {
            return value.HasValue ? new SuperfineNullable<T>(value.Value) : new SuperfineNullable<T>();
        }

        public static implicit operator System.Nullable<T>(SuperfineNullable<T> value)
        {
            return value.HasValue ? (T?)value.Value : null;
        }
    }
}