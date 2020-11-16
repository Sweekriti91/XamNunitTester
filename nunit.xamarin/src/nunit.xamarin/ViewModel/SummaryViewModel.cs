﻿// ***********************************************************************
// Copyright (c) 2015 NUnit Project
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using NUnit.Runner.Helpers;
using NUnit.Runner.View;

using NUnit.Runner.Services;

using Xamarin.Forms;
using System.Diagnostics;
using System.IO;
using System;
using Xamarin.Essentials;
using nunit.xamarin.Services;
using System.Linq;

namespace NUnit.Runner.ViewModel
{
    class SummaryViewModel : BaseViewModel
    {
        readonly TestPackage _testPackage;
        ResultSummary _results;
        bool _running;
        TestResultProcessor _resultProcessor;

        public SummaryViewModel()
        {
            ExportStatus = "Not Exported";
            _testPackage = new TestPackage();
            RunTestsCommand = new Command(async o => await ExecuteTestsAync(), o => !Running);
            ViewAllResultsCommand = new Command(
                async o => await Navigation.PushAsync(new ResultsView(new ResultsViewModel(_results.GetTestResults(), true))),
                o => !HasResults);
            ViewFailedResultsCommand = new Command(
                async o => await Navigation.PushAsync(new ResultsView(new ResultsViewModel(_results.GetTestResults(), false))),
                o => !HasResults);

            ExportTestResultsXML = new Command(OnExportResultXml);
        }

        private TestOptions options;

        /// <summary>
        /// User options for the test suite.
        /// </summary>
        public TestOptions Options {
            get
            {
                if(options == null)
                {
                    options = new TestOptions();
                }
                return options;
            }
            set
            {
                options = value;
            }
        }

        /// <summary>
        /// Called from the view when the view is appearing
        /// </summary>
        public void OnAppearing()
        {
            if(Options.AutoRun)
            {
                // Don't rerun if we navigate back
                Options.AutoRun = false;
                RunTestsCommand.Execute(null);
            }
        }

        /// <summary>
        /// The overall test results
        /// </summary>
        public ResultSummary Results
        {
            get { return _results; }
            set
            {
                if (Equals(value, _results)) return;
                _results = value;
                OnPropertyChanged();
                OnPropertyChanged("HasResults");
            }
        }

        /// <summary>
        /// True if tests are currently running
        /// </summary>
        public bool Running
        {
            get { return _running; }
            set
            {
                if (value.Equals(_running)) return;
                _running = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// True if we have test results to display
        /// </summary>
        public bool HasResults => Results != null;

        public ICommand RunTestsCommand { set; get; }
        public ICommand ViewAllResultsCommand { set; get; }
        public ICommand ViewFailedResultsCommand { set; get; }
        public ICommand ExportTestResultsXML { set; get; }

        /// <summary>
        /// Adds an assembly to be tested.
        /// </summary>
        /// <param name="testAssembly">The test assembly.</param>
        /// <returns></returns>
        internal void AddTest(Assembly testAssembly, Dictionary<string, object> options = null)
        {
            _testPackage.AddAssembly(testAssembly, options);
        }

        async Task ExecuteTestsAync()
        {
            Running = true;
            Results = null;
            TestRunResult results = await _testPackage.ExecuteTests();
            ResultSummary summary = new ResultSummary(results);

            _resultProcessor = TestResultProcessor.BuildChainOfResponsability(Options);
            await _resultProcessor.Process(summary).ConfigureAwait(false);

            var temp = summary;
            Debug.WriteLine(summary);

            Device.BeginInvokeOnMainThread(
                () =>
                    {
                        Results = summary;
                        Running = false;

                    if (Options.TerminateAfterExecution)
                        TerminateWithSuccess();
                });
        }

        public static void TerminateWithSuccess()
        {
#if __IOS__
            var selector = new ObjCRuntime.Selector("terminateWithSuccess");
            UIKit.UIApplication.SharedApplication.PerformSelector(selector, UIKit.UIApplication.SharedApplication, 0);
#elif __DROID__
            System.Environment.Exit(0);
#elif WINDOWS_UWP
            Windows.UI.Xaml.Application.Current.Exit();
#endif
        }

        private string _exportStatus;
        public string ExportStatus
        {
            get => _exportStatus;
            set
            {
                _exportStatus = value;
                OnPropertyChanged();
            }
        }

        public async void OnExportResultXml()
        {
            //uncomment to see XML value
            //var temp = _results.GetCustomTestXml();
            //write to xml file appending device name to file name
            //string xmlResults = "";
            try
            {
                var result = await WriteCustomXmlResultFile().ConfigureAwait(false);
                //xmlResults = _results.GetCustomTestXml().ToString();
                await BlobStorageService.performBlobOperation(result);
                ExportStatus = "Export Success!";
                Debug.WriteLine("Export Success!");
            }
            catch (Exception)
            {
                ExportStatus = "Fatal error while trying to write xml result file!";
                Debug.WriteLine("Fatal error while trying to write xml result file!");
                throw;
            }

            //Upload file to Blob

            /***
             * UNCOMMENT FOR SANITY CHECK OF FILE EXPORTED
             *
             * 
            var xmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library");
            //file name that gets uploaded to Blob storage
            string xtc_device_platform = DeviceInfo.Platform.ToString();
            //manufacturer_model
            string xtc_device_name = DeviceInfo.Manufacturer.ToString() + "_" + DeviceInfo.Model.ToString();
            //utc datetimenow
            string dateTimeNow = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = dateTimeNow + "_" + xtc_device_name + "_" + xtc_device_name + ".xml";
            string custom_xmlfile = Path.Combine(xmlFilePath, fileName);

            string text = File.ReadAllText(custom_xmlfile);
            Debug.WriteLine("**** FROM XML FILE IN LIBRARY*****");
            Debug.WriteLine(text);
            Debug.WriteLine("******");

             *
             *
             *
             * ***/

        }

        async Task<string> WriteCustomXmlResultFile()
        {
            var xmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library");
            //file name that gets uploaded to Blob storage
            string xtc_device_platform = DeviceInfo.Platform.ToString();
            //manufacturer_model
            string device_manufacturer = DeviceInfo.Manufacturer.ToString();
            string trim_device_manufacturer = String.Concat(device_manufacturer.Where(c => !Char.IsWhiteSpace(c)));
            string device_model = DeviceInfo.Model.ToString();
            string trim_device_model = String.Concat(device_model.Where(c => !Char.IsWhiteSpace(c)));
            string xtc_device_name = trim_device_manufacturer + "_" + trim_device_model;
            //utc datetimenow
            string dateTimeNow = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm");
            //string dateTimeNow = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string sourceString = AppInfo.Name;
            string removeString = "-Tests";
            int index = sourceString.IndexOf(removeString);
            string xtc_app_name = (index < 0) ? sourceString : sourceString.Remove(index, removeString.Length);
            string fileName = xtc_device_platform + "-" + xtc_app_name + "_" + dateTimeNow + "_" + xtc_device_name + ".xml";
            //string fileName = xtc_device_platform + "-" + xtc_app_name + "_" + xtc_device_name + ".xml";
            string custom_xmlfile = Path.Combine(xmlFilePath, fileName);

            string outputFolderName = Path.GetDirectoryName(custom_xmlfile);

            Directory.CreateDirectory(outputFolderName);

            using (var resultFileStream = new StreamWriter(custom_xmlfile, false))
            {
                var xml = _results.GetCustomTestXml().ToString();
                await resultFileStream.WriteAsync(xml);
            }

            return custom_xmlfile;
        }
    }
}