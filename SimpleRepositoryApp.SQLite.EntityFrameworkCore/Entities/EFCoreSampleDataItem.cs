using Microsoft.MobCAT.Repository.EntityFrameworkCore;

namespace SimpleRepositoryApp.SQLite.EntityFrameworkCore.Entities
{
    public class EFCoreSampleDataItem : BaseEFCoreModel
    {
        public string SampleString { get; set; }
        public int SampleInt { get; set; }
        public long TimestampTicks { get; set; }
    }
}