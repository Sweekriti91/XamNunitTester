using System;
using Microsoft.MobCAT;
using SimpleRepositoryApp.Abstractions;

namespace SimpleRepository.Test
{
    public static class TestBootstrap
    {
        const string DatastoreName = "sqlitenet_testdb";

        public static void Begin(Func<string, ISampleRepositoryContext> sampleRepositoryContext)
            => ServiceContainer.Register(sampleRepositoryContext(DatastoreName));
    }
}