using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTastic.Model
{
    public static class ExtensionMethods
    {
        public static int ToInt(this string s)
        {
            int result = int.Parse(string.IsNullOrEmpty(s) ? "0" : s);
            return result;

        }
    }
}
