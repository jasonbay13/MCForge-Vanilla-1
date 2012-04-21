using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MCForge.Utils {

    /// <summary>
    /// Set of utils for manipulating strings
    /// </summary>
    public class StringUtils {

        /// <summary>
        /// Change your string's first character to uppercase 
        /// Specified string is a pointer
        /// </summary>
        /// <param name="StringToChange">The String message to change</param>
        /// <returns>String Version of CapitolizeFirstChar</returns>
        public static string CapitolizeFirstChar(string StringToChange) {
            if (String.IsNullOrWhiteSpace(StringToChange))
                return StringToChange;

            //Ex: StringToChange = "foobar"

            //Sets "foobar" to "Ffoobar"
            StringToChange = Char.ToUpper(StringToChange[0]) + StringToChange;

            //Removes the 2nd char (which was the char to capitolize) 
            //resulting in Foobar
            StringToChange =StringToChange.Remove(1, 1);
            return StringToChange;
        }

        /// <summary>
        /// Change the specified string to title case
        /// EX: String = "foo bar"
        /// Returns "Foo Bar"
        /// 
        /// The specified string is a pointer
        /// </summary>
        /// <param name="StringToChange">The string to change</param>
        /// <returns>A string version of TitleCase</returns>
        public static string TitleCase(string StringToChange) {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(StringToChange.ToLower());
        }

    }
}
