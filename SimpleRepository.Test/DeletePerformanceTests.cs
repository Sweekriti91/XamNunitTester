using NUnit.Framework;
using SimpleRepositoryApp.Abstractions;

namespace SimpleRepository.Test
{
    public class DeletePerformanceTests : BaseRepositoryTestFixture<ISampleRepositoryContext>
    {
        [Test, Order(0)]
        [TestCase(5000, TestName = "Single Delete Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Single Delete Performance (10,000 records)")]
        [TestCase(15000, TestName = "Single Delete Performance (15,000 records)")]
#endif
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
        [TestCase(5000, TestName = "Bulk Delete Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Bulk Delete Performance (10,000 records)")]
        [TestCase(15000, TestName = "Bulk Delete Performance (15,000 records)")]
#endif
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