//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/4/3 17:06:09.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace InSpiderDevelopWindow
{
    /// <summary>
    /// Class IndexConverter
    /// </summary>
    public class IndexConverter : IMultiValueConverter
    {
        #region ...Variables  ...
        /// <summary>
        /// The m collection view
        /// </summary>
        ListCollectionView mCollectionView;
        #endregion ...Variables  ...

        #region ...Constructor...
        #endregion ...Constructor...

        #region ...Properties ...
        #endregion ...Properties ...

        #region ...Methods    ...

        #endregion ...Methods    ...

        #region ...Events     ...
        #endregion ...Events     ...

        #region ...Interfaces ...
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty" />.<see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing" /> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null)
            {
                var item = values[0];
                if (mCollectionView == null)
                {
                    mCollectionView = CollectionViewSource.GetDefaultView(values[1]) as ListCollectionView;
                }
                return (mCollectionView.IndexOf(item) + 1).ToString();
            }
            return 0;
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion ...Interfaces ...

    }

    public class BoolInvertConvert : IValueConverter
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
            return !(bool)value;
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
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DoubleValueConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dtmp = (double)value;
            if (dtmp == double.MaxValue)
            {
                return "Max";
            }
            else if (dtmp == double.MinValue)
            {
                return "Min";
            }
            else
            {
                return dtmp;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return value;
            }
            else
            {
                if (value.ToString().Equals("Max", StringComparison.InvariantCultureIgnoreCase))
                {
                    return double.MaxValue;
                }
                else if (value.ToString().Equals("Min", StringComparison.InvariantCultureIgnoreCase))
                {
                    return double.MinValue;
                }
                return double.Parse(value.ToString());
            }
        }
    }

}
