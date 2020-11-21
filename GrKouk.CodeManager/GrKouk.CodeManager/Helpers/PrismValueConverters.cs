using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Syncfusion.SfAutoComplete.XForms;
using Xamarin.Forms;
using BaseSelectionChangedEventArgs=Xamarin.Forms.SelectionChangedEventArgs;
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
            if (value is SelectionChangedEventArgs)
            {
                var slArgs = value as SelectionChangedEventArgs;
                if (slArgs != null)
                {
                    var item = slArgs.Value;
                    if (item != null)
                    {
                        return item;
                    }
                    
                }
            }

            if (value is ValueChangedEventArgs)
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
            }

            return string.Empty;
            //throw new ArgumentException("Expected value to be of type ValueChangedEventArgs", nameof(value));



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
    public class CollectionViewSelectionValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            if (value is BaseSelectionChangedEventArgs)
            {
                var evArgs = value as BaseSelectionChangedEventArgs;
                var item = evArgs.CurrentSelection;
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
