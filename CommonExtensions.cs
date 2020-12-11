// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonExtensions.cs" company="Common">
//   Copyright None.
// </copyright>
// <summary>
//   This is a Common Extensions class compiled over the course of many years
// </summary>
// ------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeThroughTheAges
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the Description attribute from an Enum value
        /// </summary>
        /// <param name="currentEnum">the enum to examine</param>
        /// <returns>the description as a string, if available. Otherwise the enum name as a string</returns>

        public static string GetDescription(this Enum currentEnum)
        {
            string description = string.Empty;

            if (currentEnum == null)
            {
                return string.Empty;
            }

            Type type = currentEnum.GetType();

            string name = Enum.GetName(type, currentEnum);
            if (name != null)
            {
                var fieldInfo = type.GetField(name);
                var attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    description = attribute.Description;
                }
            }

            if (string.IsNullOrEmpty(description))
            {
                description = currentEnum.ToString();
            }

            return description;
        }

        /// <summary>
        /// Gets the integer value of an enum
        /// </summary>
        /// <param name="value"> the enum to examine</param>
        /// <returns>the integer value of the enum</returns>
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// A shortcut to getting the string representation of the enum value
        /// This can be overloaded to give more meaningful representation
        /// </summary>
        /// <returns>The enum value as a string</returns>
        public static string Name(this Enum value)
        {
            return value.ToString();
        }

        /// <summary>Parses the object to enumeration.</summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentException">Parameter.</exception>
        public static Enum[] ParseToEnum(this object value, Type enumType)
        {
            if (value is Enum @enum)
            {
                return new Enum[1] {@enum};
            }

            var str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("parameter");
            }

            var strArray = str.Split(new char[2] {';', ','}, StringSplitOptions.RemoveEmptyEntries);
            var enumArray = new Enum[strArray.Length];
            for (var index = 0; index < strArray.Length; ++index)
            {
                enumArray[index] = (Enum) Enum.Parse(enumType, strArray[index], true);
            }

            return enumArray;
        }
    }

    public static class EnumUtilities<T> where T : struct, IConvertible
    {
        /// <summary>
        /// Converts an enum into a dictionary
        /// </summary>
        /// <returns>returns a dictionary with keys = enum values (int) and values = enum names (string)</returns>
        public static Dictionary<int, string> GetEnumAsDictionary()
        {
            var items = new Dictionary<int, string>();
            Type enumType = typeof(T);

            var basevalues = Enum.GetValues(enumType);
            var values = basevalues.Cast<int>().ToList();
            var keys = Enum.GetNames(enumType);

            for (int i = 0; i <= values.Count - 1; i++)
            {
                items.Add(values[i], keys[i]);
            }

            return items;
        }

        /// <summary>
        /// Converts an enum into a SortedList
        /// </summary>
        /// <returns>returns a SortedList with keys = enum names (string) and values = enum values (int) </returns>
        public static SortedList<string, int> GetEnumAsSortedList()
        {
            var items = new SortedList<string, int>();
            Type enumType = typeof(T);

            var basevalues = Enum.GetValues(enumType);
            var values = basevalues.Cast<int>().ToList();
            var keys = Enum.GetNames(enumType);

            for (int i = 0; i <= values.Count - 1; i++)
            {
                items.Add(keys[i], values[i]);
            }

            return items;
        }

        /// <summary>
        /// Converts an enum into a List of strings
        /// </summary>
        /// <returns>returns a list of sorted enum names</returns>
        public static List<string> GetEnumAsOrderedList()
        {
            Type enumType = typeof(T);

            var keys = Enum.GetNames(enumType);

            return keys.OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Converts an enum into a dictionary
        /// </summary>
        /// <returns>returns a dictionary with keys = enum values (int) and values = enum descriptions (string)</returns>
        public static Dictionary<int, string> GetEnumDescriptionsAsDictionary()
        {
            var items = new Dictionary<int, string>();
            Type enumType = typeof(T);

            var basevalues = Enum.GetValues(enumType);
            var values = basevalues.Cast<int>().ToList();
            var names = Enum.GetNames(enumType);
            var keys = names.Select(ToDescription).ToList();

            for (int i = 0; i <= values.Count - 1; i++)
            {
                items.Add(values[i], keys[i]);
            }

            return items;
        }

        /// <summary>
        /// Converts the descriptions of all items in an enum into a List of strings
        /// </summary>
        /// <returns>returns a list of unsorted enum descriptions</returns>
        public static List<string> GetEnumDescriptionsAsList()
        {
            Type enumType = typeof(T);

            var names = Enum.GetNames(enumType);
            var keys = names.Select(ToDescription).ToList();

            return keys;
        }

        /// <summary>
        /// Gets an enum based on the description attribute
        /// </summary>
        /// <param name="description">the description attribute of the enum</param>
        /// <returns>the corresponding enum value</returns>
        public static T FromDescription(string description)
        {
            var type = typeof(T);
            if (!type.GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (FieldInfo field in type.GetFields())
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    if (attribute.Description == description)
                    {
                        return (T) field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T) field.GetValue(null);
                    }
                }
            }

            return default;
        }

        /// <summary>
        /// Gets the Description attribute from an Enum name
        /// </summary>
        /// <param name="nameValue">the value to examine</param>
        /// <returns>the description as a string, if available. Otherwise the enum name as a string</returns>
        public static string ToDescription(string nameValue)
        {
            string description = string.Empty;
            var currentEnum = (T) Enum.Parse(typeof(T), nameValue);

            Type type = currentEnum.GetType();

            string name = Enum.GetName(type, currentEnum);
            if (name != null)
            {
                var fieldInfo = type.GetField(name);
                var attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    description = attribute.Description;
                }
            }

            if (string.IsNullOrEmpty(description))
            {
                description = currentEnum.ToString(CultureInfo.InvariantCulture);
            }

            return description;
        }

        /// <summary>
        /// Gets the Enum from its int value
        /// </summary>
        /// <param name="value">the value of the enum to examine</param>
        /// <returns>the corresponding enum item</returns>
        public static T FromIntValue(int value)
        {
            Type enumType = typeof(T);
            if (!enumType.GetTypeInfo().IsEnum)
            {
                return default;
            }

            return (T) Enum.ToObject(enumType, value);
        }

        /// <summary>
        /// Gets the Enum from its name value
        /// </summary>
        /// <param name="name">the name of the enum to examine</param>
        /// <returns>the corresponding enum item</returns>
        public static T FromName(string name)
        {
            return (T) Enum.Parse(typeof(T), name);
        }

        /// <summary>
        /// Gets the Enum from its name (case insensitive) value
        /// </summary>
        /// <param name="name">the name of the enum to examine</param>
        /// <param name="defaultValue">value to return if the input cannot be parsed</param>
        /// <returns>the corresponding enum item</returns>
        public static T? TryFromName(string name, T? defaultValue)
        {
            var couldParse = Enum.TryParse(typeof(T), name, true, out var output);
            return couldParse ? (T)output : defaultValue;
        }


        public static Enum[] ParseObjectToEnum(Type enumType, object value)
        {
            if (value is Enum @enum)
            {
                return new[] {@enum};
            }

            var str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("parameter");
            }

            var strArray = str.Split(new[] {';', ','}, StringSplitOptions.RemoveEmptyEntries);
            var enumArray = new Enum[strArray.Length];
            for (var index = 0; index < strArray.Length; ++index)
            {
                enumArray[index] = (Enum) Enum.Parse(enumType, strArray[index], true);
            }

            return enumArray;
        }
    }

    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type)
            : base(type)
        {
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType != typeof(string))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            if (value == null)
            {
                return string.Empty;
            }

            var fi = value.GetType().GetField(value.ToString());
            if (fi == null)
            {
                return string.Empty;
            }

            var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 && !string.IsNullOrEmpty(attributes[0].Description)
                ? attributes[0].Description
                : value.ToString();
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// Doubles any single apostrophe for use in a sql query
        /// </summary>
        /// <param name="input"></param>
        /// <returns>a string </returns>
        public static string CleanForSql(this string input)
        {
            return Regex.Replace(input, "'", "''");
        }

        /// <summary>
        /// Replaces  / \ , and " " characters with empty string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string Clean(this string input)
        {
            Regex.Replace(input, "/", "");
            Regex.Replace(input, "\\\\", "");
            Regex.Replace(input, ",", "");
            return Regex.Replace(input, " ", "");
        }

        /// <summary>
        /// Wraps a string in single quotes 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string Quote(this string input)
        {
            return "'" + input + "'";
        }

        /// <summary>
        /// Wraps a string in double quotes 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string QuoteDouble(this string input)
        {
            return "\"" + input + "\"";
        }

        /// <summary>
        /// Adds a back slash to the end of a string and removes any leading back slash
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string WithSlash(this string input)
        {
            if (input.Length == 0)
            {
                return input;
            }

            input = (input.StartsWith("\\", StringComparison.OrdinalIgnoreCase) ? input.Substring(1) : input);
            input = (input.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? input : input + "\\");
            return input;
        }

        /// <summary>
        /// Tries to convert a string to an integer
        /// </summary>
        /// <param name="input">the string to parse</param>
        /// <returns>the integer value, if valid. Otherwise -1</returns>
        public static int ToInt(this string input)
        {
            var isValid = int.TryParse(input, out var result);

            if (isValid)
            {
                return result;
            }

            return -1;

        }

        /// <summary>
        /// Tries to convert a string or number to a boolean
        /// This handles values of "True", "False", "1", "0"
        /// </summary>
        /// <param name="input">the string to parse</param>
        /// <returns>True or False based on the value of the string</returns>
        public static bool ToBool(this string input)
        {
            bool isBool = bool.TryParse(input, out var value);

            bool isNumber = int.TryParse(input, out var value2);

            if (isBool)
            {
                return value;
            }

            if (isNumber)
            {
                return value2 == 1;
            }

            return false;

        }

        /// <summary>
        /// Removes whitespace from a string. If specified will fill the white space with a replacement character
        /// </summary>
        /// <param name="value">the string to be updated</param>
        /// <returns>a whitespace free string</returns>
        public static string RemoveWhiteSpaces(this string value)
        {
            return RemoveWhiteSpaces(value, string.Empty);
        }

        /// <summary>
        /// Removes whitespace from a string. If specified will fill the white space with a replacement character
        /// </summary>
        /// <param name="value">the string to be updated</param>
        /// <param name="replaceWith">Optional: the character to replace the whitespace</param>
        /// <returns>a whitespace free string</returns>
        public static string RemoveWhiteSpaces(this string value, string replaceWith)
        {
            try
            {
                return Regex.Replace(value, "\\s+", replaceWith);
            }
            catch
            {
                return value;
            }
        }

        /// <summary>
        /// Provides a safe trim for a string that may or may not be null
        /// </summary>
        /// <param name="value">the string to trim</param>
        /// <returns>the trimmed string if the string has value, otherwise null</returns>
        public static string TrimNullSafe(this string value)
        {
            return !string.IsNullOrEmpty(value) ? value.Trim() : null;
        }

        /// <summary>
        /// Truncates a string to a specific length. 
        /// Unlike Substring, will return the whole string if the length specified is greater than the length of the string
        /// </summary>
        /// <param name="source">the string to truncate</param>
        /// <param name="length">the number of characters to return</param>
        /// <returns>the substring corresponding to the specified length, or the full string if the specified length is longer than the string length</returns>
        /// <remarks></remarks>
        public static string Truncate(this string source, int length)
        {
            if (source == null)
            {
                return null;
            }

            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }

            return source;
        }

        /// <summary>
        /// Truncates a string to a specific length. 
        /// Unlike Substring, will return the whole string if the length specified is greater than the length of the string
        /// </summary>
        /// <param name="source">the string to truncate</param>
        /// <param name="length">the number of characters to return</param>
        /// <param name="padding">the character to use in padding</param>
        /// <param name="padToEnd">true if the padding is applied to the end of the string, false if the padding is added to the end of the string</param>
        /// <returns>the substring corresponding to the specified length, or the full string plus padding if the specified length is longer than the string length</returns>
        public static string TruncateWithPadding(this string source, int length, char padding, bool padToEnd)
        {
            if (source == null)
            {
                source = string.Empty.PadRight(length, padding);
            }

            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            else
            {
                source = (padToEnd ? source.PadRight(length, padding) : source.PadLeft(length, padding));
            }

            return source;
        }

        /// <summary>
        /// A shortcut and extensible way of calling IsNullOrWhiteSpace
        /// </summary>
        /// <param name="value">the string value to be examined</param>
        /// <returns>true if the string is null or empty</returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Appends a string to the stringbuilder only if the string is not null or empty
        /// </summary>
        /// <param name="builder">the stringbuilder</param>
        /// <param name="toAppend">the string value to be appended</param>
        public static void AppendNullableLine(this StringBuilder builder, string toAppend)
        {
            if (string.IsNullOrEmpty(toAppend))
            {
                return;
            }

            builder.AppendLine(toAppend);
        }

        /// <summary>
        /// Returns whether the string value can be converted to an integer
        /// </summary>
        /// <param name="value">the string to be examined</param>
        /// <returns>returns True if the value can be converted</returns>
        public static bool IsInteger(this string value)
        {
            return int.TryParse(value, out _);
        }

        /// <summary>
        /// Returns the integer value of a string
        /// </summary>
        /// <param name="value">the string to be converted</param>
        /// <returns>returns the appropriate integer if the string is valid, otherwise returns -1</returns>
        public static int ToInteger(this string value)
        {
            var couldParse = int.TryParse(value, out var result);

            return (couldParse ? result : -1);
        }

        /// <summary>
        /// tokenize a string
        /// </summary>
        /// <param name="value">string to tokenize</param>
        /// <param name="delimiters">delimiters</param>
        /// <returns></returns>
        public static List<string> Tokenizer(this string value, char[] delimiters)
        {
            return Tokenizer(value, delimiters, false);
        }

        /// <summary>
        /// tokenize a string
        /// </summary>
        /// <param name="value">string to tokenize</param>
        /// <param name="delimiters">delimiters</param>
        /// <param name="convertToUpper">convert the tokens to upper - optional value defaults to false</param>
        /// <returns></returns>
        public static List<string> Tokenizer(this string value, char[] delimiters, bool convertToUpper)
        {
            if (convertToUpper)
            {
                value = value.ToUpper();
            }

            string[] tokens = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            // tokenize
            for (int index = 0; index <= tokens.Length - 1; index++)
            {
                tokens[index] = tokens[index].TrimStart(' ');
                tokens[index] = tokens[index].TrimEnd(' ');
                tokens[index] = tokens[index].Replace("'", string.Empty);
            }

            return tokens.ToList();
        }

        /// <summary>
        /// Creates an overload of the string.Contains method that peforms the comparison
        /// based on indexOf with a StringComparison enum parameter
        /// </summary>
        /// <param name="source">The based string to examine</param>
        /// <param name="toCheck">The string characters to look for</param>
        /// <param name="comp">[optional] the string comparison type. when not specified, the default value is StringComparison.InvariantCultureIgnoreCase</param>
        /// <returns><c>true</c> when the source string contains the toCheck string according to the stringComparison rule</returns>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;

        }

        /// <summary>
        /// Creates an extension the string.ContainsAny method that peforms the comparison with ignoring case
        /// based on indexOf with a StringComparison enum parameter
        /// </summary>
        /// <param name="source">The based string to examine</param>
        /// <param name="toCheck">The string characters to look for</param>
        /// <returns><c>true</c> when the source string contains the toCheck string according to the stringComparison rule</returns>
        public static bool ContainsAny(this string source, string toCheck)
        {
            return source.Contains(toCheck, StringComparison.OrdinalIgnoreCase);

        }

        /// <summary>
        /// Creates an extension the string.ContainsAny method that peforms the comparison with ignoring case
        /// based on indexOf with a StringComparison enum parameter
        /// </summary>
        /// <param name="source">The based string to examine</param>
        /// <param name="toCheck">The string characters to look for</param>
        /// <returns><c>true</c> when the source string contains the toCheck string according to the stringComparison rule</returns>
        public static bool StartsWithAny(this string source, string toCheck)
        {
            return source.StartsWith(toCheck, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// From an exception, creates a message containing the primary exception message plus any inner exception messages
        /// </summary>
        /// <param name="exception">an Exception object to interrogate</param>
        /// <returns>The message of each nested exception/inner exception collated into a newLine-delimited list</returns>
            public static string FullExceptionToString(this Exception exception)
        {
            var messages = exception
                          .FromHierarchy(ex => ex.InnerException)
                          .Select(ex => ex.Message);
            return string.Join(Environment.NewLine, messages);
        }

        /// <summary>
        /// From the specified string, finds the specified search character and returns the substring of the value up to that location
        /// </summary>
        /// <param name="value">the base string to examine</param>
        /// <param name="searchString">the character to search for within the base string</param>
        /// <returns>If the search string is not found, the base value will be returned.
        /// Otherwise, the substring of the base value starting at 0 up to (but not including) the location of the search string</returns>
        public static string SubstringUpTo(this string value, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return value;
            }

            if (!value.ContainsAny(searchString))
            {
                return string.Empty;
            }

            var index = value.IndexOf(searchString, 0, StringComparison.OrdinalIgnoreCase);
            if (index == 0)
            {
                return string.Empty;
            }

            return value.Substring(0, index);
        }

        /// <summary>
        /// From the specified string, finds the specified search character and returns the substring of the value up to that location
        /// </summary>
        /// <param name="value">the base string to examine</param>
        /// <param name="searchString">the character to search for within the base string</param>
        /// <param name="numberOfInstances">the number of instances of the search character to include</param>
        /// <param name="instancesToSkip">the number of instances of searchString to skip</param>
        /// <returns>If the search string is not found, the base value will be returned.
        /// Otherwise, the substring of the base value starting at 0 up to (but not including) the location of the search string</returns>
        public static string SubstringUpTo(this string value, string searchString, int numberOfInstances, int instancesToSkip)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return value;
            }

            if (!value.ContainsAny(searchString))
            {
                return string.Empty;
            }

            var skipIndex = instancesToSkip == 0 ? 0 : value.IndexOfToX(searchString, instancesToSkip);
            var endIndex = value.IndexOfToX(searchString, numberOfInstances, skipIndex);
            
            return value.Substring(skipIndex, endIndex - skipIndex -  1);
        }

        /// <summary>
        /// From the specified string, finds the specified search character and returns the substring of the value after that location
        /// </summary>
        /// <param name="value">the base string to examine</param>
        /// <param name="searchString">the character to search for within the base string</param>
        /// <returns>If the search string is not found, the base value will be returned.
        /// Otherwise, the substring of the base value starting at the loaction of the search string through the end of the value</returns>
        public static string SubstringAfter(this string value, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return value;
            }

            if (!value.ContainsAny(searchString))
            {
                return string.Empty;
            }

            var index = value.IndexOf(searchString, 0, StringComparison.OrdinalIgnoreCase);
            if (index == 0)
            {
                return string.Empty;
            }

            return value.Substring(index);
        }

        /// <summary>
        /// From the specified string, finds the specified search character and returns the substring of the value up to that location
        /// </summary>
        /// <param name="value">the base string to examine</param>
        /// <param name="firstString">the first character to search for within the base string</param>
        /// <param name="secondString">the last character to search for within the base string</param>
        /// <returns>If either of the search strings are not found, the base value will be returned.
        /// Otherwise, the substring of the base value starting at the index of the firstString up to (but not including) the location of the secondString </returns>
        public static string SubstringBetween(this string value, string firstString, string secondString)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var startIndex = value.IndexOf(firstString, 0, StringComparison.OrdinalIgnoreCase);
            var endIndex = value.LastIndexOf(secondString, StringComparison.OrdinalIgnoreCase);

            if (startIndex == -1 || endIndex == -1 || (startIndex > endIndex))
            {
                return string.Empty;
            }

            startIndex++;
            return value.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// From the specified string, finds the # instance of the specified search character and returns the index of it
        /// </summary>
        /// <param name="value">the base string to examine</param>
        /// <param name="searchString">the character to search for within the base string</param>
        /// <param name="numberOfInstances">the number of instances of the search character to include</param>
        /// <returns>If the search string is not found, the string length will be returned.
        /// Otherwise, the index of the nth instance of the searchString will be returned</returns>
        public static int IndexOfToX(this string value, string searchString, int numberOfInstances)
        {
            var index = 0;
            for (int i = 0; i < numberOfInstances; i++)
            {
                index = value.IndexOf(searchString, index, StringComparison.OrdinalIgnoreCase) + 1;
            }

            if(index == 0)
            {
                index = value.Length;
            }

            return index;
        }

        /// <summary>
        /// From the specified string, finds the # instance of the specified search character and returns the index of it
        /// </summary>
        /// <param name="value">the base string to examine</param>
        /// <param name="searchString">the character to search for within the base string</param>
        /// <param name="numberOfInstances">the number of instances of the search character to include</param>
        /// <param name="startIndex">the start index for the search</param>
        /// <returns>If the search string is not found, the string length will be returned.
        /// Otherwise, the index of the nth instance of the searchString will be returned</returns>
        public static int IndexOfToX(this string value, string searchString, int numberOfInstances, int startIndex)
        {
            var index = startIndex;
            for (int i = 0; i < numberOfInstances; i++)
            {
                index = value.IndexOf(searchString, index, StringComparison.OrdinalIgnoreCase) + 1;
            }

            if (index == 0)
            {
                index = value.Length;
            }

            return index;
        }

        public static string[] SplitQuotedStrings(this string value, string delimiter, string escape)
        {
            var items = new List<Tuple<string, string>>();

            int start = value.IndexOf(escape, StringComparison.OrdinalIgnoreCase);

            while (start > -1)
            {
                int stop = value.IndexOf(escape, start + 1, StringComparison.OrdinalIgnoreCase);
                if (stop > -1 && stop > start)
                {
                    var substring = value.Substring(start + 1, (stop - (start + 1)));
                    var escapedSubstring = substring.Replace(delimiter, "|");
                    items.Add(new Tuple<string, string>(substring, escapedSubstring));
                }
                start = value.IndexOf(escape, stop + 1, StringComparison.OrdinalIgnoreCase);
            }

            foreach (var part in items)
            {
                value = value.Replace(part.Item1, part.Item2);
            }

            var splitString = value.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();
            foreach (var item in splitString)
            {
                result.Add(item.Replace("|", delimiter));
            }

            return result.ToArray();

        }

        public static string CleanQuotedStrings(this string value, string delimiter)
        {
            var items = new List<Tuple<string, string>>();

            string quote = "\"";
            int start = value.IndexOf(quote, StringComparison.OrdinalIgnoreCase);

            while (start > -1)
            {
                int stop = value.IndexOf(quote, start + 1, StringComparison.OrdinalIgnoreCase);
                if (stop > -1 && stop > start)
                {
                    var substring = value.Substring(start + 1, (stop - (start + 1)));
                    var escapedSubstring = substring.Replace(delimiter, "|");
                    items.Add(new Tuple<string, string>(substring, escapedSubstring));
                }
                start = value.IndexOf(quote, stop + 1, StringComparison.OrdinalIgnoreCase);
            }

            foreach (var part in items)
            {
                value = value.Replace(part.Item1, part.Item2);
            }

            return value;
        }

        /// <summary>
        /// A case-insensitive Equals comparison of two strings
        /// </summary>
        public static bool EqualsCi(this string value, string comparableString)
        {
            //return value.ToLowerInvariant() == comparableString.ToLowerInvariant();
            return string.Equals(value, comparableString, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Executes a contains call on the stringArray using an IgnoreCase string comparison
        /// </summary>
        public static bool ContainsCi(this string[] stringArray, string value)
        {
            return stringArray.Contains(value, StringComparer.OrdinalIgnoreCase);
        }

    }

    public static class NumberExtensions
    {
        /// <summary>
        /// converts a long into a string version of its ordinal value
        /// </summary>
        /// <param name="number">the long to be converted</param>
        /// <returns>a string corresponding to the ordinal value. i.e. 7 => 7th or 21 => 21st</returns>
        public static string ToOrdinal(this long number)
        {
            if (number < 0)
            {
                return number.ToString();
            }

            long rem = number % 100;
            if (rem >= 11 && rem <= 13)
            {
                return number + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
        }

        /// <summary>
        /// converts an integer into a string version of its ordinal value
        /// </summary>
        /// <param name="number">the integer to be converted</param>
        /// <returns>a string corresponding to the ordinal value. i.e. 7 => 7th or 21 => 21st</returns>
        public static string ToOrdinal(this int number)
        {
            return Convert.ToInt64(number).ToOrdinal();
        }

        /// <summary>
        /// converts a number into its word representation
        /// </summary>
        /// <param name="number">the number to be converted</param>
        /// <returns>a string corresponding to the number value. i.e. 7 => Seven or 153 => one hundred fifty three</returns>
        public static string ToWords(this int number)
        {
            if (number == 0)
            {
                return "zero";
            }

            if (number < 0)
            {
                return "negative " + ToWords(Math.Abs(number));
            }

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += ToWords(number / 1000000).Trim() + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += ToWords(number / 1000).Trim() + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ToWords(number / 100).Trim() + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (number < 20)
                {
                    words += UnitsMap[number];
                }
                else
                {
                    words += TensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        words += "-" + UnitsMap[number % 10];
                    }
                }
            }

            return words;
        }

        private static readonly string[] UnitsMap =
        {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve",
            "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
        };

        private static readonly string[] TensMap =
            {"zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"};

        /// <summary>
        /// Converts double value of radians to degrees.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        public static double RadiansToDegrees(this double radians)
        {
            return radians * (180.0 / Math.PI);
        }

        /// <summary>
        /// Converts nullable double value of radians to degrees.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>The angle in degrees. if the radians value is null, null is returned</returns>
        public static double? RadiansToDegrees(this double? radians)
        {
            if (!radians.HasValue)
            {
                return null;
            }

            return RadiansToDegrees(radians.Value);
        }

        /// <summary>
        /// Rounds two double values to a specified number of decimal places and returns equality
        /// </summary>
        /// <param name="firstValue">the primary value</param>
        /// <param name="secondValue">the value to compare</param>
        /// <param name="precision">digits of precision (defaults to zero)</param>
        /// <returns>true if the rounded values are equal</returns>
        public static bool AreEqual(this double firstValue, double secondValue, int precision = 0)
        {
            if (precision < 0)
            {
                return false;
            }

            var firstValueRounded = Math.Round(firstValue, precision, MidpointRounding.AwayFromZero);
            var secondValueRounded = Math.Round(secondValue, precision, MidpointRounding.AwayFromZero);

            double difference = Math.Abs(firstValueRounded * .00001);

            return Math.Abs(firstValueRounded - secondValueRounded) <= difference;
        }

        /// <summary>
        /// Rounds two double values to a specified number of decimal places and returns equality
        /// </summary>
        /// <param name="firstValue">the nullable primary value</param>
        /// <param name="secondValue">the nullable value to compare</param>
        /// <param name="precision">digits of precision (defaults to zero)</param>
        /// <returns>true if the rounded values are equal</returns>
        public static bool AreEqual(this double? firstValue, double? secondValue, int precision = 0)
        {
            if (precision < 0)
            {
                return false;
            }

            if (firstValue.HasValue && !secondValue.HasValue || !firstValue.HasValue && secondValue.HasValue)
            {
                return false;
            }

            if (!firstValue.HasValue) //if the first value is null, the secondValue will also be null at this point
            {
                return true;
            }

            var firstValueRounded = Math.Round(firstValue.Value, precision, MidpointRounding.AwayFromZero);
            var secondValueRounded = Math.Round(secondValue.Value, precision, MidpointRounding.AwayFromZero);

            double difference = Math.Abs(firstValueRounded * .00001);

            return Math.Abs(firstValueRounded - secondValueRounded) <= difference;
        }

        /// <summary>
        /// Rounds two double values to a specified number of decimal places and returns equality
        /// </summary>
        /// <param name="firstValue">the primary value</param>
        /// <param name="secondValue">the nullable value to compare</param>
        /// <param name="precision">digits of precision (defaults to zero)</param>
        /// <returns>true if the rounded values are equal</returns>
        public static bool AreEqual(this double firstValue, double? secondValue, int precision = 0)
        {
            if (precision < 0)
            {
                return false;
            }

            if (!secondValue.HasValue) //if the first value is null, the secondValue will also be null at this point
            {
                return false;
            }

            var firstValueRounded = Math.Round(firstValue, precision, MidpointRounding.AwayFromZero);
            var secondValueRounded = Math.Round(secondValue.Value, precision, MidpointRounding.AwayFromZero);

            double difference = Math.Abs(firstValueRounded * .00001);

            return Math.Abs(firstValueRounded - secondValueRounded) <= difference;
        }

        /// <summary>
        /// Rounds two double values to a specified number of decimal places and returns equality
        /// </summary>
        /// <param name="firstValue">the nullable primary value</param>
        /// <param name="secondValue">the value to compare</param>
        /// <param name="precision">digits of precision (defaults to zero)</param>
        /// <returns>true if the rounded values are equal</returns>
        public static bool AreEqual(this double? firstValue, double secondValue, int precision = 0)
        {
            if (precision < 0)
            {
                return false;
            }

            if (!firstValue.HasValue) //if the first value is null, the secondValue will also be null at this point
            {
                return false;
            }

            var firstValueRounded = Math.Round(firstValue.Value, precision, MidpointRounding.AwayFromZero);
            var secondValueRounded = Math.Round(secondValue, precision, MidpointRounding.AwayFromZero);

            double difference = Math.Abs(firstValueRounded * .00001);

            return Math.Abs(firstValueRounded - secondValueRounded) <= difference;
        }

        /// <summary>
        /// Determines whether an integer is between two values
        /// </summary>
        /// <param name="value">the value to be evaluated</param>
        /// <param name="lowerBoundry"> the lower end of the between</param>
        /// <param name="upperBoundry"> the upper end of the between</param>
        /// <param name="isInclusiveOnBothBoundaries"> whether to compare both boundaries using "greater than" and "less than" (when false) 
        /// or "greater than or equal to" and "less than or equal to (when true)"</param>
        /// <returns>true if the version falls between the upper and lower bounds, otherwise false.</returns>
        public static bool IsBetween(this int value, int lowerBoundry, int upperBoundry,
                                     bool isInclusiveOnBothBoundaries = true)
        {
            return IsBetween(value, lowerBoundry, upperBoundry, isInclusiveOnBothBoundaries,
                             isInclusiveOnBothBoundaries);
        }

        /// <summary>
        /// Determines whether an integer is between two values
        /// </summary>
        /// <param name="value">the value to be evaluated</param>
        /// <param name="lowerBoundry"> the lower end of the between</param>
        /// <param name="upperBoundry"> the upper end of the between</param>
        /// <param name="isLowerInclusive"> whether to compare the lower boundry using "greater than" and "less than" (when false) 
        /// or "greater than or equal to" and "less than or equal to (when true)"</param>
        /// <param name="isUpperInclusive"> whether to compare the upper boundry using "greater than" and "less than" (when false) 
        /// or "greater than or equal to" and "less than or equal to (when true)"</param>
        /// <returns>true if the version falls between the upper and lower bounds, otherwise false.</returns>
        public static bool IsBetween(this int value, int lowerBoundry, int upperBoundry, bool isLowerInclusive,
                                     bool isUpperInclusive)
        {
            var lowerIsValid = isLowerInclusive
                ? value >= lowerBoundry
                : value > lowerBoundry;

            var upperIsValid = isUpperInclusive
                ? value <= upperBoundry
                : value < upperBoundry;

            return lowerIsValid && upperIsValid;
        }

        /// <summary>
        /// Returns the lesser of two integers
        /// </summary>
        /// <param name="value">The value to be evaluated</param>
        /// <param name="compareTo">The upper boundary of the evaluation</param>
        /// <returns>The smaller of the two numbers</returns>
        public static int ToLesserValueThan(this int value, int compareTo)
        {
            return value < compareTo ? value : compareTo;
        }

        /// <summary>
        /// Returns the greater of two integers
        /// </summary>
        /// <param name="value">The value to be evaluated</param>
        /// <param name="compareTo">The upper boundary of the evaluation</param>
        /// <returns>The greater of the two numbers</returns>
        public static int ToGreaterValueThan(this int value, int compareTo)
        {
            return value > compareTo ? value : compareTo;
        }
    }

    public static class DateTimeExtensions
    {
        /// <summary>
        /// Creates a new copy of an existing DateTime
        /// </summary>
        /// <param name="input">The dateTime object to be duplicated</param>
        /// <returns>returns a new dateTime with the same Year, Month, Day, Hour, Minute, and Kind values as the input</returns>
        public static DateTime ToNew(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, 0, input.Kind);
        }

        /// <summary>
        /// Converts a DateTime into a Date with zero time
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <returns>the date with time set to the beginning of the day</returns>
        public static DateTime StartOfDay(this DateTime input)
        {
            return input.Date;
        }

        /// <summary>
        /// Converts a DateTime into a Date with zero time
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <returns>the date with time set to the beginning of the day, or if the date is null, then returns null</returns>
        public static DateTime? StartOfDay(this DateTime? input)
        {
            return input?.StartOfDay();
        }

        /// <summary>
        /// Converts a DateTime into a Date with maximum time
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <returns>the date with time set to the end of the day</returns>
        public static DateTime EndOfDay(this DateTime input)
        {
            return input.Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Converts a DateTime into a Date with maximum time
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <returns>the date with time set to the end of the day, or if the date is null, then returns null</returns>
        public static DateTime? EndOfDay(this DateTime? input)
        {
            return input?.EndOfDay();
        }

        /// <summary>
        /// Returns the day after the specified date
        /// </summary>
        /// <param name="input">the date to increment</param>
        /// <returns>the date after the specified date</returns>
        public static DateTime NextDay(this DateTime input)
        {
            return input.Date.AddDays(1).Date;
        }

        /// <summary>
        /// Returns the day after the specified date
        /// </summary>
        /// <param name="input">the date to increment</param>
        /// <returns>the date after the specified date, or if the date is null, then returns null</returns>
        public static DateTime? NextDay(this DateTime? input)
        {
            return input?.NextDay();
        }

        /// <summary>
        /// Determines whether the specified date is between the start and end dates
        /// </summary>
        /// <param name="input">the date to increment</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <returns>returns true if the input is (inclusively) between the start and end dates
        /// returns false if the date falls outside of the start or end date OR  if any of the values are null</returns>
        public static bool Between(this DateTime? input, DateTime? startDate, DateTime? endDate)
        {
            if (!input.HasValue)
            {
                return false;
            }

            return (!startDate.HasValue || input >= startDate) && (!endDate.HasValue || input <= endDate);
        }

        /// <summary>
        /// Checks to see if a date is in the past or not set
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <returns>True if the the date is not set or occurs before the the current date. Otherwise returns false</returns>
        public static bool IsInPastOrNotSet(this DateTime input)
        {
            return IsInPastOrNotSet(input, false);
        }

        /// <summary>
        /// Checks to see if a date is in the past or not set
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <param name="inclusive">whether the calculation should return true if the date equals today</param>
        /// <returns>True if the the date is not set or occurs before the the current date. Otherwise returns false</returns>
        public static bool IsInPastOrNotSet(this DateTime input, bool inclusive)
        {
            if (IsNotSet(input))
            {
                return true;
            }

            if (inclusive)
            {
                return input <= DateTime.Today;
            }
            else
            {
                return input < DateTime.Today;
            }
        }

        /// <summary>
        /// Checks to see if a date is in the future or not set
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <returns>True if the the date occurs after the the current date. Otherwise returns false</returns>
        public static bool IsInFutureOrNotSet(this DateTime input)
        {
            return IsInFutureOrNotSet(input, false);
        }

        /// <summary>
        /// Checks to see if a date is in the future or not set
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <param name="inclusive">whether the calculation should return true if the date equals today</param>
        /// <returns>True if the the date occurs after the the current date. Otherwise returns false</returns>
        public static bool IsInFutureOrNotSet(this DateTime input, bool inclusive)
        {
            if (IsNotSet(input))
            {
                return true;
            }

            if (inclusive)
            {
                return input >= DateTime.Today;
            }
            else
            {
                return input > DateTime.Today;
            }
        }

        /// <summary>
        /// Checks to see if a date is in the past
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <returns>True if the the date occurs before the the current date. Returns false if the date is not set or occurs in the past</returns>
        public static bool IsInPast(this DateTime input)
        {
            return IsInPast(input, false);
        }

        /// <summary>
        /// Checks to see if a date is in the past
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <param name="inclusive">whether the calculation should return true if the date equals today</param>
        /// <returns>True if the the date occurs before the the current date. Returns false if the date is not set or occurs in the past</returns>
        public static bool IsInPast(this DateTime input, bool inclusive)
        {
            if (IsNotSet(input))
            {
                return false;
            }

            if (inclusive)
            {
                return input <= DateTime.Today;
            }
            else
            {
                return input < DateTime.Today;
            }
        }

        /// <summary>
        /// Checks to see if a date is in the future
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <returns>True if the the date occurs after the the current date. Returns false if the date is not set or occurs in the past</returns>
        public static bool IsInFuture(this DateTime input)
        {
            return IsInFuture(input, false);
        }

        /// <summary>
        /// Checks to see if a date is in the future
        /// </summary>
        /// <param name="input">The date to convert</param>
        /// <param name="inclusive">whether the calculation should return true if the date equals today</param>
        /// <returns>True if the the date occurs after the the current date. Returns false if the date is not set or occurs in the past</returns>
        public static bool IsInFuture(this DateTime input, bool inclusive)
        {

            if (IsNotSet(input))
            {
                return false;
            }

            if (inclusive)
            {
                return input >= DateTime.Today;
            }
            else
            {
                return input > DateTime.Today;
            }

        }

        /// <summary>
        /// Checks to see if a date is null or has a default value
        /// </summary>
        /// <param name="input">The date to check</param>
        /// <returns>True if the the date occurs before the the current date. Otherwise returns false</returns>
        public static bool IsNotSet(this DateTime? input)
        {
            return IsNotSet(input, null);
        }

        /// <summary>
        /// Checks to see if a date is null or has a default value
        /// </summary>
        /// <param name="input">The date to check</param>
        /// <param name="defaultValue">an optional defaultValue other than Nothing/1-1-1753/DateTime.MinValue</param>
        /// <returns>True if the the date occurs before the the current date. Otherwise returns false</returns>
        public static bool IsNotSet(this DateTime? input, DateTime? defaultValue)
        {
            if (input == null || input == DateTime.MinValue)
            {
                return true;
            }

            return input == defaultValue;
        }

        /// <summary>
        /// Returns the date of the day corresponding to the first day of the week (as specified)
        /// </summary>
        /// <param name="input">the date for comparison</param>
        /// <param name="day">the day representing the start of the week</param>
        /// <returns>the dateTime of the first day of the week corresponding to the specified day/dayofweek</returns>
        public static DateTime StartOfWeek(this DateTime input, DayOfWeek day)
        {
            var diff = input.DayOfWeek - day;
            if (diff < 0)
            {
                diff += 7;
            }

            return input.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Returns the date of the day corresponding to the first day of the Quarter for the specified date
        /// </summary>
        /// <param name="input">the date for comparison</param>
        /// <returns>the date corresponding to the first date of the quarter containing the specified input</returns>
        public static DateTime StartOfQuarter(this DateTime input)
        {
            var quarter = (input.Month - 1) / 3 + 1;
            return new DateTime(input.Year, 3 * quarter - 2, 1);
        }

        /// <summary>
        /// Returns the date of the day corresponding to the last day of the Quarter for the specified date
        /// </summary>
        /// <param name="input">the date for comparison</param>
        /// <returns>the date corresponding to the last date of the quarter containing the specified input</returns>
        public static DateTime EndOfQuarter(this DateTime input)
        {
            var quarter = (input.Month - 1) / 3 + 1;
            return new DateTime(input.Year, 3 * quarter + 1, 1).AddDays(-1);
        }

        private const int NumberOfDaysInWeek = 7;

        public static DateTime AddDuration(this DateTime date, int duration, DurationUnit unit)
        {
            if (duration < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            if (duration == 0)
            {
                return date;
            }

            DateTime result;

            switch (unit)
            {
                case DurationUnit.Days:
                    result = date.AddDays(duration - 1);
                    break;
                case DurationUnit.Weeks:
                    result = date.AddDays((duration * NumberOfDaysInWeek) - 1);
                    break;
                default: //DurationUnit.Months
                    result = date.AddMonths(duration).AddDays(-1);
                    break;
            }

            return result;
        }

        public static DateTime SubtractDuration(this DateTime date, int duration, DurationUnit unit)
        {
            if (duration < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            if (duration == 0)
            {
                return date;
            }

            DateTime result;

            switch (unit)
            {
                case DurationUnit.Days:
                    result = date.AddDays((-1 * duration) + 1);
                    break;
                case DurationUnit.Weeks:
                    result = date.AddDays((-1 * (duration * NumberOfDaysInWeek)) + 1);
                    break;
                default: //DurationUnit.Months
                    result = date.AddMonths((-1 * duration)).AddDays(1);
                    break;
            }

            return result;
        }

        public static int ElapsedMonths(this DateTime firstDate, DateTime secondDate)
        {
            var months = ((firstDate.Year - secondDate.Year) * 12) + firstDate.Month - secondDate.Month;

            return months;
        }
    }

    public static class ListExtensions
    {

        /// <summary>
        /// Add a range of items to a collection.
        /// </summary>
        /// <typeparam name="T">Type of objects within the collection.</typeparam>
        /// <param name="collection">The collection to add items to.</param>
        /// <param name="items">The items to add to the collection.</param>
        /// <returns>The collection.</returns>
        /// <exception cref="ArgumentNullException">An <see cref="System.ArgumentNullException"/> is thrown if <paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
        public static Collection<T> AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var each in items)
            {
                collection.Add(each);
            }

            return collection;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> data)
        {
            return new ObservableCollection<T>(data);
        }

        public static StringCollection ToCollection(this List<string> listOfString)
        {
            var collection = new StringCollection();
            foreach (var item in listOfString)
            {
                collection.Add(item);
            }

            return collection;
        }

        public static List<string> ToList(this StringCollection collection)
        {
            return collection.Cast<string>().ToList();
        }

        public static string ToBuiltString<T>(this List<T> currentList)
        {
            var sb = new StringBuilder();
            foreach (var entry in currentList)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.ToString()))
                {
                    continue;
                }

                sb.AppendLine(entry.ToString());
            }

            return sb.ToString();
        }

        public static string ToBuiltString<T, TU>(this Dictionary<T, TU> currentList)
        {
            var sb = new StringBuilder();
            foreach (var entry in currentList)
            {
                if (string.IsNullOrEmpty(entry.Key?.ToString()) || string.IsNullOrEmpty(entry.Value?.ToString()))
                {
                    continue;
                }

                sb.AppendLine($"{entry.Key}: {entry.Value}");
            }

            return sb.ToString();
        }

        public static void MergeUnique<T>(this List<T> currentList, List<T> listToMerge)
        {
            foreach (var entry in listToMerge.Where(entry => !currentList.Contains(entry)))
            {
                currentList.Add(entry);
            }
        }

        public static void MergeUnique<T>(this List<T> currentList, List<T> listToMerge, string keyProperty)
        {
            PropertyInfo keyPropertyInfo = typeof(T).GetProperty(keyProperty);

            var keyList = currentList.Select(item => keyPropertyInfo?.GetValue(item))
                                     .Select(value => value?.ToString())
                                     .ToList();

            foreach (var entry in listToMerge)
            {
                var value = keyPropertyInfo?.GetValue(entry);
                if (!keyList.Contains(value) && !currentList.Contains(entry))
                {
                    currentList.Add(entry);
                }
            }
        }

        public static void MergeUnique<T>(this ObservableCollection<T> currentList, List<T> listToMerge)
        {
            foreach (var entry in listToMerge.Where(entry => !currentList.Contains(entry)))
            {
                currentList.Add(entry);
            }
        }

        public static void MergeUnique<T>(this ObservableCollection<T> currentList, ObservableCollection<T> listToMerge)
        {
            foreach (var entry in listToMerge.Where(entry => !currentList.Contains(entry)))
            {
                currentList.Add(entry);
            }
        }

        public static void AddUnique<T>(this List<T> currentList, T itemToAdd)
        {
            if (!currentList.Contains(itemToAdd))
            {
                currentList.Add(itemToAdd);
            }
        }

        public static void AddUnique<T, U>(this Dictionary<T, U> currentList, T keyToAdd, U valueToAdd)
        {
            if (!currentList.ContainsKey(keyToAdd))
            {
                currentList.Add(keyToAdd, valueToAdd);
            }
        }

        public static void RemoveOutliers<T>(this ObservableCollection<T> currentList, List<T> listToUseAsGuide)
        {
            var except = currentList.Except(listToUseAsGuide);
            foreach (var entry in except)
            {
                currentList.Remove(entry);
            }
        }

        public static List<string> ToLowerList(this string[] stringArray)
        {
            return stringArray.Select(s => s.ToLowerInvariant()).ToList();
        }

        public static bool SafeContains(this string[] stringArray, string value)
        {
            return stringArray.Select(s => s.ToLowerInvariant()).ToList().Contains(value.ToLowerInvariant());
        }

        public static int MaxCountInGroup<T, U>(this List<T> listOfItems, Func<T, U> filterFunc)
        {
            if (listOfItems == null || !listOfItems.Any())
            {
                return 0;
            }

            var count = listOfItems
                       .GroupBy(filterFunc)
                       .Max(g => g.Count());
            return count;
        }

        public static List<T> ToNewList<T>(this T singleItem)
        {
            return new List<T> {singleItem};
        }

        public static void AddRange<T, U>(this Dictionary<T, U> existing, Dictionary<T, U> newValues)
        {
            foreach (var value in newValues)
            {
                existing.Add(value.Key, value.Value);
            }
        }
    }

    public static class ParseExtensions
    {
        public static bool ToBool(this object value, bool fallbackValue)
        {
            if (value == null)
            {
                return fallbackValue;
            }

            var couldParse = bool.TryParse(value.ToString(), out bool boolValue);
            return couldParse ? boolValue : fallbackValue;
        }

        public static T ToEnum<T>(this string value, T fallbackValue) where T : struct
        {
            if (value == null)
            {
                return fallbackValue;
            }

            var couldParse = Enum.TryParse(value, true, out T enumValue);
            return couldParse ? enumValue : fallbackValue;
        }

        public static double ToDouble(this object value, double fallbackValue)
        {
            if (value == null)
            {
                return fallbackValue;
            }

            var couldParse = double.TryParse(value.ToString(), out double doubleValue);
            return couldParse ? doubleValue : fallbackValue;
        }
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DurationUnit
    {
        Days,
        Weeks,
        Months,
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Direction
    {
        [Description("N")] North,
        [Description("S")] South,
        [Description("E")] East,
        [Description("W")] West
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum CoordinateUnit
    {
        Latitude,
        Longitude
    }

    public static class GenericTypeExtensions
    {
        /// <summary>
        /// Iterates through a nested collection of objects to find all properties of the specified type
        /// </summary>
        /// <typeparam name="TSource">the type of the source class</typeparam>
        /// <param name="source">the class containing a hierarchical set of objects</param>
        /// <param name="nextItem">the objects to hierarchically interrogate</param>
        /// <param name="canContinue">the logic to use to determine whether the cycle should continue</param>
        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        /// <summary>
        /// From the specified class, iterates through a collection and returns an enumerable of a child object/property
        /// </summary>
        /// <typeparam name="TSource">the type of the source class</typeparam>
        /// <param name="source">the class containing a hierarchical set of objects</param>
        /// <param name="nextItem">the objects to hierarchically interrogate</param>
        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        /// <summary>
        /// Determines whether a value exists in an array of values
        /// </summary>
        /// <param name="value">the value to be evaluated</param>
        /// <param name="arrayOfValues">a list of values to examine for the specified value</param>
        /// <returns>true if the value specified is contained in the array of values, otherwise false.</returns>
        public static bool IsIn<T>(this T value, T[] arrayOfValues)
        {
            if (arrayOfValues == null || value == null)
            {
                return false;
            }

            return arrayOfValues.Contains(value);
        }
    }

    /// <summary>
    /// FileHelper is a static class which provides methods to accomplish common IO File functionality
    /// </summary>
    public static class FileHelper
    {
        ///<summary>
        /// Creates a directory (if needed) based on the specified folder path
        /// </summary>
        /// <param name="rootFolder">the path and folder name of the directory</param>
        public static void CreateDirectory(string rootFolder)
        {
            RemoveDirectory(rootFolder);
            Directory.CreateDirectory(rootFolder);
        }

        /// <summary>
        /// Removes an existing directory along with any subfolders.
        /// </summary>
        /// <param name="directory">The folder to be removed</param>
        public static void RemoveDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                var files = Directory.GetFiles(directory);
                foreach (var file in files)
                {
                    File.Delete(file);
                }

                Directory.Delete(directory);
            }
        }

        ///// <summary>
        ///// Shows the folder dialog and returns the name of the selected folder
        ///// </summary>
        ///// <param name="description">The description shown on the dialog window</param>
        ///// <param name="initialDirectory">The initial directory displayed in the dialog</param>
        ///// <param name="isFolderSelected">out parameter. set to <c>true</c> if a folder is selected, otherwise <c>false</c></param>
        //public static string ShowFolderBrowserDialog(string description, string initialDirectory, out bool isFolderSelected)
        //{
        //    var folder = ShowFolderBrowserDialog(description, initialDirectory);
        //    isFolderSelected = !string.IsNullOrEmpty(folder);

        //    return folder;
        //}

        ///// <summary>
        ///// Shows the folder dialog and returns the name of the selected folder
        ///// </summary>
        ///// <param name="description">The description shown on the dialog window</param>
        ///// <param name="initialDirectory">The initial directory displayed in the dialog</param>
        //public static string ShowFolderBrowserDialog(string description, string initialDirectory)
        //{
        //    string folder = null;
        //    var openFileDialog = new FolderBrowserDialog { Description = description, SelectedPath = initialDirectory, ShowNewFolderButton = false };
        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        folder = openFileDialog.SelectedPath;
        //    }

        //    return folder;
        //}

        ///// <summary>
        ///// Displays the SaveFile dialog with the specified parameters
        ///// </summary>
        ///// <param name="title">the title of the SaveFileDialog window</param>
        ///// <param name="initialDirectory">The initial directory displayed in the dialog</param>
        ///// <param name="filter">the filter to apply to the save dialog</param>
        ///// <param name="defaultFileName">the default file name set in the file dialog</param>
        ///// <returns>the name of the file that was saved. If file was not saved, returns null</returns>
        //public static string ShowSaveFileDialog(string title, string initialDirectory, string filter, string defaultFileName)
        //{
        //    var fileDialog = new SaveFileDialog
        //    {
        //        Title = title,
        //        InitialDirectory = initialDirectory,
        //        Filter = filter,
        //        FileName = defaultFileName
        //    };

        //    if (fileDialog.ShowDialog() ?? false)
        //    {
        //        return fileDialog.FileName;
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Displays the OpenFile dialog with the specified parameters
        ///// </summary>
        ///// <param name="title">the title of the OpenFileDialog window</param>
        ///// <param name="initialDirectory">The initial directory displayed in the dialog</param>
        ///// <param name="filter">the filter to apply to the save dialog</param>
        ///// <param name="defaultFileName">the default file name set in the file dialog</param>
        ///// <returns>the name of the file that was opened. If file was not opened, returns null</returns>
        //public static string ShowOpenFileDialog(string title, string initialDirectory, string filter, string defaultFileName)
        //{
        //    var fileDialog = new OpenFileDialog
        //    {
        //        Title = title,
        //        InitialDirectory = initialDirectory,
        //        Filter = filter,
        //        FileName = defaultFileName
        //    };

        //    if (fileDialog.ShowDialog() ?? false)
        //    {
        //        return fileDialog.FileName;
        //    }
        //    return null;
        //}

    }

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                      Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                       Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }

    public static class ClassExtensions
    {
        public static string[] GetProperties<T>(T message)
        {
            var propertyInfos = typeof(T).GetProperties();
            return propertyInfos.Select(propertyInfo => propertyInfo.Name).ToArray();
        }

        public static object GetValue<T>(this T src, string property)
        {
            return src?.GetType().GetProperty(property)?.GetValue(src, null);
        }

        public static IEnumerable<KeyValuePair<string, string>> GetPropertyValues<T>(this T src)
        {
            var properties = typeof(T).GetProperties();
            return properties
                  .Select(property => new KeyValuePair<string, string>(property.Name,
                                                                       GetValue(src, property.Name).ToString()))
                  .ToList();
        }

        public static IEnumerable<KeyValuePair<string, string>> GetPropertyValues<T>(
            this T src, Type attributeType, bool includePropsWithAttribute)
        {
            var properties = includePropsWithAttribute
                ? typeof(T).GetProperties().Where(prop => Attribute.IsDefined(prop, attributeType))
                : typeof(T).GetProperties().Where(prop => !Attribute.IsDefined(prop, attributeType));
            return properties
                  .Select(property => new KeyValuePair<string, string>(property.Name,
                                                                       GetValue(src, property.Name).ToString()))
                  .ToList();
        }
    }

    public class AssemblyInfo
    {
        // The assembly information values.
        public string Title { get; }
        public string Description { get; }
        public string Company { get; }
        public string Product { get; }
        public string Copyright { get; }
        public string Trademark { get; }
        public string AssemblyVersion { get; }
        public string FileVersion { get; }
        public string Guid1 { get; }
        public string NeutralLanguage { get; }
        public bool IsComVisible { get; }

        public AssemblyInfo() : this(Assembly.GetExecutingAssembly())
        {

        }

        public AssemblyInfo(Assembly assembly)
        {
            // Load values from the assembly.
            var titleAttr = GetAssemblyAttribute<AssemblyTitleAttribute>(assembly);
            Title = titleAttr?.Title ?? string.Empty;

            var assemblyAttr = GetAssemblyAttribute<AssemblyDescriptionAttribute>(assembly);
            Description = assemblyAttr?.Description ?? string.Empty;

            var companyAttr = GetAssemblyAttribute<AssemblyCompanyAttribute>(assembly);
            Company = companyAttr?.Company ?? string.Empty;

            var productAttr = GetAssemblyAttribute<AssemblyProductAttribute>(assembly);
            Product = productAttr?.Product ?? string.Empty;

            var copyrightAttr = GetAssemblyAttribute<AssemblyCopyrightAttribute>(assembly);
            Copyright = copyrightAttr?.Copyright ?? string.Empty;

            var trademarkAttr = GetAssemblyAttribute<AssemblyTrademarkAttribute>(assembly);
            Trademark = trademarkAttr?.Trademark ?? string.Empty;

            AssemblyVersion = assembly.GetName().Version.ToString();

            var fileVersionAttr = GetAssemblyAttribute<AssemblyFileVersionAttribute>(assembly);
            FileVersion = fileVersionAttr?.Version ?? string.Empty;

            var guidAttr = GetAssemblyAttribute<GuidAttribute>(assembly);
            Guid1 = guidAttr?.Value ?? string.Empty;

            var languageAttr = GetAssemblyAttribute<NeutralResourcesLanguageAttribute>(assembly);
            NeutralLanguage = languageAttr?.CultureName ?? string.Empty;

            var comAttr = GetAssemblyAttribute<ComVisibleAttribute>(assembly);
            IsComVisible = comAttr?.Value ?? false;
        }

        public static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            // Get attributes of this type.
            var attributes = assembly.GetCustomAttributes(typeof(T), true);

            // If we didn't get anything, return null.
            if (attributes.Length == 0)
                return null;

            // Convert the first attribute value into the desired type and return it.
            return (T) attributes[0];
        }
    }

}
