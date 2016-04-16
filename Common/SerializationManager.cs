    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;

namespace TopFashion
{
    public sealed class SerializationManager
    {
        private static Dictionary<Type, KeyValuePair<TypeSerializeHandler, TypeDeserializeHandler>> handlers = new Dictionary<Type, KeyValuePair<TypeSerializeHandler, TypeDeserializeHandler>>();

        static SerializationManager()
        {
            InitDefaultSerializeHandlers();
        }

        private static string ByteArrayToString(object obj)
        {
            return Convert.ToBase64String((byte[]) obj);
        }

        public static object Deserialize(Type returnType, string data)
        {
            if (data == null)
            {
                return null;
            }
            if (handlers.ContainsKey(returnType))
            {
                KeyValuePair<TypeSerializeHandler, TypeDeserializeHandler> pair = handlers[returnType];
                return pair.Value(data);
            }
            StringReader textReader = new StringReader(data);
            object obj2 = new XmlSerializer(returnType).Deserialize(textReader);
            textReader.Close();
            return obj2;
        }

        private static void InitDefaultSerializeHandlers()
        {
            RegisterSerializeHandler(typeof(string), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadString));
            RegisterSerializeHandler(typeof(int), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadInt));
            RegisterSerializeHandler(typeof(long), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadLong));
            RegisterSerializeHandler(typeof(short), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadShort));
            RegisterSerializeHandler(typeof(byte), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadByte));
            RegisterSerializeHandler(typeof(bool), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadBool));
            RegisterSerializeHandler(typeof(decimal), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadDecimal));
            RegisterSerializeHandler(typeof(char), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadChar));
            RegisterSerializeHandler(typeof(sbyte), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadSbyte));
            RegisterSerializeHandler(typeof(float), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadFloat));
            RegisterSerializeHandler(typeof(double), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadDouble));
            RegisterSerializeHandler(typeof(byte[]), new TypeSerializeHandler(SerializationManager.ByteArrayToString), new TypeDeserializeHandler(SerializationManager.LoadByteArray));
            RegisterSerializeHandler(typeof(Guid), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadGuid));
            RegisterSerializeHandler(typeof(DateTime), new TypeSerializeHandler(SerializationManager.ToString), new TypeDeserializeHandler(SerializationManager.LoadDateTime));
        }

        private static object LoadBool(string data)
        {
            return bool.Parse(data);
        }

        private static object LoadByte(string data)
        {
            return byte.Parse(data);
        }

        private static object LoadByteArray(string data)
        {
            return Convert.FromBase64String(data);
        }

        private static object LoadChar(string data)
        {
            return char.Parse(data);
        }

        private static object LoadDateTime(string data)
        {
            return DateTime.Parse(data);
        }

        private static object LoadDecimal(string data)
        {
            return decimal.Parse(data);
        }

        private static object LoadDouble(string data)
        {
            return double.Parse(data);
        }

        private static object LoadFloat(string data)
        {
            return float.Parse(data);
        }

        private static object LoadGuid(string data)
        {
            return new Guid(data);
        }

        private static object LoadInt(string data)
        {
            return int.Parse(data);
        }

        private static object LoadLong(string data)
        {
            return long.Parse(data);
        }

        private static object LoadSbyte(string data)
        {
            return sbyte.Parse(data);
        }

        private static object LoadShort(string data)
        {
            return short.Parse(data);
        }

        private static object LoadString(string data)
        {
            return data;
        }

        public static void RegisterSerializeHandler(Type type, TypeSerializeHandler serializeHandler, TypeDeserializeHandler deserializeHandler)
        {
            lock (handlers)
            {
                if (handlers.ContainsKey(type))
                {
                    handlers[type] = new KeyValuePair<TypeSerializeHandler, TypeDeserializeHandler>(serializeHandler, deserializeHandler);
                }
                else
                {
                    handlers.Add(type, new KeyValuePair<TypeSerializeHandler, TypeDeserializeHandler>(serializeHandler, deserializeHandler));
                }
            }
        }

        public static string Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (handlers.ContainsKey(obj.GetType()))
            {
                KeyValuePair<TypeSerializeHandler, TypeDeserializeHandler> pair = handlers[obj.GetType()];
                return pair.Key(obj);
            }
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            new XmlSerializer(obj.GetType()).Serialize((TextWriter) writer, obj);
            writer.Close();
            return sb.ToString();
        }

        private static string ToString(object obj)
        {
            return obj.ToString();
        }

        public static void UnregisterSerializeHandler(Type type)
        {
            lock (handlers)
            {
                if (handlers.ContainsKey(type))
                {
                    handlers.Remove(type);
                }
            }
        }

        public delegate object TypeDeserializeHandler(string data);

        public delegate string TypeSerializeHandler(object obj);
    }
}

