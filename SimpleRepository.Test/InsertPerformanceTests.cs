using NUnit.Framework;
using SimpleRepositoryApp.Abstractions;

namespace SimpleRepository.Test
{
    public class InsertPerformanceTests : BaseRepositoryTestFixture<ISampleRepositoryContext>
    {
        [Test, Order(0)]
        [TestCase(5000, TestName = "Single Insert Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Single Insert Performance (10,000 records)")]
        [TestCase(15000, TestName = "Single Insert Performance (15,000 records)")]
#endif
        public void SingleInsertOperationsPerformanceTest(int insertionsToTest)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(insertionsToTest);

            // Perform and measure insert operations
            var executionTime = ExecutionTimeInMilliseconds(() =>
            {
                foreach (var item in testData)
                    sampleRepository.InsertItemAsync(item).GetAwaiter().GetResult();
            });

            Assert.Pass($"{insertionsToTest} records inserted: {executionTime} ms");
        }

        [Test, Order(1)]
        [TestCase(5000, TestName = "Bulk Insert Performance (5,000 records)")]
#if !QUICK_TEST
        [TestCase(10000, TestName = "Bulk Insert Performance (10,000 records)")]
        [TestCase(15000, TestName = "Bulk Insert Performance (15,000 records)")]
#endif
        public void BulkInsertOperationsPerformanceTest(int insertionsToTest)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(insertionsToTest);

            // Perform and measure insert operations
            var executionTime = ExecutionTimeInMilliseconds(() => sampleRepository.InsertAsync(testData).GetAwaiter().GetResult());

            Assert.Pass($"{insertionsToTest} records inserted: {executionTime} ms");
        }
    }
}