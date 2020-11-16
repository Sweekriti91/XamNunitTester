using NUnit.Framework;
using SimpleRepositoryApp.Abstractions;

namespace SimpleRepository.Test
{
    public class CustomQueryPerformanceTests : BaseRepositoryTestFixture<ISampleRepositoryContext>
    {
        [Test, Order(0)]
        [TestCase(5000, TestName = "Custom Query Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Custom Query Performance (10,000 records)")]
        [TestCase(15000, TestName = "Custom Query Performance (15,000 records)")]
#endif
        public void CustomQueryPerformanceTest(int itemsToQuery)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(itemsToQuery);

            // Insert initial data
            sampleRepository.InsertAsync(testData).GetAwaiter().GetResult();

            // Perform and measure delete operations
            var executionTime = ExecutionTimeInMilliseconds(() =>
            {
                sampleRepository.GetItemsWithSampleIntValueGreaterThan(itemsToQuery / 2).GetAwaiter().GetResult();
            });

            Assert.Pass($"{itemsToQuery} records queried: {executionTime} ms");
        }
    }
}