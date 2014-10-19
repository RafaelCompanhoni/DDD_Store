using System;
using System.Linq;

namespace LuaBijoux.Web.Infrastructure.Utils
{
    public class InputFormatting
    {
        public static string FirstCharToUpper(string input)
        {
            return String.IsNullOrEmpty(input) ? input : input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}