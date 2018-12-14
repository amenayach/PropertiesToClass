namespace PropToClass
{
    public static class Extensions
    {
        public static bool NotEmpty(this string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }

        public static string LowerFirstChar(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            if (text.Length == 1)
            {
                return text.ToLower();
            }

            return text[0].ToString().ToLower() + text.Substring(1);
        }
    }
}
