using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143.Stubs
{
    internal class ItemStatusStub : IItemStatus
    {
        public ItemStatusStub()
        {
            this.Signature = string.Empty;
        }

        public ItemStatusStub(IItemStatus itemStatus)
        {
            this.AvailableCount = itemStatus.AvailableCount;
            this.ClearedCount = itemStatus.ClearedCount;
            this.ClearedScenes = itemStatus.ClearedScenes;
            this.FramesOrRanges = itemStatus.FramesOrRanges;
            this.Item = itemStatus.Item;
            this.ItemLevel = itemStatus.ItemLevel;
            this.UseCount = itemStatus.UseCount;
            this.Checksum = itemStatus.Checksum;
            this.IsValid = itemStatus.IsValid;
            this.Signature = itemStatus.Signature;
            this.Size = itemStatus.Size;
            this.Version = itemStatus.Version;
        }

        public int AvailableCount { get; set; }

        public int ClearedCount { get; set; }

        public int ClearedScenes { get; set; }

        public int FramesOrRanges { get; set; }

        public ItemWithTotal Item { get; set; }

        public int ItemLevel { get; set; }

        public int UseCount { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}
