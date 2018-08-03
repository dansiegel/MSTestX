﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace TestAppRunner.ViewModels
{
    internal class TestResultGroup : List<TestResultVM>, INotifyPropertyChanged
    {
        public TestResultGroup(string group, IEnumerable<TestResultVM> tests) : base(tests)
        {
            Group = group ?? "<None>";
            foreach (var t in tests)
            {
                t.PropertyChanged += Test_PropertyChanged;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Test_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TestResultVM.Result))
            {
                OnPropertyChanged(nameof(PassedTests), nameof(FailedTests), nameof(SkippedTests), nameof(NotRunTests), nameof(TestStatus), nameof(Percentage), nameof(Outcome));
            }
        }

        public void OnPropertyChanged(params string[] propertyNames)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var p in propertyNames)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
            });
        }

        public TestOutcome Outcome
        {
            get
            {
                if (FailedTests > 0) return TestOutcome.Failed;
                if (SkippedTests == Count) return TestOutcome.Skipped;
                if (PassedTests + SkippedTests == Count) return TestOutcome.Passed;
                return TestOutcome.None;
            }
        }

        public string Group { get; }

        public int PassedTests => this.Where(t => t.Result?.Outcome == TestOutcome.Passed).Count();

        public int FailedTests => this.Where(t => t.Result?.Outcome == TestOutcome.Failed).Count();

        public int SkippedTests => this.Where(t => t.Result?.Outcome == TestOutcome.Skipped).Count();

        public int NotRunTests => this.Where(t => t.Result == null).Count();

        public double Percentage => this.Any(t=>t.Result?.Outcome == TestOutcome.Passed) ? (int)(PassedTests * 100d / (FailedTests + PassedTests)) : 0;

        public string TestStatus => $"{PassedTests} passed. {FailedTests} failed. {SkippedTests} skipped. {NotRunTests} not run. {Percentage.ToString("0")}%";
    }
}
