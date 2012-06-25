/*
Copyright 2012 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
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
            StringToChange = StringToChange.Remove(1, 1);
            return StringToChange;
        }

        public static string Truncate(string source, int length, string ending = "...") {
            if (source.Length > length) {
                return source.Substring(0, length) + ending;
            }
            return source;
        }
        /// <summary>
        /// Checks if a string is numeric
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <returns></returns>
        public static bool IsNumeric(string s) {
            int parser;
            return int.TryParse(s, out parser);
        }
        /// <summary>
        /// Change the specified string to title case
        /// EX: String = "foo bar"
        /// Returns "Foo Bar"
        /// </summary>
        /// <param name="StringToChange">The string to change</param>
        /// <returns>A string version of TitleCase</returns>
        public static string TitleCase(string StringToChange) {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(StringToChange.ToLower());
        }

        //TODO: Comment...
        /// <summary>
        /// sdfsdf
        /// </summary>
        /// <param name="message">sdfdsf</param>
        /// <returns>sdfsdf</returns>
        public static bool ContainsBadChar(string message) {
            foreach (char ch in message)
                if (ch < 32 || ch > 128 || ch == '&')
                    return true;
            return false;

        }

    }
}
