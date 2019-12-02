using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverterTests.Models.Th165.Stubs
{
    internal class BestShotHeaderStub : IBestShotHeader
    {
        public BestShotHeaderStub() { }

        public BestShotHeaderStub(IBestShotHeader header)
            : base()
        {
            this.Angle = header.Angle;
            this.AngleBonus = header.AngleBonus;
            this.BasePoint = header.BasePoint;
            this.BossShot = header.BossShot;
            this.DateTime = header.DateTime;
            this.Dream = header.Dream;
            this.FavsPerView = header.FavsPerView;
            this.Fields = header.Fields;
            this.HalfHeight = header.HalfHeight;
            this.HalfWidth = header.HalfWidth;
            this.Height = header.Height;
            this.Height2 = header.Height2;
            this.LikesPerView = header.LikesPerView;
            this.MacroBonus = header.MacroBonus;
            this.NumBlueBullets = header.NumBlueBullets;
            this.NumBullets = header.NumBullets;
            this.NumBulletsNearby = header.NumBulletsNearby;
            this.NumCyanBullets = header.NumCyanBullets;
            this.NumFavs = header.NumFavs;
            this.NumGreenBullets = header.NumGreenBullets;
            this.NumHashtags = header.NumHashtags;
            this.NumLightBullets = header.NumLightBullets;
            this.NumLikes = header.NumLikes;
            this.NumOrangeBullets = header.NumOrangeBullets;
            this.NumPurpleBullets = header.NumPurpleBullets;
            this.NumRedBullets = header.NumRedBullets;
            this.NumViewed = header.NumViewed;
            this.NumYellowBullets = header.NumYellowBullets;
            this.RiskBonus = header.RiskBonus;
            this.Score = header.Score;
            this.Score2 = header.Score2;
            this.Signature = header.Signature;
            this.SlowRate = header.SlowRate;
            this.Weekday = header.Weekday;
            this.Width = header.Width;
            this.Width2 = header.Width2;
        }

        public float Angle { get; set; }

        public float AngleBonus { get; set; }

        public int BasePoint { get; set; }

        public float BossShot { get; set; }

        public uint DateTime { get; set; }

        public short Dream { get; set; }

        public float FavsPerView { get; set; }

        public HashtagFields Fields { get; set; }

        public short HalfHeight { get; set; }

        public short HalfWidth { get; set; }

        public short Height { get; set; }

        public short Height2 { get; set; }

        public float LikesPerView { get; set; }

        public int MacroBonus { get; set; }

        public int NumBlueBullets { get; set; }

        public int NumBullets { get; set; }

        public int NumBulletsNearby { get; set; }

        public int NumCyanBullets { get; set; }

        public int NumFavs { get; set; }

        public int NumGreenBullets { get; set; }

        public int NumHashtags { get; set; }

        public int NumLightBullets { get; set; }

        public int NumLikes { get; set; }

        public int NumOrangeBullets { get; set; }

        public int NumPurpleBullets { get; set; }

        public int NumRedBullets { get; set; }

        public int NumViewed { get; set; }

        public int NumYellowBullets { get; set; }

        public int RiskBonus { get; set; }

        public int Score { get; set; }

        public int Score2 { get; set; }

        public string Signature { get; set; }

        public float SlowRate { get; set; }

        public Day Weekday { get; set; }

        public short Width { get; set; }

        public short Width2 { get; set; }
    }
}
