namespace Y.ASIS.Common.ExtensionMethod
{
    public static class StringExt
    {
        public static string ToUpperFirst(this string s)
        {
            return s.Remove(1).ToUpper() + s.Substring(1);
        }
    }
}