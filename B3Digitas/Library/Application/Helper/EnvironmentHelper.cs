using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Helper
{
    public static class EnvironmentHelper
    {
        public static string GetConfig(string name, string defaultValue = null)
        {
            string value = Environment.GetEnvironmentVariable(name);
            //Environment.
            if (value == null)
            {
                return defaultValue;
            }

            return value;
        }
    }
}
