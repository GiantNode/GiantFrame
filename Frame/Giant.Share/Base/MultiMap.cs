﻿using System.Collections.Generic;

namespace Giant.Share
{
    public class MultiMap<K,V>
    {
        private Dictionary<K, V> keyValueMap = new Dictionary<K, V>();
        private Dictionary<V, K> valueKeyMap = new Dictionary<V, K>();

        public bool TryGetValue(K key, out V value)
        {
            return keyValueMap.TryGetValue(key, out value);
        }

        public bool TryGetKey(V value, out K key)
        {
            return valueKeyMap.TryGetValue(value, out key);
        }

        public void AddRange(Dictionary<K,V> keyValues)
        {
            foreach (var kv in keyValues)
            {
                keyValueMap[kv.Key] = kv.Value;
                valueKeyMap[kv.Value] = kv.Key;
            }
        }
    }
}
