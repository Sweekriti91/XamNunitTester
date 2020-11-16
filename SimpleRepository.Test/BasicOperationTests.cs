using System;
using System.Linq;
using NUnit.Framework;
using SimpleRepositoryApp.Abstractions;
using SimpleRepositoryApp.Models;

namespace SimpleRepository.Test
{
    public class BasicOperationTests : BaseRepositoryTestFixture<ISampleRepositoryContext, SampleModel>
    {
        [Test, Order(0), TestCase(TestName = "Single Operations")]
        public void SingleOperationsTest()
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            Assert.NotNull(sampleRepository);

            var testData = TestData.GenerateSingleOperationsTestData(0);

            // Create (insert) operation
            sampleRepository.InsertItemAsync(testData).GetAwaiter().GetResult();

            // Read (get) operation
            var item = sampleRepository.GetItemAsync(testData.Id).Result;
            var comparisonItem = testData;

            ValidateOperation(item, comparisonItem, nameof(sampleRepository.InsertItemAsync));

            // Update operation
            item.SampleIntProperty = ++item.SampleIntProperty;
            item.SampleStringProperty = $"{item.SampleStringProperty} - UPDATED";
            var currentTimestamp = DateTimeOffset.UtcNow;
            item.Timestamp = currentTimestamp;
            sampleRepository.UpdateItemAsync(item).GetAwaiter().GetResult();

            // Re-read to validate updates
            var updatedItem = sampleRepository.GetItemAsync(testData.Id).GetAwaiter().GetResult();
            ValidateOperation(updatedItem, item, nameof(sampleRepository.UpdateItemAsync));

            // Upsert Operation (existing item)
            updatedItem.SampleStringProperty = updatedItem.SampleStringProperty.Replace("UPDATED", "UPSERTED");
            sampleRepository.UpsertItemAsync(updatedItem).GetAwaiter().GetResult();

            // Re-read to validate updates
            var upsertedExistingItem = sampleRepository.GetItemAsync(testData.Id).GetAwaiter().GetResult();
            ValidateOperation(upsertedExistingItem, updatedItem, nameof(sampleRepository.UpdateItemAsync));

            // Upsert Operation (new item)
            var upsertNewItem = new SampleModel
            {
                Id = testData.Id + "_new_upsert",
                SampleIntProperty = 1,
                SampleStringProperty = "UPSERTED ITEM",
                Timestamp = DateTimeOffset.UtcNow
            };

            sampleRepository.UpsertItemAsync(upsertNewItem).GetAwaiter().GetResult();

            // Re-read to validate updates
            var upsertedNewItem = sampleRepository.GetItemAsync(upsertNewItem.Id).GetAwaiter().GetResult();
            ValidateOperation(upsertedNewItem, upsertNewItem, nameof(sampleRepository.UpsertItemAsync));

            var allItems = sampleRepository.GetAsync().GetAwaiter().GetResult();
            Assert.True(allItems.Count() == 2, $"New item was not inserted as a result of the upsert operation, but the {nameof(sampleRepository.UpsertItemAsync)} operation returned successfully");

            // Delete (remove) operation
            sampleRepository.RemoveItemAsync(updatedItem).GetAwaiter().GetResult();

            // Re-read to validate updates
            var expectedNullResult = sampleRepository.GetItemAsync(testData.Id).GetAwaiter().GetResult();
            Assert.IsNull(expectedNullResult, $"Item was not deleted, but the {nameof(sampleRepository.RemoveItemAsync)} operation returned successfully");
        }

        [Test, Order(1), TestCase(TestName = "Bulk Operations")]
        public void BulkOperationsTest()
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            Assert.NotNull(sampleRepository);

            var testData = TestData.GenerateBulkOperationsTestData(5);

            // Bulk create (insert) operation
            sampleRepository.InsertAsync(testData).GetAwaiter().GetResult();

            // Read all (get all) operation
            var persistedItems = sampleRepository.GetAsync().GetAwaiter().GetResult();
            Assert.True(persistedItems.Count() == testData.Count(), $"Unexpected number of items persisted in {nameof(sampleRepository.InsertAsync)}");

            for (int i = 0; i < testData.Count(); i++)
                ValidateOperation(persistedItems.ElementAt(i), testData.ElementAt(i), nameof(sampleRepository.InsertAsync));

            // Custom Query
            int greaterThanValue = testData.Count / 2;
            var expectedResults = testData.Count - greaterThanValue;

            var queryResults = sampleRepository.GetItemsWithSampleIntValueGreaterThan(greaterThanValue).GetAwaiter().GetResult();
            Assert.AreEqual(expectedResults, queryResults.Count(), $"Unexpected number of items returned by {nameof(sampleRepository.GetItemsWithSampleIntValueGreaterThan)}");

            Assert.True(queryResults.Count(i => i.SampleIntProperty < greaterThanValue) == 0, $"Unexpected item returned by {nameof(sampleRepository.GetItemsWithSampleIntValueGreaterThan)}");

            // Bulk update operation
            var currentTimestamp = DateTimeOffset.UtcNow;

            foreach (var item in persistedItems)
            {
                item.SampleIntProperty = ++item.SampleIntProperty;
                item.SampleStringProperty = $"{item.SampleStringProperty} - UPDATED";
                item.Timestamp = currentTimestamp;
            }

            sampleRepository.UpdateAsync(persistedItems).GetAwaiter().GetResult();

            // Re-read to validate updates
            var persistedUpdatedItems = sampleRepository.GetAsync().GetAwaiter().GetResult();

            for (int i = 0; i < testData.Count(); i++)
                ValidateOperation(persistedUpdatedItems.ElementAt(i), persistedItems.ElementAt(i), nameof(sampleRepository.UpdateAsync));

            // Bulk delete (remove) operation
            sampleRepository.RemoveAsync(persistedUpdatedItems).GetAwaiter().GetResult();

            // Re-read to validate updates
            var expectedNoResults = sampleRepository.GetAsync().Result;
            Assert.That(!expectedNoResults.Any(), $"Items were not deleted, but the {nameof(sampleRepository.RemoveAsync)} operation returned successfully");
        }

        protected override void OnValidateOperation(SampleModel item, SampleModel comparisonItem, string operation)
        {
            base.OnValidateOperation(item, comparisonItem, operation);

            Assert.True(item.Id == comparisonItem.Id, $"{nameof(item.Id)} was not inserted and/or parsed correctly");
            Assert.True(item.SampleIntProperty == comparisonItem.SampleIntProperty, $"{nameof(item.SampleIntProperty)} was not inserted and/or parsed correctly");
            Assert.True(item.SampleStringProperty == comparisonItem.SampleStringProperty, $"{nameof(item.SampleStringProperty)} was not inserted and/or parsed correctly");
            Assert.True(item.Timestamp.UtcTicks == comparisonItem.Timestamp.UtcTicks, $"{nameof(item.Timestamp)} not inserted and/or parsed correctly");
        }
    }
}