using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th165;

namespace TemplateGenerator.Models.Th165
{
    public class Definitions
    {
        public static string Title { get; } = "秘封ナイトメアダイアリー";

        public static IReadOnlyDictionary<string, (string Id, string Name)> DayNames { get; } = new[]
        {
            (Day.Sunday,             "日曜日"),
            (Day.Monday,             "月曜日"),
            (Day.Tuesday,            "火曜日"),
            (Day.Wednesday,          "水曜日"),
            (Day.Thursday,           "木曜日"),
            (Day.Friday,             "金曜日"),
            (Day.Saturday,           "土曜日"),
            (Day.WrongSunday,        "裏・日曜日"),
            (Day.WrongMonday,        "裏・月曜日"),
            (Day.WrongTuesday,       "裏・火曜日"),
            (Day.WrongWednesday,     "裏・水曜日"),
            (Day.WrongThursday,      "裏・木曜日"),
            (Day.WrongFriday,        "裏・金曜日"),
            (Day.WrongSaturday,      "裏・土曜日"),
            (Day.NightmareSunday,    "悪夢日曜"),
            (Day.NightmareMonday,    "悪夢月曜"),
            (Day.NightmareTuesday,   "悪夢火曜"),
            (Day.NightmareWednesday, "悪夢水曜"),
            (Day.NightmareThursday,  "悪夢木曜"),
            (Day.NightmareFriday,    "悪夢金曜"),
            (Day.NightmareSaturday,  "悪夢土曜"),
            (Day.NightmareDiary,     "ナイトメアダイアリー"),
        }.ToDictionary(
            pair => pair.Item1.ToShortName(),
            pair => (pair.Item1.ToString(), pair.Item2));

        public static IReadOnlyDictionary<string, int> NumDreamsPerDay { get; } = new[]
        {
            (Day.Sunday,             2),
            (Day.Monday,             4),
            (Day.Tuesday,            3),
            (Day.Wednesday,          4),
            (Day.Thursday,           3),
            (Day.Friday,             3),
            (Day.Saturday,           1),
            (Day.WrongSunday,        7),
            (Day.WrongMonday,        4),
            (Day.WrongTuesday,       4),
            (Day.WrongWednesday,     6),
            (Day.WrongThursday,      5),
            (Day.WrongFriday,        5),
            (Day.WrongSaturday,      6),
            (Day.NightmareSunday,    6),
            (Day.NightmareMonday,    6),
            (Day.NightmareTuesday,   6),
            (Day.NightmareWednesday, 6),
            (Day.NightmareThursday,  6),
            (Day.NightmareFriday,    6),
            (Day.NightmareSaturday,  6),
            (Day.NightmareDiary,     4),
        }.ToStringKeyedDictionary();

        public static int NumNicknames { get; } = 50;
    }
}
