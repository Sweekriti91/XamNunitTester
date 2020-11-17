using NUnit.Framework;
using SimpleRepositoryApp.Abstractions;

namespace SimpleRepository.Test
{
    public class DeletePerformanceTests : BaseRepositoryTestFixture<ISampleRepositoryContext>
    {
        [Test, Order(0)]
        [TestCase(5, TestName = "Single Delete Performance (5,000 records)")]
        public void SingleDeletePerformanceTest(int itemsToDelete)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToDelete);

            // Insert initial data
            sampleRepository.InsertAsync(testData).GetAwaiter().GetResult();

            // Get persisted data
            var persistedItems = sampleRepository.GetAsync().GetAwaiter().GetResult();

            // Perform and measure delete operations
            var executionTime = ExecutionTimeInMilliseconds(() =>
            {
                foreach (var item in persistedItems)
                    sampleRepository.RemoveItemAsync(item).GetAwaiter().GetResult();
            });

            Assert.Pass($"{itemsToDelete} records deleted: {executionTime} ms");
        }

        [Test, Order(1)]
        [TestCase(5, TestName = "Bulk Delete Performance (5,000 records)")]
        public void BulkDeletePerformanceTest(int itemsToDelete)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToDelete);

            // Insert initial data
            sampleRepository.InsertAsync(testData).GetAwaiter().GetResult();

            // Get persisted data
            var persistedItems = sampleRepository.GetAsync().GetAwaiter().GetResult();

            // Perform and measure delete operations
            var executionTime = ExecutionTimeInMilliseconds(() => sampleRepository.RemoveAsync(persistedItems).GetAwaiter().GetResult());

            Assert.Pass($"{itemsToDelete} records deleted: {executionTime} ms");
        }
    }
}