using System;
using System.Collections.Generic;
using NUnit.Framework;
using SimpleRepositoryApp.Abstractions;
using SimpleRepositoryApp.Models;

namespace SimpleRepository.Test
{
    public class UpdatePerformanceTests : BaseRepositoryTestFixture<ISampleRepositoryContext>
    {
        [Test, Order(0)]
        [TestCase(5, TestName = "Single Update Performance (5,000 records)")]
        public void SingleUpdatePerformanceTest(int itemsToUpdate)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToUpdate);

            // Insert initial data
            sampleRepository.InsertAsync(testData).GetAwaiter().GetResult();

            // Get persisted data
            var persistedItems = sampleRepository.GetAsync().GetAwaiter().GetResult();

            // Update local copies of persisted data
            UpdateItems(ref persistedItems);

            // Perform and measure update operations
            var executionTime = ExecutionTimeInMilliseconds(() =>
            {
                foreach (var item in persistedItems)
                    sampleRepository.UpdateItemAsync(item).GetAwaiter().GetResult();
            });

            Assert.Pass($"{itemsToUpdate} records updated: {executionTime} ms");
        }

        [Test, Order(1)]
        [TestCase(5, TestName = "Bulk Update Performance (5,000 records)")]
        public void BulkUpdatePerformanceTest(int itemsToRead)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToRead);

            // Insert initial data
            sampleRepository.InsertAsync(testData).GetAwaiter().GetResult();

            // Get persisted data
            var persistedItems = sampleRepository.GetAsync().GetAwaiter().GetResult();

            // Update local copies of persisted data
            UpdateItems(ref persistedItems);

            // Perform and measure update operations
            var executionTime = ExecutionTimeInMilliseconds(() => sampleRepository.UpdateAsync(testData).GetAwaiter().GetResult());

            Assert.Pass($"{itemsToRead} records updated: {executionTime} ms");
        }

        void UpdateItems(ref IEnumerable<SampleModel> items)
        {
            var currentTimestamp = DateTimeOffset.UtcNow;

            foreach (var item in items)
            {
                item.SampleIntProperty = ++item.SampleIntProperty;
                item.SampleStringProperty = $"{item.SampleStringProperty} - UPDATED";
                item.Timestamp = currentTimestamp;
            }
        }
    }
}