using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Service.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmptyTrimmed(this string value)
            => string.IsNullOrWhiteSpace(value);
    }
}
