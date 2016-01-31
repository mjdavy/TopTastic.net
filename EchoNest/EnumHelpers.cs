using System;
using System.ComponentModel;
using System.Reflection;

namespace EchoNest
{
    internal class EnumHelpers
    {
        internal static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            Description[] attributes = (Description[])fi.GetCustomAttributes(typeof(Description), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Text;
            }

            return value.ToString();
        }
    }
}