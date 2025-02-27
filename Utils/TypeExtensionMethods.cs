using System.Reflection;
using System.Text.RegularExpressions;

namespace Utils
{
    public static class TypeExtensionMethods
    {
        public static IEnumerable<Type> GetDerivedTypes(this Type type, Assembly entryAssembly)
        {
            return new List<Assembly>() { entryAssembly }
                .Concat(entryAssembly
                    .GetReferencedAssemblies()
                    .Select(a => Assembly.Load(a)))
                .SelectMany(a => a.GetTypes())
                .Where(t => t != type && type.IsAssignableFrom(t));
        }
        public static string Decapitalize(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return Char.ToLowerInvariant(value[0]) + value.Substring(1);
            }
            return value;
        }
        public static bool IsValidEmail(this string email)
        {
            var regex = new Regex(@"(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-zA-Z0-9-]*[a-zA-Z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");

            return !string.IsNullOrEmpty(email)
                ? regex.IsMatch(email)
                : true;
        }

        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            if(string.IsNullOrEmpty(phoneNumber))
            {
                return true;
            }
            return (phoneNumber.Length == 10);
        }
    }
}
