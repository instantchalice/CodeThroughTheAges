using DdrAuto.Utilities;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CodeThroughTheAges
{
    /// <summary>
    /// Represents the converter that converts Boolean values to and from Visibility enumeration values.
    /// InvertedBooleanToVisibilityConverter converts "true" to Visibility.Collapsed and "false" to Visibility.Visible.
    /// </summary>
    public class InvertedBooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>Converts a value.</summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = false;
            if (value is bool b)
            {
                flag = b;
            }

            return (Visibility)(flag ? 2 : 0);
        }

        /// <summary>Converts a value.</summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Collapsed;
        }
    }

    /// <summary>
    /// Represents converter, which converts <see cref="T:System.Enum" /> types to and from a
    /// boolean value using the given parameter.
    /// </summary>
    /// <remarks>
    /// The <see cref="T:System.Enum" /> can be with <see cref="T:System.FlagsAttribute" />. Characters: ',' or ';'
    /// can be used to split multiple values passed in the given parameter.
    /// </remarks>
    [ValueConversion(typeof(Enum), typeof(Visibility))]
    public class EnumToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="T:System.Enum" /> value to a Boolean one if it is one from the
        /// specified in the <paramref name="parameter" /> values.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Enum" /> value .</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">
        /// One or more values, which will be check for equality
        /// against the passed <paramref name="value" />.Characters: ',' or ';' can be used to split
        /// multiple values.
        /// </param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>
        /// A boolean value indicating whether the given <paramref name="value" /> is one from the specified in
        /// the <paramref name="parameter" />. Returns null if the <paramref name="value" /> or
        /// <paramref name="parameter" /> are <c>null</c>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum))
            {
                return Visibility.Collapsed;
            }

            if (parameter == null)
            {
                return Visibility.Collapsed;
            }

            return parameter.ParseToEnum(value.GetType()).Any(value.Equals)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts the Boolean value back to the first <see cref="T:System.Enum" /> value passed in the
        /// <paramref name="parameter" />.
        /// </summary>
        /// <param name="value">The Boolean value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">
        /// One or more <see cref="T:System.Enum" /> values. The first one will be return if the <paramref name="value" />
        /// is <c>true</c>.
        /// </param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>
        /// First <see cref="T:System.Enum" /> value from the <paramref name="parameter" /> if the <paramref name="value" /> is <c>true</c>,
        /// otherwise <see cref="F:System.Windows.Data.Binding.DoNothing" />.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(bool)value)
            {
                return Binding.DoNothing;
            }

            var enums = parameter.ParseToEnum(targetType);
            return enums.Length > 0
                ? enums[0]
                : Binding.DoNothing;
        }
    }

    /// <summary>
    /// Represents converter, which converts <see cref="T:System.Enum" /> types to and from a
    /// boolean value using the given parameter.
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(Visibility))]
    public class InvertedEnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum) || parameter == null || string.IsNullOrEmpty(parameter.ToString()))
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Represents the converter that converts Color values to and from Brush values.
    /// </summary>
    public class ColorToBrushConverter : IValueConverter
    {
        /// <summary>Converts a value.</summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var solidColorBrush = new SolidColorBrush();
            if (value != null)
            {
                solidColorBrush.Color = (Color)value;
            }
            return solidColorBrush;
        }

        /// <summary>Converts a value.</summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush solidColorBrush)
            {
                return solidColorBrush.Color;
            }
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvertedBooleanConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && !(bool)value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && !(bool)value;
        }
    }

    /// <summary>
    /// Represents a converter that converts a number value to Visibility value.
    /// </summary>
    public class NumberToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a number value (double or integer) to visibility. If the value is less than or equal to 0, returns Collapsed, otherwise returns Visible.
        /// </summary>
        /// <returns>Visibility.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return d <= 0.0
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }

            if (!(value is int))
            {
                throw new ArgumentException("value must be of type double or int.");
            }

            return (int)value <= 0
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        /// <summary>
        /// Converts Visibility to number - if is Visible returns 1, otherwise returns 0.
        /// </summary>
        /// <returns>The Visibility value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
            {
                throw new ArgumentException("value must be of type System.Windows.Visibility");
            }

            if ((Visibility)value == Visibility.Visible)
            {
                return 1;
            }
            return 0;
        }
    }

    /// <summary>
    /// Represents converter, which converts <see cref="T:System.Enum" /> types to and from a
    /// boolean value using the given parameter.
    /// </summary>
    /// <remarks>
    /// The <see cref="T:System.Enum" /> can be with <see cref="T:System.FlagsAttribute" />. Characters: ',' or ';'
    /// can be used to split multiple values passed in the given parameter.
    /// </remarks>
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="T:System.Enum" /> value to a Boolean one if it is one from the
        /// specified in the <paramref name="parameter" /> values.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Enum" /> value .</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">
        /// One or more values, which will be check for equality
        /// against the passed <paramref name="value" />.Characters: ',' or ';' can be used to split
        /// multiple values.
        /// </param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>
        /// A boolean value indicating whether the given <paramref name="value" /> is one from the specified in
        /// the <paramref name="parameter" />. Returns null if the <paramref name="value" /> or
        /// <paramref name="parameter" /> are <c>null</c>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum))
            {
                return false;
            }
            if (parameter == null)
            {
                return false;
            }
            foreach (var @enum in parameter.ParseToEnum(value.GetType()))
            {
                if (value.Equals(@enum))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Converts the Boolean value back to the first <see cref="T:System.Enum" /> value passed in the
        /// <paramref name="parameter" />.
        /// </summary>
        /// <param name="value">The Boolean value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">
        /// One or more <see cref="T:System.Enum" /> values. The first one will be return if the <paramref name="value" />
        /// is <c>true</c>.
        /// </param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>
        /// First <see cref="T:System.Enum" /> value from the <paramref name="parameter" /> if the <paramref name="value" /> is <c>true</c>,
        /// otherwise <see cref="F:System.Windows.Data.Binding.DoNothing" />.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool)value)
            {
                var objectToEnum = parameter.ParseToEnum(targetType);
                if (objectToEnum.Length > 0)
                {
                    return objectToEnum[0];
                }
            }
            return Binding.DoNothing;
        }
    }

    public class EnumToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is Type type && value != null)
            {
                return (Enum)Enum.Parse(type, value.ToString());
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is Type type && value != null)
            {
                return (int)Enum.Parse(type, value.ToString());
            }
            return 0;
        }
    }

    public class IntToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var returnValueEnum = Enum.Parse(targetType, value.ToString());
            return returnValueEnum;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is Type type && value != null)
            {
                return (Enum)Enum.Parse(type, value.ToString());
            }
            return null;
        }
    }
}
