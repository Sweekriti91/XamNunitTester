using System;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace SimpleRepository.UITest.Pages
{
    public class AllResultsPage : BasePage
    {
        readonly Query resultListView;
        readonly Query singleOperationsRow;
        readonly Query bulkOperationsRow;
        readonly Query customQueryPerformanceRow;
        readonly Query singleDeletePerformanceRow;
        readonly Query bulkDeletePerformanceRow;
        readonly Query singleInsertPerformanceRow;
        readonly Query bulkInsertPerformanceRow;
        readonly Query singleReadPerformanceRow;
        readonly Query bulkReadPerformanceRow;
        readonly Query singleUpdatePerformanceRow;
        readonly Query bulkUpdatePerformanceRow;
        readonly Query singleUpsertExistingPerformanceRow;
        readonly Query singleUpsertNewPerformanceRow;
        readonly Query bulkUpsertExistingPerformanceRow;
        readonly Query bulkUpsertNewPerformanceRow;
        readonly Query backButton;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("Test Results"),
            iOS = x => x.Marked("Test Results")
        };

        public AllResultsPage()
        {
            backButton = x => x.Marked("Test Results");
            resultListView = x => x.Class("ListView");
            singleOperationsRow = x => x.Marked("Single Operations");
            bulkOperationsRow = x => x.Marked("Bulk Operations");
            customQueryPerformanceRow = x => x.Marked("Custom Query Performance (5,000 records)");
            singleDeletePerformanceRow = x => x.Marked("Single Delete Performance (5,000 records)");
            bulkDeletePerformanceRow = x => x.Marked("Bulk Delete Performance (5,000 records)");
            singleInsertPerformanceRow = x => x.Marked("Single Insert Performance (5,000 records)");
            bulkInsertPerformanceRow = x => x.Marked("Bulk Insert Performance (5,000 records)");
            singleReadPerformanceRow = x => x.Marked("Single Read Performance (5,000 records)");
            bulkReadPerformanceRow = x => x.Marked("Bulk Read Performance (5,000 records)");
            singleUpdatePerformanceRow = x => x.Marked("Single Update Performance (5,000 records)");
            bulkUpdatePerformanceRow = x => x.Marked("Bulk Update Performance (5,000 records)");
            singleUpsertExistingPerformanceRow = x => x.Marked("Single Upsert-Existing Performance (5,000 records)");
            singleUpsertNewPerformanceRow = x => x.Marked("Single Upsert-New Performance (5,000 records)");
            bulkUpsertExistingPerformanceRow = x => x.Marked("Bulk Upsert-Existing Performance (5,000 records)");
            bulkUpsertNewPerformanceRow = x => x.Marked("Bulk Upsert-New Performance (5,000 records)");
        }

        public AllResultsPage VerifySingleOperations()
        {
            app.WaitForElement(singleOperationsRow);
            app.Tap(singleOperationsRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Single Operations ");
            if(OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifyBulkOperations()
        {
            app.WaitForElement(bulkOperationsRow);
            app.Tap(bulkOperationsRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Bulk Operations ");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifySingleInsertPerformance()
        {
            app.WaitForElement(singleInsertPerformanceRow);
            app.Tap(singleInsertPerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Single Insert Performance ");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifyBulkInsertPerformance()
        {
            app.WaitForElement(bulkInsertPerformanceRow);
            app.Tap(bulkInsertPerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Bulk Insert Performance ");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifyCustomQueryPerformance()
        {
            app.WaitForElement(customQueryPerformanceRow);
            app.Tap(customQueryPerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Custom Query Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifySingleDeletePerformance()
        {
            app.WaitForElement(singleDeletePerformanceRow);
            app.Tap(singleDeletePerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Single Delete Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifyBulkDeletePerformance()
        {
            app.WaitForElement(bulkDeletePerformanceRow);
            app.Tap(bulkDeletePerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Bulk Delete Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifySingleReadPerformance()
        {
            app.WaitForElement(singleReadPerformanceRow);
            app.Tap(singleReadPerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Single Read Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifyBulkReadPerformance()
        {
            app.WaitForElement(bulkReadPerformanceRow);
            app.Tap(bulkReadPerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Bulk Read Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifySingleUpdatePerformance()
        {
            app.WaitForElement(singleUpdatePerformanceRow);
            app.Tap(singleUpdatePerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Single Update Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifyBulkUpdatePerformance()
        {
            app.WaitForElement(bulkUpdatePerformanceRow);
            app.Tap(bulkUpdatePerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Bulk Update Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifySingleUpsertExistingPerformance()
        {
            app.WaitForElement(singleUpsertExistingPerformanceRow);
            app.Tap(singleUpsertExistingPerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Single Upsert-Existing Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifySingleUpsertNewPerformance()
        {
            app.ScrollDownTo(singleUpsertNewPerformanceRow);
            app.Tap(singleUpsertNewPerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Single Upsert-New Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }


        public AllResultsPage VerifyBulkUpsertExistingPerformance()
        {
            app.ScrollDownTo(bulkUpsertExistingPerformanceRow);
            app.Tap(bulkUpsertExistingPerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Bulk Upsert-Existing Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }

        public AllResultsPage VerifyBulkUpsertNewPerformance()
        {
            app.ScrollDownTo(bulkUpsertNewPerformanceRow, timeout: TimeSpan.FromSeconds(30));
            app.Tap(bulkUpsertNewPerformanceRow);
            app.WaitForElement(x => x.Marked("Message:"));
            app.Screenshot("Bulk Upsert-New Performance");
            if (OniOS)
                app.Tap(backButton);
            if (OnAndroid)
                app.Back();

            return this;
        }
    }

}
