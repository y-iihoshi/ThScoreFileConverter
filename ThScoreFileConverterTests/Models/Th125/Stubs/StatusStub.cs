using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverterTests.Models.Th125.Stubs
{
    internal class StatusStub : IStatus
    {
        public StatusStub() { }

        public StatusStub(IStatus status)
            : this()
        {
            this.TotalPlayTime = status.TotalPlayTime;
            this.BgmFlags = status.BgmFlags?.ToArray();
            this.LastName = status.LastName?.ToArray();
            this.Checksum = status.Checksum;
            this.IsValid = status.IsValid;
            this.Signature = status.Signature;
            this.Size = status.Size;
            this.Version = status.Version;
        }

        public int TotalPlayTime { get; set; }

        public IEnumerable<byte> BgmFlags { get; set; }

        public IEnumerable<byte> LastName { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}
