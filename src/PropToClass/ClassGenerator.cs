using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PropToClass
{
    internal class ClassGenerator
    {
        internal static string Generate(string text, string nameSpace, string className)
        {
            if (text.NotEmpty())
            {
                var properties = text.Split('\n')
                    .Where(m => m.Contains("="))
                    .Select(m => new
                    {
                        PropertyName = m.Split('=')[0]?.Trim(),
                        Value = m.Split('=')[1]?.Trim()
                    });

                var model = File.ReadAllText($@"{Directory.GetCurrentDirectory()}\templates\ClassTemplate.txt");

                var code = model
                            .Replace("[namespace]", nameSpace)
                            .Replace("[classname]", className)
                            .Replace("[props]", GetProperties(properties));

                return code;
            }

            return string.Empty;
        }

        private static string GetProperties(IEnumerable<dynamic> properties)
        {
            return properties?.Select(m => $@"        /// <summary>
        /// Gets or sets the {m.PropertyName}.
        /// </summary>
        public {GetPropertyType(m)} {m.PropertyName} {{ get; set; }}")
                              .Aggregate((m1, m2) => m1 + Environment.NewLine + Environment.NewLine + m2);
        }

        private static string GetPropertyType(dynamic m)
        {
            if (m.Value.StartsWith("\"") || m.Value.EndsWith("\""))
            {
                return "string";
            }
            //DateTime should be before the "new " prefix case, or need to be handled
            else if (m.Value.Contains("Date") || m.Value.Contains("/") || m.Value.ToLower().StartsWith("Is"))
            {
                return "DateTime";
            }
            //This case also need to be handled before the "new " prefix case
            else if (m.Value.Replace(",", string.Empty).StartsWith("new <"))
            {
                return "IEnumerable<" + m.Value.Substring(5).Replace(">", string.Empty).Replace(",", string.Empty) + ">";
            }
            else if (m.Value.Replace(",", string.Empty).StartsWith("new "))
            {
                return m.Value.Substring(4).Replace(",", string.Empty);
            }
            else if (m.Value.Contains("true") || m.Value.Contains("false") || m.Value.Contains("!") || m.Value.Contains("Has") || m.Value.StartsWith("Is"))
            {
                return "bool";
            }            
            else if (int.TryParse(m.Value.Replace(",", string.Empty), out int x))
            {
                return "int";
            }
            else if (decimal.TryParse(m.Value.Replace(",", string.Empty), out decimal d))
            {
                return "decimal";
            }
            else if (Guid.TryParse(m.Value.Replace(",", string.Empty), out Guid guid) || m.Value.Contains("Guid"))
            {
                return "Guid";
            }

            return "string";
        }
    }
}