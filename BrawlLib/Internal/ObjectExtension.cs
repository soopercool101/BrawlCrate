using System.ComponentModel;
using System.Linq;

namespace BrawlLib.Internal
{
    // Use reflection to easily get properties
    public static class ObjectExtension
    {
        public static string GetDisplayName<T>(this T obj)
        {
            return ((DisplayNameAttribute) obj?.GetType().GetField(obj.ToString()).GetCustomAttributes(true)
                .FirstOrDefault(a => a is DisplayNameAttribute))?.DisplayName ?? obj?.ToString() ?? string.Empty;
        }

        public static string GetDescription<T>(this T obj)
        {
            var s = ((DescriptionAttribute) obj?.GetType().GetField(obj.ToString()).GetCustomAttributes(true)
                .FirstOrDefault(a => a is DescriptionAttribute))?.Description ?? obj?.ToString() ?? string.Empty;
            return s;
        }
    }
}