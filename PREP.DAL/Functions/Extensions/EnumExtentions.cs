using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Functions.Extensions
{
    public static class EnumExtentions<T>
    {
        //public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute:Attribute
        //{
        //    return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<TAttribute>();
        //}
        public static string GetDisplayValue(T value)
        {
            var fieldinfo = value.GetType().GetField(value.ToString());
            var desc = fieldinfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (desc == null) return String.Empty;
            return (desc.Length > 0) ? desc[0].Name : value.ToString();
        }
    }
}
