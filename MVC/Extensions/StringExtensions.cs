namespace MVC.Extensions
{
    public static class StringExtensions
    {
        public static bool IsCapitalized(this string str)
        {
            return !string.IsNullOrEmpty(str) && char.IsUpper(str[0]);
        }
    }
}
