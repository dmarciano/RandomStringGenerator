namespace SMC.Utilities.RSG
{
    public static class Extensions
    {
        public static Generator Parse(this string pattern)
        {
            return new Generator(pattern);
        }

        public static Generator Parse(this string pattern, IRandom random)
        {
            return new Generator(pattern, random);
        }
    }
}