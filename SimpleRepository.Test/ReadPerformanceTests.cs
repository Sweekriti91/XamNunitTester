﻿using NUnit.Framework;
using SimpleRepositoryApp.Abstractions;

namespace SimpleRepository.Test
{
    public class ReadPerformanceTests : BaseRepositoryTestFixture<ISampleRepositoryContext>
    {
        [Test, Order(0)]
        [TestCase(5000, TestName = "Single Read Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Single Read Performance (10,000 records)")]
        [TestCase(15000, TestName = "Single Read Performance (15,000 records)")]
#endif
        public void SingleReadPerformanceTest(int itemsToRead)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToRead);

            // Insert initial data
            sampleRepository.InsertAsync(testData).GetAwaiter().GetResult();

            // Perform and measure read operations
            var executionTime = ExecutionTimeInMilliseconds(() =>
            {
                foreach (var item in testData)
                    sampleRepository.GetItemAsync(item.Id).GetAwaiter().GetResult();
            });

            Assert.Pass($"{itemsToRead} records read: {executionTime} ms");
        }

        [Test, Order(1)]
        [TestCase(5000, TestName = "Bulk Read Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Bulk Read Performance (10,000 records)")]
        [TestCase(15000, TestName = "Bulk Read Performance (15,000 records)")]
#endif
        public void BulkReadPerformanceTest(int itemsToRead)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToRead);

            // Insert initial data
            sampleRepository.InsertAsync(testData).GetAwaiter().GetResult();

            // Perform and measure read operations
            var executionTime = ExecutionTimeInMilliseconds(() => sampleRepository.GetAsync().GetAwaiter().GetResult());

            Assert.Pass($"{itemsToRead} records read: {executionTime} ms");
        }
    }
}