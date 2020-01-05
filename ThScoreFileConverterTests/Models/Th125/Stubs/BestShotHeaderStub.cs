using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverterTests.Models.Th125.Stubs
{
    internal class BestShotHeaderStub : IBestShotHeader
    {
        public BestShotHeaderStub()
        {
            this.CardName = Enumerable.Empty<byte>();
            this.Signature = string.Empty;
        }

        public BestShotHeaderStub(IBestShotHeader header)
        {
            this.Angle = header.Angle;
            this.AngleBonus = header.AngleBonus;
            this.BasePoint = header.BasePoint;
            this.BossShot = header.BossShot;
            this.ClearShot = header.ClearShot;
            this.DateTime = header.DateTime;
            this.Fields = header.Fields;
            this.FrontSideBackShot = header.FrontSideBackShot;
            this.HalfHeight = header.HalfHeight;
            this.HalfWidth = header.HalfWidth;
            this.Height2 = header.Height2;
            this.MacroBonus = header.MacroBonus;
            this.NiceShot = header.NiceShot;
            this.ResultScore2 = header.ResultScore2;
            this.RiskBonus = header.RiskBonus;
            this.Width2 = header.Width2;
            this.CardName = header.CardName.ToArray();
            this.Height = header.Height;
            this.Level = header.Level;
            this.ResultScore = header.ResultScore;
            this.Scene = header.Scene;
            this.Signature = header.Signature;
            this.SlowRate = header.SlowRate;
            this.Width = header.Width;
        }

        public float Angle { get; set; }

        public float AngleBonus { get; set; }

        public int BasePoint { get; set; }

        public float BossShot { get; set; }

        public int ClearShot { get; set; }

        public uint DateTime { get; set; }

        public BonusFields Fields { get; set; }

        public int FrontSideBackShot { get; set; }

        public short HalfHeight { get; set; }

        public short HalfWidth { get; set; }

        public short Height2 { get; set; }

        public int MacroBonus { get; set; }

        public float NiceShot { get; set; }

        public int ResultScore2 { get; set; }

        public int RiskBonus { get; set; }

        public short Width2 { get; set; }

        public IEnumerable<byte> CardName { get; set; }

        public short Height { get; set; }

        public Level Level { get; set; }

        public int ResultScore { get; set; }

        public short Scene { get; set; }

        public string Signature { get; set; }

        public float SlowRate { get; set; }

        public short Width { get; set; }
    }
}
