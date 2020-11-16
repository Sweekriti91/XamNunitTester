using System;
using System.Collections.Generic;
using SimpleRepositoryApp.Models;

namespace SimpleRepository.Test
{
    internal static class TestData
    {
        internal static List<SampleModel> GenerateBulkOperationsTestData(int collectioncount)
        {
            List<SampleModel> dataItems = new List<SampleModel>();

            for (int i = 0; i < collectioncount; i++)
                dataItems.Add(GenerateSingleOperationsTestData(i + 1));

            return dataItems;
        }

        internal static SampleModel GenerateSingleOperationsTestData(int index)
        {
            return new SampleModel
            {
                Id = (index).ToString(),
                SampleStringProperty = $"Test item {index}",
                SampleIntProperty = index,
                Timestamp = DateTimeOffset.UtcNow
            };
        }
    }
}