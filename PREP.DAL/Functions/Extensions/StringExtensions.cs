namespace PREP.DAL.Functions.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }
    }

}
