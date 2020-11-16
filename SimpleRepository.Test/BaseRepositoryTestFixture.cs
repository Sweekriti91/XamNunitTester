using System;
using System.Diagnostics;
using Microsoft.MobCAT;
using Microsoft.MobCAT.Repository;
using Microsoft.MobCAT.Repository.Abstractions;
using NUnit.Framework;

namespace SimpleRepository.Test
{
    [TestFixture, Parallelizable(ParallelScope.None)]
    public class BaseRepositoryTestFixture<T> where T : IRepositoryContext
    {
        T _repositoryContext;
        protected T RepositoryContext => _repositoryContext == null ? (_repositoryContext = ServiceContainer.Resolve<T>()) : _repositoryContext;

        [SetUp]
        public void Arrange()
            => OnArrange();

        [TearDown]
        public void Teardown()
            => OnTeardown();

        protected virtual void OnArrange()
            => RepositoryContext.SetupAsync().GetAwaiter().GetResult();

        protected virtual void OnTeardown()
            => RepositoryContext.DeleteAsync().GetAwaiter().GetResult();

        protected long ExecutionTimeInMilliseconds(Action action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            action();

            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }
    }

    public class BaseRepositoryTestFixture<T, T2> :
        BaseRepositoryTestFixture<T> where T : IRepositoryContext where T2 : BaseModel
    {
        protected void ValidateOperation(T2 item, T2 comparisonItem, string operation)
            => OnValidateOperation(item, comparisonItem, operation);

        protected virtual void OnValidateOperation(T2 item, T2 comparisonItem, string operation)
         => Assert.True(item != null, $"Item is null, but the {nameof(operation)} operation returned successfully");
    }
}