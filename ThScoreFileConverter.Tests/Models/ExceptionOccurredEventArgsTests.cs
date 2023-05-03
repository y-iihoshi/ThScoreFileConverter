using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class ExceptionOccurredEventArgsTests
{
    [TestMethod]
    public void ExceptionOccurredEventArgsTest()
    {
        // NOTE: creating an Exception instance causes CA2201.
        var ex = new NotImplementedException();
        var args = new ExceptionOccurredEventArgs(ex);
        Assert.AreSame(ex, args.Exception);
    }
}
