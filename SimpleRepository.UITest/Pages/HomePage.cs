using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace SimpleRepository.UITest.Pages
{
    public class HomePage : BasePage
    {
        readonly Query runTestsButton;
        readonly Query allResultsButton;
        readonly Query failedResultsButton;
        readonly Query exportResultsButton;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("NUnit 3"),
            iOS = x => x.Marked("NUnit 3")
        };

        public HomePage()
        {
            allResultsButton = x => x.Marked("All Results");
            failedResultsButton = x => x.Marked("Failed Results");
            exportResultsButton = x => x.Marked("Export Results");
        }

        public HomePage TestsCompleteCheck()
        {
            var stopCheck = false;
            while (stopCheck != true)
            {
                var buttonState = app.Query(allResultsButton)[0].Enabled;
                if (buttonState == true)
                {
                    app.Screenshot("Run Complete!");
                    stopCheck = true;
                }
            }

            return this;
        }

        public void AllResults()
        {
            app.WaitForElement(allResultsButton);
            app.Tap(allResultsButton);
        }

        public void FailedResults()
        {
            app.WaitForElement(failedResultsButton);
            app.Tap(failedResultsButton);
        }

        public HomePage RunAllTests()
        {
            app.WaitForElement(runTestsButton);
            app.Tap(runTestsButton);
            return this;
        }

        public void ExportResults()
        {
            app.WaitForElement(exportResultsButton);
            app.Tap(exportResultsButton);

            app.WaitForElement("Export Success!", timeout: TimeSpan.FromMinutes(5));
            app.Screenshot("Export Success!");
        }
    }
}
