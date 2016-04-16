    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

namespace TopFashion
{
    public static class TypeHelper
    {
        public static Type FindIEnumerable(Type seqType)
        {
            if ((seqType != null) && (seqType != typeof(string)))
            {
                if (seqType.IsArray)
                {
                    return typeof(IEnumerable<>).MakeGenericType(new Type[] { seqType.GetElementType() });
                }
                if (seqType.IsGenericType)
                {
                    foreach (Type type in seqType.GetGenericArguments())
                    {
                        Type type2 = typeof(IEnumerable<>).MakeGenericType(new Type[] { type });
                        if (type2.IsAssignableFrom(seqType))
                        {
                            return type2;
                        }
                    }
                }
                Type[] interfaces = seqType.GetInterfaces();
                if ((interfaces != null) && (interfaces.Length > 0))
                {
                    foreach (Type type3 in interfaces)
                    {
                        Type type4 = FindIEnumerable(type3);
                        if (type4 != null)
                        {
                            return type4;
                        }
                    }
                }
                if ((seqType.BaseType != null) && (seqType.BaseType != typeof(object)))
                {
                    return FindIEnumerable(seqType.BaseType);
                }
            }
            return null;
        }

        public static object GetDefault(Type type)
        {
            if (!(!type.IsValueType || IsNullableType(type)))
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static Type GetElementType(Type seqType)
        {
            Type type = FindIEnumerable(seqType);
            if (type == null)
            {
                return seqType;
            }
            return type.GetGenericArguments()[0];
        }

        public static Type GetMemberType(MemberInfo mi)
        {
            FieldInfo info = mi as FieldInfo;
            if (info != null)
            {
                return info.FieldType;
            }
            PropertyInfo info2 = mi as PropertyInfo;
            if (info2 != null)
            {
                return info2.PropertyType;
            }
            EventInfo info3 = mi as EventInfo;
            if (info3 != null)
            {
                return info3.EventHandlerType;
            }
            MethodInfo info4 = mi as MethodInfo;
            if (info4 != null)
            {
                return info4.ReturnType;
            }
            return null;
        }

        public static Type GetNonNullableType(Type type)
        {
            if (IsNullableType(type))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }

        public static Type GetNullAssignableType(Type type)
        {
            if (!IsNullAssignable(type))
            {
                return typeof(Nullable<>).MakeGenericType(new Type[] { type });
            }
            return type;
        }

        public static ConstantExpression GetNullConstant(Type type)
        {
            return Expression.Constant(null, GetNullAssignableType(type));
        }

        public static Type GetSequenceType(Type elementType)
        {
            return typeof(IEnumerable<>).MakeGenericType(new Type[] { elementType });
        }

        public static bool IsFieldOrProperty(MemberInfo mi)
        {
            return ((mi is FieldInfo) || (mi is PropertyInfo));
        }

        public static bool IsInteger(Type type)
        {
            GetNonNullableType(type);
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return true;
            }
            return false;
        }

        public static bool IsNullableType(Type type)
        {
            return (((type != null) && type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        public static bool IsNullAssignable(Type type)
        {
            if (type.IsValueType)
            {
                return IsNullableType(type);
            }
            return true;
        }

        public static bool IsReadOnly(MemberInfo member)
        {
            MemberTypes memberType = member.MemberType;
            if (memberType != MemberTypes.Field)
            {
                if (memberType != MemberTypes.Property)
                {
                    return true;
                }
            }
            else
            {
                return ((((FieldInfo) member).Attributes & FieldAttributes.InitOnly) != FieldAttributes.PrivateScope);
            }
            PropertyInfo info = (PropertyInfo) member;
            if (info.CanWrite)
            {
                return (info.GetSetMethod() == null);
            }
            return true;
        }

        public static bool IsSimpleType(Type type)
        {
            if ((!type.IsPrimitive && (type != typeof(string))) && (type != typeof(decimal)))
            {
                return (type == typeof(DateTime));
            }
            return true;
        }

        public static void SetValue(MemberInfo mi, object obj, object value)
        {
            FieldInfo info = mi as FieldInfo;
            if (info != null)
            {
                info.SetValue(obj, value);
            }
            PropertyInfo info2 = mi as PropertyInfo;
            if (info2 != null)
            {
                info2.SetValue(obj, value, null);
            }
        }
    }
}

