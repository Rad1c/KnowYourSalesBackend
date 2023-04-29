using System.Globalization;

namespace BLL.Helper
{
    public class BaseHelper
    {
        /// <summary>
        /// Converts a string representation of a date time in the format "yyyy-MM-dd HH:mm:ss" to a DateTime object.
        /// </summary>
        /// <param name="dateTimeString">The string representation of the date time to convert.</param>
        /// <returns>A DateTime object representing the date time in the input string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input string is null.</exception>
        /// <exception cref="FormatException">Thrown if the input string is not in the correct format.</exception>
        public static DateTime? ConvertStringToDateTime(string dateTimeString)
        {
            try
            {
                DateTime dateTime = DateTime.ParseExact(dateTimeString,
                "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                return dateTime;
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is FormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="DateTime"/> value is greater than or equal to the current UTC date and time.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> value to compare with the current UTC date and time.</param>
        /// <returns>true if the specified <see cref="DateTime"/> value is greater than or equal to the current UTC date and time; otherwise, false.</returns>
        public static bool DateTimeGreaterThanNow(DateTime? dateTime)
        {
            if (dateTime is null) return false;

            return DateTime.UtcNow <= dateTime;
        }

        /// <summary>
        /// Converts a string in the format "yyyy-MM-dd" to a DateOnly object.
        /// </summary>
        /// <param name="dateToParse">The string to parse.</param>
        /// <returns>A DateOnly object representing the date in the input string.</returns>
        public static DateOnly ConvertStringToDateOnly(string dateToParse)
        {
            DateTime dateTime = ConvertStringToDateTime(dateToParse)!.Value;

            return DateOnly.FromDateTime(dateTime.Date);
        }

    }
}
