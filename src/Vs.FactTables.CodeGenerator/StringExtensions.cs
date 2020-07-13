using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vs.FactTables.CodeGenerator
{
    public static class StringExtensions
    {
        private static string ToPascalCase(this string s)
        {
            var words = s.Split(new[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(word => word.Substring(0, 1).ToUpper() +
                                         word.Substring(1).ToLower());

            var result = String.Concat(words);
            return result;
        }
    }
}
