using System.Threading;

namespace ThScoreFileConverter.Tests.UnitTesting;

public sealed class SkipOrSTATestMethodAttribute : TestMethodAttribute
{
    public override TestResult[] Execute(ITestMethod testMethod)
    {
        // HACK: avoid null dereferences in net8.0 tests
        //
        // Although the root cause is unknown, accessing Thread.CurrentThread
        // during testing can cause a NullReferenceException on net8.0.
        // This behavior has only been observed on the GitHub-hosted runners
        // (at least windows-2022 20231205.1.0 and 20231211.1.0).

#if NET8_0_OR_GREATER
        return [
            new TestResult { Outcome = UnitTestOutcome.Inconclusive }
        ];
#else
        if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            return base.Execute(testMethod);

        TestResult[]? result = null;
        var thread = new Thread(() => result = base.Execute(testMethod));
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        return result!;
#endif
    }
}
