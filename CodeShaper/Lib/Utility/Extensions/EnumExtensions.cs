// System Namespaces
using System;
using System.Linq;
using System.Runtime.Serialization;


// Application Namespaces


// Library Namespaces


namespace Lib.Utility.Extensions
{
    public static class EnumExtensions
    {

        public static string ToEnumString<T>(this T type)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);

            var attributes = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true));

            if (attributes.Length < 1)
                return type.ToString();

            var enumMemberAttribute = attributes.Single();
            return enumMemberAttribute.Value;
        }

        public static T ToEnum<T>(this string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMember = (EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true);

                if (enumMember.Length < 1)
                    continue;

                var enumMemberAttribute = enumMember.Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }

            return default(T);
        }
    }
}
