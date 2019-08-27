//-----------------------------------------------------------------------
// <copyright file="Th07Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Models.Th07;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.Th07.Stage, ThScoreFileConverter.Models.Th07.Level>;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th07Converter : ThConverter
    {
        // Thanks to thwiki.info and www57.atwiki.jp/2touhoukouryaku
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo(  1, "霜符「フロストコラムス」",                   Th07.Stage.St1,      Th07.Level.Hard),
                new CardInfo(  2, "霜符「フロストコラムス -Lunatic-」",         Th07.Stage.St1,      Th07.Level.Lunatic),
                new CardInfo(  3, "寒符「リンガリングコールド -Easy-」",        Th07.Stage.St1,      Th07.Level.Easy),
                new CardInfo(  4, "寒符「リンガリングコールド」",               Th07.Stage.St1,      Th07.Level.Normal),
                new CardInfo(  5, "寒符「リンガリングコールド -Hard-」",        Th07.Stage.St1,      Th07.Level.Hard),
                new CardInfo(  6, "寒符「リンガリングコールド -Lunatic-」",     Th07.Stage.St1,      Th07.Level.Lunatic),
                new CardInfo(  7, "冬符「フラワーウィザラウェイ -Easy-」",      Th07.Stage.St1,      Th07.Level.Easy),
                new CardInfo(  8, "冬符「フラワーウィザラウェイ」",             Th07.Stage.St1,      Th07.Level.Normal),
                new CardInfo(  9, "白符「アンデュレイションレイ」",             Th07.Stage.St1,      Th07.Level.Hard),
                new CardInfo( 10, "怪符「テーブルターニング」",                 Th07.Stage.St1,      Th07.Level.Lunatic),
                new CardInfo( 11, "仙符「鳳凰卵 -Easy-」",                      Th07.Stage.St2,      Th07.Level.Easy),
                new CardInfo( 12, "仙符「鳳凰卵」",                             Th07.Stage.St2,      Th07.Level.Normal),
                new CardInfo( 13, "仙符「鳳凰展翅」",                           Th07.Stage.St2,      Th07.Level.Hard),
                new CardInfo( 14, "仙符「鳳凰展翅 -Lunatic-」",                 Th07.Stage.St2,      Th07.Level.Lunatic),
                new CardInfo( 15, "式符「飛翔晴明 -Easy-」",                    Th07.Stage.St2,      Th07.Level.Easy),
                new CardInfo( 16, "式符「飛翔晴明」",                           Th07.Stage.St2,      Th07.Level.Normal),
                new CardInfo( 17, "陰陽「道満晴明」",                           Th07.Stage.St2,      Th07.Level.Hard),
                new CardInfo( 18, "陰陽「晴明大紋」",                           Th07.Stage.St2,      Th07.Level.Lunatic),
                new CardInfo( 19, "天符「天仙鳴動 -Easy-」",                    Th07.Stage.St2,      Th07.Level.Easy),
                new CardInfo( 20, "天符「天仙鳴動」",                           Th07.Stage.St2,      Th07.Level.Normal),
                new CardInfo( 21, "翔符「飛翔韋駄天」",                         Th07.Stage.St2,      Th07.Level.Hard),
                new CardInfo( 22, "童符「護法天童乱舞」",                       Th07.Stage.St2,      Th07.Level.Lunatic),
                new CardInfo( 23, "仙符「屍解永遠 -Easy-」",                    Th07.Stage.St2,      Th07.Level.Easy),
                new CardInfo( 24, "仙符「屍解永遠」",                           Th07.Stage.St2,      Th07.Level.Normal),
                new CardInfo( 25, "鬼符「鬼門金神」",                           Th07.Stage.St2,      Th07.Level.Hard),
                new CardInfo( 26, "方符「奇門遁甲」",                           Th07.Stage.St2,      Th07.Level.Lunatic),
                new CardInfo( 27, "操符「乙女文楽」",                           Th07.Stage.St3,      Th07.Level.Hard),
                new CardInfo( 28, "操符「乙女文楽 -Lunatic-」",                 Th07.Stage.St3,      Th07.Level.Lunatic),
                new CardInfo( 29, "蒼符「博愛の仏蘭西人形 -Easy-」",            Th07.Stage.St3,      Th07.Level.Easy),
                new CardInfo( 30, "蒼符「博愛の仏蘭西人形」",                   Th07.Stage.St3,      Th07.Level.Normal),
                new CardInfo( 31, "蒼符「博愛の仏蘭西人形 -Hard-」",            Th07.Stage.St3,      Th07.Level.Hard),
                new CardInfo( 32, "蒼符「博愛のオルレアン人形」",               Th07.Stage.St3,      Th07.Level.Lunatic),
                new CardInfo( 33, "紅符「紅毛の和蘭人形 -Easy-」",              Th07.Stage.St3,      Th07.Level.Easy),
                new CardInfo( 34, "紅符「紅毛の和蘭人形」",                     Th07.Stage.St3,      Th07.Level.Normal),
                new CardInfo( 35, "白符「白亜の露西亜人形」",                   Th07.Stage.St3,      Th07.Level.Hard),
                new CardInfo( 36, "白符「白亜の露西亜人形 -Lunatic-」",         Th07.Stage.St3,      Th07.Level.Lunatic),
                new CardInfo( 37, "闇符「霧の倫敦人形 -Easy-」",                Th07.Stage.St3,      Th07.Level.Easy),
                new CardInfo( 38, "闇符「霧の倫敦人形」",                       Th07.Stage.St3,      Th07.Level.Normal),
                new CardInfo( 39, "廻符「輪廻の西蔵人形」",                     Th07.Stage.St3,      Th07.Level.Hard),
                new CardInfo( 40, "雅符「春の京人形」",                         Th07.Stage.St3,      Th07.Level.Lunatic),
                new CardInfo( 41, "咒詛「魔彩光の上海人形 -Easy-」",            Th07.Stage.St3,      Th07.Level.Easy),
                new CardInfo( 42, "咒詛「魔彩光の上海人形」",                   Th07.Stage.St3,      Th07.Level.Normal),
                new CardInfo( 43, "咒詛「魔彩光の上海人形 -Hard-」",            Th07.Stage.St3,      Th07.Level.Hard),
                new CardInfo( 44, "咒詛「首吊り蓬莱人形」",                     Th07.Stage.St3,      Th07.Level.Lunatic),
                new CardInfo( 45, "騒符「ファントムディニング -Easy-」",        Th07.Stage.St4,      Th07.Level.Easy),
                new CardInfo( 46, "騒符「ファントムディニング」",               Th07.Stage.St4,      Th07.Level.Normal),
                new CardInfo( 47, "騒符「ライブポルターガイスト」",             Th07.Stage.St4,      Th07.Level.Hard),
                new CardInfo( 48, "騒符「ライブポルターガイスト -Lunatic-」",   Th07.Stage.St4,      Th07.Level.Lunatic),
                new CardInfo( 49, "弦奏「グァルネリ・デル・ジェス -Easy-」",    Th07.Stage.St4,      Th07.Level.Easy),
                new CardInfo( 50, "弦奏「グァルネリ・デル・ジェス」",           Th07.Stage.St4,      Th07.Level.Normal),
                new CardInfo( 51, "神弦「ストラディヴァリウス」",               Th07.Stage.St4,      Th07.Level.Hard),
                new CardInfo( 52, "偽弦「スードストラディヴァリウス」",         Th07.Stage.St4,      Th07.Level.Lunatic),
                new CardInfo( 53, "管霊「ヒノファンタズム -Easy-」",            Th07.Stage.St4,      Th07.Level.Easy),
                new CardInfo( 54, "管霊「ヒノファンタズム」",                   Th07.Stage.St4,      Th07.Level.Normal),
                new CardInfo( 55, "冥管「ゴーストクリフォード」",               Th07.Stage.St4,      Th07.Level.Hard),
                new CardInfo( 56, "管霊「ゴーストクリフォード -Lunatic-」",     Th07.Stage.St4,      Th07.Level.Lunatic),
                new CardInfo( 57, "冥鍵「ファツィオーリ冥奏 -Easy-」",          Th07.Stage.St4,      Th07.Level.Easy),
                new CardInfo( 58, "冥鍵「ファツィオーリ冥奏」",                 Th07.Stage.St4,      Th07.Level.Normal),
                new CardInfo( 59, "鍵霊「ベーゼンドルファー神奏」",             Th07.Stage.St4,      Th07.Level.Hard),
                new CardInfo( 60, "鍵霊「ベーゼンドルファー神奏 -Lunatic-」",   Th07.Stage.St4,      Th07.Level.Lunatic),
                new CardInfo( 61, "合葬「プリズムコンチェルト -Easy-」",        Th07.Stage.St4,      Th07.Level.Easy),
                new CardInfo( 62, "合葬「プリズムコンチェルト」",               Th07.Stage.St4,      Th07.Level.Normal),
                new CardInfo( 63, "騒葬「スティジャンリバーサイド」",           Th07.Stage.St4,      Th07.Level.Hard),
                new CardInfo( 64, "騒葬「スティジャンリバーサイド -Lunatic-」", Th07.Stage.St4,      Th07.Level.Lunatic),
                new CardInfo( 65, "大合葬「霊車コンチェルトグロッソ -Easy-」",  Th07.Stage.St4,      Th07.Level.Easy),
                new CardInfo( 66, "大合葬「霊車コンチェルトグロッソ」",         Th07.Stage.St4,      Th07.Level.Normal),
                new CardInfo( 67, "大合葬「霊車コンチェルトグロッソ改」",       Th07.Stage.St4,      Th07.Level.Hard),
                new CardInfo( 68, "大合葬「霊車コンチェルトグロッソ怪」",       Th07.Stage.St4,      Th07.Level.Lunatic),
                new CardInfo( 69, "幽鬼剣「妖童餓鬼の断食 -Easy-」",            Th07.Stage.St5,      Th07.Level.Easy),
                new CardInfo( 70, "幽鬼剣「妖童餓鬼の断食」",                   Th07.Stage.St5,      Th07.Level.Normal),
                new CardInfo( 71, "餓鬼剣「餓鬼道草紙」",                       Th07.Stage.St5,      Th07.Level.Hard),
                new CardInfo( 72, "餓王剣「餓鬼十王の報い」",                   Th07.Stage.St5,      Th07.Level.Lunatic),
                new CardInfo( 73, "獄界剣「二百由旬の一閃 -Easy-」",            Th07.Stage.St5,      Th07.Level.Easy),
                new CardInfo( 74, "獄界剣「二百由旬の一閃」",                   Th07.Stage.St5,      Th07.Level.Normal),
                new CardInfo( 75, "獄炎剣「業風閃影陣」",                       Th07.Stage.St5,      Th07.Level.Hard),
                new CardInfo( 76, "獄神剣「業風神閃斬」",                       Th07.Stage.St5,      Th07.Level.Lunatic),
                new CardInfo( 77, "畜趣剣「無為無策の冥罰 -Easy-」",            Th07.Stage.St5,      Th07.Level.Easy),
                new CardInfo( 78, "畜趣剣「無為無策の冥罰」",                   Th07.Stage.St5,      Th07.Level.Normal),
                new CardInfo( 79, "修羅剣「現世妄執」",                         Th07.Stage.St5,      Th07.Level.Hard),
                new CardInfo( 80, "修羅剣「現世妄執 -Lunatic-」",               Th07.Stage.St5,      Th07.Level.Lunatic),
                new CardInfo( 81, "人界剣「悟入幻想 -Easy-」",                  Th07.Stage.St5,      Th07.Level.Easy),
                new CardInfo( 82, "人界剣「悟入幻想」",                         Th07.Stage.St5,      Th07.Level.Normal),
                new CardInfo( 83, "人世剣「大悟顕晦」",                         Th07.Stage.St5,      Th07.Level.Hard),
                new CardInfo( 84, "人神剣「俗諦常住」",                         Th07.Stage.St5,      Th07.Level.Lunatic),
                new CardInfo( 85, "天上剣「天人の五衰 -Easy-」",                Th07.Stage.St5,      Th07.Level.Easy),
                new CardInfo( 86, "天上剣「天人の五衰」",                       Th07.Stage.St5,      Th07.Level.Normal),
                new CardInfo( 87, "天界剣「七魄忌諱」",                         Th07.Stage.St5,      Th07.Level.Hard),
                new CardInfo( 88, "天神剣「三魂七魄」",                         Th07.Stage.St5,      Th07.Level.Lunatic),
                new CardInfo( 89, "六道剣「一念無量劫 -Easy-」",                Th07.Stage.St6,      Th07.Level.Easy),
                new CardInfo( 90, "六道剣「一念無量劫」",                       Th07.Stage.St6,      Th07.Level.Normal),
                new CardInfo( 91, "六道剣「一念無量劫 -Hard-」",                Th07.Stage.St6,      Th07.Level.Hard),
                new CardInfo( 92, "六道剣「一念無量劫 -Lunatic-」",             Th07.Stage.St6,      Th07.Level.Lunatic),
                new CardInfo( 93, "亡郷「亡我郷 -さまよえる魂-」",              Th07.Stage.St6,      Th07.Level.Easy),
                new CardInfo( 94, "亡郷「亡我郷 -宿罪-」",                      Th07.Stage.St6,      Th07.Level.Normal),
                new CardInfo( 95, "亡郷「亡我郷 -道無き道-」",                  Th07.Stage.St6,      Th07.Level.Hard),
                new CardInfo( 96, "亡郷「亡我郷 -自尽-」",                      Th07.Stage.St6,      Th07.Level.Lunatic),
                new CardInfo( 97, "亡舞「生者必滅の理 -眩惑-」",                Th07.Stage.St6,      Th07.Level.Easy),
                new CardInfo( 98, "亡舞「生者必滅の理 -死蝶-」",                Th07.Stage.St6,      Th07.Level.Normal),
                new CardInfo( 99, "亡舞「生者必滅の理 -毒蛾-」",                Th07.Stage.St6,      Th07.Level.Hard),
                new CardInfo(100, "亡舞「生者必滅の理 -魔境-」",                Th07.Stage.St6,      Th07.Level.Lunatic),
                new CardInfo(101, "華霊「ゴーストバタフライ」",                 Th07.Stage.St6,      Th07.Level.Easy),
                new CardInfo(102, "華霊「スワローテイルバタフライ」",           Th07.Stage.St6,      Th07.Level.Normal),
                new CardInfo(103, "華霊「ディープルーティドバタフライ」",       Th07.Stage.St6,      Th07.Level.Hard),
                new CardInfo(104, "華霊「バタフライディルージョン」",           Th07.Stage.St6,      Th07.Level.Lunatic),
                new CardInfo(105, "幽曲「リポジトリ・オブ・ヒロカワ -偽霊-」",  Th07.Stage.St6,      Th07.Level.Easy),
                new CardInfo(106, "幽曲「リポジトリ・オブ・ヒロカワ -亡霊-」",  Th07.Stage.St6,      Th07.Level.Normal),
                new CardInfo(107, "幽曲「リポジトリ・オブ・ヒロカワ -幻霊-」",  Th07.Stage.St6,      Th07.Level.Hard),
                new CardInfo(108, "幽曲「リポジトリ・オブ・ヒロカワ -神霊-」",  Th07.Stage.St6,      Th07.Level.Lunatic),
                new CardInfo(109, "桜符「完全なる墨染の桜 -封印-」",            Th07.Stage.St6,      Th07.Level.Easy),
                new CardInfo(110, "桜符「完全なる墨染の桜 -亡我-」",            Th07.Stage.St6,      Th07.Level.Normal),
                new CardInfo(111, "桜符「完全なる墨染の桜 -春眠-」",            Th07.Stage.St6,      Th07.Level.Hard),
                new CardInfo(112, "桜符「完全なる墨染の桜 -開花-」",            Th07.Stage.St6,      Th07.Level.Lunatic),
                new CardInfo(113, "「反魂蝶 -一分咲-」",                        Th07.Stage.St6,      Th07.Level.Easy),
                new CardInfo(114, "「反魂蝶 -参分咲-」",                        Th07.Stage.St6,      Th07.Level.Normal),
                new CardInfo(115, "「反魂蝶 -伍分咲-」",                        Th07.Stage.St6,      Th07.Level.Hard),
                new CardInfo(116, "「反魂蝶 -八分咲-」",                        Th07.Stage.St6,      Th07.Level.Lunatic),
                new CardInfo(117, "鬼符「青鬼赤鬼」",                           Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(118, "鬼神「飛翔毘沙門天」",                       Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(119, "式神「仙狐思念」",                           Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(120, "式神「十二神将の宴」",                       Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(121, "式輝「狐狸妖怪レーザー」",                   Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(122, "式輝「四面楚歌チャーミング」",               Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(123, "式輝「プリンセス天狐 -Illusion-」",          Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(124, "式弾「アルティメットブディスト」",           Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(125, "式弾「ユーニラタルコンタクト」",             Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(126, "式神「橙」",                                 Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(127, "「狐狗狸さんの契約」",                       Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(128, "幻神「飯綱権現降臨」",                       Th07.Stage.Extra,    Th07.Level.Extra),
                new CardInfo(129, "式神「前鬼後鬼の守護」",                     Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(130, "式神「憑依荼吉尼天」",                       Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(131, "結界「夢と現の呪」",                         Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(132, "結界「動と静の均衡」",                       Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(133, "結界「光と闇の網目」",                       Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(134, "罔両「ストレートとカーブの夢郷」",           Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(135, "罔両「八雲紫の神隠し」",                     Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(136, "罔両「禅寺に棲む妖蝶」",                     Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(137, "魍魎「二重黒死蝶」",                         Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(138, "式神「八雲藍」",                             Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(139, "「人間と妖怪の境界」",                       Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(140, "結界「生と死の境界」",                       Th07.Stage.Phantasm, Th07.Level.Phantasm),
                new CardInfo(141, "紫奥義「弾幕結界」",                         Th07.Stage.Phantasm, Th07.Level.Phantasm),
            }.ToDictionary(card => card.Id);

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly List<HighScore> InitialRanking =
            new List<HighScore>()
            {
                new HighScore(100000),
                new HighScore( 90000),
                new HighScore( 80000),
                new HighScore( 70000),
                new HighScore( 60000),
                new HighScore( 50000),
                new HighScore( 40000),
                new HighScore( 30000),
                new HighScore( 20000),
                new HighScore( 10000),
            };

        private static new readonly EnumShortNameParser<Th07.Level> LevelParser =
            new EnumShortNameParser<Th07.Level>();

        private static new readonly EnumShortNameParser<Th07.LevelWithTotal> LevelWithTotalParser =
            new EnumShortNameParser<Th07.LevelWithTotal>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly EnumShortNameParser<CharaWithTotal> CharaWithTotalParser =
            new EnumShortNameParser<CharaWithTotal>();

        private static new readonly EnumShortNameParser<Th07.Stage> StageParser =
            new EnumShortNameParser<Th07.Stage>();

        private static new readonly EnumShortNameParser<Th07.StageWithTotal> StageWithTotalParser =
            new EnumShortNameParser<Th07.StageWithTotal>();

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00b"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th07decoded.dat", FileMode.Create, FileAccess.ReadWrite))
#else
            using (var decoded = new MemoryStream())
#endif
            {
                if (!Decrypt(input, decrypted))
                    return false;

                decrypted.Seek(0, SeekOrigin.Begin);
                if (!Extract(decrypted, decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                if (!Validate(decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                this.allScoreData = Read(decoded);

                return this.allScoreData != null;
            }
        }

        protected override IEnumerable<IStringReplaceable> CreateReplacers(
            bool hideUntriedCards, string outputFilePath)
        {
            return new List<IStringReplaceable>
            {
                new ScoreReplacer(this),
                new CareerReplacer(this),
                new CardReplacer(this, hideUntriedCards),
                new CollectRateReplacer(this),
                new ClearReplacer(this),
                new PlayReplacer(this),
                new TimeReplacer(this),
                new PracticeReplacer(this),
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var size = (int)input.Length;
            var data = new byte[size];
            input.Read(data, 0, size);

            uint checksum = 0;
            byte temp = 0;
            for (var index = 2; index < size; index++)
            {
                temp += data[index - 1];
                temp = (byte)((temp >> 5) | (temp << 3));
                data[index] ^= temp;
                if (index > 3)
                    checksum += data[index];
            }

            output.Write(data, 0, size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        private static bool Extract(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new FileHeader();

                header.ReadFrom(reader);
                if (!header.IsValid)
                    return false;
                if (header.Size + header.EncodedBodySize != input.Length)
                    return false;

                header.WriteTo(writer);

                Lzss.Extract(input, output);
                output.Flush();
                output.SetLength(output.Position);

                return output.Position == header.DecodedAllSize;
            }
        }

        private static bool Validate(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var header = new FileHeader();
                var chapter = new Th06.Chapter();

                header.ReadFrom(reader);
                var remainSize = header.DecodedAllSize - header.Size;
                if (remainSize <= 0)
                    return false;

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (chapter.Size1 == 0)
                            return false;

                        switch (chapter.Signature)
                        {
                            case Header.ValidSignature:
                                if (chapter.FirstByteOfData != 0x01)
                                    return false;
                                break;
                            case VersionInfo.ValidSignature:
                                if (chapter.FirstByteOfData != 0x01)
                                    return false;
                                //// th07.exe does something more things here...
                                break;
                            default:
                                break;
                        }

                        remainSize -= chapter.Size1;
                    }
                }
                catch (EndOfStreamException)
                {
                    // It's OK, do nothing.
                }

                return remainSize == 0;
            }
        }

        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Th06.Chapter>>
            {
                { Header.ValidSignature,        (data, ch) => data.Set(new Header(ch))        },
                { HighScore.ValidSignature,     (data, ch) => data.Set(new HighScore(ch))     },
                { ClearData.ValidSignature,     (data, ch) => data.Set(new ClearData(ch))     },
                { CardAttack.ValidSignature,    (data, ch) => data.Set(new CardAttack(ch))    },
                { PracticeScore.ValidSignature, (data, ch) => data.Set(new PracticeScore(ch)) },
                { PlayStatus.ValidSignature,    (data, ch) => data.Set(new PlayStatus(ch))    },
                { LastName.ValidSignature,      (data, ch) => data.Set(new LastName(ch))      },
                { VersionInfo.ValidSignature,   (data, ch) => data.Set(new VersionInfo(ch))   },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Th06.Chapter();

                reader.ReadExactBytes(FileHeader.ValidSize);

                try
                {
                    while (true)
                    {
                        chapter.ReadFrom(reader);
                        if (dictionary.TryGetValue(chapter.Signature, out var setChapter))
                            setChapter(allScoreData, chapter);
                    }
                }
                catch (EndOfStreamException)
                {
                    // It's OK, do nothing.
                }

                if ((allScoreData.Header != null) &&
                    //// (allScoreData.rankings.Count >= 0) &&
                    (allScoreData.ClearData.Count == Enum.GetValues(typeof(Chara)).Length) &&
                    //// (allScoreData.cardAttacks.Length == NumCards) &&
                    //// (allScoreData.practiceScores.Count >= 0) &&
                    (allScoreData.PlayStatus != null) &&
                    (allScoreData.LastName != null) &&
                    (allScoreData.VersionInfo != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T07SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07SCR({0})({1})(\d)([1-5])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    var key = (chara, level);
                    var score = parent.allScoreData.Rankings.ContainsKey(key)
                        ? parent.allScoreData.Rankings[key][rank] : InitialRanking[rank];

                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(score.Name).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((score.Score * 10) + score.ContinueCount);
                        case 3:     // stage
                            return score.StageProgress.ToShortName();
                        case 4:     // date
                            return Encoding.Default.GetString(score.Date).TrimEnd('\0');
                        case 5:     // slow rate
                            return Utils.Format("{0:F3}%", score.SlowRate);
                        default:    // unreachable
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T07C[xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07C(\d{{3}})({0})([1-3])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<CardAttack, long> getValue;
                    if (type == 1)
                        getValue = (attack => attack.MaxBonuses[chara]);
                    else if (type == 2)
                        getValue = (attack => attack.ClearCounts[chara]);
                    else
                        getValue = (attack => attack.TrialCounts[chara]);

                    if (number == 0)
                    {
                        return Utils.ToNumberString(parent.allScoreData.CardAttacks.Values.Sum(getValue));
                    }
                    else if (CardTable.ContainsKey(number))
                    {
                        if (parent.allScoreData.CardAttacks.TryGetValue(number, out var attack))
                            return Utils.ToNumberString(getValue(attack));
                        else
                            return "0";
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T07CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T07CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th07Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[2].Value.ToUpperInvariant();

                    if (CardTable.ContainsKey(number))
                    {
                        if (hideUntriedCards)
                        {
                            if (!parent.allScoreData.CardAttacks.TryGetValue(number, out var attack) ||
                                !attack.HasTried())
                                return (type == "N") ? "??????????" : "?????";
                        }

                        return (type == "N") ? CardTable[number].Name : CardTable[number].Level.ToString();
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T07CRG[w][xx][yy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07CRG({0})({1})({2})([12])",
                LevelWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern,
                StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var stage = StageWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if ((stage == Th07.StageWithTotal.Extra) || (stage == Th07.StageWithTotal.Phantasm))
                        return match.ToString();

                    Func<CardAttack, bool> findByStage;
                    if (stage == Th07.StageWithTotal.Total)
                        findByStage = (attack => true);
                    else
                        findByStage = (attack => CardTable[attack.CardId].Stage == (Th07.Stage)stage);

                    Func<CardAttack, bool> findByLevel = (attack => true);
                    switch (level)
                    {
                        case Th07.LevelWithTotal.Total:
                            // Do nothing
                            break;
                        case Th07.LevelWithTotal.Extra:
                            findByStage = (attack => CardTable[attack.CardId].Stage == Th07.Stage.Extra);
                            break;
                        case Th07.LevelWithTotal.Phantasm:
                            findByStage = (attack => CardTable[attack.CardId].Stage == Th07.Stage.Phantasm);
                            break;
                        default:
                            findByLevel = (attack => CardTable[attack.CardId].Level == (Th07.Level)level);
                            break;
                    }

                    Func<CardAttack, bool> findByType;
                    if (type == 1)
                        findByType = (attack => attack.ClearCounts[chara] > 0);
                    else
                        findByType = (attack => attack.TrialCounts[chara] > 0);

                    return parent.allScoreData.CardAttacks.Values
                        .Count(Utils.MakeAndPredicate(findByLevel, findByStage, findByType))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T07CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);

                    var key = (chara, level);
                    if (parent.allScoreData.Rankings.ContainsKey(key))
                    {
                        var stageProgress = parent.allScoreData
                            .Rankings[key].Max(rank => rank.StageProgress);
                        if ((stageProgress == StageProgress.Extra) ||
                            (stageProgress == StageProgress.Phantasm))
                            return "Not Clear";
                        else
                            return stageProgress.ToShortName();
                    }
                    else
                    {
                        return StageProgress.None.ToShortName();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T07PLAY[x][yy]
        private class PlayReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07PLAY({0})({1}|CL|CN|PR|RT)",
                LevelWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PlayReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var charaAndMore = match.Groups[2].Value.ToUpperInvariant();

                    var playCount = parent.allScoreData.PlayStatus.PlayCounts[level];
                    switch (charaAndMore)
                    {
                        case "CL":  // clear count
                            return Utils.ToNumberString(playCount.TotalClear);
                        case "CN":  // continue count
                            return Utils.ToNumberString(playCount.TotalContinue);
                        case "PR":  // practice count
                            return Utils.ToNumberString(playCount.TotalPractice);
                        case "RT":  // retry count
                            return Utils.ToNumberString(playCount.TotalRetry);
                        default:
                            {
                                var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                                return Utils.ToNumberString((chara == CharaWithTotal.Total)
                                    ? playCount.TotalTrial : playCount.Trials[(Chara)chara]);
                            }
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T07TIME(ALL|PLY)
        private class TimeReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T07TIME(ALL|PLY)";

            private readonly MatchEvaluator evaluator;

            public TimeReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();

                    return (kind == "ALL")
                        ? parent.allScoreData.PlayStatus.TotalRunningTime.ToLongString()
                        : parent.allScoreData.PlayStatus.TotalPlayTime.ToLongString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T07PRAC[w][xx][y][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07PRAC({0})({1})({2})([12])",
                LevelParser.Pattern,
                CharaParser.Pattern,
                StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var stage = StageParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if ((level == Th07.Level.Extra) || (level == Th07.Level.Phantasm))
                        return match.ToString();
                    if ((stage == Th07.Stage.Extra) || (stage == Th07.Stage.Phantasm))
                        return match.ToString();

                    var key = (chara, level);
                    if (parent.allScoreData.PracticeScores.ContainsKey(key))
                    {
                        var scores = parent.allScoreData.PracticeScores[key];
                        if (type == 1)
                        {
                            return scores.ContainsKey(stage)
                                ? Utils.ToNumberString(scores[stage].HighScore * 10) : "0";
                        }
                        else
                        {
                            return scores.ContainsKey(stage)
                                ? Utils.ToNumberString(scores[stage].TrialCount) : "0";
                        }
                    }
                    else
                    {
                        return "0";
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class AllScoreData
        {
            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                var numPairs = numCharas * Enum.GetValues(typeof(Th07.Level)).Length;
                this.Rankings = new Dictionary<(Chara, Th07.Level), List<HighScore>>(numPairs);
                this.ClearData = new Dictionary<Chara, ClearData>(numCharas);
                this.CardAttacks = new Dictionary<int, CardAttack>(CardTable.Count);
                this.PracticeScores = new Dictionary<(Chara, Th07.Level), Dictionary<Th07.Stage, PracticeScore>>(numPairs);
            }

            public Header Header { get; private set; }

            public Dictionary<(Chara, Th07.Level), List<HighScore>> Rankings { get; private set; }

            public Dictionary<Chara, ClearData> ClearData { get; private set; }

            public Dictionary<int, CardAttack> CardAttacks { get; private set; }

            public Dictionary<(Chara, Th07.Level), Dictionary<Th07.Stage, PracticeScore>> PracticeScores { get; private set; }

            public PlayStatus PlayStatus { get; private set; }

            public LastName LastName { get; private set; }

            public VersionInfo VersionInfo { get; private set; }

            public void Set(Header header) => this.Header = header;

            public void Set(HighScore score)
            {
                var key = (score.Chara, score.Level);
                if (!this.Rankings.ContainsKey(key))
                    this.Rankings.Add(key, new List<HighScore>(InitialRanking));
                var ranking = this.Rankings[key];
                ranking.Add(score);
                ranking.Sort((lhs, rhs) => rhs.Score.CompareTo(lhs.Score));
                ranking.RemoveAt(ranking.Count - 1);
            }

            public void Set(ClearData data)
            {
                if (!this.ClearData.ContainsKey(data.Chara))
                    this.ClearData.Add(data.Chara, data);
            }

            public void Set(CardAttack attack)
            {
                if (!this.CardAttacks.ContainsKey(attack.CardId))
                    this.CardAttacks.Add(attack.CardId, attack);
            }

            public void Set(PracticeScore score)
            {
                if ((score.Level != Th07.Level.Extra) && (score.Level != Th07.Level.Phantasm) &&
                    (score.Stage != Th07.Stage.Extra) && (score.Stage != Th07.Stage.Phantasm))
                {
                    var key = (score.Chara, score.Level);
                    if (!this.PracticeScores.ContainsKey(key))
                    {
                        var numStages = Utils.GetEnumerator<Th07.Stage>()
                            .Where(st => (st != Th07.Stage.Extra) && (st != Th07.Stage.Phantasm)).Count();
                        this.PracticeScores.Add(key, new Dictionary<Th07.Stage, PracticeScore>(numStages));
                    }

                    var scores = this.PracticeScores[key];
                    if (!scores.ContainsKey(score.Stage))
                        scores.Add(score.Stage, score);
                }
            }

            public void Set(PlayStatus status) => this.PlayStatus = status;

            public void Set(LastName name) => this.LastName = name;

            public void Set(VersionInfo info) => this.VersionInfo = info;
        }
    }
}
