using System;
using System.Collections.Generic;
using NUnit.Framework;
using SimpleRepositoryApp.Abstractions;
using SimpleRepositoryApp.Models;

namespace SimpleRepository.Test
{
    public class UpsertPerformanceTests : BaseRepositoryTestFixture<ISampleRepositoryContext>
    {
        [Test, Order(0)]
        [TestCase(5000, TestName = "Single Upsert-Existing Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Single Upsert-Existing Performance (10,000 records)")]
        [TestCase(15000, TestName = "Single Upsert-Existing Performance (15,000 records)")]
#endif
        public void UpsertExistingPerformanceTest(int itemsToUpsert)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToUpsert);

            // Insert initial data
            sampleRepository.InsertAsync(testData).GetAwaiter().GetResult();

            // Get persisted data
            var persistedItems = sampleRepository.GetAsync().GetAwaiter().GetResult();

            // Update local copies of persisted data
            UpdateItems(ref persistedItems);

            // Perform and measure upsert operations
            var executionTime = ExecutionTimeInMilliseconds(() =>
            {
                foreach (var item in persistedItems)
                    sampleRepository.UpsertItemAsync(item).GetAwaiter().GetResult();
            });

            Assert.Pass($"{itemsToUpsert} records upserted: {executionTime} ms");
        }

        [Test, Order(1)]
        [TestCase(5000, TestName = "Single Upsert-New Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Single Upsert-New Performance (10,000 records)")]
        [TestCase(15000, TestName = "Single Upsert-New Performance (15,000 records)")]
#endif
        public void UpsertNewPerformanceTest(int itemsToUpsert)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToUpsert);

            // Perform and measure upsert operations
            var executionTime = ExecutionTimeInMilliseconds(() =>
            {
                foreach (var item in testData)
                    sampleRepository.UpsertItemAsync(item).GetAwaiter().GetResult();
            });

            Assert.Pass($"{itemsToUpsert} records upserted: {executionTime} ms");
        }

        [Test, Order(2)]
        [TestCase(5000, TestName = "Bulk Upsert-Existing Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Bulk Upsert-Existing Performance (10,000 records)")]
        [TestCase(15000, TestName = "Bulk Upsert-Existing Performance (15,000 records)")]
#endif
        public void BulkUpsertExistingPerformanceTest(int itemsToUpsert)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToUpsert);

            // Insert initial data
            sampleRepository.InsertAsync(testData).GetAwaiter().GetResult();

            // Get persisted data
            var persistedItems = sampleRepository.GetAsync().GetAwaiter().GetResult();

            // Update local copies of persisted data
            UpdateItems(ref persistedItems);

            // Perform and measure upsert operations
            var executionTime = ExecutionTimeInMilliseconds(() => sampleRepository.UpsertAsync(persistedItems).GetAwaiter().GetResult());

            Assert.Pass($"{itemsToUpsert} records upserted: {executionTime} ms");
        }

        [Test, Order(3)]
        [TestCase(5000, TestName = "Bulk Upsert-New Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Bulk Upsert-New Performance (10,000 records)")]
        [TestCase(15000, TestName = "Bulk Upsert-New Performance (15,000 records)")]
#endif
        public void BulkUpsertNewOperationsPerformanceTest(int itemsToUpsert)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToUpsert);

            // Perform and measure insert operations
            var executionTime = ExecutionTimeInMilliseconds(() => sampleRepository.UpsertAsync(testData).GetAwaiter().GetResult());

            Assert.Pass($"{itemsToUpsert} records inserted: {executionTime} ms");
        }

        void UpdateItems(ref IEnumerable<SampleModel> items)
        {
            var currentTimestamp = DateTimeOffset.UtcNow;

            foreach (var item in items)
            {
                item.SampleIntProperty = ++item.SampleIntProperty;
                item.SampleStringProperty = $"{item.SampleStringProperty} - UPSERTED";
                item.Timestamp = currentTimestamp;
            }
        }
    }
}