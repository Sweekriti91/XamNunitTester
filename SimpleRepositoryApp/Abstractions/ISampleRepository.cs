using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.MobCAT.Repository.Abstractions;
using SimpleRepositoryApp.Models;

namespace SimpleRepositoryApp.Abstractions
{
    public interface ISampleRepository: IBaseRepository<SampleModel>
    {
        // Add model-specific queries and other actions here
        Task<IEnumerable<SampleModel>> GetItemsWithSampleIntValueGreaterThan(int value);
    }
}