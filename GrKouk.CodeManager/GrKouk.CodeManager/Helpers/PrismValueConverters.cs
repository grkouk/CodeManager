using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

using Syncfusion.SfAutoComplete.XForms;

using Xamarin.Forms;
using SelectionChangedEventArgs = Syncfusion.SfAutoComplete.XForms.SelectionChangedEventArgs;
using ComboChangedEventArgs = Syncfusion.XForms.ComboBox.SelectionChangedEventArgs;
using ValueChangedEventArgs = Syncfusion.SfAutoComplete.XForms.ValueChangedEventArgs;
using ValueChangedEventArgsCombobox = Syncfusion.XForms.ComboBox.ValueChangedEventArgs;

namespace Prism.Converters
{
    public class AutoCompleteSelectionChangedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var evArgs = value as SelectionChangedEventArgs;

            if (evArgs != null)
            {
                var item = evArgs.Value;
                if (item == null)
                {
                    throw new ArgumentException("Expected value to be of type ProductListInfo", nameof(value));
                }
                return item;
            }
            else
            {
                throw new ArgumentException("Expected value to be of type SelectionChangedEventArgs", nameof(value));
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AutoCompleteValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var evArgs = value as ValueChangedEventArgs;

            if (evArgs != null)
            {
                var item = evArgs.Value;
                if (item == null)
                {
                    throw new ArgumentException("Expected value to be of type string", nameof(value));
                }
                return item;
            }
            else
            {
                throw new ArgumentException("Expected value to be of type ValueChangedEventArgs", nameof(value));
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
            Debug.WriteLine(value.GetType().ToString() + " --- " + typeof(ComboChangedEventArgs));
            Debug.WriteLine(value.GetType().ToString() + " --- " + typeof(ValueChangedEventArgs));
            if (value is ComboChangedEventArgs)
            {
                var evArgs = value as ComboChangedEventArgs;
                var item = evArgs.Value;
                if (item == null)
                {
                    throw new ArgumentException("Expected value to be of type ComboChangedEventArgs", nameof(value));
                }
                return item;

            }
            Debug.WriteLine(value.GetType().ToString()+" --- " + typeof(ComboChangedEventArgs));
            Debug.WriteLine(value.GetType().ToString() + " --- " + typeof(ValueChangedEventArgs));
            if (value is ValueChangedEventArgsCombobox)
            {
                var evArgs = value as ValueChangedEventArgsCombobox;
                var item = evArgs.Value;
                if (item == null)
                {
                    throw new ArgumentException("Expected value to be of type ComboChangedEventArgs", nameof(value));
                }
                return item;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
