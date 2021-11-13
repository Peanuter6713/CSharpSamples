using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SliderDemo
{
    class SettingVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            };

            return value.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            };

            return value.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
        }

    }
}
