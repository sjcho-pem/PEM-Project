using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;

namespace H_MarsSSD
{
    public class EnumArray<TEnum, TValue>
    {
        private TValue[] data;
        public TValue[] Data { get => data;}

        public int Count
        {
            get => data.Length;
        }
        

        public EnumArray(TEnum eCount)
        {
            data = new TValue[ConvertToIndex(eCount)];
        }

        public EnumArray(int nCount)
        {
            data = new TValue[nCount];
        }

        public TValue this[TEnum ekey]
        {
            get { return data[ConvertToIndex(ekey)]; }
            set { data[ConvertToIndex(ekey)] = value; }
        }

        public TValue this[int nkey]
        {
            get { return data[nkey]; }
            set { data[nkey] = value; }
        }

        int ConvertToIndex(TEnum key)
        {
            return CastTo<int>.From(key);
        }

        public void Clear()
        {
            Array.Clear(data, 0, data.Length);
        }

        public bool Equal(EnumArray<TEnum, TValue> enumArray)
        {
            return data.SequenceEqual(enumArray.data);
        }
    }

    // https://stackoverflow.com/questions/1189144/c-sharp-non-boxing-conversion-of-generic-enum-to-int
    /// <summary>
    /// Class to cast to type <see cref="T"/>
    /// </summary>
    /// <typeparam name="T">Target type</typeparam>
    public static class CastTo<T>
    {
        /// <summary>
        /// Casts <see cref="S"/> to <see cref="T"/>.
        /// This does not cause boxing for value types.
        /// Useful in generic methods.
        /// </summary>
        /// <typeparam name="S">Source type to cast from. Usually a generic type.</typeparam>
        public static T From<S>(S s)
        {
            return Cache<S>.caster(s);
        }

        private static class Cache<S>
        {
            public static readonly Func<S, T> caster = Get();

            private static Func<S, T> Get()
            {
                var p = Expression.Parameter(typeof(S), "s");
                var c = Expression.ConvertChecked(p, typeof(T));
                return Expression.Lambda<Func<S, T>>(c, p).Compile();
            }
        }


    }

    public static class EnumHelper
    {
        public static string GetDescription(Enum en)
        {

            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                //해당 text 값을 배열로 추출해 옵니다.
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DefaultValueAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DefaultValueAttribute)attrs[0]).Value.ToString();
                }
            }
            return en.ToString();
        }

        public static string GetCategory(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                //해당 text 값을 배열로 추출해 옵니다.
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DefaultValueAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DefaultValueAttribute)attrs[0]).Value.ToString();
                }
            }
            return en.ToString();
        }

        public static object GetDefaultValue(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                //해당 값을 배열로 추출해 옵니다.
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DefaultValueAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DefaultValueAttribute)attrs[0]).Value;
                }
            }
            return en.ToString();
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DefaultValueAttribute)) as DefaultValueAttribute;
                if (attribute != null)
                {
                    if (attribute.Value.ToString() == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
			return default(T);
        }

    }
}
