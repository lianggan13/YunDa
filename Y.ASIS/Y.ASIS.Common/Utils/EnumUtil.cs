using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Y.ASIS.Common.Utils
{
    public static class EnumUtil
    {
        public static T MergeEnum<T>(this IEnumerable<T> enums) where T : Enum
        {
            var enumValue = 0;
            foreach (T item in enums)
            {
                enumValue |= Convert.ToInt32(item);
            }

            return (T)Enum.ToObject(typeof(T), enumValue);
        }

        public static string GetDescription(Type enumType, string fieldName = null)
        {
            DescriptionAttribute dna = null;
            if (string.IsNullOrEmpty(fieldName))
            {
                dna = (DescriptionAttribute)Attribute.GetCustomAttribute(enumType, typeof(DescriptionAttribute));
            }
            else
            {
                FieldInfo fi = enumType.GetField(fieldName);
                dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
            }
            return dna.Description;
        }

        public static string GetDescription<T>(this T t) where T : Enum
        {
            FieldInfo fi = t.GetType().GetField(t.ToString());
            DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

            return dna.Description;
        }

        public static string GetDescription(Enum e)
        {
            var fi = e.GetType().GetField(e.ToString());
            DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

            return dna.Description;
        }
    }
}
