﻿using System.Globalization;

namespace ThScoreFileConverter.Core.Tests.UnitTesting;

public static class TestHelper
{
    public static IEnumerable<object[]> GetInvalidEnumerators<TEnum>()
        where TEnum : struct, Enum
    {
        Enum.GetUnderlyingType(typeof(TEnum)).ShouldBeSameAs(typeof(int));

#pragma warning disable CA2021 // Don't call Enumerable.Cast<T> or Enumerable.OfType<T> with incompatible types
        var values = Enum.GetValues<TEnum>().Cast<int>().ToArray();
#pragma warning restore CA2021 // Don't call Enumerable.Cast<T> or Enumerable.OfType<T> with incompatible types

        yield return new object[] { values.Min() - 1 };
        yield return new object[] { values.Max() + 1 };
    }

    public static void ShouldNotReachHere(string? message = null)
    {
        Assert.Fail(string.IsNullOrEmpty(message) ? nameof(ShouldNotReachHere) : $"{nameof(ShouldNotReachHere)}: {message}");
    }

    public static IDisposable BackupCultureInfo()
    {
        return new CultureInfoDisposable();
    }

    private sealed class CultureInfoDisposable : IDisposable
    {
        private readonly CultureInfo cultureInfo;
        private readonly CultureInfo uiCultureInfo;

        public CultureInfoDisposable()
        {
            this.cultureInfo = CultureInfo.CurrentCulture;
            this.uiCultureInfo = CultureInfo.CurrentUICulture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = this.cultureInfo;
            CultureInfo.CurrentUICulture = this.uiCultureInfo;
        }
    }
}
