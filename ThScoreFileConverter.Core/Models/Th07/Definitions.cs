﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Helpers;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Th07.Stage, ThScoreFileConverter.Core.Models.Th07.Level>;

namespace ThScoreFileConverter.Core.Models.Th07;

/// <summary>
/// Provides several PCB specific definitions.
/// </summary>
public static class Definitions
{
    /// <summary>
    /// Gets the dictionary of PCB spell cards.
    /// </summary>
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new(  1, "霜符「フロストコラムス」",                   Stage.One,      Level.Hard),
        new(  2, "霜符「フロストコラムス -Lunatic-」",         Stage.One,      Level.Lunatic),
        new(  3, "寒符「リンガリングコールド -Easy-」",        Stage.One,      Level.Easy),
        new(  4, "寒符「リンガリングコールド」",               Stage.One,      Level.Normal),
        new(  5, "寒符「リンガリングコールド -Hard-」",        Stage.One,      Level.Hard),
        new(  6, "寒符「リンガリングコールド -Lunatic-」",     Stage.One,      Level.Lunatic),
        new(  7, "冬符「フラワーウィザラウェイ -Easy-」",      Stage.One,      Level.Easy),
        new(  8, "冬符「フラワーウィザラウェイ」",             Stage.One,      Level.Normal),
        new(  9, "白符「アンデュレイションレイ」",             Stage.One,      Level.Hard),
        new( 10, "怪符「テーブルターニング」",                 Stage.One,      Level.Lunatic),
        new( 11, "仙符「鳳凰卵 -Easy-」",                      Stage.Two,      Level.Easy),
        new( 12, "仙符「鳳凰卵」",                             Stage.Two,      Level.Normal),
        new( 13, "仙符「鳳凰展翅」",                           Stage.Two,      Level.Hard),
        new( 14, "仙符「鳳凰展翅 -Lunatic-」",                 Stage.Two,      Level.Lunatic),
        new( 15, "式符「飛翔晴明 -Easy-」",                    Stage.Two,      Level.Easy),
        new( 16, "式符「飛翔晴明」",                           Stage.Two,      Level.Normal),
        new( 17, "陰陽「道満晴明」",                           Stage.Two,      Level.Hard),
        new( 18, "陰陽「晴明大紋」",                           Stage.Two,      Level.Lunatic),
        new( 19, "天符「天仙鳴動 -Easy-」",                    Stage.Two,      Level.Easy),
        new( 20, "天符「天仙鳴動」",                           Stage.Two,      Level.Normal),
        new( 21, "翔符「飛翔韋駄天」",                         Stage.Two,      Level.Hard),
        new( 22, "童符「護法天童乱舞」",                       Stage.Two,      Level.Lunatic),
        new( 23, "仙符「屍解永遠 -Easy-」",                    Stage.Two,      Level.Easy),
        new( 24, "仙符「屍解永遠」",                           Stage.Two,      Level.Normal),
        new( 25, "鬼符「鬼門金神」",                           Stage.Two,      Level.Hard),
        new( 26, "方符「奇門遁甲」",                           Stage.Two,      Level.Lunatic),
        new( 27, "操符「乙女文楽」",                           Stage.Three,    Level.Hard),
        new( 28, "操符「乙女文楽 -Lunatic-」",                 Stage.Three,    Level.Lunatic),
        new( 29, "蒼符「博愛の仏蘭西人形 -Easy-」",            Stage.Three,    Level.Easy),
        new( 30, "蒼符「博愛の仏蘭西人形」",                   Stage.Three,    Level.Normal),
        new( 31, "蒼符「博愛の仏蘭西人形 -Hard-」",            Stage.Three,    Level.Hard),
        new( 32, "蒼符「博愛のオルレアン人形」",               Stage.Three,    Level.Lunatic),
        new( 33, "紅符「紅毛の和蘭人形 -Easy-」",              Stage.Three,    Level.Easy),
        new( 34, "紅符「紅毛の和蘭人形」",                     Stage.Three,    Level.Normal),
        new( 35, "白符「白亜の露西亜人形」",                   Stage.Three,    Level.Hard),
        new( 36, "白符「白亜の露西亜人形 -Lunatic-」",         Stage.Three,    Level.Lunatic),
        new( 37, "闇符「霧の倫敦人形 -Easy-」",                Stage.Three,    Level.Easy),
        new( 38, "闇符「霧の倫敦人形」",                       Stage.Three,    Level.Normal),
        new( 39, "廻符「輪廻の西蔵人形」",                     Stage.Three,    Level.Hard),
        new( 40, "雅符「春の京人形」",                         Stage.Three,    Level.Lunatic),
        new( 41, "咒詛「魔彩光の上海人形 -Easy-」",            Stage.Three,    Level.Easy),
        new( 42, "咒詛「魔彩光の上海人形」",                   Stage.Three,    Level.Normal),
        new( 43, "咒詛「魔彩光の上海人形 -Hard-」",            Stage.Three,    Level.Hard),
        new( 44, "咒詛「首吊り蓬莱人形」",                     Stage.Three,    Level.Lunatic),
        new( 45, "騒符「ファントムディニング -Easy-」",        Stage.Four,     Level.Easy),
        new( 46, "騒符「ファントムディニング」",               Stage.Four,     Level.Normal),
        new( 47, "騒符「ライブポルターガイスト」",             Stage.Four,     Level.Hard),
        new( 48, "騒符「ライブポルターガイスト -Lunatic-」",   Stage.Four,     Level.Lunatic),
        new( 49, "弦奏「グァルネリ・デル・ジェス -Easy-」",    Stage.Four,     Level.Easy),
        new( 50, "弦奏「グァルネリ・デル・ジェス」",           Stage.Four,     Level.Normal),
        new( 51, "神弦「ストラディヴァリウス」",               Stage.Four,     Level.Hard),
        new( 52, "偽弦「スードストラディヴァリウス」",         Stage.Four,     Level.Lunatic),
        new( 53, "管霊「ヒノファンタズム -Easy-」",            Stage.Four,     Level.Easy),
        new( 54, "管霊「ヒノファンタズム」",                   Stage.Four,     Level.Normal),
        new( 55, "冥管「ゴーストクリフォード」",               Stage.Four,     Level.Hard),
        new( 56, "管霊「ゴーストクリフォード -Lunatic-」",     Stage.Four,     Level.Lunatic),
        new( 57, "冥鍵「ファツィオーリ冥奏 -Easy-」",          Stage.Four,     Level.Easy),
        new( 58, "冥鍵「ファツィオーリ冥奏」",                 Stage.Four,     Level.Normal),
        new( 59, "鍵霊「ベーゼンドルファー神奏」",             Stage.Four,     Level.Hard),
        new( 60, "鍵霊「ベーゼンドルファー神奏 -Lunatic-」",   Stage.Four,     Level.Lunatic),
        new( 61, "合葬「プリズムコンチェルト -Easy-」",        Stage.Four,     Level.Easy),
        new( 62, "合葬「プリズムコンチェルト」",               Stage.Four,     Level.Normal),
        new( 63, "騒葬「スティジャンリバーサイド」",           Stage.Four,     Level.Hard),
        new( 64, "騒葬「スティジャンリバーサイド -Lunatic-」", Stage.Four,     Level.Lunatic),
        new( 65, "大合葬「霊車コンチェルトグロッソ -Easy-」",  Stage.Four,     Level.Easy),
        new( 66, "大合葬「霊車コンチェルトグロッソ」",         Stage.Four,     Level.Normal),
        new( 67, "大合葬「霊車コンチェルトグロッソ改」",       Stage.Four,     Level.Hard),
        new( 68, "大合葬「霊車コンチェルトグロッソ怪」",       Stage.Four,     Level.Lunatic),
        new( 69, "幽鬼剣「妖童餓鬼の断食 -Easy-」",            Stage.Five,     Level.Easy),
        new( 70, "幽鬼剣「妖童餓鬼の断食」",                   Stage.Five,     Level.Normal),
        new( 71, "餓鬼剣「餓鬼道草紙」",                       Stage.Five,     Level.Hard),
        new( 72, "餓王剣「餓鬼十王の報い」",                   Stage.Five,     Level.Lunatic),
        new( 73, "獄界剣「二百由旬の一閃 -Easy-」",            Stage.Five,     Level.Easy),
        new( 74, "獄界剣「二百由旬の一閃」",                   Stage.Five,     Level.Normal),
        new( 75, "獄炎剣「業風閃影陣」",                       Stage.Five,     Level.Hard),
        new( 76, "獄神剣「業風神閃斬」",                       Stage.Five,     Level.Lunatic),
        new( 77, "畜趣剣「無為無策の冥罰 -Easy-」",            Stage.Five,     Level.Easy),
        new( 78, "畜趣剣「無為無策の冥罰」",                   Stage.Five,     Level.Normal),
        new( 79, "修羅剣「現世妄執」",                         Stage.Five,     Level.Hard),
        new( 80, "修羅剣「現世妄執 -Lunatic-」",               Stage.Five,     Level.Lunatic),
        new( 81, "人界剣「悟入幻想 -Easy-」",                  Stage.Five,     Level.Easy),
        new( 82, "人界剣「悟入幻想」",                         Stage.Five,     Level.Normal),
        new( 83, "人世剣「大悟顕晦」",                         Stage.Five,     Level.Hard),
        new( 84, "人神剣「俗諦常住」",                         Stage.Five,     Level.Lunatic),
        new( 85, "天上剣「天人の五衰 -Easy-」",                Stage.Five,     Level.Easy),
        new( 86, "天上剣「天人の五衰」",                       Stage.Five,     Level.Normal),
        new( 87, "天界剣「七魄忌諱」",                         Stage.Five,     Level.Hard),
        new( 88, "天神剣「三魂七魄」",                         Stage.Five,     Level.Lunatic),
        new( 89, "六道剣「一念無量劫 -Easy-」",                Stage.Six,      Level.Easy),
        new( 90, "六道剣「一念無量劫」",                       Stage.Six,      Level.Normal),
        new( 91, "六道剣「一念無量劫 -Hard-」",                Stage.Six,      Level.Hard),
        new( 92, "六道剣「一念無量劫 -Lunatic-」",             Stage.Six,      Level.Lunatic),
        new( 93, "亡郷「亡我郷 -さまよえる魂-」",              Stage.Six,      Level.Easy),
        new( 94, "亡郷「亡我郷 -宿罪-」",                      Stage.Six,      Level.Normal),
        new( 95, "亡郷「亡我郷 -道無き道-」",                  Stage.Six,      Level.Hard),
        new( 96, "亡郷「亡我郷 -自尽-」",                      Stage.Six,      Level.Lunatic),
        new( 97, "亡舞「生者必滅の理 -眩惑-」",                Stage.Six,      Level.Easy),
        new( 98, "亡舞「生者必滅の理 -死蝶-」",                Stage.Six,      Level.Normal),
        new( 99, "亡舞「生者必滅の理 -毒蛾-」",                Stage.Six,      Level.Hard),
        new(100, "亡舞「生者必滅の理 -魔境-」",                Stage.Six,      Level.Lunatic),
        new(101, "華霊「ゴーストバタフライ」",                 Stage.Six,      Level.Easy),
        new(102, "華霊「スワローテイルバタフライ」",           Stage.Six,      Level.Normal),
        new(103, "華霊「ディープルーティドバタフライ」",       Stage.Six,      Level.Hard),
        new(104, "華霊「バタフライディルージョン」",           Stage.Six,      Level.Lunatic),
        new(105, "幽曲「リポジトリ・オブ・ヒロカワ -偽霊-」",  Stage.Six,      Level.Easy),
        new(106, "幽曲「リポジトリ・オブ・ヒロカワ -亡霊-」",  Stage.Six,      Level.Normal),
        new(107, "幽曲「リポジトリ・オブ・ヒロカワ -幻霊-」",  Stage.Six,      Level.Hard),
        new(108, "幽曲「リポジトリ・オブ・ヒロカワ -神霊-」",  Stage.Six,      Level.Lunatic),
        new(109, "桜符「完全なる墨染の桜 -封印-」",            Stage.Six,      Level.Easy),
        new(110, "桜符「完全なる墨染の桜 -亡我-」",            Stage.Six,      Level.Normal),
        new(111, "桜符「完全なる墨染の桜 -春眠-」",            Stage.Six,      Level.Hard),
        new(112, "桜符「完全なる墨染の桜 -開花-」",            Stage.Six,      Level.Lunatic),
        new(113, "「反魂蝶 -一分咲-」",                        Stage.Six,      Level.Easy),
        new(114, "「反魂蝶 -参分咲-」",                        Stage.Six,      Level.Normal),
        new(115, "「反魂蝶 -伍分咲-」",                        Stage.Six,      Level.Hard),
        new(116, "「反魂蝶 -八分咲-」",                        Stage.Six,      Level.Lunatic),
        new(117, "鬼符「青鬼赤鬼」",                           Stage.Extra,    Level.Extra),
        new(118, "鬼神「飛翔毘沙門天」",                       Stage.Extra,    Level.Extra),
        new(119, "式神「仙狐思念」",                           Stage.Extra,    Level.Extra),
        new(120, "式神「十二神将の宴」",                       Stage.Extra,    Level.Extra),
        new(121, "式輝「狐狸妖怪レーザー」",                   Stage.Extra,    Level.Extra),
        new(122, "式輝「四面楚歌チャーミング」",               Stage.Extra,    Level.Extra),
        new(123, "式輝「プリンセス天狐 -Illusion-」",          Stage.Extra,    Level.Extra),
        new(124, "式弾「アルティメットブディスト」",           Stage.Extra,    Level.Extra),
        new(125, "式弾「ユーニラタルコンタクト」",             Stage.Extra,    Level.Extra),
        new(126, "式神「橙」",                                 Stage.Extra,    Level.Extra),
        new(127, "「狐狗狸さんの契約」",                       Stage.Extra,    Level.Extra),
        new(128, "幻神「飯綱権現降臨」",                       Stage.Extra,    Level.Extra),
        new(129, "式神「前鬼後鬼の守護」",                     Stage.Phantasm, Level.Phantasm),
        new(130, "式神「憑依荼吉尼天」",                       Stage.Phantasm, Level.Phantasm),
        new(131, "結界「夢と現の呪」",                         Stage.Phantasm, Level.Phantasm),
        new(132, "結界「動と静の均衡」",                       Stage.Phantasm, Level.Phantasm),
        new(133, "結界「光と闇の網目」",                       Stage.Phantasm, Level.Phantasm),
        new(134, "罔両「ストレートとカーブの夢郷」",           Stage.Phantasm, Level.Phantasm),
        new(135, "罔両「八雲紫の神隠し」",                     Stage.Phantasm, Level.Phantasm),
        new(136, "罔両「禅寺に棲む妖蝶」",                     Stage.Phantasm, Level.Phantasm),
        new(137, "魍魎「二重黒死蝶」",                         Stage.Phantasm, Level.Phantasm),
        new(138, "式神「八雲藍」",                             Stage.Phantasm, Level.Phantasm),
        new(139, "「人間と妖怪の境界」",                       Stage.Phantasm, Level.Phantasm),
        new(140, "結界「生と死の境界」",                       Stage.Phantasm, Level.Phantasm),
        new(141, "紫奥義「弾幕結界」",                         Stage.Phantasm, Level.Phantasm),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(static card => card.Id);

    /// <summary>
    /// Gets wheter you can practice the specified level or not.
    /// </summary>
    /// <param name="level">A level.</param>
    /// <returns><see langword="true"/> if it can be practiced, otherwize <see langword="false"/>.</returns>
    public static bool CanPractice(Level level)
    {
        return EnumHelper.IsDefined(level) && (level != Level.Extra) && (level != Level.Phantasm);
    }

    /// <summary>
    /// Gets wheter you can practice the specified stage or not.
    /// </summary>
    /// <param name="stage">A stage.</param>
    /// <returns><see langword="true"/> if it can be practiced, otherwize <see langword="false"/>.</returns>
    public static bool CanPractice(Stage stage)
    {
        return EnumHelper.IsDefined(stage) && (stage != Stage.Extra) && (stage != Stage.Phantasm);
    }
}
