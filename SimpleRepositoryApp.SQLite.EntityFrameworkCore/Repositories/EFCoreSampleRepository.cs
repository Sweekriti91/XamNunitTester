using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.MobCAT.Repository.EntityFrameworkCore;
using SimpleRepositoryApp.Abstractions;
using SimpleRepositoryApp.Models;
using SimpleRepositoryApp.SQLite.EntityFrameworkCore.Entities;

namespace SimpleRepositoryApp.SQLite.EntityFrameworkCore.Repositories
{
    public class EFCoreSampleRepository : BaseEFCoreRepository<SampleModel, EFCoreSampleDataItem>, ISampleRepository
    {
        public EFCoreSampleRepository(SqliteConnection connection) : base(connection)
        {
        }

        protected override EFCoreSampleDataItem ToRepositoryType(SampleModel modelType)
        {
            return modelType != null ? new EFCoreSampleDataItem
            {
                Id = modelType.Id,
                SampleString = modelType.SampleStringProperty,
                SampleInt = modelType.SampleIntProperty,
                TimestampTicks = modelType.Timestamp.UtcTicks
            } : null;
        }

        protected override SampleModel ToModelType(EFCoreSampleDataItem repositoryType)
        {
            return repositoryType != null ? new SampleModel
            {
                Id = repositoryType.Id,
                SampleStringProperty = repositoryType.SampleString,
                SampleIntProperty = repositoryType.SampleInt,
                Timestamp = new DateTimeOffset(repositoryType.TimestampTicks, TimeSpan.Zero)
            } : null;
        }

        // Add model-specific queries and other actions here
        public async Task<IEnumerable<SampleModel>> GetItemsWithSampleIntValueGreaterThan(int value)
           => (await ExecuteTableQueryAsync(i => i.SampleInt > value))?
               .Select(i => ToModelType(i))?.ToList();
    }
}