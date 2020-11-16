using Microsoft.MobCAT.Repository.Abstractions;

namespace SimpleRepositoryApp.Abstractions
{
    public interface ISampleRepositoryContext : IRepositoryContext
    {
        ISampleRepository SampleRepository { get; }
    }
}