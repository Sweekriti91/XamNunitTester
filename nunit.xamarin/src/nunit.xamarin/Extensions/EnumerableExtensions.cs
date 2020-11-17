using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework.Interfaces;

[EditorBrowsable(EditorBrowsableState.Never)]
internal static class EnumerableExtensions
{
    internal static IEnumerable<ITest> Flatten(this IEnumerable<ITest> tests)
    {
        foreach (var test in tests)
        {
            if (test.Tests.Any())
            {
                foreach (var child in test.Tests.Flatten())
                {
                    yield return child;
                }
            }
            else
            {
                yield return test;
            }
        }
    }

    internal static IEnumerable<ITestResult> Flatten(this IEnumerable<ITestResult> results)
    {
        foreach (var result in results)
        {
            if (result.Children.Any())
            {
                foreach (var child in result.Children.Flatten())
                {
                    yield return child;
                }
            }

            yield return result;
        }
    }
}
