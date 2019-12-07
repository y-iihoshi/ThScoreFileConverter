using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th17;

namespace ThScoreFileConverterTests.Models.Th17.Stubs
{
    internal class StatusStub : IStatus
    {
        public StatusStub() { }

        public StatusStub(IStatus status)
            : this()
        {
            this.Achievements = status.Achievements?.ToArray();
            this.BgmFlags = status.BgmFlags?.ToArray();
            this.LastName = status.LastName?.ToArray();
            this.TotalPlayTime = status.TotalPlayTime;
            this.Checksum = status.Checksum;
            this.IsValid = status.IsValid;
            this.Signature = status.Signature;
            this.Size = status.Size;
            this.Version = status.Version;
        }

        public IEnumerable<byte> Achievements { get; set; }

        public IEnumerable<byte> BgmFlags { get; set; }

        public IEnumerable<byte> LastName { get; set; }

        public int TotalPlayTime { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}
