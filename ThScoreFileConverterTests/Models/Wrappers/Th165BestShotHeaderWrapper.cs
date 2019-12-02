using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th165BestShotHeaderWrapper
    {
        private static readonly Type ParentType = typeof(Th165Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+BestShotHeader";

        private readonly PrivateObject pobj = null;

        public static Th165BestShotHeaderWrapper Create(byte[] array)
        {
            var header = new Th165BestShotHeaderWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    header.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return header;
        }

        public Th165BestShotHeaderWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Th165BestShotHeaderWrapper(object original)
            => this.pobj = new PrivateObject(original);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public Day? Weekday
            => this.pobj.GetProperty(nameof(this.Weekday)) as Day?;
        public short? Dream
            => this.pobj.GetProperty(nameof(this.Dream)) as short?;
        public short? Width
            => this.pobj.GetProperty(nameof(this.Width)) as short?;
        public short? Height
            => this.pobj.GetProperty(nameof(this.Height)) as short?;
        public short? Width2
            => this.pobj.GetProperty(nameof(this.Width2)) as short?;
        public short? Height2
            => this.pobj.GetProperty(nameof(this.Height2)) as short?;
        public short? HalfWidth
            => this.pobj.GetProperty(nameof(this.HalfWidth)) as short?;
        public short? HalfHeight
            => this.pobj.GetProperty(nameof(this.HalfHeight)) as short?;
        public float? SlowRate
            => this.pobj.GetProperty(nameof(this.SlowRate)) as float?;
        public uint? DateTime
            => this.pobj.GetProperty(nameof(this.DateTime)) as uint?;
        public float? Angle
            => this.pobj.GetProperty(nameof(this.Angle)) as float?;
        public int? Score
            => this.pobj.GetProperty(nameof(this.Score)) as int?;
        public HashtagFields? Fields
            => this.pobj.GetProperty(nameof(this.Fields)) as HashtagFields?;
        public int? Score2
            => this.pobj.GetProperty(nameof(this.Score2)) as int?;
        public int? BasePoint
            => this.pobj.GetProperty(nameof(this.BasePoint)) as int?;
        public int? NumViewed
            => this.pobj.GetProperty(nameof(this.NumViewed)) as int?;
        public int? NumLikes
            => this.pobj.GetProperty(nameof(this.NumLikes)) as int?;
        public int? NumFavs
            => this.pobj.GetProperty(nameof(this.NumFavs)) as int?;
        public int? NumBullets
            => this.pobj.GetProperty(nameof(this.NumBullets)) as int?;
        public int? NumBulletsNearby
            => this.pobj.GetProperty(nameof(this.NumBulletsNearby)) as int?;
        public int? RiskBonus
            => this.pobj.GetProperty(nameof(this.RiskBonus)) as int?;
        public float? BossShot
            => this.pobj.GetProperty(nameof(this.BossShot)) as float?;
        public float? AngleBonus
            => this.pobj.GetProperty(nameof(this.AngleBonus)) as float?;
        public int? MacroBonus
            => this.pobj.GetProperty(nameof(this.MacroBonus)) as int?;
        public float? LikesPerView
            => this.pobj.GetProperty(nameof(this.LikesPerView)) as float?;
        public float? FavsPerView
            => this.pobj.GetProperty(nameof(this.FavsPerView)) as float?;
        public int? NumHashtags
            => this.pobj.GetProperty(nameof(this.NumHashtags)) as int?;
        public int? NumRedBullets
            => this.pobj.GetProperty(nameof(this.NumRedBullets)) as int?;
        public int? NumPurpleBullets
            => this.pobj.GetProperty(nameof(this.NumPurpleBullets)) as int?;
        public int? NumBlueBullets
            => this.pobj.GetProperty(nameof(this.NumBlueBullets)) as int?;
        public int? NumCyanBullets
            => this.pobj.GetProperty(nameof(this.NumCyanBullets)) as int?;
        public int? NumGreenBullets
            => this.pobj.GetProperty(nameof(this.NumGreenBullets)) as int?;
        public int? NumYellowBullets
            => this.pobj.GetProperty(nameof(this.NumYellowBullets)) as int?;
        public int? NumOrangeBullets
            => this.pobj.GetProperty(nameof(this.NumOrangeBullets)) as int?;
        public int? NumLightBullets
            => this.pobj.GetProperty(nameof(this.NumLightBullets)) as int?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(this.ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
