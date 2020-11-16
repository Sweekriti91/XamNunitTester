using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SimpleRepository.UITest.Pages;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace SimpleRepository.UITest
{
    public class Tests : BaseTestFixture
    {
        public Tests(Platform platform)
        : base(platform)
        {
        }

        //[Test]
        //public void Repl()
        //{
        //    app.Repl();
        //}

        [Test]
        public void PerformanceTests()
        {
            new HomePage()
                .TestsCompleteCheck()
                //.AllResults()
                .ExportResults();

            //new AllResultsPage()
            //    .VerifySingleOperations()
            //    .VerifyBulkOperations()
            //    .VerifySingleInsertPerformance()
            //    .VerifyBulkInsertPerformance()
            //    .VerifyCustomQueryPerformance()
            //    .VerifySingleDeletePerformance()
            //    .VerifyBulkDeletePerformance()
            //    .VerifySingleReadPerformance()
            //    .VerifyBulkReadPerformance()
            //    .VerifySingleUpdatePerformance()
            //    .VerifyBulkUpdatePerformance()
            //    .VerifySingleUpsertExistingPerformance()
            //    .VerifySingleUpsertNewPerformance()
            //    .VerifyBulkUpsertExistingPerformance()
            //    .VerifyBulkUpsertNewPerformance();
        }
    }
}
