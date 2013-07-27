# テンプレート書式

<style>
<!--
ul ul li { display: inline; }
table { border: 1px solid #000; margin: 1em; }
table caption { font-weight: bold; text-align: left; }
table td { border: 1px solid #999; vertical-align: top; }
table td.header { width: 60px; }
table td.format { width: 60px; }
table td ul { margin: 0; padding-left: 2em; }
table td p.legends { margin: 0 0 0 2em; }
table td pre { margin: 0 0 0 2em; }
-->
</style>

## Table of Contents

* [テンプレートファイルとは](#AboutTemplateFile)
* [テンプレート書式とは](#AboutTemplateFormat)
* [東方紅魔郷用テンプレート書式](#Th06Formats)
    * [スコアランキング](#T06SCR)
    * [御札戦歴](#T06C)
    * [スペルカード基本情報](#T06CARD)
    * [スペルカード蒐集率](#T06CRG)
    * [クリア達成度](#T06CLEAR)
    * [プラクティススコア](#T06PRAC)
* [東方妖々夢用テンプレート書式](#Th07Formats)
    * [スコアランキング](#T07SCR)
    * [御札戦歴](#T07C)
    * [スペルカード基本情報](#T07CARD)
    * [スペルカード蒐集率](#T07CRG)
    * [クリア達成度](#T07CLEAR)
    * [プレイ回数](#T07PLAY)
    * [総起動時間](#T07TIMEALL)
    * [総プレイ時間](#T07TIMEPLY)
    * [プラクティススコア](#T07PRAC)
* [東方永夜抄用テンプレート書式](#Th08Formats)
    * [スコアランキング](#T08SCR)
    * [御札戦歴](#T08C)
    * [スペルカード基本情報](#T08CARD)
    * [スペルカード蒐集率](#T08CRG)
    * [クリア達成度](#T08CLEAR)
    * [プレイ回数](#T08PLAY)
    * [総起動時間](#T08TIMEALL)
    * [総プレイ時間](#T08TIMEPLY)
    * [プラクティススコア](#T08PRAC)
* [東方花映塚用テンプレート書式](#Th09Formats)
    * [スコアランキング](#T09SCR)
    * [クリア達成度](#T09CLEAR)
    * [総起動時間](#T09TIMEALL)
* [東方文花帖用テンプレート書式](#Th095Formats)
    * [スコア一覧](#T95SCR)
    * [スコア合計](#T95SCRTL)
    * [被写体 & スペルカード情報](#T95CARD)
    * [ベストショット出力](#T95SHOT)
    * [ベストショット出力（詳細版）](#T95SHOTEX)
* [東方風神録用テンプレート書式](#Th10Formats)
    * [スコアランキング](#T10SCR)
    * [御札戦歴](#T10C)
    * [スペルカード基本情報](#T10CARD)
    * [スペルカード蒐集率](#T10CRG)
    * [クリア達成度](#T10CLEAR)
    * [キャラごとの個別データ](#T10CHARA)
    * [キャラごとの個別データ（詳細版）](#T10CHARAEX)

----------------------------------------

## <a id="AboutTemplateFile">テンプレートファイルとは</a>

本ツールが扱うテンプレートファイルとは、特定の文字列（[テンプレート書式](#AboutTemplateFormat)）を含んだ任意のテキストファイルです。拡張子は何であっても構いません。

----------------------------------------

## <a id="AboutTemplateFormat">テンプレート書式とは</a>

本ツールが扱うテンプレート書式は、基本的に[東方メモリマネージャー][ThMM]と同じです。  
ただし、書式は同じでも変換結果は全く同一とは限りません。詳細は、以下の各表の「相違点」の行を参照して下さい。

* 「%」から始まる半角英数字の文字列です。
* 英字の大小は区別されません。
* 変換処理の際、本ツールの作品名コンボボックスで選択された作品用のテンプレート書式のみが変換されます。それ以外はそのまま出力されます。

  [ThMM]: http://www.sue445.net/downloads/ThMemoryManager.html

----------------------------------------

## <a id="Th06Formats">東方紅魔郷用テンプレート書式</a>

<table>
 <caption id="T06SCR">スコアランキング</caption>
 <tr>
  <td class="header" rowspan="5">書式</td>
  <td colspan="2"><code>%T06SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前<br />
   [RA]: 霊夢（霊） [RB]: 霊夢（夢） [MA]: 魔理沙（魔） [MB]: 魔理沙（恋）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位<br />
   [1～9]: 1～9 位 [0]: 10 位
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 登録名 [2]: スコア [3]: 到達ステージ
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T06SCRNMB12</code> … Normal 魔理沙（恋）の 1 位のスコア<br />
   <code>%T06SCRXRA41</code> … Extra 霊夢（霊）の 4 位の登録名
  </td>
 </tr>
</table>

<table>
 <caption id="T06C">御札戦歴</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T06C[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   スペルカードの番号など<br />
   [01～64]: スペルカードの番号 [00]: 全スペルカードの合計値
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目<br />
   [1]: 取得回数（勝率の分子） [2]: 挑戦回数（勝率の分母）
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T06C011</code> … 月符「ムーンライトレイ」の取得回数<br />
   <code>%T06C022</code> … 夜符「ナイトバード」の挑戦回数
  </td>
 </tr>
</table>

<table>
 <caption id="T06CARD">スペルカード基本情報</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T06CARD[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   スペルカードの番号<br />
   [01～64]: スペルカードの番号
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目<br />
   [N]: スペルカードの名前
   [R]: スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T06CARD01N</code> … 月符「ムーンライトレイ」<br />
   <code>%T06CARD01R</code> … Hard, Lunatic
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>1 枚が複数の難易度にまたがっているスペルカードについては、難易度はカンマ区切りで出力されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>「Hard,Lunatic」ではなく「Hard, Lunatic」のように半角空白も出力されます。</li>
 <li>未挑戦のスペルカードについては、名前・難易度ともに「?????」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T06CRG">スペルカード蒐集率</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T06CRG[x][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   ステージ<br />
   [0]: 全ステージ合計 [1～6]: Stage1～6 [X]: Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目<br />
   [1]: 取得数（勝率の分子） [2]: 挑戦数（勝率の分母）
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T06CRG01</code> … 全ステージ合計の取得数<br />
   <code>%T06CRG12</code> … Stage1 の挑戦数
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>紅魔郷では 1 枚で複数の難易度にまたがっているスペルカードがあるため、難易度の指定はできません。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T06CLEAR">クリア達成度</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T06CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前<br />
   [RA]: 霊夢（霊） [RB]: 霊夢（夢） [MA]: 魔理沙（魔） [MB]: 魔理沙（恋）
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T06CLEARXMA</code> … Extra 魔理沙（魔）のクリア達成度<br />
   <code>%T06CLEARNRA</code> … Normal 霊夢（霊）のクリア達成度
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>
  クリア達成度（ゲームの進行状況）に応じて以下の文字列が出力されます。
  <p class="legends">
   -------（未プレイ）, Stage 1, Stage 2, Stage 3, Stage 4, Stage 5, Stage 6,
   All Clear, Not Clear（Extra 未クリア）
  </p>
 </li>
 <li>本ツールでは、ランキングを基にクリア達成度を算出しているため、実際はクリア済みでもランキング上に存在していなければ未クリア扱いになってしまいます。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T06PRAC">プラクティススコア</caption>
 <tr>
  <td class="header" rowspan="4">書式</td>
  <td colspan="2"><code>%T06PRAC[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前<br />
   [RA]: 霊夢（霊） [RB]: 霊夢（夢） [MA]: 魔理沙（魔） [MB]: 魔理沙（恋）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   ステージ<br />
   [1～6]: Stage1～6
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T06PRACEMA1</code> … Easy 魔理沙（魔）の Stage 1 のプラクティススコア<br />
   <code>%T06PRACNRA4</code> … Normal 霊夢（霊）の Stage 4 のプラクティススコア
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>存在しない難易度とステージの組み合わせ（つまり Easy の Stage6）は無視されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>このテンプレート書式は本ツール独自のものです。</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## <a id="Th07Formats">東方妖々夢用テンプレート書式</a>

<table>
 <caption id="T07SCR">スコアランキング</caption>
 <tr>
  <td class="header" rowspan="5">書式</td>
  <td colspan="2"><code>%T07SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra [P]: Phantasm
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前<br />
   [RA]: 霊夢（霊） [RB]: 霊夢（夢） [MA]: 魔理沙（魔） [MB]: 魔理沙（恋）
   [SA]: 咲夜（幻） [SB]: 咲夜（時）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位<br />
   [1～9]: 1～9 位 [0]: 10 位
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 登録名 [2]: スコア [3]: 到達ステージ [4]: 日付 [5]: 処理落ち率
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T07SCRNSB12</code> … Normal 咲夜（時）の 1 位のスコア<br />
   <code>%T07SCRXRA44</code> … Extra 霊夢（霊）の 4 位の日付
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>日付は月日だけが「mm/dd」の形式で出力されます。年や時分秒はそもそもスコアファイルに保存されていません。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>スコアの 1 の位には、原作と同様にコンティニュー回数が出力されます。</li>
 <li>処理落ち率はとりあえず小数点以下第 3 位まで（% 記号付きで）出力されます。今後、この桁数を設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T07C">御札戦歴</caption>
 <tr>
  <td class="header" rowspan="4">書式</td>
  <td colspan="2"><code>%T07C[xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など<br />
   [001～141]: スペルカードの番号 [000]: 全スペルカードの合計値
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など<br />
   [TL]: 全主人公合計
   [RA]: 霊夢（霊） [RB]: 霊夢（夢） [MA]: 魔理沙（魔） [MB]: 魔理沙（恋）
   [SA]: 咲夜（幻） [SB]: 咲夜（時）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: MaxBonus [2]: 取得回数（勝率の分子） [3]: 挑戦回数（勝率の分母）
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T07C001TL1</code> …
   全主人公合計の霜符「フロストコラムス」の MaxBonus<br />
   <code>%T07C002SB3</code> …
   咲夜（時）の霜符「フロストコラムス -Lunatic-」の挑戦回数
  </td>
 </tr>
</table>

<table>
 <caption id="T07CARD">スペルカード基本情報</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T07CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号<br />
   [001～141]: スペルカードの番号
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目<br />
   [N]: スペルカードの名前
   [R]: スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra, Phantasm)
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T07CARD001N</code> … 霜符「フロストコラムス」<br />
   <code>%T07CARD001R</code> … Hard
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>未挑戦のスペルカードについては、名前・難易度ともに「?????」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T07CRG">スペルカード蒐集率</caption>
 <tr>
  <td class="header" rowspan="5">書式</td>
  <td colspan="2"><code>%T07CRG[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度など<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra [P]: Phantasm
   [T]: Total
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など<br />
   [TL]: 全主人公合計
   [RA]: 霊夢（霊） [RB]: 霊夢（夢） [MA]: 魔理沙（魔） [MB]: 魔理沙（恋）
   [SA]: 咲夜（幻） [SB]: 咲夜（時）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージ<br />
   [0]: 全ステージ合計 [1～6]: Stage1～6<br />
   （Extra, Phantasm ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 取得数（勝率の分子） [2]: 挑戦数（勝率の分母）
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T07CRGERA01</code> … Easy 霊夢（霊）の全ステージ合計の取得数<br />
   <code>%T07CRGTSB41</code> … 咲夜（時）の Stage4 の全難易度合計の取得数<br />
   <code>%T07CRGTTL02</code> … 全難易度・全キャラ・全ステージ合計の挑戦数
  </td>
 </tr>
</table>

<table>
 <caption id="T07CLEAR">クリア達成度</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T07CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra [P]: Phantasm
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前<br />
   [RA]: 霊夢（霊） [RB]: 霊夢（夢） [MA]: 魔理沙（魔） [MB]: 魔理沙（恋）
   [SA]: 咲夜（幻） [SB]: 咲夜（時）
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T07CLEARXMA</code> … Extra 魔理沙（魔）のクリア達成度<br />
   <code>%T07CLEARNSB</code> … Normal 咲夜（時）のクリア達成度
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>
  クリア達成度（ゲームの進行状況）に応じて以下の文字列が出力されます。
  <p class="legends">
   -------（未プレイ）, Stage 1, Stage 2, Stage 3, Stage 4, Stage 5, Stage 6,
   All Clear, Not Clear（Extra, Phantasm 未クリア）
  </p>
 </li>
 <li>本ツールでは、ランキングを基にクリア達成度を算出しているため、実際はクリア済みでもランキング上に存在していなければ未クリア扱いになってしまいます。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T07PLAY">プレイ回数</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T07PLAY[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra [P]: Phantasm
   [T]: Total
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など<br />
   [TL]: 全主人公合計
   [RA]: 霊夢（霊） [RB]: 霊夢（夢） [MA]: 魔理沙（魔） [MB]: 魔理沙（恋）
   [SA]: 咲夜（幻） [SB]: 咲夜（時）
   [CL]: クリア回数 [CN]: コンティニュー回数 [PR]: プラクティスプレイ回数
   [RT]: リトライ回数
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T07PLAYHRB</code> … Hard 霊夢（夢）のプレイ回数<br />
   <code>%T07PLAYLCL</code> … Lunatic のクリア回数
  </td>
 </tr>
</table>

<table>
 <caption id="T07TIMEALL">総起動時間</caption>
 <tr>
  <td class="header">書式</td>
  <td><code>%T07TIMEALL</code></td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「h:mm:ss.ddd」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td>
<ul>
 <li>秒とミリ秒の間は「:」ではなく「.」で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T07TIMEPLY">総プレイ時間</caption>
 <tr>
  <td class="header">書式</td>
  <td><code>%T07TIMEPLY</code></td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「h:mm:ss.ddd」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td>
<ul>
 <li>秒とミリ秒の間は「:」ではなく「.」で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T07PRAC">プラクティススコア</caption>
 <tr>
  <td class="header" rowspan="5">書式</td>
  <td colspan="2"><code>%T07PRAC[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前<br />
   [RA]: 霊夢（霊） [RB]: 霊夢（夢） [MA]: 魔理沙（魔） [MB]: 魔理沙（恋）
   [SA]: 咲夜（幻） [SB]: 咲夜（時）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージ<br />
   [1～6]: Stage1～6
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: スコア [2]: プレイ回数
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T07PRACESB11</code> … Easy 咲夜（時）の Stage 1 のプラクティススコア<br />
   <code>%T07PRACNRA42</code> … Normal 霊夢（霊）の Stage 4 のプラクティスプレイ回数
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>このテンプレート書式は本ツール独自のものです。</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## <a id="Th08Formats">東方永夜抄用テンプレート書式</a>

<table>
 <caption id="T08SCR">スコアランキング</caption>
 <tr>
  <td class="header" rowspan="5">書式</td>
  <td colspan="2"><code>%T08SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前<br />
   [RY]: 霊夢 &amp; 紫 [MA]: 魔理沙 &amp; アリス [SR]: 咲夜 &amp; レミリア
   [YY]: 妖夢 &amp; 幽々子 [RM]: 霊夢 [YK]: 紫 [MR]: 魔理沙 [AL]: アリス
   [SK]: 咲夜 [RL]: レミリア [YM]: 妖夢 [YU]: 幽々子
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位<br />
   [1～9]: 1～9 位 [0]: 10 位
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 登録名 [2]: スコア [3]: 到達ステージ [4]: 日付 [5]: 処理落ち率
   [6]: プレイ時間 [7]: 初期プレイヤー数 [8]: 得点アイテム数 [9]: 刻符数
   [0]: ミス回数 [A]: ボム回数 [B]: ラストスペル回数 [C]: ポーズ回数
   [D]: コンティニュー回数 [E]: 人間率 [F]: 取得スペルカード一覧
   [G]: 取得スペルカード枚数
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T08SCRNSR12</code> … Normal 咲夜 &amp; レミリアの 1 位のスコア<br />
   <code>%T08SCRXRM45</code> … Extra 霊夢の 4 位の処理落ち率
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>日付は月日だけが「mm/dd」の形式で出力されます。年や時分秒はそもそもスコアファイルに保存されていません。</li>
 <li>取得スペルカード一覧について、東方永夜抄の score.txt にある「総取得回数/総遭遇回数」は出力されません。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>スコアの 1 の位には、原作と同様にコンティニュー回数が出力されます。</li>
 <li>処理落ち率はとりあえず小数点以下第 3 位まで（% 記号付きで）出力されます。今後、この桁数を設定可能にするかも知れません。</li>
 <li>プレイ時間は時分秒が「h:mm:ss」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。</li>
 <li>人間率は小数点以下第 2 位まで（% 記号付きで）出力されます。第 3 位以下はスコアファイルに保存されていません。</li>
 <li>本ツールには、<a href="http://www.sue445.net/downloads/ThMemoryManager.html">東方メモリマネージャー</a>の GetSpellListTag 相当の設定項目はありません。今後対応するかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T08C">御札戦歴</caption>
 <tr>
  <td class="header" rowspan="5">書式</td>
  <td colspan="2"><code>%T08C[w][xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   戦歴の種類<br />
   [S]: ゲーム本編 [P]: スペルプラクティス</td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など<br />
   [001～222]: スペルカードの番号 [000]: 全スペルカードの合計値</td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など<br />
   [TL]: 全主人公合計
   [RY]: 霊夢 &amp; 紫 [MA]: 魔理沙 &amp; アリス [SR]: 咲夜 &amp; レミリア
   [YY]: 妖夢 &amp; 幽々子 [RM]: 霊夢 [YK]: 紫 [MR]: 魔理沙 [AL]: アリス
   [SK]: 咲夜 [RL]: レミリア [YM]: 妖夢 [YU]: 幽々子
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: MaxBonus [2]: 取得回数（勝率の分子） [3]: 挑戦回数（勝率の分母）
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T08CS003TL1</code> … ゲーム本編
   全主人公合計の灯符「ファイヤフライフェノメノン」の MaxBonus<br />
   <code>%T08CP008RY2</code> … スペルプラクティス
   霊夢 &amp; 紫の蠢符「リトルバグストーム」の取得回数
  </td>
 </tr>
</table>

<table>
 <caption id="T08CARD">スペルカード基本情報</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T08CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号<br />
   [001～222]: スペルカードの番号
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目<br />
   [N]: スペルカードの名前
   [R]: スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra, Last Word)
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T08CARD023N</code> … 鷹符「イルスタードダイブ」<br />
   <code>%T08CARD023R</code> … Normal
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>ゲーム本編・スペルプラクティスの両方とも未挑戦のスペルカードについては、名前・難易度ともに「?????」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T08CRG">スペルカード蒐集率</caption>
 <tr>
  <td class="header" rowspan="6">書式</td>
  <td colspan="2"><code>%T08CRG[v][w][xx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[v]</code></td>
  <td>
   戦歴の種類<br />
   [S]: ゲーム本編 [P]: スペルプラクティス</td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度など<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra
   [W]: Last Word [T]: Total
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など<br />
   [TL]: 全主人公合計
   [RY]: 霊夢 &amp; 紫 [MA]: 魔理沙 &amp; アリス [SR]: 咲夜 &amp; レミリア
   [YY]: 妖夢 &amp; 幽々子 [RM]: 霊夢 [YK]: 紫 [MR]: 魔理沙 [AL]: アリス
   [SK]: 咲夜 [RL]: レミリア [YM]: 妖夢 [YU]: 幽々子
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   ステージ<br />
   [00]: 全ステージ合計
   [1A]: Stage1 [2A]: Stage2 [3A]: Stage3 [4A]: Stage4A [4B]: Stage4B
   [5A]: Stage5 [6A]: Stage6A [6B]: Stage6B<br />
   （Extra, Last Word ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 取得数（勝率の分子） [2]: 挑戦数（勝率の分母）
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T08CRGSERY2A1</code> … ゲーム本編
   Easy 霊夢 &amp; 紫の Stage2 の取得数<br />
   <code>%T08CRGSTYY4A1</code> … ゲーム本編
   妖夢 &amp; 幽々子の Stage4A の全難易度合計の取得数<br />
   <code>%T08CRGPTTL002</code> … スペルプラクティス
   全難易度・全キャラ・全ステージ合計の挑戦数
  </td>
 </tr>
</table>

<table>
 <caption id="T08CLEAR">クリア達成度</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T08CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前<br />
   [RY]: 霊夢 &amp; 紫 [MA]: 魔理沙 &amp; アリス [SR]: 咲夜 &amp; レミリア
   [YY]: 妖夢 &amp; 幽々子 [RM]: 霊夢 [YK]: 紫 [MR]: 魔理沙 [AL]: アリス
   [SK]: 咲夜 [RL]: レミリア [YM]: 妖夢 [YU]: 幽々子
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T08CLEARXMA</code> … Extra 魔理沙 &amp; アリスのクリア達成度<br />
   <code>%T08CLEARNSK</code> … Normal 咲夜のクリア達成度
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>
  クリア達成度（ゲームの進行状況）に応じて以下の文字列が出力されます。
  <p class="legends">
   -------（未プレイ）, Stage 1, Stage 2, Stage 3, Stage 4, Stage 5, Stage 6A,
   FinalA Clear, All Clear, Not Clear（Extra 未クリア）
  </p>
 </li>
 <li>本ツールでは、ランキングを基にクリア達成度を算出しているため、実際はクリア済みでもランキング上に存在していなければ未クリア扱いになってしまいます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>本ツールの FinalA Clear の判定方法が間違っているかも知れません…。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T08PLAY">プレイ回数</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T08PLAY[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra [T]: Total
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など<br />
   [TL]: 全主人公合計
   [RY]: 霊夢 &amp; 紫 [MA]: 魔理沙 &amp; アリス [SR]: 咲夜 &amp; レミリア
   [YY]: 妖夢 &amp; 幽々子 [RM]: 霊夢 [YK]: 紫 [MR]: 魔理沙 [AL]: アリス
   [SK]: 咲夜 [RL]: レミリア [YM]: 妖夢 [YU]: 幽々子
   [CL]: クリア回数 [CN]: コンティニュー回数 [PR]: プラクティスプレイ回数
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T08PLAYHYY</code> … Hard 妖夢 &amp; 幽々子のプレイ回数<br />
   <code>%T08PLAYLCN</code> … Lunatic のコンティニュー回数
  </td>
 </tr>
</table>

<table>
 <caption id="T08TIMEALL">総起動時間</caption>
 <tr>
  <td class="header">書式</td>
  <td><code>%T08TIMEALL</code></td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「h:mm:ss.ddd」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td>
<ul>
 <li>秒とミリ秒の間は「:」ではなく「.」で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T08TIMEPLY">総プレイ時間</caption>
 <tr>
  <td class="header">書式</td>
  <td><code>%T08TIMEPLY</code></td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「h:mm:ss.ddd」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td>
<ul>
 <li>秒とミリ秒の間は「:」ではなく「.」で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T08PRAC">プラクティススコア</caption>
 <tr>
  <td class="header" rowspan="5">書式</td>
  <td colspan="2"><code>%T08PRAC[w][xx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前<br />
   [RY]: 霊夢 &amp; 紫 [MA]: 魔理沙 &amp; アリス [SR]: 咲夜 &amp; レミリア
   [YY]: 妖夢 &amp; 幽々子 [RM]: 霊夢 [YK]: 紫 [MR]: 魔理沙 [AL]: アリス
   [SK]: 咲夜 [RL]: レミリア [YM]: 妖夢 [YU]: 幽々子
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   ステージ<br />
   [1A]: Stage1 [2A]: Stage2 [3A]: Stage3 [4A]: Stage4A [4B]: Stage4B
   [5A]: Stage5 [6A]: Stage6A [6B]: Stage6B
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: スコア [2]: プレイ回数
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T08PRACEYM1A1</code> … Easy 妖夢の Stage 1 のプラクティススコア<br />
   <code>%T08PRACNRY4B2</code> … Normal 霊夢 &amp; 紫の Stage 4B のプラクティスプレイ回数
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>このテンプレート書式は本ツール独自のものです。</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## <a id="Th09Formats">東方花映塚用テンプレート書式</a>

<table>
 <caption id="T09SCR">スコアランキング</caption>
 <tr>
  <td class="header" rowspan="5">書式</td>
  <td colspan="2"><code>%T09SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前<br />
   [RM]: 霊夢 [MR]: 魔理沙 [SK]: 咲夜 [YM]: 妖夢 [RS]: 鈴仙 [CI]: チルノ
   [LY]: リリカ [MY]: ミスティア [TW]: てゐ [AY]: 文 [MD]: メディスン
   [YU]: 幽香 [KM]: 小町 [SI]: 四季映姫
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位<br />
   [1～5]: 1～5 位
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 登録名 [2]: スコア [3]: 日付
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T09SCRNMR12</code> … Normal 魔理沙の 1 位のスコア<br />
   <code>%T09SCRXRM41</code> … Extra 霊夢の 4 位の登録名
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>日付は年月日が「yy/mm/dd」の形式で出力されます。年は西暦の下 2 桁だけがスコアファイルに保存されています。また、時分秒はそもそもスコアファイルに保存されていません。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>スコアの 1 の位には、原作と同様にコンティニュー回数が出力されます。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T09CLEAR">クリア達成度</caption>
 <tr>
  <td class="header" rowspan="4">書式</td>
  <td colspan="2"><code>%T09CLEAR[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前<br />
   [RM]: 霊夢 [MR]: 魔理沙 [SK]: 咲夜 [YM]: 妖夢 [RS]: 鈴仙 [CI]: チルノ
   [LY]: リリカ [MY]: ミスティア [TW]: てゐ [AY]: 文 [MD]: メディスン
   [YU]: 幽香 [KM]: 小町 [SI]: 四季映姫
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   出力形式<br />
   [1]: クリア回数 [2]: クリアしたかどうかのフラグ情報
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T09CLEARXMR1</code> … Extra 魔理沙のクリア回数<br />
   <code>%T09CLEARNSK2</code> … Normal 咲夜のクリアフラグ
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>
  フラグ情報は、ゲームの進行状況に応じて以下の文字列が出力されます。
  <p class= "legends">-------（未プレイ）, Not Cleared, Cleared</p>
 </li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>1 回以上プレイしているが未クリアの場合に「Not Cleared」が出力されます。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T09TIMEALL">総起動時間</caption>
 <tr>
  <td class="header">書式</td>
  <td><code>%T09TIMEALL</code></td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「h:mm:ss.ddd」の形式で出力されます。</li>
 <li>スコアファイルには総プレイ時間のようなものも保存されているようですが、確証を持てないので（本ツールでも）出力しません。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td>
<ul>
 <li>秒とミリ秒の間は「:」ではなく「.」で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## <a id="Th095Formats">東方文花帖用テンプレート書式</a>

<table>
 <caption id="T95SCR">スコア一覧</caption>
 <tr>
  <td class="header" rowspan="4">書式</td>
  <td colspan="2"><code>%T95SCR[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル<br />
   [1～9]: Level 1～9 [0]: Level 10 [X]: Level Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン<br />
   [1～9]: 1～9
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: ハイスコア [2]: 登録してあるベストショットのスコア [3]: 撮影枚数
   [4]: 処理落ち率
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T95SCR111</code> … 1-1 でのハイスコア<br />
   <code>%T95SCR233</code> … 2-3 での撮影枚数
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-9 など）は無視されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>処理落ち率はとりあえず小数点以下第 3 位まで（% 記号付きで）出力されます。今後、この桁数を設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T95SCRTL">スコア合計</caption>
 <tr>
  <td class="header" rowspan="2">書式</td>
  <td colspan="2"><code>%T95SCRTL[x]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   項目<br />
   [1]: 撮影総合評価点 [2]: 登録してあるベストショットのスコアの合計
   [3]: 総撮影枚数 [4]: 撮影に成功したシーン数の合計
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T95SCRTL1</code> … 撮影総合評価点<br />
   <code>%T95SCRTL3</code> … 総撮影枚数
  </td>
 </tr>
</table>

<table>
 <caption id="T95CARD">被写体 &amp; スペルカード情報</caption>
 <tr>
  <td class="header" rowspan="4">書式</td>
  <td colspan="2"><code>%T95CARD[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル<br />
   [1～9]: Level 1～9 [0]: Level 10 [X]: Level Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン<br />
   [1～9]: 1～9
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 被写体の名前 [2]: スペルカード名
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T95CARD111</code> … 1-1 の被写体の名前<br />
   <code>%T95CARD232</code> … 2-3 のスペルカード名
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-9 など）は無視されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>未挑戦のものについては、被写体の名前・スペルカード名ともに「?????」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T95SHOT">ベストショット出力</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T95SHOT[x][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル<br />
   [1～9]: Level 1～9 [0]: Level 10 [X]: Level Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン<br />
   [1～9]: 1～9
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T95SHOT12</code> … 1-2 のベストショット<br />
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-9 など）は無視されます。</li>
 <li>このテンプレート書式は「&lt;img src="./bestshot/bs_01_1.png" alt="～" title="～" border=0&gt;」のような HTML の IMG タグに置換されます。<br />
  同時に、対象となるベストショットファイル (bs_??_?.dat) を PNG 形式に変換した画像ファイルが出力されます。</li>
 <li>IMG タグの alt 属性と title 属性には、ベストショット撮影時のスコアと処理落ち率、及びスペルカード名が出力されます。</li>
 <li>画像ファイルは、「出力先(O):」欄で指定されたフォルダ内の「bestshot」フォルダに出力されます。</li>
 <li>ベストショットファイルが存在しない場合、IMG タグや画像ファイルは出力されません。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>本ツールでは、ベストショットファイルから PNG 形式への変換を自前で行います。そのため Susie プラグインは不要です。</li>
 <li>自前で変換する都合上、東方文花帖 ver. 1.02a 以外で撮影されたベストショットファイルの変換には非対応です。対応予定も今のところありません。</li>
 <li>ベストショットファイルの変換は、このテンプレート書式がテンプレートファイル内に無くても実行されます。</li>
 <li>本ツールには、<a href="http://www.sue445.net/downloads/ThMemoryManager.html">東方メモリマネージャー</a>の ImgPath 相当の設定項目はありません。つまり画像ファイルの出力先フォルダの変更はできません。今後対応するかも知れません。</li>
 <li>画像ファイルの出力先フォルダが存在しない場合、本ツールが自動で作成します。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T95SHOTEX">ベストショット出力（詳細版）</caption>
 <tr>
  <td class="header" rowspan="4">書式</td>
  <td colspan="2"><code>%T95SHOTEX[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル<br />
   [1～9]: Level 1～9 [0]: Level 10 [X]: Level Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン<br />
   [1～9]: 1～9
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 画像ファイルへの相対パス [2]: 画像ファイルの幅 (px)
   [3]: 画像ファイルの高さ (px) [4]: ベストショット撮影時のスコア
   [5]: ベストショット撮影時の処理落ち率 [6]: ベストショット撮影日時
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T95SHOTEX121</code> … 1-2 の画像ファイルへの相対パス<br />
   <code>%T95SHOTEX236</code> … 2-3 のベストショット撮影日時
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-9 など）は無視されます。</li>
 <li>
  このテンプレート書式を使って、例えば <code>%T95SHOT12</code> と同等の出力結果を得るには、テンプレートファイルに以下の通りに記載します。
<pre>
&lt;img src="%T95SHOTEX121" alt="Score: %T95SHOTEX124
Slow: %T95SHOTEX125
SpellName: %T95CARD122" title="Score: %T95SHOTEX124
Slow: %T95SHOTEX125
SpellName: %T95CARD122" border=0&gt;
</pre>
 </li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>
  このテンプレート書式は本ツール独自のものです。<br />
  「<a href="#T95SHOT">ベストショット出力</a>」により出力される IMG タグが気に食わなかったから、この書式を新規追加し、かつベストショットファイルの変換を自前で実装したようなものです。
 </li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## <a id="Th10Formats">東方風神録用テンプレート書式</a>

<table>
 <caption id="T10SCR">スコアランキング</caption>
 <tr>
  <td class="header" rowspan="5">書式</td>
  <td colspan="2"><code>%T10SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前<br />
   [RA]: 霊夢 (A) [RB]: 霊夢 (B) [RC]: 霊夢 (C)
   [MA]: 魔理沙 (A) [MB]: 魔理沙 (B) [MC]: 魔理沙 (C)
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位<br />
   [1～9]: 1～9 位 [0]: 10 位
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 登録名 [2]: スコア [3]: 到達ステージ [4]: 日時 [5]: 処理落ち率
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T10SCRNMC12</code> … Normal 魔理沙 (C) の 1 位のスコア<br />
   <code>%T10SCRXRA44</code> … Extra 霊夢 (A) の 4 位の日時
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>スコアの 1 の位には、原作と同様にコンティニュー回数が出力されます。</li>
 <li>日時は年月日及び時分秒が「yyyy/mm/dd hh:mm:ss」の形式で出力されます。</li>
 <li>処理落ち率はとりあえず小数点以下第 3 位まで（% 記号付きで）出力されます。今後、この桁数を設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T10C">御札戦歴</caption>
 <tr>
  <td class="header" rowspan="4">書式</td>
  <td colspan="2"><code>%T10C[xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など<br />
   [001～110]: スペルカードの番号 [000]: 全スペルカードの合計値
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など<br />
   [TL]: 全主人公合計
   [RA]: 霊夢 (A) [RB]: 霊夢 (B) [RC]: 霊夢 (C)
   [MA]: 魔理沙 (A) [MB]: 魔理沙 (B) [MC]: 魔理沙 (C)
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 取得回数（勝率の分子） [2]: 挑戦回数（勝率の分母）
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T10C003TL1</code> …
   全主人公合計の秋符「オータムスカイ」の取得回数<br />
   <code>%T10C003MC2</code> …
   魔理沙 (C) の秋符「オータムスカイ」の挑戦回数
  </td>
 </tr>
</table>

<table>
 <caption id="T10CARD">スペルカード基本情報</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T10CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号<br />
   [001～110]: スペルカードの番号
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目<br />
   [N]: スペルカードの名前
   [R]: スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T10CARD003N</code> … 秋符「オータムスカイ」<br />
   <code>%T10CARD003R</code> … Easy
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>未挑戦のスペルカードの名前は「?????」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
 <li>一方、スペルカードの難易度は、未挑戦かどうかにかかわらず常に出力されます。原作でも Result 画面を見れば難易度はバレるので、このような仕様にしています。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T10CRG">スペルカード蒐集率</caption>
 <tr>
  <td class="header" rowspan="5">書式</td>
  <td colspan="2"><code>%T10CRG[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度など<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra [T]: Total
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など<br />
   [TL]: 全主人公合計
   [RA]: 霊夢 (A) [RB]: 霊夢 (B) [RC]: 霊夢 (C)
   [MA]: 魔理沙 (A) [MB]: 魔理沙 (B) [MC]: 魔理沙 (C)
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージ<br />
   [0]: 全ステージ合計 [1～6]: Stage1～6<br />
   （Extra ではこの指定は無視され、Total ではそのステージの Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 取得数（勝率の分子） [2]: 挑戦数（勝率の分母）
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T10CRGERA01</code> … Easy 霊夢 (A) の全ステージ合計の取得数<br />
   <code>%T10CRGTMC41</code> … 魔理沙 (C) の Stage4 の全難易度合計の取得数<br />
   <code>%T10CRGTTL02</code> … 全難易度・全キャラ・全ステージ合計の挑戦数
  </td>
 </tr>
</table>

<table>
 <caption id="T10CLEAR">クリア達成度</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T10CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前<br />
   [RA]: 霊夢 (A) [RB]: 霊夢 (B) [RC]: 霊夢 (C)
   [MA]: 魔理沙 (A) [MB]: 魔理沙 (B) [MC]: 魔理沙 (C)
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T10CLEARXMA</code> … Extra 魔理沙 (A)のクリア達成度<br />
   <code>%T10CLEARNRB</code> … Normal 霊夢 (B) のクリア達成度
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>
  クリア達成度（ゲームの進行状況）に応じて以下の文字列が出力されます。
  <p class="legends">
   -------（未プレイ）, Stage 1, Stage 2, Stage 3, Stage 4, Stage 5, Stage 6,
   All Clear, Not Clear（Extra 未クリア）
  </p>
 </li>
 <li>本ツールでは、ランキングを基にクリア達成度を算出しているため、実際はクリア済みでもランキング上に存在していなければ未クリア扱いになってしまいます。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T10CHARA">キャラごとの個別データ</caption>
 <tr>
  <td class="header" rowspan="3">書式</td>
  <td colspan="2"><code>%T10CHARA[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など<br />
   [TL]: 全主人公合計
   [RA]: 霊夢 (A) [RB]: 霊夢 (B) [RC]: 霊夢 (C)
   [MA]: 魔理沙 (A) [MB]: 魔理沙 (B) [MC]: 魔理沙 (C)
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目<br />
   [1]: 総プレイ回数 [2]: プレイ時間 [3]: クリア回数
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T10CHARATL2</code> … 全主人公合計のプレイ時間<br />
   <code>%T10CHARARA1</code> … 霊夢 (A) の総プレイ回数
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>プレイ時間は時分秒が「h:mm:ss」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。</li>
</ul>
  </td>
 </tr>
</table>

<table>
 <caption id="T10CHARAEX">キャラごとの個別データ（詳細版）</caption>
 <tr>
  <td class="header" rowspan="4">書式</td>
  <td colspan="2"><code>%T10CHARAEX[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度<br />
   [E]: Easy [N]: Normal [H]: Hard [L]: Lunatic [X]: Extra<br />
   （総プレイ回数とプレイ時間ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など<br />
   [TL]: 全主人公合計
   [RA]: 霊夢 (A) [RB]: 霊夢 (B) [RC]: 霊夢 (C)
   [MA]: 魔理沙 (A) [MB]: 魔理沙 (B) [MC]: 魔理沙 (C)
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目<br />
   [1]: 総プレイ回数 [2]: プレイ時間 [3]: クリア回数
  </td>
 </tr>
 <tr>
  <td class="header">例</td>
  <td colspan="2">
   <code>%T10CHARAEXETL2</code> … 全主人公合計のプレイ時間<br />
   <code>%T10CHARAEXERA1</code> … 霊夢 (A) の総プレイ回数<br />
   <code>%T10CHARAEXNMC3</code> … Normal 魔理沙 (C) のクリア回数
  </td>
 </tr>
 <tr>
  <td class="header">補足</td>
  <td colspan="2">
<ul>
 <li>プレイ時間は時分秒が「h:mm:ss」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td class="header">相違点</td>
  <td colspan="2">
<ul>
 <li>このテンプレート書式は本ツール独自のものです。</li>
</ul>
  </td>
 </tr>
</table>
