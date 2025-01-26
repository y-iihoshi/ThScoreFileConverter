using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverter.Tests.Models.Th075.Stubs;

internal sealed class HighScoreStub : IHighScore
{
    public HighScoreStub()
    {
        this.EncodedName = [];
        this.Name = string.Empty;
    }

    public HighScoreStub(in HighScoreStub stub)
    {
        this.EncodedName = stub.EncodedName.ToArray();
        this.Day = stub.Day;
        this.Month = stub.Month;
        this.Name = stub.Name;
        this.Score = stub.Score;
    }

    public IEnumerable<byte> EncodedName { get; set; }

    public byte Day { get; set; }

    public byte Month { get; set; }

    public string Name { get; set; }

    public int Score { get; set; }
}
