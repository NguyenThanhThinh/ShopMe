namespace ShopMe.Domains.Extensions
{
    public static  class ExtensionMethods
    {
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
        public static bool IsValidString(this string input, bool acceptWhiteSpace = false)
        {
            if (acceptWhiteSpace)
            {
                return !input.IsNullOrEmpty();
            }
            else
            {
                return !input.IsNullOrEmpty() && !input.IsNullOrWhiteSpace();
            }
        }
        /// <summary>
        /// truncate input string with specify length
        /// </summary>
        /// <param name="input">a string of text to truncate</param>
        /// <param name="length">final length</param>
        /// <param name="ellipsedWord">if true, result text will have '...' at the end</param>
        /// <returns></returns>
        public static string Truncate(this string input, int length = 20, bool ellipsedWord = true)
        {
            if (input.IsValidString() == false) return string.Empty;

            if (input.Length < length) return input;

            if (ellipsedWord)
            {
                return input.Substring(0, length) + "..." ;
            }

            return input.Substring(0, length);
        }
    }
}
