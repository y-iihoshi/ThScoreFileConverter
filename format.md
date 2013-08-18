<style type="text/css">
<!--
	div.toc ul ul ul li { display: inline; }
	table { border: 1px solid #000; margin: 1em; }
	colgroup.header { width: 60px; background-color: #eee; }
	td { border: 1px solid #999; vertical-align: top; }
	td.format { width: 60px; }
	td ul { margin: 0; padding-left: 2em; }
	td dl { margin: 0; }
	td pre { margin: 0 0 0 2em; }
	td p.legends { margin: 0 0 0 2em; }
	dl.format dt { display: inline; }
	dl.format dt:after { content: ": "; }
	dl.format dd { display: inline; margin: 0 0.5em 0 0; }
	dl.example dt { float: left; clear: both; }
	dl.example dt:after { content: "\00a0…\00a0"; }
-->
</style>

# テンプレート書式 {: #TemplateFormats }

## Table of Contents {: #ToC }

[TOC]

----------------------------------------

## テンプレートファイルとは {: #AboutTemplateFile }

本ツールが扱うテンプレートファイルとは、特定の文字列（[テンプレート書式](#AboutTemplateFormat)）を含んだ任意のテキストファイルです。拡張子は何であっても構いません。

----------------------------------------

## テンプレート書式とは {: #AboutTemplateFormat }

本ツールが扱うテンプレート書式は、基本的に[東方メモリマネージャー][ThMM]と同じです。  
ただし、書式は同じでも変換結果は全く同一とは限りません。詳細は、以下の各表の「相違点」の行を参照して下さい。

* 「`%`」から始まる半角英数字の文字列です。
* 英字の大小は区別されません。
* 変換処理の際、本ツールの作品名コンボボックスで選択された作品用のテンプレート書式のみが変換されます。それ以外はそのまま出力されます。

  [ThMM]: http://www.sue445.net/downloads/ThMemoryManager.html

----------------------------------------

## 東方紅魔郷用テンプレート書式 {: #Th06Formats }

### スコアランキング {: #T06SCR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T06SCR[w][xx][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RA]`</dt><dd>霊夢（霊）</dd>
 <dt>`[RB]`</dt><dd>霊夢（夢）</dd>
 <dt>`[MA]`</dt><dd>魔理沙（魔）</dd>
 <dt>`[MB]`</dt><dd>魔理沙（恋）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   順位
<dl class="format">
 <dt>`[1～9]`</dt><dd>1～9 位</dd>
 <dt>`[0]`</dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>登録名</dd>
 <dt>`[2]`</dt><dd>スコア</dd>
 <dt>`[3]`</dt><dd>到達ステージ</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T06SCRNMB12`</dt><dd>Normal 魔理沙（恋）の 1 位のスコア</dd>
 <dt>`%T06SCRXRA41`</dt><dd>Extra 霊夢（霊）の 4 位の登録名</dd>
</dl>
  </td>
 </tr>
</table>

### 御札戦歴 {: #T06C }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T06C[xx][y]`</td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt>`[00]`</dt><dd>全スペルカードの合計値</dd>
 <dt>`[01～64]`</dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>取得回数（勝率の分子）</dd>
 <dt>`[2]`</dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T06C011`</dt><dd>月符「ムーンライトレイ」の取得回数</dd>
 <dt>`%T06C022`</dt><dd>夜符「ナイトバード」の挑戦回数</dd>
</dl>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T06CARD }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T06CARD[xx][y]`</td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt>`[01～64]`</dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[N]`</dt><dd>スペルカードの名前</dd>
 <dt>`[R]`</dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T06CARD01N`</dt><dd>月符「ムーンライトレイ」</dd>
 <dt>`%T06CARD01R`</dt><dd>Hard, Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>1 枚が複数の難易度にまたがっているスペルカードについては、難易度はカンマ区切りで出力されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>「<samp>Hard,Lunatic</samp>」ではなく「<samp>Hard, Lunatic</samp>」のように半角空白も出力されます。</li>
 <li>未挑戦のスペルカードについては、名前・難易度ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T06CRG }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T06CRG[x][y]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   ステージ
<dl class="format">
 <dt>`[0]`</dt><dd>全ステージ合計</dd>
 <dt>`[1～6]`</dt><dd>Stage 1～6</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>取得数（勝率の分子）</dd>
 <dt>`[2]`</dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T06CRG01`</dt><dd>全ステージ合計の取得数</dd>
 <dt>`%T06CRG12`</dt><dd>Stage 1 の挑戦数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>紅魔郷では 1 枚で複数の難易度にまたがっているスペルカードがあるため、難易度の指定はできません。</li>
</ul>
  </td>
 </tr>
</table>

### クリア達成度 {: #T06CLEAR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T06CLEAR[x][yy]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RA]`</dt><dd>霊夢（霊）</dd>
 <dt>`[RB]`</dt><dd>霊夢（夢）</dd>
 <dt>`[MA]`</dt><dd>魔理沙（魔）</dd>
 <dt>`[MB]`</dt><dd>魔理沙（恋）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T06CLEARXMA`</dt><dd>Extra 魔理沙（魔）のクリア達成度</dd>
 <dt>`%T06CLEARNRA`</dt><dd>Normal 霊夢（霊）のクリア達成度</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>
  クリア達成度（ゲームの進行状況）に応じて以下の文字列が出力されます。
  <p class="legends">
   <samp>-------</samp>（未プレイ）, <samp>Stage 1</samp>,
   <samp>Stage 2</samp>, <samp>Stage 3</samp>, <samp>Stage 4</samp>,
   <samp>Stage 5</samp>, <samp>Stage 6</samp>, <samp>All Clear</samp>,
   <samp>Not Clear</samp>（Extra 未クリア）
  </p>
 </li>
 <li>本ツールでは、ランキングを基にクリア達成度を算出しているため、実際はクリア済みでもランキング上に存在していなければ未クリア扱いになってしまいます。</li>
</ul>
  </td>
 </tr>
</table>

### プラクティススコア {: #T06PRAC }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T06PRAC[x][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RA]`</dt><dd>霊夢（霊）</dd>
 <dt>`[RB]`</dt><dd>霊夢（夢）</dd>
 <dt>`[MA]`</dt><dd>魔理沙（魔）</dd>
 <dt>`[MB]`</dt><dd>魔理沙（恋）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   ステージ
<dl class="format">
 <dt>`[1～6]`</dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T06PRACEMA1`</dt>
 <dd>Easy 魔理沙（魔）の Stage 1 のプラクティススコア</dd>
 <dt>`%T06PRACNRA4`</dt>
 <dd>Normal 霊夢（霊）の Stage 4 のプラクティススコア</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない難易度とステージの組み合わせ（つまり Easy の Stage 6）は無視されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>このテンプレート書式は本ツール独自のものです。</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## 東方妖々夢用テンプレート書式 {: #Th07Formats }

### スコアランキング {: #T07SCR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T07SCR[w][xx][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
 <dt>`[P]`</dt><dd>Phantasm</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RA]`</dt><dd>霊夢（霊）</dd>
 <dt>`[RB]`</dt><dd>霊夢（夢）</dd>
 <dt>`[MA]`</dt><dd>魔理沙（魔）</dd>
 <dt>`[MB]`</dt><dd>魔理沙（恋）</dd>
 <dt>`[SA]`</dt><dd>咲夜（幻）</dd>
 <dt>`[SB]`</dt><dd>咲夜（時）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   順位
<dl class="format">
 <dt>`[1～9]`</dt><dd>1～9 位</dd>
 <dt>`[0]`</dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>登録名</dd>
 <dt>`[2]`</dt><dd>スコア</dd>
 <dt>`[3]`</dt><dd>到達ステージ</dd>
 <dt>`[4]`</dt><dd>日付</dd>
 <dt>`[5]`</dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T07SCRNSB12`</dt><dd>Normal 咲夜（時）の 1 位のスコア</dd>
 <dt>`%T07SCRXRA44`</dt><dd>Extra 霊夢（霊）の 4 位の日付</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>日付は月日だけが「<samp>mm/dd</samp>」の形式で出力されます。年や時分秒はそもそもスコアファイルに保存されていません。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>スコアの 1 の位には、原作と同様にコンティニュー回数が出力されます。</li>
 <li>処理落ち率は小数点以下第 3 位まで（% 記号付きで）出力されます。今後、この桁数を設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

### 御札戦歴 {: #T07C }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T07C[xxx][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[xxx]`</td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt>`[000]`</dt><dd>全スペルカードの合計値</dd>
 <dt>`[001～141]`</dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RA]`</dt><dd>霊夢（霊）</dd>
 <dt>`[RB]`</dt><dd>霊夢（夢）</dd>
 <dt>`[MA]`</dt><dd>魔理沙（魔）</dd>
 <dt>`[MB]`</dt><dd>魔理沙（恋）</dd>
 <dt>`[SA]`</dt><dd>咲夜（幻）</dd>
 <dt>`[SB]`</dt><dd>咲夜（時）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>MaxBonus</dd>
 <dt>`[2]`</dt><dd>取得回数（勝率の分子）</dd>
 <dt>`[3]`</dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T07C001TL1`</dt>
 <dd>全主人公合計の霜符「フロストコラムス」の MaxBonus</dd>
 <dt>`%T07C002SB3`</dt>
 <dd>咲夜（時）の霜符「フロストコラムス -Lunatic-」の挑戦回数</dd>
</dl>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T07CARD }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T07CARD[xxx][y]`</td>
 </tr>
 <tr>
  <td class="format">`[xxx]`</td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt>`[001～141]`</dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[N]`</dt><dd>スペルカードの名前</dd>
 <dt>`[R]`</dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra, Phantasm)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T07CARD001N`</dt><dd>霜符「フロストコラムス」</dd>
 <dt>`%T07CARD001R`</dt><dd>Hard</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>未挑戦のスペルカードについては、名前・難易度ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T07CRG }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T07CRG[w][xx][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度など
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
 <dt>`[P]`</dt><dd>Phantasm</dd>
 <dt>`[T]`</dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RA]`</dt><dd>霊夢（霊）</dd>
 <dt>`[RB]`</dt><dd>霊夢（夢）</dd>
 <dt>`[MA]`</dt><dd>魔理沙（魔）</dd>
 <dt>`[MB]`</dt><dd>魔理沙（恋）</dd>
 <dt>`[SA]`</dt><dd>咲夜（幻）</dd>
 <dt>`[SB]`</dt><dd>咲夜（時）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   ステージ
<dl class="format">
 <dt>`[0]`</dt><dd>全ステージ合計</dd>
 <dt>`[1～6]`</dt><dd>Stage 1～6</dd>
</dl>
   （Extra, Phantasm ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>取得数（勝率の分子）</dd>
 <dt>`[2]`</dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T07CRGERA01`</dt><dd>Easy 霊夢（霊）の全ステージ合計の取得数</dd>
 <dt>`%T07CRGTSB41`</dt><dd>咲夜（時）の Stage 4 の全難易度合計の取得数</dd>
 <dt>`%T07CRGTTL02`</dt><dd>全難易度・全キャラ・全ステージ合計の挑戦数</dd>
</dl>
  </td>
 </tr>
</table>

### クリア達成度 {: #T07CLEAR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T07CLEAR[x][yy]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
 <dt>`[P]`</dt><dd>Phantasm</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RA]`</dt><dd>霊夢（霊）</dd>
 <dt>`[RB]`</dt><dd>霊夢（夢）</dd>
 <dt>`[MA]`</dt><dd>魔理沙（魔）</dd>
 <dt>`[MB]`</dt><dd>魔理沙（恋）</dd>
 <dt>`[SA]`</dt><dd>咲夜（幻）</dd>
 <dt>`[SB]`</dt><dd>咲夜（時）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T07CLEARXMA`</dt><dd>Extra 魔理沙（魔）のクリア達成度</dd>
 <dt>`%T07CLEARNSB`</dt><dd>Normal 咲夜（時）のクリア達成度</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>
  クリア達成度（ゲームの進行状況）に応じて以下の文字列が出力されます。
  <p class="legends">
   <samp>-------</samp>（未プレイ）, <samp>Stage 1</samp>,
   <samp>Stage 2</samp>, <samp>Stage 3</samp>, <samp>Stage 4</samp>,
   <samp>Stage 5</samp>, <samp>Stage 6</samp>, <samp>All Clear</samp>,
   <samp>Not Clear</samp>（Extra, Phantasm 未クリア）
  </p>
 </li>
 <li>本ツールでは、ランキングを基にクリア達成度を算出しているため、実際はクリア済みでもランキング上に存在していなければ未クリア扱いになってしまいます。</li>
</ul>
  </td>
 </tr>
</table>

### プレイ回数 {: #T07PLAY }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T07PLAY[x][yy]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度など
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
 <dt>`[P]`</dt><dd>Phantasm</dd>
 <dt>`[T]`</dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RA]`</dt><dd>霊夢（霊）</dd>
 <dt>`[RB]`</dt><dd>霊夢（夢）</dd>
 <dt>`[MA]`</dt><dd>魔理沙（魔）</dd>
 <dt>`[MB]`</dt><dd>魔理沙（恋）</dd>
 <dt>`[SA]`</dt><dd>咲夜（幻）</dd>
 <dt>`[SB]`</dt><dd>咲夜（時）</dd>
 <dt>`[CL]`</dt><dd>クリア回数</dd>
 <dt>`[CN]`</dt><dd>コンティニュー回数</dd>
 <dt>`[PR]`</dt><dd>プラクティスプレイ回数</dd>
 <dt>`[RT]`</dt><dd>リトライ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T07PLAYHRB`</dt><dd>Hard 霊夢（夢）のプレイ回数</dd>
 <dt>`%T07PLAYLCL`</dt><dd>Lunatic のクリア回数</dd>
</dl>
  </td>
 </tr>
</table>

### 総起動時間 {: #T07TIMEALL }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td>書式</td>
  <td>`%T07TIMEALL`</td>
 </tr>
 <tr>
  <td>補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「<samp>h:mm:ss.ddd</samp>」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td>
<ul>
 <li>秒とミリ秒の間は「<samp>:</samp>」ではなく「<samp>.</samp>」で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

### 総プレイ時間 {: #T07TIMEPLY }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td>書式</td>
  <td>`%T07TIMEPLY`</td>
 </tr>
 <tr>
  <td>補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「<samp>h:mm:ss.ddd</samp>」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td>
<ul>
 <li>秒とミリ秒の間は「<samp>:</samp>」ではなく「<samp>.</samp>」で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

### プラクティススコア {: #T07PRAC }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T07PRAC[w][xx][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RA]`</dt><dd>霊夢（霊）</dd>
 <dt>`[RB]`</dt><dd>霊夢（夢）</dd>
 <dt>`[MA]`</dt><dd>魔理沙（魔）</dd>
 <dt>`[MB]`</dt><dd>魔理沙（恋）</dd>
 <dt>`[SA]`</dt><dd>咲夜（幻）</dd>
 <dt>`[SB]`</dt><dd>咲夜（時）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   ステージ
<dl class="format">
 <dt>`[1～6]`</dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>スコア</dd>
 <dt>`[2]`</dt><dd>プレイ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T07PRACESB11`</dt>
 <dd>Easy 咲夜（時）の Stage 1 のプラクティススコア</dd>
 <dt>`%T07PRACNRA42`</dt>
 <dd>Normal 霊夢（霊）の Stage 4 のプラクティスプレイ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>このテンプレート書式は本ツール独自のものです。</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## 東方永夜抄用テンプレート書式 {: #Th08Formats }

### スコアランキング {: #T08SCR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T08SCR[w][xx][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[SR]`</dt><dd>咲夜 & レミリア</dd>
 <dt>`[YY]`</dt><dd>妖夢 & 幽々子</dd>
 <dt>`[RM]`</dt><dd>霊夢</dd>
 <dt>`[YK]`</dt><dd>紫</dd>
 <dt>`[MR]`</dt><dd>魔理沙</dd>
 <dt>`[AL]`</dt><dd>アリス</dd>
 <dt>`[SK]`</dt><dd>咲夜</dd>
 <dt>`[RL]`</dt><dd>レミリア</dd>
 <dt>`[YM]`</dt><dd>妖夢</dd>
 <dt>`[YU]`</dt><dd>幽々子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   順位
<dl class="format">
 <dt>`[1～9]`</dt><dd>1～9 位</dd>
 <dt>`[0]`</dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>登録名</dd>
 <dt>`[2]`</dt><dd>スコア</dd>
 <dt>`[3]`</dt><dd>到達ステージ</dd>
 <dt>`[4]`</dt><dd>日付</dd>
 <dt>`[5]`</dt><dd>処理落ち率</dd>
 <dt>`[6]`</dt><dd>プレイ時間</dd>
 <dt>`[7]`</dt><dd>初期プレイヤー数</dd>
 <dt>`[8]`</dt><dd>得点アイテム数</dd>
 <dt>`[9]`</dt><dd>刻符数</dd>
 <dt>`[0]`</dt><dd>ミス回数</dd>
 <dt>`[A]`</dt><dd>ボム回数</dd>
 <dt>`[B]`</dt><dd>ラストスペル回数</dd>
 <dt>`[C]`</dt><dd>ポーズ回数</dd>
 <dt>`[D]`</dt><dd>コンティニュー回数</dd>
 <dt>`[E]`</dt><dd>人間率</dd>
 <dt>`[F]`</dt><dd>取得スペルカード一覧</dd>
 <dt>`[G]`</dt><dd>取得スペルカード枚数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T08SCRNSR12`</dt><dd>Normal 咲夜 & レミリアの 1 位のスコア</dd>
 <dt>`%T08SCRXRM45`</dt><dd>Extra 霊夢の 4 位の処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>日付は月日だけが「<samp>mm/dd</samp>」の形式で出力されます。年や時分秒はそもそもスコアファイルに保存されていません。</li>
 <li>取得スペルカード一覧について、東方永夜抄の score.txt にある「総取得回数/総遭遇回数」は出力されません。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>スコアの 1 の位には、原作と同様にコンティニュー回数が出力されます。</li>
 <li>処理落ち率は小数点以下第 3 位まで（% 記号付きで）出力されます。今後、この桁数を設定可能にするかも知れません。</li>
 <li>
  プレイ時間は時分秒が「<samp>h:mm:ss</samp>」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。
 </li>
 <li>人間率は小数点以下第 2 位まで（% 記号付きで）出力されます。第 3 位以下はスコアファイルに保存されていません。</li>
 <li>本ツールには、[東方メモリマネージャー][ThMM]の <var>GetSpellListTag</var> 相当の設定項目はありません。今後対応するかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

### 御札戦歴 {: #T08C }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T08C[w][xxx][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   戦歴の種類
<dl class="format">
 <dt>`[S]`</dt><dd>ゲーム本編</dd>
 <dt>`[P]`</dt><dd>スペルプラクティス</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xxx]`</td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt>`[000]`</dt><dd>全スペルカードの合計値</dd>
 <dt>`[001～222]`</dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[SR]`</dt><dd>咲夜 & レミリア</dd>
 <dt>`[YY]`</dt><dd>妖夢 & 幽々子</dd>
 <dt>`[RM]`</dt><dd>霊夢</dd>
 <dt>`[YK]`</dt><dd>紫</dd>
 <dt>`[MR]`</dt><dd>魔理沙</dd>
 <dt>`[AL]`</dt><dd>アリス</dd>
 <dt>`[SK]`</dt><dd>咲夜</dd>
 <dt>`[RL]`</dt><dd>レミリア</dd>
 <dt>`[YM]`</dt><dd>妖夢</dd>
 <dt>`[YU]`</dt><dd>幽々子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>MaxBonus</dd>
 <dt>`[2]`</dt><dd>取得回数（勝率の分子）</dd>
 <dt>`[3]`</dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T08CS003TL1`</dt>
 <dd>ゲーム本編 全主人公合計の灯符「ファイヤフライフェノメノン」の MaxBonus</dd>
 <dt>`%T08CP008RY2`</dt>
 <dd>スペルプラクティス 霊夢 & 紫の蠢符「リトルバグストーム」の取得回数</dd>
</dl>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T08CARD }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T08CARD[xxx][y]`</td>
 </tr>
 <tr>
  <td class="format">`[xxx]`</td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt>`[001～222]`</dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[N]`</dt><dd>スペルカードの名前</dd>
 <dt>`[R]`</dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra, Last Word)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T08CARD023N`</dt><dd>鷹符「イルスタードダイブ」</dd>
 <dt>`%T08CARD023R`</dt><dd>Normal</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>ゲーム本編・スペルプラクティスの両方とも未挑戦のスペルカードについては、名前・難易度ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T08CRG }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="6">書式</td>
  <td colspan="2">`%T08CRG[v][w][xx][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[v]`</td>
  <td>
   戦歴の種類
<dl class="format">
 <dt>`[S]`</dt><dd>ゲーム本編</dd>
 <dt>`[P]`</dt><dd>スペルプラクティス</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度など
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
 <dt>`[W]`</dt><dd>Last Word</dd>
 <dt>`[T]`</dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[SR]`</dt><dd>咲夜 & レミリア</dd>
 <dt>`[YY]`</dt><dd>妖夢 & 幽々子</dd>
 <dt>`[RM]`</dt><dd>霊夢</dd>
 <dt>`[YK]`</dt><dd>紫</dd>
 <dt>`[MR]`</dt><dd>魔理沙</dd>
 <dt>`[AL]`</dt><dd>アリス</dd>
 <dt>`[SK]`</dt><dd>咲夜</dd>
 <dt>`[RL]`</dt><dd>レミリア</dd>
 <dt>`[YM]`</dt><dd>妖夢</dd>
 <dt>`[YU]`</dt><dd>幽々子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   ステージ
<dl class="format">
 <dt>`[00]`</dt><dd>全ステージ合計</dd>
 <dt>`[1A]`</dt><dd>Stage 1</dd>
 <dt>`[2A]`</dt><dd>Stage 2</dd>
 <dt>`[3A]`</dt><dd>Stage 3</dd>
 <dt>`[4A]`</dt><dd>Stage 4A</dd>
 <dt>`[4B]`</dt><dd>Stage 4B</dd>
 <dt>`[5A]`</dt><dd>Stage 5</dd>
 <dt>`[6A]`</dt><dd>Stage 6A</dd>
 <dt>`[6B]`</dt><dd>Stage 6B</dd>
</dl>
   （Extra, Last Word ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>取得数（勝率の分子）</dd>
 <dt>`[2]`</dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T08CRGSERY2A1`</dt>
 <dd>ゲーム本編 Easy 霊夢 & 紫の Stage 2 の取得数</dd>
 <dt>`%T08CRGSTYY4A1`</dt>
 <dd>ゲーム本編 妖夢 & 幽々子の Stage 4A の全難易度合計の取得数</dd>
 <dt>`%T08CRGPTTL002`</dt>
 <dd>スペルプラクティス 全難易度・全キャラ・全ステージ合計の挑戦数</dd>
</dl>
  </td>
 </tr>
</table>

### クリア達成度 {: #T08CLEAR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T08CLEAR[x][yy]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[SR]`</dt><dd>咲夜 & レミリア</dd>
 <dt>`[YY]`</dt><dd>妖夢 & 幽々子</dd>
 <dt>`[RM]`</dt><dd>霊夢</dd>
 <dt>`[YK]`</dt><dd>紫</dd>
 <dt>`[MR]`</dt><dd>魔理沙</dd>
 <dt>`[AL]`</dt><dd>アリス</dd>
 <dt>`[SK]`</dt><dd>咲夜</dd>
 <dt>`[RL]`</dt><dd>レミリア</dd>
 <dt>`[YM]`</dt><dd>妖夢</dd>
 <dt>`[YU]`</dt><dd>幽々子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T08CLEARXMA`</dt><dd>Extra 魔理沙 & アリスのクリア達成度</dd>
 <dt>`%T08CLEARNSK`</dt><dd>Normal 咲夜のクリア達成度</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>
  クリア達成度（ゲームの進行状況）に応じて以下の文字列が出力されます。
  <p class="legends">
   <samp>-------</samp>（未プレイ）, <samp>Stage 1</samp>,
   <samp>Stage 2</samp>, <samp>Stage 3</samp>, <samp>Stage 4</samp>,
   <samp>Stage 5</samp>, <samp>Stage 6A</samp>, <samp>FinalA Clear</samp>,
   <samp>All Clear</samp>, <samp>Not Clear</samp>（Extra 未クリア）
  </p>
 </li>
 <li>本ツールでは、ランキングを基にクリア達成度を算出しているため、実際はクリア済みでもランキング上に存在していなければ未クリア扱いになってしまいます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>本ツールの FinalA Clear の判定方法が間違っているかも知れません…。</li>
</ul>
  </td>
 </tr>
</table>

### プレイ回数 {: #T08PLAY }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T08PLAY[x][yy]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度など
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
 <dt>`[T]`</dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[SR]`</dt><dd>咲夜 & レミリア</dd>
 <dt>`[YY]`</dt><dd>妖夢 & 幽々子</dd>
 <dt>`[RM]`</dt><dd>霊夢</dd>
 <dt>`[YK]`</dt><dd>紫</dd>
 <dt>`[MR]`</dt><dd>魔理沙</dd>
 <dt>`[AL]`</dt><dd>アリス</dd>
 <dt>`[SK]`</dt><dd>咲夜</dd>
 <dt>`[RL]`</dt><dd>レミリア</dd>
 <dt>`[YM]`</dt><dd>妖夢</dd>
 <dt>`[YU]`</dt><dd>幽々子</dd>
 <dt>`[CL]`</dt><dd>クリア回数</dd>
 <dt>`[CN]`</dt><dd>コンティニュー回数</dd>
 <dt>`[PR]`</dt><dd>プラクティスプレイ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T08PLAYHYY`</dt><dd>Hard 妖夢 & 幽々子のプレイ回数</dd>
 <dt>`%T08PLAYLCN`</dt><dd>Lunatic のコンティニュー回数</dd>
</dl>
  </td>
 </tr>
</table>

### 総起動時間 {: #T08TIMEALL }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td>書式</td>
  <td>`%T08TIMEALL`</td>
 </tr>
 <tr>
  <td>補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「<samp>h:mm:ss.ddd</samp>」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td>
<ul>
 <li>秒とミリ秒の間は「<samp>:</samp>」ではなく「<samp>.</samp>」で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

### 総プレイ時間 {: #T08TIMEPLY }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td>書式</td>
  <td>`%T08TIMEPLY`</td>
 </tr>
 <tr>
  <td>補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「<samp>h:mm:ss.ddd</samp>」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td>
<ul>
 <li>秒とミリ秒の間は「<samp>:</samp>」ではなく「<samp>.</samp>」で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

### プラクティススコア {: #T08PRAC }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T08PRAC[w][xx][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[SR]`</dt><dd>咲夜 & レミリア</dd>
 <dt>`[YY]`</dt><dd>妖夢 & 幽々子</dd>
 <dt>`[RM]`</dt><dd>霊夢</dd>
 <dt>`[YK]`</dt><dd>紫</dd>
 <dt>`[MR]`</dt><dd>魔理沙</dd>
 <dt>`[AL]`</dt><dd>アリス</dd>
 <dt>`[SK]`</dt><dd>咲夜</dd>
 <dt>`[RL]`</dt><dd>レミリア</dd>
 <dt>`[YM]`</dt><dd>妖夢</dd>
 <dt>`[YU]`</dt><dd>幽々子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   ステージ
<dl class="format">
 <dt>`[1A]`</dt><dd>Stage 1</dd>
 <dt>`[2A]`</dt><dd>Stage 2</dd>
 <dt>`[3A]`</dt><dd>Stage 3</dd>
 <dt>`[4A]`</dt><dd>Stage 4A</dd>
 <dt>`[4B]`</dt><dd>Stage 4B</dd>
 <dt>`[5A]`</dt><dd>Stage 5</dd>
 <dt>`[6A]`</dt><dd>Stage 6A</dd>
 <dt>`[6B]`</dt><dd>Stage 6B</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>スコア</dd>
 <dt>`[2]`</dt><dd>プレイ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T08PRACEYM1A1`</dt>
 <dd>Easy 妖夢の Stage 1 のプラクティススコア</dd>
 <dt>`%T08PRACNRY4B2`</dt>
 <dd>Normal 霊夢 & 紫の Stage 4B のプラクティスプレイ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>このテンプレート書式は本ツール独自のものです。</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## 東方花映塚用テンプレート書式 {: #Th09Formats }

### スコアランキング {: #T09SCR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T09SCR[w][xx][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RM]`</dt><dd>霊夢</dd>
 <dt>`[MR]`</dt><dd>魔理沙</dd>
 <dt>`[SK]`</dt><dd>咲夜</dd>
 <dt>`[YM]`</dt><dd>妖夢</dd>
 <dt>`[RS]`</dt><dd>鈴仙</dd>
 <dt>`[CI]`</dt><dd>チルノ</dd>
 <dt>`[LY]`</dt><dd>リリカ</dd>
 <dt>`[MY]`</dt><dd>ミスティア</dd>
 <dt>`[TW]`</dt><dd>てゐ</dd>
 <dt>`[AY]`</dt><dd>文</dd>
 <dt>`[MD]`</dt><dd>メディスン</dd>
 <dt>`[YU]`</dt><dd>幽香</dd>
 <dt>`[KM]`</dt><dd>小町</dd>
 <dt>`[SI]`</dt><dd>四季映姫</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   順位
<dl class="format">
 <dt>`[1～5]`</dt><dd>1～5 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>登録名</dd>
 <dt>`[2]`</dt><dd>スコア</dd>
 <dt>`[3]`</dt><dd>日付</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T09SCRNMR12`</dt><dd>Normal 魔理沙の 1 位のスコア</dd>
 <dt>`%T09SCRXRM41`</dt><dd>Extra 霊夢の 4 位の登録名</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>日付は年月日が「<samp>yy/mm/dd</samp>」の形式で出力されます。年は西暦の下 2 桁だけがスコアファイルに保存されています。また、時分秒はそもそもスコアファイルに保存されていません。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>スコアの 1 の位には、原作と同様にコンティニュー回数が出力されます。</li>
</ul>
  </td>
 </tr>
</table>

### クリア達成度 {: #T09CLEAR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T09CLEAR[x][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RM]`</dt><dd>霊夢</dd>
 <dt>`[MR]`</dt><dd>魔理沙</dd>
 <dt>`[SK]`</dt><dd>咲夜</dd>
 <dt>`[YM]`</dt><dd>妖夢</dd>
 <dt>`[RS]`</dt><dd>鈴仙</dd>
 <dt>`[CI]`</dt><dd>チルノ</dd>
 <dt>`[LY]`</dt><dd>リリカ</dd>
 <dt>`[MY]`</dt><dd>ミスティア</dd>
 <dt>`[TW]`</dt><dd>てゐ</dd>
 <dt>`[AY]`</dt><dd>文</dd>
 <dt>`[MD]`</dt><dd>メディスン</dd>
 <dt>`[YU]`</dt><dd>幽香</dd>
 <dt>`[KM]`</dt><dd>小町</dd>
 <dt>`[SI]`</dt><dd>四季映姫</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   出力形式
<dl class="format">
 <dt>`[1]`</dt><dd>クリア回数</dd>
 <dt>`[2]`</dt><dd>クリアしたかどうかのフラグ情報</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T09CLEARXMR1`</dt><dd>Extra 魔理沙のクリア回数</dd>
 <dt>`%T09CLEARNSK2`</dt><dd>Normal 咲夜のクリアフラグ</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>
  フラグ情報は、ゲームの進行状況に応じて以下の文字列が出力されます。
  <p class="legends">
   <samp>-------</samp>（未プレイ）, <samp>Not Cleared</samp>,
   <samp>Cleared</samp>
  </p>
 </li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>1 回以上プレイしているが未クリアの場合に「<samp>Not Cleared</samp>」が出力されます。</li>
</ul>
  </td>
 </tr>
</table>

### 総起動時間 {: #T09TIMEALL }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td>書式</td>
  <td>`%T09TIMEALL`</td>
 </tr>
 <tr>
  <td>補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「<samp>h:mm:ss.ddd</samp>」の形式で出力されます。</li>
 <li>スコアファイルには総プレイ時間のようなものも保存されているようですが、確証を持てないので（本ツールでも）出力しません。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td>
<ul>
 <li>秒とミリ秒の間は「<samp>:</samp>」ではなく「<samp>.</samp>」で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## 東方文花帖用テンプレート書式 {: #Th095Formats }

### スコア一覧 {: #T95SCR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T95SCR[x][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   レベル
<dl class="format">
 <dt>`[1～9]`</dt><dd>Level 1～9</dd>
 <dt>`[0]`</dt><dd>Level 10</dd>
 <dt>`[X]`</dt><dd>Level Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   シーン
<dl class="format">
 <dt>`[1～9]`</dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>ハイスコア</dd>
 <dt>`[2]`</dt><dd>登録してあるベストショットのスコア</dd>
 <dt>`[3]`</dt><dd>撮影枚数</dd>
 <dt>`[4]`</dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T95SCR111`</dt><dd>1-1 でのハイスコア</dd>
 <dt>`%T95SCR233`</dt><dd>2-3 での撮影枚数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-9 など）は無視されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>処理落ち率は小数点以下第 3 位まで（% 記号付きで）出力されます。今後、この桁数を設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

### スコア合計 {: #T95SCRTL }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="2">書式</td>
  <td colspan="2">`%T95SCRTL[x]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>撮影総合評価点</dd>
 <dt>`[2]`</dt><dd>登録してあるベストショットのスコアの合計</dd>
 <dt>`[3]`</dt><dd>総撮影枚数</dd>
 <dt>`[4]`</dt><dd>撮影に成功したシーン数の合計</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T95SCRTL1`</dt><dd>撮影総合評価点</dd>
 <dt>`%T95SCRTL3`</dt><dd>総撮影枚数</dd>
</dl>
  </td>
 </tr>
</table>

### 被写体 & スペルカード情報 {: #T95CARD }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T95CARD[x][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   レベル
<dl class="format">
 <dt>`[1～9]`</dt><dd>Level 1～9</dd>
 <dt>`[0]`</dt><dd>Level 10</dd>
 <dt>`[X]`</dt><dd>Level Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   シーン
<dl class="format">
 <dt>`[1～9]`</dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>被写体の名前</dd>
 <dt>`[2]`</dt><dd>スペルカード名</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T95CARD111`</dt><dd>1-1 の被写体の名前</dd>
 <dt>`%T95CARD232`</dt><dd>2-3 のスペルカード名</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-9 など）は無視されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>未挑戦のものについては、被写体の名前・スペルカード名ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

### ベストショット出力 {: #T95SHOT }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T95SHOT[x][y]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   レベル
<dl class="format">
 <dt>`[1～9]`</dt><dd>Level 1～9</dd>
 <dt>`[0]`</dt><dd>Level 10</dd>
 <dt>`[X]`</dt><dd>Level Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   シーン
<dl class="format">
 <dt>`[1～9]`</dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T95SHOT12`</dt><dd>1-2 のベストショット</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-9 など）は無視されます。</li>
 <li>
  このテンプレート書式は「<samp>&lt;img src="./bestshot/bs_01_1.png" alt="～" title="～" border=0&gt;</samp>」のような HTML の IMG タグに置換されます。<br />
  同時に、対象となるベストショットファイル (bs_??_?.dat) を PNG 形式に変換した画像ファイルが出力されます。
 </li>
 <li>IMG タグの alt 属性と title 属性には、ベストショット撮影時のスコアと処理落ち率、及びスペルカード名が出力されます。</li>
 <li>画像ファイルは、「出力先(O):」欄で指定されたフォルダ内の「bestshot」フォルダに出力されます。</li>
 <li>ベストショットファイルが存在しない場合、IMG タグや画像ファイルは出力されません。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>本ツールでは、ベストショットファイルから PNG 形式への変換を自前で行います。そのため Susie プラグインは不要です。</li>
 <li>自前で変換する都合上、東方文花帖 ver. 1.02a 以外で撮影されたベストショットファイルの変換には非対応です。対応予定も今のところありません。</li>
 <li>ベストショットファイルの変換は、このテンプレート書式がテンプレートファイル内に無くても実行されます。</li>
 <li>本ツールには、[東方メモリマネージャー][ThMM]の <var>ImgPath</var> 相当の設定項目はありません。つまり画像ファイルの出力先フォルダの変更はできません。今後対応するかも知れません。</li>
 <li>画像ファイルの出力先フォルダが存在しない場合、本ツールが自動で作成します。</li>
</ul>
  </td>
 </tr>
</table>

### ベストショット出力（詳細版） {: #T95SHOTEX }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T95SHOTEX[x][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   レベル
<dl class="format">
 <dt>`[1～9]`</dt><dd>Level 1～9</dd>
 <dt>`[0]`</dt><dd>Level 10</dd>
 <dt>`[X]`</dt><dd>Level Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   シーン
<dl class="format">
 <dt>`[1～9]`</dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>画像ファイルへの相対パス</dd>
 <dt>`[2]`</dt><dd>画像ファイルの幅 (px)</dd>
 <dt>`[3]`</dt><dd>画像ファイルの高さ (px)</dd>
 <dt>`[4]`</dt><dd>ベストショット撮影時のスコア</dd>
 <dt>`[5]`</dt><dd>ベストショット撮影時の処理落ち率</dd>
 <dt>`[6]`</dt><dd>ベストショット撮影日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T95SHOTEX121`</dt><dd>1-2 の画像ファイルへの相対パス</dd>
 <dt>`%T95SHOTEX236`</dt><dd>2-3 のベストショット撮影日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-9 など）は無視されます。</li>
 <li>
  このテンプレート書式を使って、例えば `%T95SHOT12` と同等の出力結果を得るには、テンプレートファイルに以下の通りに記載します。
<pre>`&lt;img src="%T95SHOTEX121" alt="Score: %T95SHOTEX124
Slow: %T95SHOTEX125
SpellName: %T95CARD122" title="Score: %T95SHOTEX124
Slow: %T95SHOTEX125
SpellName: %T95CARD122" border=0&gt;
`</pre>
 </li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>
  このテンプレート書式は本ツール独自のものです。<br />
  「[ベストショット出力](#T95SHOT)」により出力される IMG タグが気に食わなかったから、この書式を新規追加し、かつベストショットファイルの変換を自前で実装したようなものです。
 </li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## 東方風神録用テンプレート書式 {: #Th10Formats }

### スコアランキング {: #T10SCR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T10SCR[w][xx][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RA]`</dt><dd>霊夢 (A)</dd>
 <dt>`[RB]`</dt><dd>霊夢 (B)</dd>
 <dt>`[RC]`</dt><dd>霊夢 (C)</dd>
 <dt>`[MA]`</dt><dd>魔理沙 (A)</dd>
 <dt>`[MB]`</dt><dd>魔理沙 (B)</dd>
 <dt>`[MC]`</dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   順位
<dl class="format">
 <dt>`[1～9]`</dt><dd>1～9 位</dd>
 <dt>`[0]`</dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>登録名</dd>
 <dt>`[2]`</dt><dd>スコア</dd>
 <dt>`[3]`</dt><dd>到達ステージ</dd>
 <dt>`[4]`</dt><dd>日時</dd>
 <dt>`[5]`</dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T10SCRNMC12`</dt><dd>Normal 魔理沙 (C) の 1 位のスコア</dd>
 <dt>`%T10SCRXRA44`</dt><dd>Extra 霊夢 (A) の 4 位の日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>スコアの 1 の位には、原作と同様にコンティニュー回数が出力されます。</li>
 <li>日時は年月日及び時分秒が「<samp>yyyy/mm/dd hh:mm:ss</samp>」の形式で出力されます。</li>
 <li>処理落ち率は小数点以下第 3 位まで（% 記号付きで）出力されます。今後、この桁数を設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

### 御札戦歴 {: #T10C }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T10C[xxx][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[xxx]`</td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt>`[000]`</dt><dd>全スペルカードの合計値</dd>
 <dt>`[001～110]`</dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RA]`</dt><dd>霊夢 (A)</dd>
 <dt>`[RB]`</dt><dd>霊夢 (B)</dd>
 <dt>`[RC]`</dt><dd>霊夢 (C)</dd>
 <dt>`[MA]`</dt><dd>魔理沙 (A)</dd>
 <dt>`[MB]`</dt><dd>魔理沙 (B)</dd>
 <dt>`[MC]`</dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>取得回数（勝率の分子）</dd>
 <dt>`[2]`</dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T10C003TL1`</dt><dd>全主人公合計の秋符「オータムスカイ」の取得回数</dd>
 <dt>`%T10C003MC2`</dt><dd>魔理沙 (C) の秋符「オータムスカイ」の挑戦回数</dd>
</dl>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T10CARD }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T10CARD[xxx][y]`</td>
 </tr>
 <tr>
  <td class="format">`[xxx]`</td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt>`[001～110]`</dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[N]`</dt><dd>スペルカードの名前</dd>
 <dt>`[R]`</dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T10CARD003N`</dt><dd>秋符「オータムスカイ」</dd>
 <dt>`%T10CARD003R`</dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>未挑戦のスペルカードの名前は「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
 <li>一方、スペルカードの難易度は、未挑戦かどうかにかかわらず常に出力されます。原作でも Result 画面を見れば難易度はバレるので、このような仕様にしています。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T10CRG }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T10CRG[w][xx][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度など
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
 <dt>`[T]`</dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RA]`</dt><dd>霊夢 (A)</dd>
 <dt>`[RB]`</dt><dd>霊夢 (B)</dd>
 <dt>`[RC]`</dt><dd>霊夢 (C)</dd>
 <dt>`[MA]`</dt><dd>魔理沙 (A)</dd>
 <dt>`[MB]`</dt><dd>魔理沙 (B)</dd>
 <dt>`[MC]`</dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   ステージ
<dl class="format">
 <dt>`[0]`</dt><dd>全ステージ合計</dd>
 <dt>`[1～6]`</dt><dd>Stage 1～6</dd>
</dl>
   （Extra ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>取得数（勝率の分子）</dd>
 <dt>`[2]`</dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T10CRGERA01`</dt><dd>Easy 霊夢 (A) の全ステージ合計の取得数</dd>
 <dt>`%T10CRGTMC41`</dt><dd>魔理沙 (C) の Stage 4 の全難易度合計の取得数</dd>
 <dt>`%T10CRGTTL02`</dt><dd>全難易度・全キャラ・全ステージ合計の挑戦数</dd>
</dl>
  </td>
 </tr>
</table>

### クリア達成度 {: #T10CLEAR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T10CLEAR[x][yy]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RA]`</dt><dd>霊夢 (A)</dd>
 <dt>`[RB]`</dt><dd>霊夢 (B)</dd>
 <dt>`[RC]`</dt><dd>霊夢 (C)</dd>
 <dt>`[MA]`</dt><dd>魔理沙 (A)</dd>
 <dt>`[MB]`</dt><dd>魔理沙 (B)</dd>
 <dt>`[MC]`</dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T10CLEARXMA`</dt><dd>Extra 魔理沙 (A) のクリア達成度</dd>
 <dt>`%T10CLEARNRB`</dt><dd>Normal 霊夢 (B) のクリア達成度</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>
  クリア達成度（ゲームの進行状況）に応じて以下の文字列が出力されます。
  <p class="legends">
   <samp>-------</samp>（未プレイ）, <samp>Stage 1</samp>,
   <samp>Stage 2</samp>, <samp>Stage 3</samp>, <samp>Stage 4</samp>,
   <samp>Stage 5</samp>, <samp>Stage 6</samp>, <samp>All Clear</samp>,
   <samp>Not Clear</samp>（Extra 未クリア）
  </p>
 </li>
 <li>本ツールでは、ランキングを基にクリア達成度を算出しているため、実際はクリア済みでもランキング上に存在していなければ未クリア扱いになってしまいます。</li>
</ul>
  </td>
 </tr>
</table>

### キャラごとの個別データ {: #T10CHARA }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T10CHARA[xx][y]`</td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RA]`</dt><dd>霊夢 (A)</dd>
 <dt>`[RB]`</dt><dd>霊夢 (B)</dd>
 <dt>`[RC]`</dt><dd>霊夢 (C)</dd>
 <dt>`[MA]`</dt><dd>魔理沙 (A)</dd>
 <dt>`[MB]`</dt><dd>魔理沙 (B)</dd>
 <dt>`[MC]`</dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>総プレイ回数</dd>
 <dt>`[2]`</dt><dd>プレイ時間</dd>
 <dt>`[3]`</dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T10CHARATL2`</dt><dd>全主人公合計のプレイ時間</dd>
 <dt>`%T10CHARARA1`</dt><dd>霊夢 (A) の総プレイ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>
  プレイ時間は時分秒が「<samp>h:mm:ss</samp>」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。
 </li>
</ul>
  </td>
 </tr>
</table>

### キャラごとの個別データ（詳細版） {: #T10CHARAEX }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T10CHARAEX[x][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
   （総プレイ回数とプレイ時間ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RA]`</dt><dd>霊夢 (A)</dd>
 <dt>`[RB]`</dt><dd>霊夢 (B)</dd>
 <dt>`[RC]`</dt><dd>霊夢 (C)</dd>
 <dt>`[MA]`</dt><dd>魔理沙 (A)</dd>
 <dt>`[MB]`</dt><dd>魔理沙 (B)</dd>
 <dt>`[MC]`</dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>総プレイ回数</dd>
 <dt>`[2]`</dt><dd>プレイ時間</dd>
 <dt>`[3]`</dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T10CHARAEXETL2`</dt><dd>全主人公合計のプレイ時間</dd>
 <dt>`%T10CHARAEXERA1`</dt><dd>霊夢 (A) の総プレイ回数</dd>
 <dt>`%T10CHARAEXNMC3`</dt><dd>Normal 魔理沙 (C) のクリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>
  プレイ時間は時分秒が「<samp>h:mm:ss</samp>」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。
 </li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>このテンプレート書式は本ツール独自のものです。</li>
</ul>
  </td>
 </tr>
</table>

### プラクティススコア {: #T10PRAC }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T10PRAC[x][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RA]`</dt><dd>霊夢 (A)</dd>
 <dt>`[RB]`</dt><dd>霊夢 (B)</dd>
 <dt>`[RC]`</dt><dd>霊夢 (C)</dd>
 <dt>`[MA]`</dt><dd>魔理沙 (A)</dd>
 <dt>`[MB]`</dt><dd>魔理沙 (B)</dd>
 <dt>`[MC]`</dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   ステージ
<dl class="format">
 <dt>`[1～6]`</dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T10PRACEMC1`</dt><dd>Easy 魔理沙 (C) の Stage 1 のプラクティススコア</dd>
 <dt>`%T10PRACNRA4`</dt><dd>Normal 霊夢 (A) の Stage 4 のプラクティススコア</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li>このテンプレート書式は本ツール独自のものです。</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## 東方地霊殿用テンプレート書式 {: #Th11Formats }

### スコアランキング {: #T11SCR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T11SCR[w][xx][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[RS]`</dt><dd>霊夢 & 萃香</dd>
 <dt>`[RA]`</dt><dd>霊夢 & 文</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[MP]`</dt><dd>魔理沙 & パチュリー</dd>
 <dt>`[MN]`</dt><dd>魔理沙 & にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   順位
<dl class="format">
 <dt>`[1～9]`</dt><dd>1～9 位</dd>
 <dt>`[0]`</dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>登録名</dd>
 <dt>`[2]`</dt><dd>スコア</dd>
 <dt>`[3]`</dt><dd>到達ステージ</dd>
 <dt>`[4]`</dt><dd>日時</dd>
 <dt>`[5]`</dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T11SCRNMN12`</dt><dd>Normal 魔理沙 & にとりの 1 位のスコア</dd>
 <dt>`%T11SCRXRY44`</dt><dd>Extra 霊夢 & 紫の 4 位の日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>スコアの 1 の位には、原作と同様にコンティニュー回数が出力されます。</li>
 <li>日時は年月日及び時分秒が「<samp>yyyy/mm/dd hh:mm:ss</samp>」の形式で出力されます。</li>
 <li>処理落ち率は小数点以下第 3 位まで（% 記号付きで）出力されます。今後、この桁数を設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

### 御札戦歴 {: #T11C }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T11C[xxx][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[xxx]`</td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt>`[000]`</dt><dd>全スペルカードの合計値</dd>
 <dt>`[001～175]`</dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[RS]`</dt><dd>霊夢 & 萃香</dd>
 <dt>`[RA]`</dt><dd>霊夢 & 文</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[MP]`</dt><dd>魔理沙 & パチュリー</dd>
 <dt>`[MN]`</dt><dd>魔理沙 & にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>取得回数（勝率の分子）</dd>
 <dt>`[2]`</dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T11C003TL1`</dt>
 <dd>全主人公合計の罠符「キャプチャーウェブ」(Easy) の取得回数</dd>
 <dt>`%T11C003MN2`</dt>
 <dd>魔理沙 & にとりの罠符「キャプチャーウェブ」(Easy) の挑戦回数</dd>
</dl>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T11CARD }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T11CARD[xxx][y]`</td>
 </tr>
 <tr>
  <td class="format">`[xxx]`</td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt>`[001～175]`</dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[N]`</dt><dd>スペルカードの名前</dd>
 <dt>`[R]`</dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T11CARD003N`</dt><dd>罠符「キャプチャーウェブ」</dd>
 <dt>`%T11CARD003R`</dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>未挑戦のスペルカードの名前は「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）今後、このように隠すかどうかを設定可能にするかも知れません。</li>
 <li>一方、スペルカードの難易度は、未挑戦かどうかにかかわらず常に出力されます。原作でも Result 画面を見れば難易度はバレるので、このような仕様にしています。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T11CRG }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2">`%T11CRG[w][xx][y][z]`</td>
 </tr>
 <tr>
  <td class="format">`[w]`</td>
  <td>
   難易度など
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
 <dt>`[T]`</dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[RS]`</dt><dd>霊夢 & 萃香</dd>
 <dt>`[RA]`</dt><dd>霊夢 & 文</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[MP]`</dt><dd>魔理沙 & パチュリー</dd>
 <dt>`[MN]`</dt><dd>魔理沙 & にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   ステージ
<dl class="format">
 <dt>`[0]`</dt><dd>全ステージ合計</dd>
 <dt>`[1～6]`</dt><dd>Stage 1～6</dd>
</dl>
   （Extra ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>取得数（勝率の分子）</dd>
 <dt>`[2]`</dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T11CRGERY01`</dt>
 <dd>Easy 霊夢 & 紫の全ステージ合計の取得数</dd>
 <dt>`%T11CRGTMN41`</dt>
 <dd>魔理沙 & にとりの Stage 4 の全難易度合計の取得数</dd>
 <dt>`%T11CRGTTL02`</dt>
 <dd>全難易度・全キャラ・全ステージ合計の挑戦数</dd>
</dl>
  </td>
 </tr>
</table>

### クリア達成度 {: #T11CLEAR }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T11CLEAR[x][yy]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[RS]`</dt><dd>霊夢 & 萃香</dd>
 <dt>`[RA]`</dt><dd>霊夢 & 文</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[MP]`</dt><dd>魔理沙 & パチュリー</dd>
 <dt>`[MN]`</dt><dd>魔理沙 & にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T11CLEARXMA`</dt><dd>Extra 魔理沙 & アリスのクリア達成度</dd>
 <dt>`%T11CLEARNRS`</dt><dd>Normal 霊夢 & 萃香のクリア達成度</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>
  クリア達成度（ゲームの進行状況）に応じて以下の文字列が出力されます。
  <p class="legends">
   <samp>-------</samp>（未プレイ）, <samp>Stage 1</samp>,
   <samp>Stage 2</samp>, <samp>Stage 3</samp>, <samp>Stage 4</samp>,
   <samp>Stage 5</samp>, <samp>Stage 6</samp>, <samp>All Clear</samp>,
   <samp>Not Clear</samp>（Extra 未クリア）
  </p>
 </li>
 <li>本ツールでは、ランキングを基にクリア達成度を算出しているため、実際はクリア済みでもランキング上に存在していなければ未クリア扱いになってしまいます。</li>
</ul>
  </td>
 </tr>
</table>

### キャラごとの個別データ {: #T11CHARA }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2">`%T11CHARA[xx][y]`</td>
 </tr>
 <tr>
  <td class="format">`[xx]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[RS]`</dt><dd>霊夢 & 萃香</dd>
 <dt>`[RA]`</dt><dd>霊夢 & 文</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[MP]`</dt><dd>魔理沙 & パチュリー</dd>
 <dt>`[MN]`</dt><dd>魔理沙 & にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[y]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>総プレイ回数</dd>
 <dt>`[2]`</dt><dd>プレイ時間</dd>
 <dt>`[3]`</dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T11CHARATL2`</dt><dd>全主人公合計のプレイ時間</dd>
 <dt>`%T11CHARARY1`</dt><dd>霊夢 & 紫の総プレイ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>
  プレイ時間は時分秒が「<samp>h:mm:ss</samp>」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。
 </li>
</ul>
  </td>
 </tr>
</table>

### キャラごとの個別データ（詳細版） {: #T11CHARAEX }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T11CHARAEX[x][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
 <dt>`[X]`</dt><dd>Extra</dd>
</dl>
   （総プレイ回数とプレイ時間ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前など
<dl class="format">
 <dt>`[TL]`</dt><dd>全主人公合計</dd>
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[RS]`</dt><dd>霊夢 & 萃香</dd>
 <dt>`[RA]`</dt><dd>霊夢 & 文</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[MP]`</dt><dd>魔理沙 & パチュリー</dd>
 <dt>`[MN]`</dt><dd>魔理沙 & にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   項目
<dl class="format">
 <dt>`[1]`</dt><dd>総プレイ回数</dd>
 <dt>`[2]`</dt><dd>プレイ時間</dd>
 <dt>`[3]`</dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T11CHARAEXETL2`</dt><dd>全主人公合計のプレイ時間</dd>
 <dt>`%T11CHARAEXERY1`</dt><dd>霊夢 & 紫の総プレイ回数</dd>
 <dt>`%T11CHARAEXNMN3`</dt><dd>Normal 魔理沙 & にとりのクリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>
  プレイ時間は時分秒が「<samp>h:mm:ss</samp>」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。
 </li>
</ul>
  </td>
 </tr>
</table>

### プラクティススコア {: #T11PRAC }
<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2">`%T11PRAC[x][yy][z]`</td>
 </tr>
 <tr>
  <td class="format">`[x]`</td>
  <td>
   難易度
<dl class="format">
 <dt>`[E]`</dt><dd>Easy</dd>
 <dt>`[N]`</dt><dd>Normal</dd>
 <dt>`[H]`</dt><dd>Hard</dd>
 <dt>`[L]`</dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[yy]`</td>
  <td>
   キャラの名前
<dl class="format">
 <dt>`[RY]`</dt><dd>霊夢 & 紫</dd>
 <dt>`[RS]`</dt><dd>霊夢 & 萃香</dd>
 <dt>`[RA]`</dt><dd>霊夢 & 文</dd>
 <dt>`[MA]`</dt><dd>魔理沙 & アリス</dd>
 <dt>`[MP]`</dt><dd>魔理沙 & パチュリー</dd>
 <dt>`[MN]`</dt><dd>魔理沙 & にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format">`[z]`</td>
  <td>
   ステージ
<dl class="format">
 <dt>`[1～6]`</dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt>`%T11PRACEMN1`</dt>
 <dd>Easy 魔理沙 & にとりの Stage 1 のプラクティススコア</dd>
 <dt>`%T11PRACNRY4`</dt>
 <dd>Normal 霊夢 & 紫の Stage 4 のプラクティススコア</dd>
</dl>
  </td>
 </tr>
</table>
