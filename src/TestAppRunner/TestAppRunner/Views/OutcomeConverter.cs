﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TestAppRunner.Views
{
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class OutcomeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as TestResult;
            if(v != null)
            {
                if(v.Outcome == TestOutcome.Failed)
                {
                    if(v.ErrorStackTrace == null && v.ErrorMessage?.Contains("timeout") == true)
                    {
                        if (targetType == typeof(Color))
                            return Color.OrangeRed;
                        return "Timed out";
                    }
                    if (targetType == typeof(Color))
                        return Color.Red;
                }
                if(v.Outcome == TestOutcome.Passed)
                {
                    if (targetType == typeof(Color))
                        return Color.Green;
                    return "Passed";
                }
                if (v.Outcome == TestOutcome.Skipped)
                {
                    if (targetType == typeof(Color))
                        return Color.Orange;
                    return "Skipped";
                }
                return v.Outcome.ToString();
            }
            if (v == null)
            {
                if (targetType == typeof(Color))
                    return Color.Gray;
                return "Not Executed";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class Outcome2Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var outcome = (TestOutcome)value;
            if (outcome == TestOutcome.Failed)
            {
                return Color.Red;
            }
            if (outcome == TestOutcome.Passed)
            {
                return Color.Green;
            }
            if (outcome == TestOutcome.Skipped)
            {
                return Color.Orange;
            }
            return Color.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
