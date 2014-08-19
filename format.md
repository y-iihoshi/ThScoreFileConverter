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

* 「<code>%</code>」から始まる半角英数字の文字列です。
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
  <td colspan="2"><code>%T06SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（恋）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9 位</dd>
 <dt><code>[0]</code></dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>登録名</dd>
 <dt><code>[2]</code></dt><dd>スコア</dd>
 <dt><code>[3]</code></dt><dd>到達ステージ</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T06SCRNMB12</code></dt><dd>Normal 魔理沙（恋）の 1 位のスコア</dd>
 <dt><code>%T06SCRXRA41</code></dt><dd>Extra 霊夢（霊）の 4 位の登録名</dd>
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
  <td colspan="2"><code>%T06C[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[00]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[01～64]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T06C011</code></dt><dd>月符「ムーンライトレイ」の取得回数</dd>
 <dt><code>%T06C022</code></dt><dd>夜符「ナイトバード」の挑戦回数</dd>
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
  <td colspan="2"><code>%T06CARD[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[01～64]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T06CARD01N</code></dt><dd>月符「ムーンライトレイ」</dd>
 <dt><code>%T06CARD01R</code></dt><dd>Hard, Lunatic</dd>
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
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前・難易度ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
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
  <td colspan="2"><code>%T06CRG[x][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[0]</code></dt><dd>全ステージ合計</dd>
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T06CRG01</code></dt><dd>全ステージ合計の取得数</dd>
 <dt><code>%T06CRG12</code></dt><dd>Stage 1 の挑戦数</dd>
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
  <td colspan="2"><code>%T06CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（恋）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T06CLEARXMA</code></dt><dd>Extra 魔理沙（魔）のクリア達成度</dd>
 <dt><code>%T06CLEARNRA</code></dt><dd>Normal 霊夢（霊）のクリア達成度</dd>
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
  <td colspan="2"><code>%T06PRAC[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（恋）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T06PRACEMA1</code></dt>
 <dd>Easy 魔理沙（魔）の Stage 1 のプラクティススコア</dd>
 <dt><code>%T06PRACNRA4</code></dt>
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
  <td colspan="2"><code>%T07SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[P]</code></dt><dd>Phantasm</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[SA]</code></dt><dd>咲夜（幻）</dd>
 <dt><code>[SB]</code></dt><dd>咲夜（時）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9 位</dd>
 <dt><code>[0]</code></dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>登録名</dd>
 <dt><code>[2]</code></dt><dd>スコア</dd>
 <dt><code>[3]</code></dt><dd>到達ステージ</dd>
 <dt><code>[4]</code></dt><dd>日付</dd>
 <dt><code>[5]</code></dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T07SCRNSB12</code></dt><dd>Normal 咲夜（時）の 1 位のスコア</dd>
 <dt><code>%T07SCRXRA44</code></dt><dd>Extra 霊夢（霊）の 4 位の日付</dd>
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
  <td colspan="2"><code>%T07C[xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[000]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[001～141]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[SA]</code></dt><dd>咲夜（幻）</dd>
 <dt><code>[SB]</code></dt><dd>咲夜（時）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>MaxBonus</dd>
 <dt><code>[2]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[3]</code></dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T07C001TL1</code></dt>
 <dd>全主人公合計の霜符「フロストコラムス」の MaxBonus</dd>
 <dt><code>%T07C002SB3</code></dt>
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
  <td colspan="2"><code>%T07CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[001～141]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra, Phantasm)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T07CARD001N</code></dt><dd>霜符「フロストコラムス」</dd>
 <dt><code>%T07CARD001R</code></dt><dd>Hard</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前・難易度ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
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
  <td colspan="2"><code>%T07CRG[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[P]</code></dt><dd>Phantasm</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[SA]</code></dt><dd>咲夜（幻）</dd>
 <dt><code>[SB]</code></dt><dd>咲夜（時）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[0]</code></dt><dd>全ステージ合計</dd>
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
   （Extra, Phantasm ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T07CRGERA01</code></dt><dd>Easy 霊夢（霊）の全ステージ合計の取得数</dd>
 <dt><code>%T07CRGTSB41</code></dt><dd>咲夜（時）の Stage 4 の全難易度合計の取得数</dd>
 <dt><code>%T07CRGTTL02</code></dt><dd>全難易度・全キャラ・全ステージ合計の挑戦数</dd>
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
  <td colspan="2"><code>%T07CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[P]</code></dt><dd>Phantasm</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[SA]</code></dt><dd>咲夜（幻）</dd>
 <dt><code>[SB]</code></dt><dd>咲夜（時）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T07CLEARXMA</code></dt><dd>Extra 魔理沙（魔）のクリア達成度</dd>
 <dt><code>%T07CLEARNSB</code></dt><dd>Normal 咲夜（時）のクリア達成度</dd>
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
  <td colspan="2"><code>%T07PLAY[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[P]</code></dt><dd>Phantasm</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[SA]</code></dt><dd>咲夜（幻）</dd>
 <dt><code>[SB]</code></dt><dd>咲夜（時）</dd>
 <dt><code>[CL]</code></dt><dd>クリア回数</dd>
 <dt><code>[CN]</code></dt><dd>コンティニュー回数</dd>
 <dt><code>[PR]</code></dt><dd>プラクティスプレイ回数</dd>
 <dt><code>[RT]</code></dt><dd>リトライ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T07PLAYHRB</code></dt><dd>Hard 霊夢（夢）のプレイ回数</dd>
 <dt><code>%T07PLAYLCL</code></dt><dd>Lunatic のクリア回数</dd>
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
  <td><code>%T07TIMEALL</code></td>
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
  <td><code>%T07TIMEPLY</code></td>
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
  <td colspan="2"><code>%T07PRAC[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[SA]</code></dt><dd>咲夜（幻）</dd>
 <dt><code>[SB]</code></dt><dd>咲夜（時）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>スコア</dd>
 <dt><code>[2]</code></dt><dd>プレイ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T07PRACESB11</code></dt>
 <dd>Easy 咲夜（時）の Stage 1 のプラクティススコア</dd>
 <dt><code>%T07PRACNRA42</code></dt>
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

## 東方萃夢想用テンプレート書式 {: #Th075Formats }

### スコアランキング {: #T75SCR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T75SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[PC]</code></dt><dd>パチュリー</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[SU]</code></dt><dd>萃香</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9 位</dd>
 <dt><code>[0]</code></dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>スコア</dd>
 <dt><code>[2]</code></dt><dd>名前</dd>
 <dt><code>[3]</code></dt><dd>日付</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T75SCRNSK11</code></dt><dd>Normal 咲夜の 1 位のスコア</dd>
 <dt><code>%T75SCRLRM43</code></dt><dd>Lunatic 霊夢の 4 位の日付</dd>
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
</table>

### 御札戦歴 {: #T75C }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T75C[xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[000]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[001～100]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[PC]</code></dt><dd>パチュリー</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[SU]</code></dt><dd>萃香</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>MaxBonus</dd>
 <dt><code>[2]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[3]</code></dt><dd>挑戦回数（勝率の分母）</dd>
 <dt><code>[4]</code></dt><dd>★</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T75C003RM1</code></dt>
 <dd>霊夢の符の壱「スターダストレヴァリエ」-Hard- の MaxBonus</dd>
 <dt><code>%T75C008MR2</code></dt>
 <dd>魔理沙の符の弐「ドールクルセイダー」-Lunatic- の取得回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>キャラが異なると、スペルカードの番号が同じでもその中身は異なります。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T75CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T75CARD[xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[001～100]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[PC]</code></dt><dd>パチュリー</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[SU]</code></dt><dd>萃香</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T75CARD001SKN</code></dt><dd>符の壱「夢想妙珠連」-Easy-</dd>
 <dt><code>%T75CARD001SKR</code></dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>キャラが異なると、スペルカードの番号が同じでもその中身は異なります。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前・難易度ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T75CRG }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T75CRG[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[PC]</code></dt><dd>パチュリー</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[SU]</code></dt><dd>萃香</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
 <dt><code>[3]</code></dt><dd>真に取得した枚数（★の数）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T75CRGEYK1</code></dt><dd>Easy 紫の取得数</dd>
 <dt><code>%T75CRGHRL2</code></dt><dd>Hard レミリアの挑戦数</dd>
</dl>
  </td>
 </tr>
</table>

### キャラクター戦歴 {: #T75CHR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T75CHR[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[PC]</code></dt><dd>パチュリー</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[SU]</code></dt><dd>萃香</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>使用回数</dd>
 <dt><code>[2]</code></dt><dd>クリア回数</dd>
 <dt><code>[3]</code></dt><dd>最大連続技数</dd>
 <dt><code>[4]</code></dt><dd>最大ダメージ</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T75CHREYK1</code></dt><dd>Easy 紫の使用回数</dd>
 <dt><code>%T75CHRHRL2</code></dt><dd>Hard レミリアのクリア回数</dd>
</dl>
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
  <td colspan="2"><code>%T08SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[SR]</code></dt><dd>咲夜 &amp; レミリア</dd>
 <dt><code>[YY]</code></dt><dd>妖夢 &amp; 幽々子</dd>
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9 位</dd>
 <dt><code>[0]</code></dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>登録名</dd>
 <dt><code>[2]</code></dt><dd>スコア</dd>
 <dt><code>[3]</code></dt><dd>到達ステージ</dd>
 <dt><code>[4]</code></dt><dd>日付</dd>
 <dt><code>[5]</code></dt><dd>処理落ち率</dd>
 <dt><code>[6]</code></dt><dd>プレイ時間</dd>
 <dt><code>[7]</code></dt><dd>初期プレイヤー数</dd>
 <dt><code>[8]</code></dt><dd>得点アイテム数</dd>
 <dt><code>[9]</code></dt><dd>刻符数</dd>
 <dt><code>[0]</code></dt><dd>ミス回数</dd>
 <dt><code>[A]</code></dt><dd>ボム回数</dd>
 <dt><code>[B]</code></dt><dd>ラストスペル回数</dd>
 <dt><code>[C]</code></dt><dd>ポーズ回数</dd>
 <dt><code>[D]</code></dt><dd>コンティニュー回数</dd>
 <dt><code>[E]</code></dt><dd>人間率</dd>
 <dt><code>[F]</code></dt><dd>取得スペルカード一覧</dd>
 <dt><code>[G]</code></dt><dd>取得スペルカード枚数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T08SCRNSR12</code></dt><dd>Normal 咲夜 &amp; レミリアの 1 位のスコア</dd>
 <dt><code>%T08SCRXRM45</code></dt><dd>Extra 霊夢の 4 位の処理落ち率</dd>
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
 <li>本ツールには、<a href="http://www.sue445.net/downloads/ThMemoryManager.html">東方メモリマネージャー</a>の <var>GetSpellListTag</var> 相当の設定項目はありません。今後対応するかも知れません。</li>
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
  <td colspan="2"><code>%T08C[w][xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   戦歴の種類
<dl class="format">
 <dt><code>[S]</code></dt><dd>ゲーム本編</dd>
 <dt><code>[P]</code></dt><dd>スペルプラクティス</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[000]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[001～222]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[SR]</code></dt><dd>咲夜 &amp; レミリア</dd>
 <dt><code>[YY]</code></dt><dd>妖夢 &amp; 幽々子</dd>
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>MaxBonus</dd>
 <dt><code>[2]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[3]</code></dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T08CS003TL1</code></dt>
 <dd>ゲーム本編 全主人公合計の灯符「ファイヤフライフェノメノン」の MaxBonus</dd>
 <dt><code>%T08CP008RY2</code></dt>
 <dd>スペルプラクティス 霊夢 &amp; 紫の蠢符「リトルバグストーム」の取得回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない組み合わせ（つまりゲーム本編の No.206～222）は無視されます。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T08CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T08CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[001～222]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra, Last Word)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T08CARD023N</code></dt><dd>鷹符「イルスタードダイブ」</dd>
 <dt><code>%T08CARD023R</code></dt><dd>Normal</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前・難易度ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
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
  <td colspan="2"><code>%T08CRG[v][w][xx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[v]</code></td>
  <td>
   戦歴の種類
<dl class="format">
 <dt><code>[S]</code></dt><dd>ゲーム本編</dd>
 <dt><code>[P]</code></dt><dd>スペルプラクティス</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[W]</code></dt><dd>Last Word</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[SR]</code></dt><dd>咲夜 &amp; レミリア</dd>
 <dt><code>[YY]</code></dt><dd>妖夢 &amp; 幽々子</dd>
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[00]</code></dt><dd>全ステージ合計</dd>
 <dt><code>[1A]</code></dt><dd>Stage 1</dd>
 <dt><code>[2A]</code></dt><dd>Stage 2</dd>
 <dt><code>[3A]</code></dt><dd>Stage 3</dd>
 <dt><code>[4A]</code></dt><dd>Stage 4A</dd>
 <dt><code>[4B]</code></dt><dd>Stage 4B</dd>
 <dt><code>[5A]</code></dt><dd>Stage 5</dd>
 <dt><code>[6A]</code></dt><dd>Stage 6A</dd>
 <dt><code>[6B]</code></dt><dd>Stage 6B</dd>
</dl>
   （Extra, Last Word ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T08CRGSERY2A1</code></dt>
 <dd>ゲーム本編 Easy 霊夢 &amp; 紫の Stage 2 の取得数</dd>
 <dt><code>%T08CRGSTYY4A1</code></dt>
 <dd>ゲーム本編 妖夢 &amp; 幽々子の Stage 4A の全難易度合計の取得数</dd>
 <dt><code>%T08CRGPTTL002</code></dt>
 <dd>スペルプラクティス 全難易度・全キャラ・全ステージ合計の挑戦数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない組み合わせ（つまりゲーム本編の Last Word）は無視されます。</li>
</ul>
  </td>
 </tr>
</table>

### クリア達成度 {: #T08CLEAR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T08CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[SR]</code></dt><dd>咲夜 &amp; レミリア</dd>
 <dt><code>[YY]</code></dt><dd>妖夢 &amp; 幽々子</dd>
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T08CLEARXMA</code></dt><dd>Extra 魔理沙 &amp; アリスのクリア達成度</dd>
 <dt><code>%T08CLEARNSK</code></dt><dd>Normal 咲夜のクリア達成度</dd>
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
 <li>スコアファイル内に、クリア達成度を示すフラグのようなものが保存されているため、<samp>FinalA Clear</samp> の判定に使用しています。</li>
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
  <td colspan="2"><code>%T08PLAY[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[SR]</code></dt><dd>咲夜 &amp; レミリア</dd>
 <dt><code>[YY]</code></dt><dd>妖夢 &amp; 幽々子</dd>
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[CL]</code></dt><dd>クリア回数</dd>
 <dt><code>[CN]</code></dt><dd>コンティニュー回数</dd>
 <dt><code>[PR]</code></dt><dd>プラクティスプレイ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T08PLAYHYY</code></dt><dd>Hard 妖夢 &amp; 幽々子のプレイ回数</dd>
 <dt><code>%T08PLAYLCN</code></dt><dd>Lunatic のコンティニュー回数</dd>
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
  <td><code>%T08TIMEALL</code></td>
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
  <td><code>%T08TIMEPLY</code></td>
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
  <td colspan="2"><code>%T08PRAC[w][xx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[SR]</code></dt><dd>咲夜 &amp; レミリア</dd>
 <dt><code>[YY]</code></dt><dd>妖夢 &amp; 幽々子</dd>
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[1A]</code></dt><dd>Stage 1</dd>
 <dt><code>[2A]</code></dt><dd>Stage 2</dd>
 <dt><code>[3A]</code></dt><dd>Stage 3</dd>
 <dt><code>[4A]</code></dt><dd>Stage 4A</dd>
 <dt><code>[4B]</code></dt><dd>Stage 4B</dd>
 <dt><code>[5A]</code></dt><dd>Stage 5</dd>
 <dt><code>[6A]</code></dt><dd>Stage 6A</dd>
 <dt><code>[6B]</code></dt><dd>Stage 6B</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>スコア</dd>
 <dt><code>[2]</code></dt><dd>プレイ回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T08PRACEYM1A1</code></dt>
 <dd>Easy 妖夢の Stage 1 のプラクティススコア</dd>
 <dt><code>%T08PRACNRY4B2</code></dt>
 <dd>Normal 霊夢 &amp; 紫の Stage 4B のプラクティスプレイ回数</dd>
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
  <td colspan="2"><code>%T09SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RS]</code></dt><dd>鈴仙</dd>
 <dt><code>[CI]</code></dt><dd>チルノ</dd>
 <dt><code>[LY]</code></dt><dd>リリカ</dd>
 <dt><code>[MY]</code></dt><dd>ミスティア</dd>
 <dt><code>[TW]</code></dt><dd>てゐ</dd>
 <dt><code>[AY]</code></dt><dd>文</dd>
 <dt><code>[MD]</code></dt><dd>メディスン</dd>
 <dt><code>[YU]</code></dt><dd>幽香</dd>
 <dt><code>[KM]</code></dt><dd>小町</dd>
 <dt><code>[SI]</code></dt><dd>四季映姫</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～5]</code></dt><dd>1～5 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>登録名</dd>
 <dt><code>[2]</code></dt><dd>スコア</dd>
 <dt><code>[3]</code></dt><dd>日付</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T09SCRNMR12</code></dt><dd>Normal 魔理沙の 1 位のスコア</dd>
 <dt><code>%T09SCRXRM41</code></dt><dd>Extra 霊夢の 4 位の登録名</dd>
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
  <td colspan="2"><code>%T09CLEAR[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RS]</code></dt><dd>鈴仙</dd>
 <dt><code>[CI]</code></dt><dd>チルノ</dd>
 <dt><code>[LY]</code></dt><dd>リリカ</dd>
 <dt><code>[MY]</code></dt><dd>ミスティア</dd>
 <dt><code>[TW]</code></dt><dd>てゐ</dd>
 <dt><code>[AY]</code></dt><dd>文</dd>
 <dt><code>[MD]</code></dt><dd>メディスン</dd>
 <dt><code>[YU]</code></dt><dd>幽香</dd>
 <dt><code>[KM]</code></dt><dd>小町</dd>
 <dt><code>[SI]</code></dt><dd>四季映姫</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   出力形式
<dl class="format">
 <dt><code>[1]</code></dt><dd>クリア回数</dd>
 <dt><code>[2]</code></dt><dd>クリアしたかどうかのフラグ情報</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T09CLEARXMR1</code></dt><dd>Extra 魔理沙のクリア回数</dd>
 <dt><code>%T09CLEARNSK2</code></dt><dd>Normal 咲夜のクリアフラグ</dd>
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
  <td><code>%T09TIMEALL</code></td>
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
  <td colspan="2"><code>%T95SCR[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>Level 1～9</dd>
 <dt><code>[0]</code></dt><dd>Level 10</dd>
 <dt><code>[X]</code></dt><dd>Level EX</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>ハイスコア</dd>
 <dt><code>[2]</code></dt><dd>登録してあるベストショットのスコア</dd>
 <dt><code>[3]</code></dt><dd>撮影枚数</dd>
 <dt><code>[4]</code></dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T95SCR111</code></dt><dd>1-1 でのハイスコア</dd>
 <dt><code>%T95SCR233</code></dt><dd>2-3 での撮影枚数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-7 など）は無視されます。</li>
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
  <td colspan="2"><code>%T95SCRTL[x]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>撮影総合評価点</dd>
 <dt><code>[2]</code></dt><dd>登録してあるベストショットのスコアの合計</dd>
 <dt><code>[3]</code></dt><dd>総撮影枚数</dd>
 <dt><code>[4]</code></dt><dd>撮影に成功したシーン数の合計</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T95SCRTL1</code></dt><dd>撮影総合評価点</dd>
 <dt><code>%T95SCRTL3</code></dt><dd>総撮影枚数</dd>
</dl>
  </td>
 </tr>
</table>

### 被写体 &amp; スペルカード情報 {: #T95CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T95CARD[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>Level 1～9</dd>
 <dt><code>[0]</code></dt><dd>Level 10</dd>
 <dt><code>[X]</code></dt><dd>Level EX</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>被写体の名前</dd>
 <dt><code>[2]</code></dt><dd>スペルカード名</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T95CARD111</code></dt><dd>1-1 の被写体の名前</dd>
 <dt><code>%T95CARD232</code></dt><dd>2-3 のスペルカード名</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-7 など）は無視されます。</li>
</ul>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは被写体の名前・スペルカード名ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
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
  <td colspan="2"><code>%T95SHOT[x][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>Level 1～9</dd>
 <dt><code>[0]</code></dt><dd>Level 10</dd>
 <dt><code>[X]</code></dt><dd>Level EX</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T95SHOT12</code></dt><dd>1-2 のベストショット</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-7 など）は無視されます。</li>
 <li>
  このテンプレート書式は「<samp>&lt;img src="./bestshot/bs_01_1.png" alt="～" title="～" border=0&gt;</samp>」のような HTML の IMG タグに置換されます。<br />
  同時に、対象となるベストショットファイル (bs_??_?.dat) を PNG 形式に変換した画像ファイルが出力されます。
 </li>
 <li>IMG タグの alt 属性と title 属性には、ベストショット撮影時のスコアと処理落ち率、及びスペルカード名が出力されます。</li>
 <li>画像ファイルは、「出力先(O):」欄で指定されたフォルダ内の「画像出力先(I):」欄で指定されたフォルダに出力されます。</li>
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
  <td colspan="2"><code>%T95SHOTEX[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>Level 1～9</dd>
 <dt><code>[0]</code></dt><dd>Level 10</dd>
 <dt><code>[X]</code></dt><dd>Level EX</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>画像ファイルへの相対パス</dd>
 <dt><code>[2]</code></dt><dd>画像ファイルの幅 (px)</dd>
 <dt><code>[3]</code></dt><dd>画像ファイルの高さ (px)</dd>
 <dt><code>[4]</code></dt><dd>ベストショット撮影時のスコア</dd>
 <dt><code>[5]</code></dt><dd>ベストショット撮影時の処理落ち率</dd>
 <dt><code>[6]</code></dt><dd>ベストショット撮影日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T95SHOTEX121</code></dt><dd>1-2 の画像ファイルへの相対パス</dd>
 <dt><code>%T95SHOTEX236</code></dt><dd>2-3 のベストショット撮影日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-7 など）は無視されます。</li>
 <li>
  このテンプレート書式を使って、例えば <code>%T95SHOT12</code> と同等の出力結果を得るには、テンプレートファイルに以下の通りに記載します。
<pre><code>&lt;img src="%T95SHOTEX121" alt="Score: %T95SHOTEX124
Slow: %T95SHOTEX125
SpellName: %T95CARD122" title="Score: %T95SHOTEX124
Slow: %T95SHOTEX125
SpellName: %T95CARD122" border=0&gt;
</code></pre>
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
  「<a href="#T95SHOT">ベストショット出力</a>」により出力される IMG タグが気に食わなかったから、この書式を新規追加し、かつベストショットファイルの変換を自前で実装したようなものです。
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
  <td colspan="2"><code>%T10SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[RC]</code></dt><dd>霊夢 (C)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[MC]</code></dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9 位</dd>
 <dt><code>[0]</code></dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>登録名</dd>
 <dt><code>[2]</code></dt><dd>スコア</dd>
 <dt><code>[3]</code></dt><dd>到達ステージ</dd>
 <dt><code>[4]</code></dt><dd>日時</dd>
 <dt><code>[5]</code></dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T10SCRNMC12</code></dt><dd>Normal 魔理沙 (C) の 1 位のスコア</dd>
 <dt><code>%T10SCRXRA44</code></dt><dd>Extra 霊夢 (A) の 4 位の日時</dd>
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
  <td colspan="2"><code>%T10C[xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[000]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[001～110]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[RC]</code></dt><dd>霊夢 (C)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[MC]</code></dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T10C003TL1</code></dt><dd>全主人公合計の秋符「オータムスカイ」の取得回数</dd>
 <dt><code>%T10C003MC2</code></dt><dd>魔理沙 (C) の秋符「オータムスカイ」の挑戦回数</dd>
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
  <td colspan="2"><code>%T10CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[001～110]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T10CARD003N</code></dt><dd>秋符「オータムスカイ」</dd>
 <dt><code>%T10CARD003R</code></dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>相違点</td>
  <td colspan="2">
<ul>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前が「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
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
  <td colspan="2"><code>%T10CRG[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[RC]</code></dt><dd>霊夢 (C)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[MC]</code></dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[0]</code></dt><dd>全ステージ合計</dd>
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
   （Extra ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T10CRGERA01</code></dt><dd>Easy 霊夢 (A) の全ステージ合計の取得数</dd>
 <dt><code>%T10CRGTMC41</code></dt><dd>魔理沙 (C) の Stage 4 の全難易度合計の取得数</dd>
 <dt><code>%T10CRGTTL02</code></dt><dd>全難易度・全キャラ・全ステージ合計の挑戦数</dd>
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
  <td colspan="2"><code>%T10CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[RC]</code></dt><dd>霊夢 (C)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[MC]</code></dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T10CLEARXMA</code></dt><dd>Extra 魔理沙 (A) のクリア達成度</dd>
 <dt><code>%T10CLEARNRB</code></dt><dd>Normal 霊夢 (B) のクリア達成度</dd>
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
  <td colspan="2"><code>%T10CHARA[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[RC]</code></dt><dd>霊夢 (C)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[MC]</code></dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T10CHARATL2</code></dt><dd>全主人公合計のプレイ時間</dd>
 <dt><code>%T10CHARARA1</code></dt><dd>霊夢 (A) の総プレイ回数</dd>
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
  <td colspan="2"><code>%T10CHARAEX[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
   （総プレイ回数とプレイ時間ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[RC]</code></dt><dd>霊夢 (C)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[MC]</code></dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T10CHARAEXETL2</code></dt><dd>全主人公合計のプレイ時間</dd>
 <dt><code>%T10CHARAEXERA1</code></dt><dd>霊夢 (A) の総プレイ回数</dd>
 <dt><code>%T10CHARAEXTMC3</code></dt><dd>魔理沙 (C) の全難易度合計のクリア回数</dd>
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
  <td colspan="2"><code>%T10PRAC[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[RC]</code></dt><dd>霊夢 (C)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[MC]</code></dt><dd>魔理沙 (C)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T10PRACEMC1</code></dt><dd>Easy 魔理沙 (C) の Stage 1 のプラクティススコア</dd>
 <dt><code>%T10PRACNRA4</code></dt><dd>Normal 霊夢 (A) の Stage 4 のプラクティススコア</dd>
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

## 東方緋想天用テンプレート書式 {: #Th105Formats }

### 御札戦歴 {: #T105C }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T105C[xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[000]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[001～100]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[PC]</code></dt><dd>パチュリー</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[SU]</code></dt><dd>萃香</dd>
 <dt><code>[RS]</code></dt><dd>鈴仙</dd>
 <dt><code>[AY]</code></dt><dd>文</dd>
 <dt><code>[KM]</code></dt><dd>小町</dd>
 <dt><code>[IK]</code></dt><dd>衣玖</dd>
 <dt><code>[TN]</code></dt><dd>天子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦回数（勝率の分母）</dd>
 <dt><code>[3]</code></dt><dd>残り時間</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T105C001RM1</code></dt>
 <dd>霊夢の星符「ポラリスユニーク」(Easy) の取得回数</dd>
 <dt><code>%T105C008MR2</code></dt>
 <dd>魔理沙の剣符「ソルジャーオブクロス」(Lunatic) の挑戦回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>キャラが異なると、スペルカードの番号が同じでもその中身は異なります。</li>
 <li>存在しないキャラと番号の組み合わせ（霊夢の No.77 など）は無視されます。</li>
 <li>
  残り時間は分秒およびミリ秒が「<samp>mm:ss.ddd</samp>」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。
 </li>
</ul>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T105CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T105CARD[xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[001～100]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[PC]</code></dt><dd>パチュリー</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[SU]</code></dt><dd>萃香</dd>
 <dt><code>[RS]</code></dt><dd>鈴仙</dd>
 <dt><code>[AY]</code></dt><dd>文</dd>
 <dt><code>[KM]</code></dt><dd>小町</dd>
 <dt><code>[IK]</code></dt><dd>衣玖</dd>
 <dt><code>[TN]</code></dt><dd>天子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T105CARD001SKN</code></dt><dd>星符「ポラリスユニーク」</dd>
 <dt><code>%T105CARD001SKR</code></dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>キャラが異なると、スペルカードの番号が同じでもその中身は異なります。</li>
 <li>存在しないキャラと番号の組み合わせ（霊夢の No.77 など）は無視されます。</li>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前・難易度ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T105CRG }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T105CRG[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[PC]</code></dt><dd>パチュリー</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[SU]</code></dt><dd>萃香</dd>
 <dt><code>[RS]</code></dt><dd>鈴仙</dd>
 <dt><code>[AY]</code></dt><dd>文</dd>
 <dt><code>[KM]</code></dt><dd>小町</dd>
 <dt><code>[IK]</code></dt><dd>衣玖</dd>
 <dt><code>[TN]</code></dt><dd>天子</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T105CRGEYK1</code></dt><dd>Easy 紫の取得数</dd>
 <dt><code>%T105CRGHRL2</code></dt><dd>Hard レミリアの挑戦数</dd>
</dl>
  </td>
 </tr>
</table>

### デッキ用カード蒐集歴 {: #T105DC }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T105DC[ww][x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[ww]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[PC]</code></dt><dd>パチュリー</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[SU]</code></dt><dd>萃香</dd>
 <dt><code>[RS]</code></dt><dd>鈴仙</dd>
 <dt><code>[AY]</code></dt><dd>文</dd>
 <dt><code>[KM]</code></dt><dd>小町</dd>
 <dt><code>[IK]</code></dt><dd>衣玖</dd>
 <dt><code>[TN]</code></dt><dd>天子</dd>
</dl>
   （System ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   カードの種類
<dl class="format">
 <dt><code>[Y]</code></dt><dd>System</dd>
 <dt><code>[K]</code></dt><dd>Skill</dd>
 <dt><code>[P]</code></dt><dd>Spell</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   カードの番号
<dl class="format">
 <dt><code>[01～11]</code></dt><dd>カードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>カードの名前</dd>
 <dt><code>[C]</code></dt><dd>蒐集枚数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T105DCRMY01N</code></dt><dd>「気質発現」</dd>
 <dt><code>%T105DCMRK02N</code></dt><dd>ミアズマスウィープ</dd>
 <dt><code>%T105DCSKP03C</code></dt><dd>傷符「インスクライブレッドソウル」の蒐集枚数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>キャラが異なると、カードの番号が同じでもその中身は異なります。（System は除く。）</li>
 <li>存在しないカードの指定（霊夢の Spell No.09 など）は無視されます。</li>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、未入手のカードの名前が「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
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
  <td colspan="2"><code>%T11SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[RS]</code></dt><dd>霊夢 &amp; 萃香</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 &amp; 文</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[MP]</code></dt><dd>魔理沙 &amp; パチュリー</dd>
 <dt><code>[MN]</code></dt><dd>魔理沙 &amp; にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9 位</dd>
 <dt><code>[0]</code></dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>登録名</dd>
 <dt><code>[2]</code></dt><dd>スコア</dd>
 <dt><code>[3]</code></dt><dd>到達ステージ</dd>
 <dt><code>[4]</code></dt><dd>日時</dd>
 <dt><code>[5]</code></dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T11SCRNMN12</code></dt><dd>Normal 魔理沙 &amp; にとりの 1 位のスコア</dd>
 <dt><code>%T11SCRXRY44</code></dt><dd>Extra 霊夢 &amp; 紫の 4 位の日時</dd>
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
  <td colspan="2"><code>%T11C[xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[000]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[001～175]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[RS]</code></dt><dd>霊夢 &amp; 萃香</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 &amp; 文</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[MP]</code></dt><dd>魔理沙 &amp; パチュリー</dd>
 <dt><code>[MN]</code></dt><dd>魔理沙 &amp; にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T11C003TL1</code></dt>
 <dd>全主人公合計の罠符「キャプチャーウェブ」(Easy) の取得回数</dd>
 <dt><code>%T11C003MN2</code></dt>
 <dd>魔理沙 &amp; にとりの罠符「キャプチャーウェブ」(Easy) の挑戦回数</dd>
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
  <td colspan="2"><code>%T11CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[001～175]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T11CARD003N</code></dt><dd>罠符「キャプチャーウェブ」</dd>
 <dt><code>%T11CARD003R</code></dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前が「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
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
  <td colspan="2"><code>%T11CRG[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[RS]</code></dt><dd>霊夢 &amp; 萃香</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 &amp; 文</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[MP]</code></dt><dd>魔理沙 &amp; パチュリー</dd>
 <dt><code>[MN]</code></dt><dd>魔理沙 &amp; にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[0]</code></dt><dd>全ステージ合計</dd>
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
   （Extra ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T11CRGERY01</code></dt>
 <dd>Easy 霊夢 &amp; 紫の全ステージ合計の取得数</dd>
 <dt><code>%T11CRGTMN41</code></dt>
 <dd>魔理沙 &amp; にとりの Stage 4 の全難易度合計の取得数</dd>
 <dt><code>%T11CRGTTL02</code></dt>
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
  <td colspan="2"><code>%T11CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[RS]</code></dt><dd>霊夢 &amp; 萃香</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 &amp; 文</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[MP]</code></dt><dd>魔理沙 &amp; パチュリー</dd>
 <dt><code>[MN]</code></dt><dd>魔理沙 &amp; にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T11CLEARXMA</code></dt><dd>Extra 魔理沙 &amp; アリスのクリア達成度</dd>
 <dt><code>%T11CLEARNRS</code></dt><dd>Normal 霊夢 &amp; 萃香のクリア達成度</dd>
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
  <td colspan="2"><code>%T11CHARA[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[RS]</code></dt><dd>霊夢 &amp; 萃香</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 &amp; 文</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[MP]</code></dt><dd>魔理沙 &amp; パチュリー</dd>
 <dt><code>[MN]</code></dt><dd>魔理沙 &amp; にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T11CHARATL2</code></dt><dd>全主人公合計のプレイ時間</dd>
 <dt><code>%T11CHARARY1</code></dt><dd>霊夢 &amp; 紫の総プレイ回数</dd>
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
  <td colspan="2"><code>%T11CHARAEX[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
   （総プレイ回数とプレイ時間ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[RS]</code></dt><dd>霊夢 &amp; 萃香</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 &amp; 文</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[MP]</code></dt><dd>魔理沙 &amp; パチュリー</dd>
 <dt><code>[MN]</code></dt><dd>魔理沙 &amp; にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T11CHARAEXETL2</code></dt><dd>全主人公合計のプレイ時間</dd>
 <dt><code>%T11CHARAEXERY1</code></dt><dd>霊夢 &amp; 紫の総プレイ回数</dd>
 <dt><code>%T11CHARAEXTMN3</code></dt><dd>魔理沙 &amp; にとりの全難易度合計のクリア回数</dd>
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
  <td colspan="2"><code>%T11PRAC[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RY]</code></dt><dd>霊夢 &amp; 紫</dd>
 <dt><code>[RS]</code></dt><dd>霊夢 &amp; 萃香</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 &amp; 文</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 &amp; アリス</dd>
 <dt><code>[MP]</code></dt><dd>魔理沙 &amp; パチュリー</dd>
 <dt><code>[MN]</code></dt><dd>魔理沙 &amp; にとり</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T11PRACEMN1</code></dt>
 <dd>Easy 魔理沙 &amp; にとりの Stage 1 のプラクティススコア</dd>
 <dt><code>%T11PRACNRY4</code></dt>
 <dd>Normal 霊夢 &amp; 紫の Stage 4 のプラクティススコア</dd>
</dl>
  </td>
 </tr>
</table>

----------------------------------------

## 東方星蓮船用テンプレート書式 {: #Th12Formats }

### スコアランキング {: #T12SCR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T12SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[SA]</code></dt><dd>早苗（蛇）</dd>
 <dt><code>[SB]</code></dt><dd>早苗（蛙）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9 位</dd>
 <dt><code>[0]</code></dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>登録名</dd>
 <dt><code>[2]</code></dt><dd>スコア</dd>
 <dt><code>[3]</code></dt><dd>到達ステージ</dd>
 <dt><code>[4]</code></dt><dd>日時</dd>
 <dt><code>[5]</code></dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T12SCRNSB12</code></dt><dd>Normal 早苗（蛙）の 1 位のスコア</dd>
 <dt><code>%T12SCRXRA44</code></dt><dd>Extra 霊夢（夢）の 4 位の日時</dd>
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

### 御札戦歴 {: #T12C }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T12C[xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[000]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[001～113]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[SA]</code></dt><dd>早苗（蛇）</dd>
 <dt><code>[SB]</code></dt><dd>早苗（蛙）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T12C003TL1</code></dt>
 <dd>全主人公合計の捜符「レアメタルディテクター」(Easy) の取得回数</dd>
 <dt><code>%T12C003SB2</code></dt>
 <dd>早苗（蛙）の捜符「レアメタルディテクター」(Easy) の挑戦回数</dd>
</dl>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T12CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T12CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[001～113]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T12CARD003N</code></dt><dd>捜符「レアメタルディテクター」</dd>
 <dt><code>%T12CARD003R</code></dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前が「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
 <li>一方、スペルカードの難易度は、未挑戦かどうかにかかわらず常に出力されます。原作でも Result 画面を見れば難易度はバレるので、このような仕様にしています。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T12CRG }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T12CRG[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[SA]</code></dt><dd>早苗（蛇）</dd>
 <dt><code>[SB]</code></dt><dd>早苗（蛙）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[0]</code></dt><dd>全ステージ合計</dd>
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
   （Extra ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T12CRGERA01</code></dt>
 <dd>Easy 霊夢（夢）の全ステージ合計の取得数</dd>
 <dt><code>%T12CRGTSB41</code></dt>
 <dd>早苗（蛙）の Stage 4 の全難易度合計の取得数</dd>
 <dt><code>%T12CRGTTL02</code></dt>
 <dd>全難易度・全キャラ・全ステージ合計の挑戦数</dd>
</dl>
  </td>
 </tr>
</table>

### クリア達成度 {: #T12CLEAR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T12CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[SA]</code></dt><dd>早苗（蛇）</dd>
 <dt><code>[SB]</code></dt><dd>早苗（蛙）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T12CLEARXMB</code></dt><dd>Extra 魔理沙（魔）のクリア達成度</dd>
 <dt><code>%T12CLEARNRB</code></dt><dd>Normal 霊夢（霊）のクリア達成度</dd>
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

### キャラごとの個別データ {: #T12CHARA }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T12CHARA[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[SA]</code></dt><dd>早苗（蛇）</dd>
 <dt><code>[SB]</code></dt><dd>早苗（蛙）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T12CHARATL2</code></dt><dd>全主人公合計のプレイ時間</dd>
 <dt><code>%T12CHARARA1</code></dt><dd>霊夢（夢）の総プレイ回数</dd>
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

### キャラごとの個別データ（詳細版） {: #T12CHARAEX }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T12CHARAEX[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
   （総プレイ回数とプレイ時間ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[SA]</code></dt><dd>早苗（蛇）</dd>
 <dt><code>[SB]</code></dt><dd>早苗（蛙）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T12CHARAEXETL2</code></dt><dd>全主人公合計のプレイ時間</dd>
 <dt><code>%T12CHARAEXERA1</code></dt><dd>霊夢（夢）の総プレイ回数</dd>
 <dt><code>%T12CHARAEXTSB3</code></dt><dd>早苗（蛙）の全難易度合計のクリア回数</dd>
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

### プラクティススコア {: #T12PRAC }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T12PRAC[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢（夢）</dd>
 <dt><code>[RB]</code></dt><dd>霊夢（霊）</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙（恋）</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙（魔）</dd>
 <dt><code>[SA]</code></dt><dd>早苗（蛇）</dd>
 <dt><code>[SB]</code></dt><dd>早苗（蛙）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T12PRACESB1</code></dt>
 <dd>Easy 早苗（蛙）の Stage 1 のプラクティススコア</dd>
 <dt><code>%T12PRACNRA4</code></dt>
 <dd>Normal 霊夢（夢）の Stage 4 のプラクティススコア</dd>
</dl>
  </td>
 </tr>
</table>

----------------------------------------

## 東方非想天則用テンプレート書式 {: #Th123Formats }

### 御札戦歴 {: #T123C }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T123C[xx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[00]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[01～64]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[CI]</code></dt><dd>チルノ</dd>
 <dt><code>[ML]</code></dt><dd>美鈴</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦回数（勝率の分母）</dd>
 <dt><code>[3]</code></dt><dd>残り時間</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T123C01SN1</code></dt>
 <dd>早苗の氷符「アイシクルフォール」(Easy) の取得回数</dd>
 <dt><code>%T123C08CI2</code></dt>
 <dd>チルノの奇跡「ファフロッキーズの奇跡」(Lunatic) の挑戦回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>キャラが異なると、スペルカードの番号が同じでもその中身は異なります。</li>
 <li>
  残り時間は分秒およびミリ秒が「<samp>mm:ss.ddd</samp>」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。
 </li>
</ul>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T123CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T123CARD[xx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[01～64]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[CI]</code></dt><dd>チルノ</dd>
 <dt><code>[ML]</code></dt><dd>美鈴</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T123CARD01SNN</code></dt><dd>氷符「アイシクルフォール」</dd>
 <dt><code>%T123CARD01SNR</code></dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>キャラが異なると、スペルカードの番号が同じでもその中身は異なります。</li>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前・難易度ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T123CRG }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T123CRG[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[CI]</code></dt><dd>チルノ</dd>
 <dt><code>[ML]</code></dt><dd>美鈴</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T123CRGESN1</code></dt><dd>Easy 早苗の取得数</dd>
 <dt><code>%T123CRGHCI2</code></dt><dd>Hard チルノの挑戦数</dd>
</dl>
  </td>
 </tr>
</table>

### デッキ用カード蒐集歴 {: #T123DC }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T123DC[ww][x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[ww]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SK]</code></dt><dd>咲夜</dd>
 <dt><code>[AL]</code></dt><dd>アリス</dd>
 <dt><code>[PC]</code></dt><dd>パチュリー</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
 <dt><code>[RL]</code></dt><dd>レミリア</dd>
 <dt><code>[YU]</code></dt><dd>幽々子</dd>
 <dt><code>[YK]</code></dt><dd>紫</dd>
 <dt><code>[SU]</code></dt><dd>萃香</dd>
 <dt><code>[RS]</code></dt><dd>鈴仙</dd>
 <dt><code>[AY]</code></dt><dd>文</dd>
 <dt><code>[KM]</code></dt><dd>小町</dd>
 <dt><code>[IK]</code></dt><dd>衣玖</dd>
 <dt><code>[TN]</code></dt><dd>天子</dd>
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[CI]</code></dt><dd>チルノ</dd>
 <dt><code>[ML]</code></dt><dd>美鈴</dd>
 <dt><code>[UT]</code></dt><dd>空</dd>
 <dt><code>[SW]</code></dt><dd>諏訪子</dd>
</dl>
   （System ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   カードの種類
<dl class="format">
 <dt><code>[Y]</code></dt><dd>System</dd>
 <dt><code>[K]</code></dt><dd>Skill</dd>
 <dt><code>[P]</code></dt><dd>Spell</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   カードの番号
<dl class="format">
 <dt><code>[01～21]</code></dt><dd>カードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>カードの名前</dd>
 <dt><code>[C]</code></dt><dd>蒐集枚数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T123DCRMY01N</code></dt><dd>「霊撃札」</dd>
 <dt><code>%T123DCMRK02N</code></dt><dd>ナロースパーク</dd>
 <dt><code>%T123DCSKP03C</code></dt><dd>奇術「エターナルミーク」の蒐集枚数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>キャラが異なると、カードの番号が同じでもその中身は異なります。（System は除く。）</li>
 <li>存在しないカードの指定（霊夢の Spell No.11 など）は無視されます。</li>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、未入手のカードの名前が「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## ダブルスポイラー用テンプレート書式 {: #Th125Formats }

### スコア一覧 {: #T125SCR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T125SCR[w][x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[A]</code></dt><dd>文</dd>
 <dt><code>[H]</code></dt><dd>はたて</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>Level 1～9</dd>
 <dt><code>[A～C]</code></dt><dd>Level 10～12</dd>
 <dt><code>[X]</code></dt><dd>Level EX</dd>
 <dt><code>[S]</code></dt><dd>SPOILER</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>得点</dd>
 <dt><code>[2]</code></dt><dd>登録してあるベストショットの得点</dd>
 <dt><code>[3]</code></dt><dd>撮影枚数</dd>
 <dt><code>[4]</code></dt><dd>初成功時枚数</dd>
 <dt><code>[5]</code></dt><dd>日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T125SCRA111</code></dt><dd>文の 1-1 の得点</dd>
 <dt><code>%T125SCRH233</code></dt><dd>はたての 2-3 の撮影枚数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-7 など）は無視されます。</li>
</ul>
  </td>
 </tr>
</table>

### スコア合計 {: #T125SCRTL }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T125SCRTL[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[A]</code></dt><dd>文</dd>
 <dt><code>[H]</code></dt><dd>はたて</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   集計方法
<dl class="format">
 <dt><code>[1]</code></dt><dd>ゲーム内表示準拠</dd>
 <dt><code>[2]</code></dt><dd>自機準拠</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総撮影得点</dd>
 <dt><code>[2]</code></dt><dd>登録してあるベストショットの得点の合計</dd>
 <dt><code>[3]</code></dt><dd>総撮影枚数</dd>
 <dt><code>[4]</code></dt><dd>初成功時枚数の合計</dd>
 <dt><code>[5]</code></dt><dd>クリアシーン数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T125SCRTLA11</code></dt><dd>文の総撮影得点（ゲーム内表示準拠）</dd>
 <dt><code>%T125SCRTLH23</code></dt><dd>はたての総撮影枚数（自機準拠）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>集計方法を変えると、SPOILER の ??-5～??-9 の得点などが、文とはたてのどちらの集計結果に含まれるかが変わります。「ゲーム内表示準拠」では文の方に、「自機準拠」でははたての方に含まれます。</li>
</ul>
  </td>
 </tr>
</table>

### 被写体 &amp; スペルカード情報 {: #T125CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T125CARD[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>Level 1～9</dd>
 <dt><code>[A～C]</code></dt><dd>Level 10～12</dd>
 <dt><code>[X]</code></dt><dd>Level EX</dd>
 <dt><code>[S]</code></dt><dd>SPOILER</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>被写体の名前</dd>
 <dt><code>[2]</code></dt><dd>スペルカード名</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T125CARD111</code></dt><dd>1-1 の被写体の名前</dd>
 <dt><code>%T125CARD232</code></dt><dd>2-3 のスペルカード名</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-7 など）は無視されます。</li>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは被写体の名前・スペルカード名ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
</ul>
  </td>
 </tr>
</table>

### 総プレイ時間 {: #T125TIMEPLY }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td>書式</td>
  <td><code>%T125TIMEPLY</code></td>
 </tr>
 <tr>
  <td>補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「<samp>h:mm:ss.ddd</samp>」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

### ベストショット出力 {: #T125SHOT }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T125SHOT[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[A]</code></dt><dd>文</dd>
 <dt><code>[H]</code></dt><dd>はたて</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   レベル
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>Level 1～9</dd>
 <dt><code>[A～C]</code></dt><dd>Level 10～12</dd>
 <dt><code>[X]</code></dt><dd>Level EX</dd>
 <dt><code>[S]</code></dt><dd>SPOILER</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T125SHOTA12</code></dt><dd>文の 1-2 のベストショット</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-7 など）は無視されます。</li>
 <li>
  このテンプレート書式は「<samp>&lt;img src="./bestshot/bs_01_1.png" alt="～" title="～" border=0&gt;</samp>」のような HTML の IMG タグに置換されます。<br />
  同時に、対象となるベストショットファイル (bs_??_?.dat や bs2_??_?.dat) を PNG 形式に変換した画像ファイルが出力されます。
 </li>
 <li>IMG タグの alt 属性と title 属性には、ベストショット撮影時の得点と処理落ち率、及びスペルカード名が出力されます。</li>
 <li>画像ファイルは、「出力先(O):」欄で指定されたフォルダ内の「画像出力先(I):」欄で指定されたフォルダに出力されます。</li>
 <li>画像ファイルの出力先フォルダが存在しない場合、本ツールが自動で作成します。</li>
 <li>ベストショットファイルが存在しない場合、IMG タグや画像ファイルは出力されません。</li>
 <li>ベストショットファイルの変換は、このテンプレート書式がテンプレートファイル内に無くても実行されます。</li>
</ul>
  </td>
 </tr>
</table>

### ベストショット出力（詳細版） {: #T125SHOTEX }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T125SHOTEX[w][x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[A]</code></dt><dd>文</dd>
 <dt><code>[H]</code></dt><dd>はたて</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   レベル
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>Level 1～9</dd>
 <dt><code>[A～C]</code></dt><dd>Level 10～12</dd>
 <dt><code>[X]</code></dt><dd>Level EX</dd>
 <dt><code>[S]</code></dt><dd>SPOILER</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>画像ファイルへの相対パス</dd>
 <dt><code>[2]</code></dt><dd>画像ファイルの幅 (px)</dd>
 <dt><code>[3]</code></dt><dd>画像ファイルの高さ (px)</dd>
 <dt><code>[4]</code></dt><dd>ベストショット撮影時の得点</dd>
 <dt><code>[5]</code></dt><dd>ベストショット撮影時の処理落ち率</dd>
 <dt><code>[6]</code></dt><dd>ベストショット撮影日時</dd>
 <dt><code>[7]</code></dt><dd>詳細情報（獲得ボーナスなど）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T125SHOTEXA121</code></dt><dd>文の 1-2 の画像ファイルへの相対パス</dd>
 <dt><code>%T125SHOTEXH236</code></dt><dd>はたての 2-3 のベストショット撮影日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しないレベルとシーンの組み合わせ（1-7 など）は無視されます。</li>
 <li>
  このテンプレート書式を使って、例えば <code>%T125SHOTA12</code> と同等の出力結果を得るには、テンプレートファイルに以下の通りに記載します。
<pre><code>&lt;img src="%T125SHOTEXA121" alt="Score: %T125SHOTEXA124
Slow: %T125SHOTEXA125
SpellName: %T125CARD122" title="Score: %T125SHOTEXA124
Slow: %T125SHOTEXA125
SpellName: %T125CARD122" border=0&gt;
</code></pre>
 </li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## 妖精大戦争テンプレート書式 {: #Th128Formats }

### スコアランキング {: #T128SCR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T128SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   ルート
<dl class="format">
 <dt><code>[A1]</code></dt><dd>ルート A1</dd>
 <dt><code>[A2]</code></dt><dd>ルート A2</dd>
 <dt><code>[B1]</code></dt><dd>ルート B1</dd>
 <dt><code>[B2]</code></dt><dd>ルート B2</dd>
 <dt><code>[C1]</code></dt><dd>ルート C1</dd>
 <dt><code>[C2]</code></dt><dd>ルート C2</dd>
 <dt><code>[EX]</code></dt><dd>ルート EX</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9 位</dd>
 <dt><code>[0]</code></dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>登録名</dd>
 <dt><code>[2]</code></dt><dd>スコア</dd>
 <dt><code>[3]</code></dt><dd>到達ステージ</dd>
 <dt><code>[4]</code></dt><dd>日時</dd>
 <dt><code>[5]</code></dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T128SCRNC212</code></dt><dd>Normal ルート C2 の 1 位のスコア</dd>
 <dt><code>%T128SCRXEX44</code></dt><dd>Extra の 4 位の日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない難易度とルートの組み合わせ（Easy のルート EX や Extra のルート A1 など）は無視されます。</li>
 <li>スコアの 1 の位には、原作と同様にコンティニュー回数が出力されます。</li>
 <li>日時は年月日及び時分秒が「<samp>yyyy/mm/dd hh:mm:ss</samp>」の形式で出力されます。</li>
 <li>処理落ち率は小数点以下第 3 位まで（% 記号付きで）出力されます。今後、この桁数を設定可能にするかも知れません。</li>
</ul>
  </td>
 </tr>
</table>

### 御札戦歴 {: #T128C }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T128C[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[000]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[001～250]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>ノーアイス勝利回数</dd>
 <dt><code>[2]</code></dt><dd>ノーミス勝利回数</dd>
 <dt><code>[3]</code></dt><dd>挑戦回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T128C0011</code></dt>
 <dd>月符「ルナティックレイン」(Easy) のノーアイス処理回数</dd>
 <dt><code>%T128C0053</code></dt>
 <dd>月符「ルナサイクロン」(Easy) の挑戦回数</dd>
</dl>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T128CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T128CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[001～250]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T128CARD001N</code></dt><dd>月符「ルナティックレイン」</dd>
 <dt><code>%T128CARD001R</code></dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前が「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
 <li>一方、スペルカードの難易度は、未挑戦かどうかにかかわらず常に出力されます。原作でも Result 画面を見れば難易度はバレるので、このような仕様にしています。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T128CRG }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T128CRG[x][yyy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージなど
<dl class="format">
 <dt><code>[TTL]</code></dt><dd>全ステージ合計</dd>
 <dt><code>[A11]</code></dt><dd>Stage A-1</dd>
 <dt><code>[A12]</code></dt><dd>Stage A1-2</dd>
 <dt><code>[A13]</code></dt><dd>Stage A1-3</dd>
 <dt><code>[A22]</code></dt><dd>Stage A2-2</dd>
 <dt><code>[A23]</code></dt><dd>Stage A2-3</dd>
 <dt><code>[B11]</code></dt><dd>Stage B-1</dd>
 <dt><code>[B12]</code></dt><dd>Stage B1-2</dd>
 <dt><code>[B13]</code></dt><dd>Stage B1-3</dd>
 <dt><code>[B22]</code></dt><dd>Stage B2-2</dd>
 <dt><code>[B23]</code></dt><dd>Stage B2-3</dd>
 <dt><code>[C11]</code></dt><dd>Stage C-1</dd>
 <dt><code>[C12]</code></dt><dd>Stage C1-2</dd>
 <dt><code>[C13]</code></dt><dd>Stage C1-3</dd>
 <dt><code>[C22]</code></dt><dd>Stage C2-2</dd>
 <dt><code>[C23]</code></dt><dd>Stage C2-3</dd>
</dl>
   （Extra ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>ノーアイス勝利数</dd>
 <dt><code>[2]</code></dt><dd>ノーミス勝利数</dd>
 <dt><code>[3]</code></dt><dd>挑戦数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T128CRGETTL1</code></dt>
 <dd>Easy の全ステージ合計のノーアイス勝利数</dd>
 <dt><code>%T128CRGTC232</code></dt>
 <dd>Stage C2-3 の全難易度合計のノーミス勝利数</dd>
 <dt><code>%T128CRGTTTL3</code></dt>
 <dd>全難易度・全ステージ合計の挑戦数</dd>
</dl>
  </td>
 </tr>
</table>

### クリア達成度 {: #T128CLEAR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T128CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   ルート
<dl class="format">
 <dt><code>[A1]</code></dt><dd>ルート A1</dd>
 <dt><code>[A2]</code></dt><dd>ルート A2</dd>
 <dt><code>[B1]</code></dt><dd>ルート B1</dd>
 <dt><code>[B2]</code></dt><dd>ルート B2</dd>
 <dt><code>[C1]</code></dt><dd>ルート C1</dd>
 <dt><code>[C2]</code></dt><dd>ルート C2</dd>
 <dt><code>[EX]</code></dt><dd>ルート EX</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T128CLEARXEX</code></dt><dd>Extra のクリア達成度</dd>
 <dt><code>%T128CLEARNA2</code></dt><dd>Normal ルート A2 のクリア達成度</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない難易度とルートの組み合わせ（Easy のルート EX や Extra のルート A1 など）は無視されます。</li>
 <li>
  クリア達成度（ゲームの進行状況）に応じて以下の文字列が出力されます。
  <p class="legends">
   <samp>-------</samp>（未プレイ）,
   <samp>Stage A-1</samp>, <samp>Stage A1-2</samp>, <samp>Stage A1-3</samp>,
   <samp>Stage A2-2</samp>, <samp>Stage A2-3</samp>,
   <samp>A1 Clear</samp>, <samp>A2 Clear</samp>,
   <samp>Stage B-1</samp>, <samp>Stage B1-2</samp>, <samp>Stage B1-3</samp>,
   <samp>Stage B2-2</samp>, <samp>Stage B2-3</samp>,
   <samp>B1 Clear</samp>, <samp>B2 Clear</samp>,
   <samp>Stage C-1</samp>, <samp>Stage C1-2</samp>, <samp>Stage C1-3</samp>,
   <samp>Stage C2-2</samp>, <samp>Stage C2-3</samp>,
   <samp>C1 Clear</samp>, <samp>C2 Clear</samp>,
   <samp>Extra Stage</samp>, <samp>Extra Clear</samp>
  </p>
 </li>
 <li>本ツールでは、ランキングを基にクリア達成度を算出しているため、実際はクリア済みでもランキング上に存在していなければ未クリア扱いになってしまいます。</li>
</ul>
  </td>
 </tr>
</table>

### ルートごとの個別データ {: #T128ROUTE }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T128ROUTE[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   ルートなど
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全ルート合計</dd>
 <dt><code>[A1]</code></dt><dd>ルート A1</dd>
 <dt><code>[A2]</code></dt><dd>ルート A2</dd>
 <dt><code>[B1]</code></dt><dd>ルート B1</dd>
 <dt><code>[B2]</code></dt><dd>ルート B2</dd>
 <dt><code>[C1]</code></dt><dd>ルート C1</dd>
 <dt><code>[C2]</code></dt><dd>ルート C2</dd>
 <dt><code>[EX]</code></dt><dd>ルート EX</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T128ROUTETL2</code></dt><dd>全ルート合計のプレイ時間</dd>
 <dt><code>%T128ROUTEA11</code></dt><dd>ルート A1 の総プレイ回数</dd>
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
 <li>
  総プレイ回数は、ゲーム内部ではルート A1～C2 毎ではなくルート A, B, C 毎にカウントされているようです。この書式での出力結果は参考程度としてください。
 </li>
</ul>
  </td>
 </tr>
</table>

### ルートごとの個別データ（詳細版） {: #T128ROUTEEX }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T128ROUTEEX[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
   （総プレイ回数とプレイ時間ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   ルートなど
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全ルート合計</dd>
 <dt><code>[A1]</code></dt><dd>ルート A1</dd>
 <dt><code>[A2]</code></dt><dd>ルート A2</dd>
 <dt><code>[B1]</code></dt><dd>ルート B1</dd>
 <dt><code>[B2]</code></dt><dd>ルート B2</dd>
 <dt><code>[C1]</code></dt><dd>ルート C1</dd>
 <dt><code>[C2]</code></dt><dd>ルート C2</dd>
 <dt><code>[EX]</code></dt><dd>ルート EX</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T128ROUTEEXETL2</code></dt><dd>全ルート合計のプレイ時間</dd>
 <dt><code>%T128ROUTEEXEA11</code></dt><dd>ルート A1 の総プレイ回数</dd>
 <dt><code>%T128ROUTEEXTC23</code></dt><dd>ルート C2 の全難易度合計のクリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない難易度とルートの組み合わせ（Easy のルート EX や Extra のルート A1 など）は無視されます。</li>
 <li>
  プレイ時間は時分秒が「<samp>h:mm:ss</samp>」の形式で出力されます。<br />
  なお、スコアファイルにはフレーム数単位で保存されているため、60fps 固定と見なして換算した結果を出力しています。
 </li>
 <li>
  総プレイ回数は、ゲーム内部ではルート A1～C2 毎ではなくルート A, B, C 毎にカウントされているようです。この書式での出力結果は参考程度としてください。
 </li>
 <li>
  この書式で出力される全ルート合計のプレイ時間は、ゲーム内で表示される時間と一致しません。代わりに <a href="#T128TIMEPLY">総プレイ時間</a> の書式を使ってください。
 </li>
</ul>
  </td>
 </tr>
</table>

### 総プレイ時間 {: #T128TIMEPLY }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td>書式</td>
  <td><code>%T128TIMEPLY</code></td>
 </tr>
 <tr>
  <td>補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「<samp>h:mm:ss.ddd</samp>」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

----------------------------------------

## 東方神霊廟用テンプレート書式 {: #Th13Formats }

### スコアランキング {: #T13SCR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T13SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9 位</dd>
 <dt><code>[0]</code></dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>登録名</dd>
 <dt><code>[2]</code></dt><dd>スコア</dd>
 <dt><code>[3]</code></dt><dd>到達ステージ</dd>
 <dt><code>[4]</code></dt><dd>日時</dd>
 <dt><code>[5]</code></dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T13SCRNSN12</code></dt><dd>Normal 早苗の 1 位のスコア</dd>
 <dt><code>%T13SCRXRM44</code></dt><dd>Extra 霊夢の 4 位の日時</dd>
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

### 御札戦歴 {: #T13C }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T13C[w][xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   戦歴の種類
<dl class="format">
 <dt><code>[S]</code></dt><dd>ゲーム本編</dd>
 <dt><code>[P]</code></dt><dd>スペルプラクティス</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[000]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[001～127]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T13CS001TL1</code></dt>
 <dd>ゲーム本編 全主人公合計の符牒「死蝶の舞」(Easy) の取得回数</dd>
 <dt><code>%T13CP001SN2</code></dt>
 <dd>スペルプラクティス 早苗の符牒「死蝶の舞」(Easy) の挑戦回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない組み合わせ（つまりゲーム本編の No.120～127）は無視されます。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T13CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T13CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[001～127]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra, Over Drive)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T13CARD001N</code></dt><dd>符牒「死蝶の舞」</dd>
 <dt><code>%T13CARD001R</code></dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前が「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
 <li>一方、スペルカードの難易度は、未挑戦かどうかにかかわらず常に出力されます。原作でも Result 画面を見れば難易度はバレるので、このような仕様にしています。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T13CRG }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="6">書式</td>
  <td colspan="2"><code>%T13CRG[v][w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[v]</code></td>
  <td>
   戦歴の種類
<dl class="format">
 <dt><code>[S]</code></dt><dd>ゲーム本編</dd>
 <dt><code>[P]</code></dt><dd>スペルプラクティス</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[D]</code></dt><dd>Over Drive</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[0]</code></dt><dd>全ステージ合計</dd>
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
   （Extra, Over Drive ではこの指定は無視され、Total ではそのステージの
   Easy～Lunatic の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T13CRGSERM01</code></dt>
 <dd>ゲーム本編 Easy 霊夢の全ステージ合計の取得数</dd>
 <dt><code>%T13CRGSTSN41</code></dt>
 <dd>ゲーム本編 早苗の Stage 4 の全難易度合計の取得数</dd>
 <dt><code>%T13CRGPTTL02</code></dt>
 <dd>スペルプラクティス 全難易度・全キャラ・全ステージ合計の挑戦数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない組み合わせ（つまりゲーム本編の Over Drive）は無視されます。</li>
</ul>
  </td>
 </tr>
</table>

### クリア達成度 {: #T13CLEAR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T13CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T13CLEARXMR</code></dt><dd>Extra 魔理沙のクリア達成度</dd>
 <dt><code>%T13CLEARNRM</code></dt><dd>Normal 霊夢のクリア達成度</dd>
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

### キャラごとの個別データ {: #T13CHARA }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T13CHARA[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T13CHARATL2</code></dt><dd>全主人公合計のプレイ時間</dd>
 <dt><code>%T13CHARARM1</code></dt><dd>霊夢の総プレイ回数</dd>
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

### キャラごとの個別データ（詳細版） {: #T13CHARAEX }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T13CHARAEX[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
   （総プレイ回数とプレイ時間ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T13CHARAEXETL2</code></dt><dd>全主人公合計のプレイ時間</dd>
 <dt><code>%T13CHARAEXERM1</code></dt><dd>霊夢の総プレイ回数</dd>
 <dt><code>%T13CHARAEXTSN3</code></dt><dd>早苗の全難易度合計のクリア回数</dd>
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

### プラクティススコア {: #T13PRAC }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T13PRAC[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RM]</code></dt><dd>霊夢</dd>
 <dt><code>[MR]</code></dt><dd>魔理沙</dd>
 <dt><code>[SN]</code></dt><dd>早苗</dd>
 <dt><code>[YM]</code></dt><dd>妖夢</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T13PRACESN1</code></dt>
 <dd>Easy 早苗の Stage 1 のプラクティススコア</dd>
 <dt><code>%T13PRACNRM4</code></dt>
 <dd>Normal 霊夢の Stage 4 のプラクティススコア</dd>
</dl>
  </td>
 </tr>
</table>

----------------------------------------

## 東方輝針城用テンプレート書式 {: #Th14Formats }

### スコアランキング {: #T14SCR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T14SCR[w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[SA]</code></dt><dd>咲夜 (A)</dd>
 <dt><code>[SB]</code></dt><dd>咲夜 (B)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   順位
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9 位</dd>
 <dt><code>[0]</code></dt><dd>10 位</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>登録名</dd>
 <dt><code>[2]</code></dt><dd>スコア</dd>
 <dt><code>[3]</code></dt><dd>到達ステージ</dd>
 <dt><code>[4]</code></dt><dd>日時</dd>
 <dt><code>[5]</code></dt><dd>処理落ち率</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T14SCRNSB12</code></dt><dd>Normal 咲夜 (B) の 1 位のスコア</dd>
 <dt><code>%T14SCRXRA44</code></dt><dd>Extra 霊夢 (A) の 4 位の日時</dd>
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

### 御札戦歴 {: #T14C }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T14C[w][xxx][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   戦歴の種類
<dl class="format">
 <dt><code>[S]</code></dt><dd>ゲーム本編</dd>
 <dt><code>[P]</code></dt><dd>スペルプラクティス</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号など
<dl class="format">
 <dt><code>[000]</code></dt><dd>全スペルカードの合計値</dd>
 <dt><code>[001～120]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[SA]</code></dt><dd>咲夜 (A)</dd>
 <dt><code>[SB]</code></dt><dd>咲夜 (B)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得回数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦回数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T14CS003TL1</code></dt>
 <dd>ゲーム本編 全主人公合計の氷符「テイルフィンスラップ」(Easy) の取得回数</dd>
 <dt><code>%T14CP003SB2</code></dt>
 <dd>スペルプラクティス 咲夜 (B) の氷符「テイルフィンスラップ」(Easy) の挑戦回数</dd>
</dl>
  </td>
 </tr>
</table>

### スペルカード基本情報 {: #T14CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T14CARD[xxx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xxx]</code></td>
  <td>
   スペルカードの番号
<dl class="format">
 <dt><code>[001～120]</code></dt><dd>スペルカードの番号</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[N]</code></dt><dd>スペルカードの名前</dd>
 <dt><code>[R]</code></dt>
 <dd>スペルカードの難易度 (Easy, Normal, Hard, Lunatic, Extra)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T14CARD003N</code></dt><dd>氷符「テイルフィンスラップ」</dd>
 <dt><code>%T14CARD003R</code></dt><dd>Easy</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは名前が「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
 <li>一方、スペルカードの難易度は、未挑戦かどうかにかかわらず常に出力されます。原作でも Result 画面を見れば難易度はバレるので、このような仕様にしています。</li>
</ul>
  </td>
 </tr>
</table>

### スペルカード蒐集率 {: #T14CRG }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="6">書式</td>
  <td colspan="2"><code>%T14CRG[v][w][xx][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[v]</code></td>
  <td>
   戦歴の種類
<dl class="format">
 <dt><code>[S]</code></dt><dd>ゲーム本編</dd>
 <dt><code>[P]</code></dt><dd>スペルプラクティス</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[SA]</code></dt><dd>咲夜 (A)</dd>
 <dt><code>[SB]</code></dt><dd>咲夜 (B)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[0]</code></dt><dd>全ステージ合計</dd>
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
   （Extra ではこの指定は無視され、Total ではそのステージの Easy～Lunatic
   の合計が出力されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>取得数（勝率の分子）</dd>
 <dt><code>[2]</code></dt><dd>挑戦数（勝率の分母）</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T14CRGSERA01</code></dt>
 <dd>ゲーム本編 Easy 霊夢 (A) の全ステージ合計の取得数</dd>
 <dt><code>%T14CRGSTSB41</code></dt>
 <dd>ゲーム本編 咲夜 (B) の Stage 4 の全難易度合計の取得数</dd>
 <dt><code>%T14CRGPTTL02</code></dt>
 <dd>スペルプラクティス 全難易度・全キャラ・全ステージ合計の挑戦数</dd>
</dl>
  </td>
 </tr>
</table>

### クリア達成度 {: #T14CLEAR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T14CLEAR[x][yy]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[SA]</code></dt><dd>咲夜 (A)</dd>
 <dt><code>[SB]</code></dt><dd>咲夜 (B)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T14CLEARXMB</code></dt><dd>Extra 魔理沙 (B) のクリア達成度</dd>
 <dt><code>%T14CLEARNRA</code></dt><dd>Normal 霊夢 (A) のクリア達成度</dd>
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

### キャラごとの個別データ {: #T14CHARA }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T14CHARA[xx][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[SA]</code></dt><dd>咲夜 (A)</dd>
 <dt><code>[SB]</code></dt><dd>咲夜 (B)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T14CHARATL2</code></dt><dd>全主人公合計のプレイ時間</dd>
 <dt><code>%T14CHARARA1</code></dt><dd>霊夢 (A) の総プレイ回数</dd>
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

### キャラごとの個別データ（詳細版） {: #T14CHARAEX }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T14CHARAEX[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度など
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
 <dt><code>[X]</code></dt><dd>Extra</dd>
 <dt><code>[T]</code></dt><dd>Total</dd>
</dl>
   （総プレイ回数とプレイ時間ではこの指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前など
<dl class="format">
 <dt><code>[TL]</code></dt><dd>全主人公合計</dd>
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[SA]</code></dt><dd>咲夜 (A)</dd>
 <dt><code>[SB]</code></dt><dd>咲夜 (B)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>総プレイ回数</dd>
 <dt><code>[2]</code></dt><dd>プレイ時間</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T14CHARAEXETL2</code></dt><dd>全主人公合計のプレイ時間</dd>
 <dt><code>%T14CHARAEXERA1</code></dt><dd>霊夢 (A) の総プレイ回数</dd>
 <dt><code>%T14CHARAEXTSB3</code></dt><dd>咲夜 (B) の全難易度合計のクリア回数</dd>
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

### プラクティススコア {: #T14PRAC }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T14PRAC[x][yy][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   難易度
<dl class="format">
 <dt><code>[E]</code></dt><dd>Easy</dd>
 <dt><code>[N]</code></dt><dd>Normal</dd>
 <dt><code>[H]</code></dt><dd>Hard</dd>
 <dt><code>[L]</code></dt><dd>Lunatic</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[yy]</code></td>
  <td>
   キャラの名前
<dl class="format">
 <dt><code>[RA]</code></dt><dd>霊夢 (A)</dd>
 <dt><code>[RB]</code></dt><dd>霊夢 (B)</dd>
 <dt><code>[MA]</code></dt><dd>魔理沙 (A)</dd>
 <dt><code>[MB]</code></dt><dd>魔理沙 (B)</dd>
 <dt><code>[SA]</code></dt><dd>咲夜 (A)</dd>
 <dt><code>[SB]</code></dt><dd>咲夜 (B)</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   ステージ
<dl class="format">
 <dt><code>[1～6]</code></dt><dd>Stage 1～6</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T14PRACESB1</code></dt>
 <dd>Easy 咲夜 (B) の Stage 1 のプラクティススコア</dd>
 <dt><code>%T14PRACNRA4</code></dt>
 <dd>Normal 霊夢 (A) の Stage 4 のプラクティススコア</dd>
</dl>
  </td>
 </tr>
</table>

----------------------------------------

## 弾幕アマノジャク用テンプレート書式 {: #Th143Formats }

### スコア一覧 {: #T143SCR }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="5">書式</td>
  <td colspan="2"><code>%T143SCR[w][x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[w]</code></td>
  <td>
   日付
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>一日目～九日目</dd>
 <dt><code>[L]</code></dt><dd>最終日</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
 <dt><code>[0]</code></dt><dd>10</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   アイテムなど
<dl class="format">
 <dt><code>[0]</code></dt><dd>ノーアイテム</dd>
 <dt><code>[1]</code></dt><dd>ひらり布</dd>
 <dt><code>[2]</code></dt><dd>天狗のトイカメラ</dd>
 <dt><code>[3]</code></dt><dd>隙間の折りたたみ傘</dd>
 <dt><code>[4]</code></dt><dd>亡霊の送り提灯</dd>
 <dt><code>[5]</code></dt><dd>血に飢えた陰陽玉</dd>
 <dt><code>[6]</code></dt><dd>四尺マジックボム</dd>
 <dt><code>[7]</code></dt><dd>身代わり地蔵</dd>
 <dt><code>[8]</code></dt><dd>呪いのデコイ人形</dd>
 <dt><code>[9]</code></dt><dd>打ち出の小槌（レプリカ）</dd>
 <dt><code>[T]</code></dt><dd>合計</dd>
</dl>
   （項目が得点の場合、この指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>得点</dd>
 <dt><code>[2]</code></dt><dd>挑戦回数</dd>
 <dt><code>[3]</code></dt><dd>クリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T143SCR11T1</code></dt><dd>一日目 シーン 1 の得点</dd>
 <dt><code>%T143SCR2313</code></dt><dd>二日目 シーン 3 のひらり布でのクリア回数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない日付とシーンの組み合わせ（一日目 シーン 7 など）は無視されます。</li>
</ul>
  </td>
 </tr>
</table>

### スコア合計 {: #T143SCRTL }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T143SCRTL[x][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   アイテムなど
<dl class="format">
 <dt><code>[0]</code></dt><dd>ノーアイテム</dd>
 <dt><code>[1]</code></dt><dd>ひらり布</dd>
 <dt><code>[2]</code></dt><dd>天狗のトイカメラ</dd>
 <dt><code>[3]</code></dt><dd>隙間の折りたたみ傘</dd>
 <dt><code>[4]</code></dt><dd>亡霊の送り提灯</dd>
 <dt><code>[5]</code></dt><dd>血に飢えた陰陽玉</dd>
 <dt><code>[6]</code></dt><dd>四尺マジックボム</dd>
 <dt><code>[7]</code></dt><dd>身代わり地蔵</dd>
 <dt><code>[8]</code></dt><dd>呪いのデコイ人形</dd>
 <dt><code>[9]</code></dt><dd>打ち出の小槌（レプリカ）</dd>
 <dt><code>[T]</code></dt><dd>合計</dd>
</dl>
   （項目が得点の合計の場合、この指定は無視されます。）
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>得点の合計</dd>
 <dt><code>[2]</code></dt><dd>挑戦回数の合計</dd>
 <dt><code>[3]</code></dt><dd>クリア回数の合計</dd>
 <dt><code>[4]</code></dt><dd>クリアシーン数</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T143SCRTLT1</code></dt><dd>得点の合計</dd>
 <dt><code>%T143SCRTL04</code></dt><dd>ノーアイテムでのクリアシーン数</dd>
</dl>
  </td>
 </tr>
</table>

### スペルカード情報 {: #T143CARD }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T143CARD[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   日付
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>一日目～九日目</dd>
 <dt><code>[L]</code></dt><dd>最終日</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
 <dt><code>[0]</code></dt><dd>10</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>敵の名前</dd>
 <dt><code>[2]</code></dt><dd>スペルカード名</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T143CARD111</code></dt><dd>一日目 シーン 1 の敵の名前</dd>
 <dt><code>%T143CARD232</code></dt><dd>二日目 シーン 3 のスペルカード名</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない日付とシーンの組み合わせ（一日目 シーン 7 など）は無視されます。</li>
 <li><a href="../manual.html#HowToUse">未挑戦のスペルカード名を出力しない</a>設定にしている場合、該当するものは敵の名前・スペルカード名ともに「<samp>?????</samp>」のように出力されます。（一応ネタバレ防止のため。）</li>
</ul>
  </td>
 </tr>
</table>

### 異名情報 {: #T143NICK }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="2">書式</td>
  <td colspan="2"><code>%T143NICK[xx]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[xx]</code></td>
  <td>
   番号
<dl class="format">
 <dt><code>[01～70]</code></dt><dd>1～70</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T143NICK11</code></dt><dd>はじめてのアマノジャク</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>未取得の異名は「<samp>?????</samp>」のように出力されます。</li>
 <li>No.29 の異名のゲーム内表示は誤記と思われるため、修正しています。</li>
</ul>
  </td>
 </tr>
</table>

### 総プレイ時間 {: #T143TIMEPLY }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td>書式</td>
  <td><code>%T143TIMEPLY</code></td>
 </tr>
 <tr>
  <td>補足</td>
  <td>
<ul>
 <li>時分秒およびミリ秒が「<samp>h:mm:ss.ddd</samp>」の形式で出力されます。</li>
</ul>
  </td>
 </tr>
</table>

### スクリーンショット出力 {: #T143SHOT }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="3">書式</td>
  <td colspan="2"><code>%T143SHOT[x][y]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   日付
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>一日目～九日目</dd>
 <dt><code>[L]</code></dt><dd>最終日</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
 <dt><code>[0]</code></dt><dd>10</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T143SHOT12</code></dt><dd>一日目 シーン 2 のスクリーンショット</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない日付とシーンの組み合わせ（一日目 シーン 7 など）は無視されます。</li>
 <li>
  このテンプレート書式は「<samp>&lt;img src="./screenshot/sc01_01.png" alt="～" title="～" border=0&gt;</samp>」のような HTML の IMG タグに置換されます。<br />
  同時に、対象となるスクリーンショットファイル (sc??_??.dat<!--_-->) を PNG 形式に変換した画像ファイルが出力されます。
 </li>
 <li>IMG タグの alt 属性と title 属性には日時が出力されます。</li>
 <li>画像ファイルは、「出力先(O):」欄で指定されたフォルダ内の「画像出力先(I):」欄で指定されたフォルダに出力されます。</li>
 <li>画像ファイルの出力先フォルダが存在しない場合、本ツールが自動で作成します。</li>
 <li>スクリーンショットファイルが存在しない場合、IMG タグや画像ファイルは出力されません。</li>
 <li>スクリーンショットファイルの変換は、このテンプレート書式がテンプレートファイル内に無くても実行されます。</li>
</ul>
  </td>
 </tr>
</table>

### スクリーンショット出力（詳細版） {: #T143SHOTEX }

<table>
 <colgroup class="header"></colgroup>
 <colgroup span="2"></colgroup>
 <tr>
  <td rowspan="4">書式</td>
  <td colspan="2"><code>%T143SHOTEX[x][y][z]</code></td>
 </tr>
 <tr>
  <td class="format"><code>[x]</code></td>
  <td>
   日付
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>一日目～九日目</dd>
 <dt><code>[L]</code></dt><dd>最終日</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[y]</code></td>
  <td>
   シーン
<dl class="format">
 <dt><code>[1～9]</code></dt><dd>1～9</dd>
 <dt><code>[0]</code></dt><dd>10</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td class="format"><code>[z]</code></td>
  <td>
   項目
<dl class="format">
 <dt><code>[1]</code></dt><dd>画像ファイルへの相対パス</dd>
 <dt><code>[2]</code></dt><dd>画像ファイルの幅 (px)</dd>
 <dt><code>[3]</code></dt><dd>画像ファイルの高さ (px)</dd>
 <dt><code>[4]</code></dt><dd>スクリーンショット撮影日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>例</td>
  <td colspan="2">
<dl class="example">
 <dt><code>%T143SHOTEX121</code></dt><dd>一日目 シーン 2 の画像ファイルへの相対パス</dd>
 <dt><code>%T143SHOTEX234</code></dt><dd>二日目 シーン 3 のスクリーンショット撮影日時</dd>
</dl>
  </td>
 </tr>
 <tr>
  <td>補足</td>
  <td colspan="2">
<ul>
 <li>存在しない日付とシーンの組み合わせ（一日目 シーン 7 など）は無視されます。</li>
 <li>
  このテンプレート書式を使って、例えば <code>%T143SHOT12</code> と同等の出力結果を得るには、テンプレートファイルに以下の通りに記載します。
<pre><code>&lt;img src="%T143SHOTEX121" alt="SpellName: %T143CARD122" title="SpellName: %T143CARD122" border=0&gt;
</code></pre>
 </li>
</ul>
  </td>
 </tr>
</table>
