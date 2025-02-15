using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Superfine
{
    [Serializable]
    public class SuperfineDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<KeyValuePair> list = new List<KeyValuePair>();
        [SerializeField, HideInInspector]
        private Dictionary<TKey, int> indexByKey = new Dictionary<TKey, int>();
        [SerializeField, HideInInspector]
        private Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        public Dictionary<TKey, TValue> GetDictionary()
        {
            return dict;
        }

#pragma warning disable 0414
        [SerializeField, HideInInspector]
        private bool keyCollision;
#pragma warning restore 0414

        [Serializable]
        private struct KeyValuePair
        {
            public TKey Key;
            public TValue Value;
            public KeyValuePair(TKey Key, TValue Value)
            {
                this.Key = Key;
                this.Value = Value;
            }
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            dict.Clear();
            indexByKey.Clear();
            keyCollision = false;
            for (int i = 0; i < list.Count; i++)
            {
                var key = list[i].Key;
                if (key != null && !ContainsKey(key))
                {
                    dict.Add(key, list[i].Value);
                    indexByKey.Add(key, i);
                }
                else
                {
                    keyCollision = true;
                }
            }
        }

        public TValue this[TKey key]
        {
            get => dict[key];
            set
            {
                dict[key] = value;
                if (indexByKey.ContainsKey(key))
                {
                    var index = indexByKey[key];
                    list[index] = new KeyValuePair(key, value);
                }
                else
                {
                    list.Add(new KeyValuePair(key, value));
                    indexByKey.Add(key, list.Count - 1);
                }
            }
        }

        public ICollection<TKey> Keys => dict.Keys;
        public ICollection<TValue> Values => dict.Values;

        public void Add(TKey key, TValue value)
        {
            dict.Add(key, value);
            list.Add(new KeyValuePair(key, value));
            indexByKey.Add(key, list.Count - 1);
        }

        public bool ContainsKey(TKey key) => dict.ContainsKey(key);

        public bool Remove(TKey key)
        {
            if (dict.Remove(key))
            {
                var index = indexByKey[key];
                list.RemoveAt(index);
                UpdateIndexLookup(index);
                indexByKey.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateIndexLookup(int removedIndex)
        {
            for (int i = removedIndex; i < list.Count; i++)
            {
                var key = list[i].Key;
                indexByKey[key]--;
            }
        }

        public bool TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);

        // ICollection
        public int Count => dict.Count;
        public bool IsReadOnly { get; set; }

        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            Add(pair.Key, pair.Value);
        }

        public void Clear()
        {
            dict.Clear();
            list.Clear();
            indexByKey.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair)
        {
            TValue value;
            if (dict.TryGetValue(pair.Key, out value))
            {
                return EqualityComparer<TValue>.Default.Equals(value, pair.Value);
            }
            else
            {
                return false;
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (array.Length - arrayIndex < dict.Count)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            foreach (var pair in dict)
            {
                array[arrayIndex] = pair;
                arrayIndex++;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> pair)
        {
            TValue value;
            if (dict.TryGetValue(pair.Key, out value))
            {
                bool valueMatch = EqualityComparer<TValue>.Default.Equals(value, pair.Value);
                if (valueMatch)
                {
                    return Remove(pair.Key);
                }
            }
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => dict.GetEnumerator();
    }
}