<style type="text/css">
<!--
    div.toc ul ul ul li { display: inline; }
-->
</style>

# ThScoreFileConverter User's Manual {: #UsersManual }

## Table of Contents {: #ToC }

[TOC]

<ul>
 <li><a href="manual/format.html">テンプレート書式</a></li>
</ul>

----------------------------------------

## Introduction {: #Introduction }

ThScoreFileConverter （以下、本ツール）は、東方 Project の各作品のスコアファイルからランキングや御札戦歴などのデータを読み込み、任意のテキストファイルに書き出すことのできるツールです。

「[東方メモリマネージャー][ThMM]」の代替ツールとなることを目指して開発中です。

  [ThMM]: http://www.sue445.net/downloads/ThMemoryManager.html

----------------------------------------

## Disclaimer {: #Disclaimer }

**本ツールの利用は自己責任の下で実施して下さい。**

本ツールの免責事項などの詳細は [License](#License) の通りとします。（つまり BSD 2-Clause License です。）

----------------------------------------

## Supported Titles {: #SupportedTitles }

### 対応済み {: #Supported }

対応している東方 Project の作品名・バージョン:

* 東方紅魔郷 ver. 1.02h
* 東方妖々夢 ver. 1.00b
* 東方萃夢想 ver. 1.11
* 東方永夜抄 ver. 1.00d
* 東方花映塚 ver. 1.50a
* 東方文花帖 ver. 1.02a
* 東方風神録 ver. 1.00a
* 東方緋想天 ver. 1.06a
* 東方地霊殿 ver. 1.00a
* 東方星蓮船 ver. 1.00b
* 東方非想天則 ver. 1.10a
* ダブルスポイラー ver. 1.00a
* 妖精大戦争 ver. 1.00a
* 東方神霊廟 ver. 1.00c
* 東方心綺楼 ver. 1.34b
* 東方輝針城 ver. 1.00b
* 弾幕アマノジャク ver. 1.00a

### 対応予定あり {: #WillSupport }

以下作品については、本ツール作者の原作プレイ進捗状況に伴って今後対応予定です:

* 東方深秘録
* 東方紺珠伝

----------------------------------------

## Environments {: #Environments }

.NET Framework 3.5 がインストールされている環境（[参考記事][@IT]）であれば動作すると思います。

  [@IT]: http://www.atmarkit.co.jp/ait/articles/1211/16/news093.html "Windows TIPS: .NET Frameworkのバージョンを整理する - @IT"

### ビルド環境 {: #BuiltEnv }

* Windows 7 Professional SP1 (64bit)
* Visual Studio Express 2012 for Windows Desktop Update 4
* .NET Framework 3.5 Client Profile

### 動作確認済み環境 {: #TestedEnv }

* Windows XP Home Edition SP3 (32bit) + .NET Framework 3.5 SP1 （Ver.1.6.1.0 まで確認済み）
* Windows 7 Professional SP1 (64bit) + .NET Framework 3.5 SP1
* Windows 8 (64bit) + .NET Framework 3.5 SP1 （Ver.1.2.0.0 まで確認済み）
* Windows 8.1 (64bit) + .NET Framework 3.5 SP1

----------------------------------------

## Installation {: #Installation }

### インストール {: #Install }

Web からダウンロードした zip ファイルを任意の場所に展開して下さい。

### アンインストール {: #Uninstall }

展開したフォルダごと削除して下さい。本ツールはレジストリの書き換えをしません。

----------------------------------------

## How to Use {: #HowToUse }

### メイン画面 {: #MainWindow }

![メイン画面のスクリーンショット](manual/screenshot-main.png)

1. ThScoreFileConverter.exe を起動する。
2. 左上のコンボボックスから作品名を選択する。
3. 2.で選択した作品について初めて実行する場合、以下の手順を行う。  
以前に一度でも実行していた場合は、その時の選択状況が設定ファイル (settings.xml) から読み込まれるため、以下の手順は省略可。
    1. スコアファイル（score.dat など）を選択する。
        * 「スコア(S):」欄右側の「開く(O)...」ボタンを押し、ファイル選択ダイアログから選択する。
        * 「スコア(S):」欄にスコアファイルを Drag & Drop する。
    2. ベストショットファイル（bs\_01\_1.dat など）が保存されているフォルダを選択する。（東方文花帖、ダブルスポイラーのみ）
        * 「ベストショット(B):」欄右側の「開く(O)...」ボタンを押し、フォルダ選択から選択する。
        * 「ベストショット(B):」欄にそのフォルダを Drag & Drop する。
    3. [テンプレートファイル](manual/format.html#AboutTemplateFile)を選択する。（複数選択可）
        * 「テンプレート(T):」欄右側の「追加(A)...」「削除(C)」「全て削除(L)」ボタンを使って選択する。
        * 「テンプレート(T):」欄にテンプレートファイルを Drag & Drop する。（複数可）
    4. 変換後のファイルの出力先フォルダを選択する。
        * 「出力先(O):」欄右側の「開く(O)...」ボタンを押し、フォルダ選択ダイアログから選択する。
        * 「出力先(O):」欄にそのフォルダを Drag & Drop する。
    5. ベストショットファイル変換後の画像ファイルの出力先フォルダ名を入力する。（東方文花帖、ダブルスポイラーのみ）
    6. 未挑戦のスペルカード名も出力する場合はチェックを外す。（東方花映塚以外）
4. 「変換(V)」ボタンを押す。

### 設定画面 {: #SettingWindow }

![設定画面のスクリーンショット](manual/screenshot-settings.png)

* フォント設定
    1. 「変更(C)...」ボタンを押してフォント選択ダイアログを表示する。
    2. 好みのフォントとサイズを選択する。
    3. 「OK」「キャンセル」「適用(A)」ボタンのどれかを押す。
        * 「OK」ボタン: 選択内容が即座に反映されフォント選択ダイアログが閉じる。
        * 「キャンセル」ボタン: 選択内容は破棄されフォント選択ダイアログが閉じる。
        * 「適用(A)」ボタン: 選択内容が即座に反映される。
    4. システム既定値に戻すには「リセット(R)」ボタンを押す。
* 出力書式設定
    * 「数値を桁区切り形式で出力する(S)」チェックボックスを押す。
        * チェックあり: 「1,234,567」のように出力される（但し西暦を除く）。
        * チェックなし: 「1234567」のように出力される。
* 文字エンコーディング設定
    * 「入力ファイル(I):」コンボボックスから、テンプレートファイルに使われている文字コードを選択する。
    * 「出力ファイル(O):」コンボボックスから、変換後のファイルに使う文字コードを選択する。

----------------------------------------

## Notes {: #Notes }

以下の注意点は、本ツールの今後の更新により変更される可能性があります。

* <del>基本的に、スコアファイルに記録されているものを、誤植なども含めてそのまま出力します。  
（スペルカード名などの一覧を本ツールの内部データとして全部持っておく、といったことはあまりしたくないので。）</del>  
<ins>Ver. 1.6.1.0 以降、本ツールの内部データとして全部持ち、それを出力するようにしました。</ins>
* 東方文花帖やダブルスポイラーのベストショット出力のためにプラグインを別途用意する必要は*ありません*。
* ソースコードの公開は、対応予定作品（[Supported Titles](#SupportedTitles) 参照）への対応が一通り済んでから、と考えています。  
（本ツール作者がこれらの作品を未プレイ and/or プレイ中の間に、他の誰かに先を越されたらちょっと悔しいので…。）
* フォント設定によっては、ボタンなどの文字列が一部見切れる場合があります。  
（ボタンなどのサイズを全て、メイリオ 12pt を前提として固定しているため。）
* フォントに対してアンチエイリアシングが常に有効になる（ＭＳ Ｐゴシックを選択すると分かり易い）のは、.NET Framework 3.x の仕様のようです。

----------------------------------------

## Copyrights {: #Copyrights }

* 東方 Project の各作品の著作権は[上海アリス幻樂団][ZUN]様にあります。本ツール作者とは一切関係がありません。
* 東方萃夢想、東方緋想天、東方非想天則、東方心綺楼の著作権は[黄昏フロンティア][tasofro]様及び[上海アリス幻樂団][ZUN]様にあります。本ツール作者とは一切関係がありません。
* 東方メモリマネージャーの著作権は [sue445][sue445] 様にあります。本ツール作者とは一切関係がありません。
* 本ツールの著作権は IIHOSHI Yoshinori ([Web site][MyWeb], [Twitter][MyTwitter]) にあります。

  [ZUN]: http://www16.big.or.jp/~zun/ "上海アリス幻樂団"
  [tasofro]: http://www.tasofro.net "Twilight-Frontier"
  [sue445]: http://www.sue445.net/ "sue445.NET"
  [thwiki]: http://thwiki.info/ "東方Wiki"
  [MyWeb]: http://www.colorless-sight.jp "Colorless Sight"
  [MyTwitter]: http://twitter.com/iihoshi

----------------------------------------

## License {: #License }

Copyright (c) 2013-2014, IIHOSHI Yoshinori  
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

----------------------------------------

## ChangeLog {: #ChangeLog }

201y/mm/dd ver. 1.10.1.0

* T.B.D.

2014/10/05 ver. 1.10.0.0

* 東方心綺楼に対応

2014/08/24 ver. 1.9.1.0

* 東方非想天則
    * スペルカード名の誤記を修正

2014/08/20 ver. 1.9.0.0

* 東方非想天則に対応
* 東方萃夢想
    * [スペルカード基本情報](manual/format.html#T75CARD)において、スペルカードの番号に 000 が指定された場合を無視していなかったため修正
* 東方永夜抄
    * [スコアランキング](manual/format.html#T08SCR)において、取得スペルカード一覧を正しく出力できていなかったため修正
* 東方文花帖
    * 存在しないレベルとシーンの組み合わせを無視していなかったため修正
* 東方緋想天
    * [スペルカード基本情報](manual/format.html#T105CARD)において、スペルカードの番号に 000 が指定された場合を無視していなかったため修正
* ダブルスポイラー
    * 存在しないレベルとシーンの組み合わせを無視していなかったため修正
* 弾幕アマノジャク
    * 存在しない日付とシーンの組み合わせを無視していなかったため修正
* 全体的にソースコードを整理（いつまでやるのか…）
* [テンプレート書式](manual/format.html)の誤記を修正

2014/07/13 ver. 1.8.1.0

* 入出力ファイルの文字エンコーディングを指定できるようにした
    * 今までは入出力ともに Shift\_JIS 固定だった
* 作品名コンボボックスのドロップダウンリストの高さを修正
* 東方萃夢想
    * 初期化処理の整理により処理時間を短縮
* 弾幕アマノジャク
    * テンプレートファイルを 1 つ追加（HTML5 Canvas を使用）
* 全体的にソースコードを整理
    * 結果的に本ツールのファイルサイズが若干縮小した

2014/06/08 ver. 1.8.0.0

* 東方緋想天に対応
* 弾幕アマノジャクに対応
* テンプレートファイルのフッター部分の誤記・リンク先を修正
* 全体的にソースコードを整理

2014/04/13 ver. 1.7.0.0

* 東方萃夢想に対応
* 全体的にソースコードを整理

2014/03/10 ver. 1.6.1.0

* スペルカードの情報を全て内部データとして持つようにした（Thanks to [東方Wiki][thwiki]）
* 未挑戦のスペルカード名を出力するかどうか選択可能にした
* スペルカードに挑戦済みかどうかの判定方法を一部修正
* 東方永夜抄
    * [スペルカード基本情報](manual/format.html#T08CARD)において、Last Word のスペルカードの難易度を Normal と出力していたため修正
    * [御札戦歴](manual/format.html#T08C)と[スペルカード蒐集率](manual/format.html#T08CRG)において、ゲーム本編と Last Word の組み合わせを無視していなかったため修正
* 東方神霊廟
    * [御札戦歴](manual/format.html#T13C)と[スペルカード蒐集率](manual/format.html#T13CRG)において、ゲーム本編と Over Drive の組み合わせを無視していなかったため修正
    * 御札戦歴（ゲーム本編）のテンプレートファイルに Over Drive の分を誤掲載していたため削除
* 全体的にソースコードを整理

2014/03/02 ver. 1.6.0.0

* 東方輝針城に対応
* 東方永夜抄
    * [クリア達成度](manual/format.html#T08CLEAR)において、<samp>FinalA Clear</samp> を出力する判定方法を修正
* 東方星蓮船
    * プラクティススコアのテンプレートファイルの誤記を修正（Ver.1.5.0.0 でのリリース漏れ）
* 妖精大戦争
    * スペルカード蒐集率のテンプレートファイルの誤記を修正
* 全体的にソースコードを整理

2014/02/03 ver. 1.5.1.0

* 東方神霊廟
    * [スコアランキング](manual/format.html#T13SCR)の到達ステージと[クリア達成度](manual/format.html#T13CLEAR)において、Extra クリア済みの場合に <samp>-------</samp> と出力していたため修正

2014/01/27 ver. 1.5.0.0

* 東方神霊廟に対応
* <del>東方星蓮船</del>
    * <del>プラクティススコアのテンプレートファイルの誤記を修正</del>
* 東方妖々夢
    * [スペルカード蒐集率](manual/format.html#T07CRG)で難易度を指定した場合に、ステージの指定に関係なく全ステージ合計の値を出力していたため修正
* [ビルド環境](#BuiltEnv)を .NET Framework 3.0 から 3.5 に変更
* 全体的にソースコードを整理

2013/12/10 ver. 1.4.0.0

* 妖精大戦争に対応

2013/11/16 ver. 1.3.1.0

* 東方文花帖、ダブルスポイラー
    * ベストショットファイル変換後の画像ファイルの出力先フォルダ名を変更可能にした

2013/11/11 ver. 1.3.0.0

* ダブルスポイラーに対応

2013/09/26 ver. 1.2.0.0

* 東方星蓮船に対応

2013/09/12 ver. 1.1.3.0

* 数値を桁区切り形式で出力するかどうか選択可能にした
* 東方妖々夢
    * [スコアランキング](manual/format.html#T07SCR)の日付と[スペルカード基本情報](manual/format.html#T07CARD)のスペルカード名において、余計な null 文字を出力していたため修正
* 東方永夜抄
    * [スコアランキング](manual/format.html#T08SCR)の日付と[スペルカード基本情報](manual/format.html#T08CARD)のスペルカード名において、余計な null 文字を出力していたため修正

2013/08/30 ver. 1.1.2.0

* UI のフォントを変更可能にした

2013/08/24 ver. 1.1.1.0

* 東方風神録
    * 全主人公合計のプレイ時間などがテンプレートファイルから漏れていたため修正
    * [キャラごとの個別データ（詳細版）](manual/format.html#T10CHARAEX)にて、全難易度合計のクリア回数の出力に対応
* 東方地霊殿
    * 全主人公合計のプレイ時間などがテンプレートファイルから漏れていたため修正
    * [キャラごとの個別データ（詳細版）](manual/format.html#T11CHARAEX)にて、全難易度合計のクリア回数の出力に対応

2013/08/12 ver. 1.1.0.0

* 東方地霊殿に対応

2013/07/28 ver. 1.0.2.0

* 東方紅魔郷
    * score.dat が初期状態の場合に変換失敗する不具合を修正
    * [プラクティススコア](manual/format.html#T06PRAC)を新規追加
* 東方妖々夢
    * score.dat が初期状態の場合に変換失敗する不具合を修正
    * [プラクティススコア](manual/format.html#T07PRAC)を新規追加
* 東方永夜抄
    * score.dat が初期状態の場合に変換失敗する不具合を修正
    * [プラクティススコア](manual/format.html#T08PRAC)を新規追加
* 東方風神録
    * [プラクティススコア](manual/format.html#T10PRAC)を新規追加

2013/07/21 ver. 1.0.1.0

* 東方風神録
    * Easy 以外のクリア回数が出力に含まれていなかった不具合を修正
    * [キャラごとの個別データ（詳細版）](manual/format.html#T10CHARAEX)を新規追加

2013/07/08 ver. 1.0.0.0

* 公開開始
* 東方紅魔郷～東方風神録に対応
