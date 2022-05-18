// System Namespaces
using System;
using System.Linq;
using System.Runtime.Serialization;


// Application Namespaces


// Library Namespaces
using EasyConsole;


namespace CLI.Utility.Extensions
{
    public static class InputExtension
    {
        public static TEnum ReadEnumAttr<TEnum>(string prompt) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            Type type = typeof(TEnum);

            if (!type.IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            Output.WriteLine(prompt);
            EasyConsole.Menu menu = new();

            TEnum choice = default;
            foreach (var value in Enum.GetValues(type))
            {
                var enumType = typeof(TEnum);
                var name = Enum.GetName(enumType, value);

                var field = enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true);

                if (field.Length > 0)
                    menu.Add(((EnumMemberAttribute[])field).Single().Value, () => { choice = (TEnum)value; });
                else
                    menu.Add(Enum.GetName(type, value), () => { choice = (TEnum)value; });
            }

            menu.Display();

            return choice;
        }
    }
}
