using System;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class ThCryptTests
{
    private readonly byte[] decrypted = [0x01, 0x04, 0x09, 0x10, 0x19, 0x24, 0x31, 0x40];
    private readonly byte[] encryptedBySmallBlock = [0x02, 0x42, 0x73, 0xAF, 0xA2, 0x32, 0x7B, 0x67];
    private readonly byte[] encryptedByLargeBlock = [0x52, 0x62, 0x6A, 0xAA, 0xD3, 0x0F, 0x43, 0x7F];

    private const int KEY = 0x12;
    private const int STEP = 0x34;
    private const int SMALL_BLOCK = 0x04;   // < this.decrypted.Length
    private const int LARGE_BLOCK = 0x10;   // > this.decrypted.Length
    private const int LIMIT = 0x20;

    [TestMethod]
    public void EncryptTest()
    {
        using var input = new MemoryStream(this.decrypted);
        using var output = new MemoryStream();

        _ = Assert.ThrowsException<NotImplementedException>(
            () => ThCrypt.Encrypt(input, output, (int)input.Length, KEY, STEP, SMALL_BLOCK, LIMIT));
    }

    [TestMethod]
    public void DecryptTest()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, SMALL_BLOCK, LIMIT);

        var actual = output.ToArray();
        Assert.AreEqual(this.decrypted.Length, actual.Length);
        CollectionAssert.AreEqual(this.decrypted, actual);
    }

    [TestMethod]
    public void DecryptTestNullStreamInput()
    {
        using var output = new MemoryStream();

        ThCrypt.Decrypt(Stream.Null, output, this.encryptedBySmallBlock.Length, KEY, STEP, SMALL_BLOCK, LIMIT);
        Assert.AreEqual(0, output.Length);
    }

    [TestMethod]
    public void DecryptTestEmptyInput()
    {
        using var input = new MemoryStream();
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, this.encryptedBySmallBlock.Length, KEY, STEP, SMALL_BLOCK, LIMIT);
        Assert.AreEqual(0, output.Length);
    }

    [TestMethod]
    public void DecryptTestUnreadableInput()
    {
        using var input = new UnreadableMemoryStream();
        using var output = new MemoryStream();

        _ = Assert.ThrowsException<NotSupportedException>(
            () => ThCrypt.Decrypt(input, output, this.encryptedBySmallBlock.Length, KEY, STEP, SMALL_BLOCK, LIMIT));
    }

    [TestMethod]
    public void DecryptTestClosedInput()
    {
        using var output = new MemoryStream();
        var input = new MemoryStream(this.encryptedBySmallBlock);
        input.Close();

        _ = Assert.ThrowsException<ObjectDisposedException>(
            () => ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, SMALL_BLOCK, LIMIT));
    }

    [TestMethod]
    public void DecryptTestShortenedInput()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock, 0, this.encryptedBySmallBlock.Length - 1);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, SMALL_BLOCK, LIMIT);

        var actual = output.ToArray();
        Assert.AreNotEqual(this.decrypted.Length, actual.Length);
    }

    [TestMethod]
    public void DecryptTestInvalidInput()
    {
        var invalid = new byte[this.encryptedBySmallBlock.Length];
        this.encryptedBySmallBlock.CopyTo(invalid, 0);
        ++invalid[^1];

        using var input = new MemoryStream(invalid);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, SMALL_BLOCK, LIMIT);

        var actual = output.ToArray();
        Assert.AreEqual(this.decrypted.Length, actual.Length);
        CollectionAssert.AreNotEqual(this.decrypted, actual);
    }

    [TestMethod]
    public void DecryptTestUnwritableOutput()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream([], false);

        _ = Assert.ThrowsException<NotSupportedException>(
            () => ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, SMALL_BLOCK, LIMIT));
    }

    [TestMethod]
    public void DecryptTestClosedOutput()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        var output = new MemoryStream();
        output.Close();

        _ = Assert.ThrowsException<ObjectDisposedException>(
            () => ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, SMALL_BLOCK, LIMIT));
    }

    [TestMethod]
    public void DecryptTestNegativeSize()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => ThCrypt.Decrypt(input, output, -1, KEY, STEP, SMALL_BLOCK, LIMIT));
    }

    [TestMethod]
    public void DecryptTestZeroSize()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, 0, KEY, STEP, SMALL_BLOCK, LIMIT);
        Assert.AreEqual(0, output.Length);
    }

    [TestMethod]
    public void DecryptTestShortenedSize()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, (int)input.Length - 1, KEY, STEP, SMALL_BLOCK, LIMIT);

        var actual = output.ToArray();
        Assert.AreNotEqual(this.decrypted.Length, actual.Length);
    }

    [TestMethod]
    public void DecryptTestExceededSize()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, (int)input.Length + 1, KEY, STEP, SMALL_BLOCK, LIMIT);

        var actual = output.ToArray();
        CollectionAssert.AreEqual(this.decrypted, actual);
    }

    [TestMethod]
    public void DecryptTestZeroKey()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, (int)input.Length, 0, STEP, SMALL_BLOCK, LIMIT);

        var actual = output.ToArray();
        Assert.AreEqual(this.decrypted.Length, actual.Length);
        CollectionAssert.AreNotEqual(this.decrypted, actual);
    }

    [TestMethod]
    public void DecryptTestInvalidKey()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, (int)input.Length, 1, STEP, SMALL_BLOCK, LIMIT);

        var actual = output.ToArray();
        Assert.AreEqual(this.decrypted.Length, actual.Length);
        CollectionAssert.AreNotEqual(this.decrypted, actual);
    }

    [TestMethod]
    public void DecryptTestZeroStep()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, (int)input.Length, KEY, 0, SMALL_BLOCK, LIMIT);

        var actual = output.ToArray();
        Assert.AreEqual(this.decrypted.Length, actual.Length);
        CollectionAssert.AreNotEqual(this.decrypted, actual);
    }

    [TestMethod]
    public void DecryptTestInvalidStep()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, (int)input.Length, KEY, 1, SMALL_BLOCK, LIMIT);

        var actual = output.ToArray();
        Assert.AreEqual(this.decrypted.Length, actual.Length);
        CollectionAssert.AreNotEqual(this.decrypted, actual);
    }

    [TestMethod]
    public void DecryptTestNegativeBlock()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, -1, LIMIT));
    }

    [TestMethod]
    public void DecryptTestZeroBlock()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        _ = Assert.ThrowsException<DivideByZeroException>(
            () => ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, 0, LIMIT));
    }

    [TestMethod]
    public void DecryptTestInvalidSmallBlock()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        var size = (int)input.Length;
        for (var block = 1; block <= SMALL_BLOCK * 2; ++block)
        {
            if (block == SMALL_BLOCK)
                continue;

            input.Position = 0;
            output.Position = 0;
            ThCrypt.Decrypt(input, output, size, KEY, STEP, block, LIMIT);

            var actual = output.ToArray();

            // We are doing the decryption in a vague atmosphere.
            Assert.AreEqual(this.decrypted.Length, actual.Length);
            CollectionAssert.AreNotEqual(this.decrypted, actual);
            CollectionAssert.AreNotEqual(this.encryptedBySmallBlock, actual);
        }
    }

    [TestMethod]
    public void DecryptTestInvalidLargeBlock()
    {
        using var input = new MemoryStream(this.encryptedByLargeBlock);
        using var output = new MemoryStream();

        var size = (int)input.Length;
        for (var block = 1; block <= LARGE_BLOCK * 2; ++block)
        {
            if (block == LARGE_BLOCK)
                continue;

            input.Position = 0;
            output.Position = 0;
            ThCrypt.Decrypt(input, output, size, KEY, STEP, block, LIMIT);

            var actual = output.ToArray();

            // We are doing the decryption in a vague atmosphere.
            if (block >= size)
            {
                CollectionAssert.AreEqual(this.decrypted, actual);
            }
            else
            {
                Assert.AreEqual(this.decrypted.Length, actual.Length);
                CollectionAssert.AreNotEqual(this.decrypted, actual);
                CollectionAssert.AreNotEqual(this.encryptedByLargeBlock, actual);
            }
        }
    }

    [TestMethod]
    public void DecryptTestNegativeLimit()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, SMALL_BLOCK, -1));
    }

    [TestMethod]
    public void DecryptTestZeroLimit()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, SMALL_BLOCK, 0);

        var actual = output.ToArray();
        Assert.AreEqual(this.decrypted.Length, actual.Length);
        CollectionAssert.AreNotEqual(this.decrypted, actual);
        CollectionAssert.AreEqual(this.encryptedBySmallBlock, actual);
    }

    [TestMethod]
    public void DecryptTestInvalidLimitWithSmallBlock()
    {
        using var input = new MemoryStream(this.encryptedBySmallBlock);
        using var output = new MemoryStream();

        for (var limit = 1; limit <= LIMIT * 2; ++limit)
        {
            if (limit == LIMIT)
                continue;

            input.Position = 0;
            output.Position = 0;
            ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, SMALL_BLOCK, limit);

            var actual = output.ToArray();

            // We are doing the decryption in a vague atmosphere.
            if (limit > SMALL_BLOCK)
            {
                CollectionAssert.AreEqual(this.decrypted, actual);
            }
            else
            {
                Assert.AreEqual(this.decrypted.Length, actual.Length);
                CollectionAssert.AreNotEqual(this.decrypted, actual);
                CollectionAssert.AreNotEqual(this.encryptedBySmallBlock, actual);
            }
        }
    }

    [TestMethod]
    public void DecryptTestInvalidLimitWithLargeBlock()
    {
        using var input = new MemoryStream(this.encryptedByLargeBlock);
        using var output = new MemoryStream();

        // NOTE: Any value for limit has no effect when size <= block.
        for (var limit = 1; limit <= LIMIT * 2; ++limit)
        {
            if (limit == LIMIT)
                continue;

            input.Position = 0;
            output.Position = 0;
            ThCrypt.Decrypt(input, output, (int)input.Length, KEY, STEP, LARGE_BLOCK, limit);

            var actual = output.ToArray();
            CollectionAssert.AreEqual(this.decrypted, actual);
        }
    }
}
