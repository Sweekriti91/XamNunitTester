// ***********************************************************************
// Copyright (c) 2017 NUnit Project
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Runner.Services;

namespace NUnit.Runner.Helpers
{
    internal class TestPackage
    {
        private static readonly NUnitTestAssemblyRunner _runner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());

        public int TestsCount => _runner.IsTestLoaded ? _runner.CountTestCases(TestFilter.Empty) : 0;

        public TestOptions Options { get; set; }

        public IEnumerable<ITest> LoadedTests
        {
            get
            {
                var tests = _runner.LoadedTest.Tests;

                while (tests?.Count == 1)
                {
                    tests = tests.Single().Tests;
                }

                return tests;
            }
        }

        /***
         * 
         * NEW CODE STUFF HERE
         * 
         * 
         * **/
        private readonly List<(Assembly, Dictionary<string, object>)> _testAssemblies = new List<(Assembly, Dictionary<string, object>)>();

        public void AddAssembly(Assembly testAssembly, Dictionary<string, object> options = null)
        {
            _testAssemblies.Add((testAssembly, options));
            _runner.Load(testAssembly, options ?? new Dictionary<string, object>());
        }

        private static async Task<NUnitTestAssemblyRunner> LoadTestAssemblyAsync(Assembly assembly, Dictionary<string, object> options)
        {
            await Task.Run(() => _runner.Load(assembly, options ?? new Dictionary<string, object>()));
            return _runner;
        }

        /***
           * 
           * NEW CODE STUFF HERE
           * 
           * 
           * **/

        public async Task<TestRunResult> ExecuteTests()
        {
            var resultPackage = new TestRunResult();
            foreach (var (assembly, options) in _testAssemblies)
            {
                NUnitTestAssemblyRunner runner = await LoadTestAssemblyAsync(assembly, options).ConfigureAwait(false);
                ITestResult result = await Task.Run(() => runner.Run(TestListener.NULL, TestFilter.Empty)).ConfigureAwait(false);
                LogTestRun(result);
                resultPackage.AddResult(result);
            }


            resultPackage.CompleteTestRun();
            return resultPackage;
        }

        public async Task<TestRunResult> ExecuteTests(IEnumerable<ITest> tests, bool force = false)
        {
            var resultPackage = new TestRunResult();

            var filter = new CustomTestFilter(tests, force);
            var listener = Options?.LogToOutput == true ? new CustomTestListener() : TestListener.NULL;
            var result = await Task.Run(() => _runner.Run(listener, filter)).ConfigureAwait(false);

            LogTestRun(result);

            resultPackage.AddResult(result);
            resultPackage.CompleteTestRun();
            return resultPackage;
        }

        public async Task<ResultSummary> ProcessResults(TestRunResult results)
        {
            var summary = new ResultSummary(results);

            var _resultProcessor = TestResultProcessor.BuildChainOfResponsability(Options);
            await _resultProcessor.Process(summary).ConfigureAwait(false);

            return summary;
        }

        private static void LogTestRun(ITestResult result)
        {
            var total = result.FailCount + result.PassCount + result.InconclusiveCount;
            var message = $"Tests run: {total} Passed: {result.PassCount} Failed: {result.FailCount} Inconclusive: {result.InconclusiveCount}";
            Debug.WriteLine(message);
        }

        private class CustomTestListener : ITestListener
        {
            public void SendMessage(TestMessage message)
            {
                throw new NotImplementedException();
            }

            public void TestFinished(ITestResult result)
            {
                if (!result.Test.IsSuite)
                {
                    var className = result.Test.ClassName?.Split('.').LastOrDefault();
                    var status = result.ResultState.Status.ToString().ToUpper();
                    var message = $"\t[{status}] {className}.{result.Test.Name}";
                    Debug.WriteLine(message);
                }
            }

            public void TestOutput(TestOutput output)
            {
            }

            public void TestStarted(ITest test)
            {
            }
        }

        private class CustomTestFilter : TestFilter
        {
            private readonly HashSet<string> _testNames;
            private readonly bool _force;

            public CustomTestFilter(IEnumerable<ITest> tests, bool force)
            {
                if (tests != null)
                {
                    var names = tests.Flatten()
                                     .Select(t => t.FullName);

                    _testNames = new HashSet<string>(names);
                }

                _force = force;
            }

            public override TNode AddToXml(TNode parentNode, bool recursive)
            {
                return parentNode.AddElement("filter");
            }

            public override bool Match(ITest test)
            {
                // If filter was created with null tests collection, we assume we want to run all tests
                if (_testNames?.Contains(test.FullName) ?? true)
                {
                    // We don't want to run explicit tests
                    if (!_force)
                    {
                        var parent = test;
                        while (parent != null)
                        {
                            if (parent.RunState != RunState.Runnable)
                            {
                                return false;
                            }

                            parent = parent.Parent;
                        }
                    }

                    return true;
                }

                return false;
            }
        }
    }
}
