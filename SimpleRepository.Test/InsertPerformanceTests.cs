using NUnit.Framework;
using SimpleRepositoryApp.Abstractions;

namespace SimpleRepository.Test
{
    public class InsertPerformanceTests : BaseRepositoryTestFixture<ISampleRepositoryContext>
    {
        [Test, Order(0)]
        [TestCase(5, TestName = "Single Insert Performance (5,000 records)")]
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
        [TestCase(5, TestName = "Bulk Insert Performance (5,000 records)")]
        public void BulkInsertOperationsPerformanceTest(int insertionsToTest)
        {
            ISampleRepository sampleRepository = RepositoryContext.SampleRepository;
            var testData = TestData.GenerateBulkOperationsTestData(insertionsToTest);

            // Perform and measure insert operations
            var executionTime = ExecutionTimeInMilliseconds(() => sampleRepository.InsertAsync(testData).GetAwaiter().GetResult());

            Assert.Pass($"{insertionsToTest} records inserted: {executionTime} ms");
        }

        //[Test, Order(0)]
        //[TestCase(1, TestName = "Test Pass")]
        //public void PassTest(int insertionsToTest)
        //{
        //    Assert.Pass($" {insertionsToTest} HEY THIS PASSED!");
        //}

        [Test, Order(2)]
        [TestCase(5, TestName = "Test Fail")]
        public void FailTest(int insertionsToTest)
        {
            Assert.Fail($" {insertionsToTest} HEY THIS FAILED!");
        }
    }
}