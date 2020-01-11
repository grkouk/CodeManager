using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Syncfusion.SfAutoComplete.XForms;

using Xamarin.Forms;
using SelectionChangedEventArgs = Syncfusion.SfAutoComplete.XForms.SelectionChangedEventArgs;
using ComboChangedEventArgs = Syncfusion.XForms.ComboBox.SelectionChangedEventArgs;

namespace Prism.Converters
{
    public class AutoCompleteValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var evArgs = value as SelectionChangedEventArgs;

            if (evArgs != null)
            {
                var item = evArgs.Value;
                if (item == null)
                {
                    throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));
                }
                return item;
            }
            else
            {
                throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ComboValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var evArgs = value as ComboChangedEventArgs;

            if (evArgs != null)
            {
                var item = evArgs.Value;
                if (item == null)
                {
                    throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));
                }
                return item;
            }
            else
            {
                throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
