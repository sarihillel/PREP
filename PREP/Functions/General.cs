using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PREP.Functions
{
    public class General
    {

        public static object ToType<T>(object obj, T type)
        {
            object tmp = Activator.CreateInstance(Type.GetType(type.ToString()));
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                tmp.GetType().GetProperty(pi.Name).SetValue(tmp, pi.GetValue(obj, null), null);
            }
            return tmp;
        }

        public static object ToType<T>(object obj, Type type)
        {
            object tmp = Activator.CreateInstance(type);
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                tmp.GetType().GetProperty(pi.Name).SetValue(tmp, pi.GetValue(obj, null), null);
            }
            return tmp;
        }

        public static IEnumerable<T> Convert<T>(IEnumerable<object> enumerable)
        {
            foreach (var o in enumerable)
            {
                var objConvert = ToType<T>(o, typeof(T));

                yield return (T)objConvert;
            }
        }

    }
}