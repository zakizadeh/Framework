using System.Text.RegularExpressions;

namespace Htsc.Shared.ExcelHelper
{
    public static class TimeValidation
    {
        public static bool IsValidTime(string thetime)
        {
            if (string.IsNullOrEmpty(thetime)) return false;

            Regex checktime =
                new Regex(@"^([01]\d|2[0-3]):?([0-5]\d)$");

            return checktime.IsMatch(thetime);
        }
    }
}
