using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TopFashion
{
    [Serializable]
    public class FieldMap<T, K>
        where T : class
        where K : class
    {
        int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string name = "未命名映射";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public byte[] Map
        {
            get
            {
                byte[] buffer;
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, fieldMap);
                    buffer = ms.ToArray();
                }
                return buffer;
            }
            set
            {
                if (value != null)
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        fieldMap = (Dictionary<T, K>)bf.Deserialize(ms);
                    }
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }

        Dictionary<T, K> fieldMap;

        public Dictionary<T, K> Relation
        {
            get { return fieldMap; }
        }

        public FieldMap()
        {
            fieldMap = new Dictionary<T, K>();
        }

        public Dictionary<T, K>.Enumerator GetEnumerator()
        {
            return fieldMap.GetEnumerator();
        }

        public bool Add(KeyValuePair<T, K> relation)
        {
            if (!fieldMap.ContainsKey(relation.Key))
            {
                fieldMap.Add(relation.Key, relation.Value);
                return true;
            }
            return false;
        }

        public bool Add(T key, K value)
        {
            if (!fieldMap.ContainsKey(key))
            {
                fieldMap.Add(key, value);
                return true;
            }
            return false;
        }

        public bool Remove(T key)
        {
            return this.Remove(key);
        }

        public void Clear()
        {
            fieldMap.Clear();
        }

        public K this[T key]
        {
            get
            {
                if (fieldMap.ContainsKey(key))
                    return fieldMap[key];
                else
                    return null;
            }
            set
            {
                if (fieldMap.ContainsKey(key))
                    fieldMap[key] = value;
            }
        }

        public Dictionary<T, K>.KeyCollection Keys
        {
            get
            {
                return fieldMap.Keys;
            }
        }

        public Dictionary<T, K>.ValueCollection Values
        {
            get
            {
                return fieldMap.Values;
            }
        }

        public int Count
        {
            get
            {
                return fieldMap.Count;
            }
        }

        public void Import(Dictionary<T, K> map)
        {
            this.Clear();
            if (map != null && map.Count > 0)
            {
                foreach (KeyValuePair<T, K> relation in map)
                {
                    this.Add(relation);
                }
            }
        }
    }
}
