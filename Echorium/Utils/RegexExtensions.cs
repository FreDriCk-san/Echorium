using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Echorium.Utils
{
    public static class RegexExtensions
    {
        public static bool IsValidRegex(this string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrWhiteSpace(pattern))
                return false;

            try
            {
                var regex = new Regex(pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}
