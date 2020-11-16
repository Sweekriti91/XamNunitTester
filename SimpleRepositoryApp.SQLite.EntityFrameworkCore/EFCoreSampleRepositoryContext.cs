using Microsoft.MobCAT.Repository.EntityFrameworkCore;
using SimpleRepositoryApp.Abstractions;
using SimpleRepositoryApp.SQLite.EntityFrameworkCore.Repositories;

namespace SimpleRepositoryApp.SQLite.EntityFrameworkCore
{
    public class EFCoreSampleRepositoryContext : BaseEFCoreRepositoryContext, ISampleRepositoryContext
    {
        ISampleRepository _sampleRepository;

        public EFCoreSampleRepositoryContext(string folderPath, string datastoreName)
            : base(folderPath, datastoreName) { }

        public ISampleRepository SampleRepository => _sampleRepository ?? (_sampleRepository = new EFCoreSampleRepository(Connection));

        protected override void OnResetRepositories()
            => _sampleRepository = null;
    }
}