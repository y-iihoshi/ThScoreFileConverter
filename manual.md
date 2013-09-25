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

## Environments {: #Environments }

.NET Framework 3.0 以上がインストールされている環境（[参考記事][@IT]）であれば動作すると思います。

  [@IT]: http://www.atmarkit.co.jp/ait/articles/1211/16/news093.html "Windows TIPS: .NET Frameworkのバージョンを整理する - @IT"

### ビルド環境 {: #BuiltEnv }

* Windows 7 Professional SP1 (64bit) + .NET Framework 3.0

### 動作確認済み環境 {: #TestedEnv }

* Windows XP Home Edition SP3 (32bit)  
以下をインストール済み。実際どれで動いているのかはよく分かっていない…（もちろん、ちゃんと調べれば分かるだろうけど、面倒）
    * .NET Framework 2.0 SP2 （これで動かないのは確か）
    * .NET Framework 3.0 SP2
    * .NET Framework 3.5 SP1
    * .NET Framework 4.0 Client Profile
* Windows 7 Professional SP1 (64bit) + .NET Framework 4.5
* Windows 8 (64bit) + .NET Framework 4.5

----------------------------------------

## Supported Titles {: #SupportedTitles }

対応している東方 Project の作品名・バージョン：

* 東方紅魔郷 ver. 1.02h
* 東方妖々夢 ver. 1.00b
* 東方永夜抄 ver. 1.00d
* 東方花映塚 ver. 1.50a
* 東方文花帖 ver. 1.02a
* 東方風神録 ver. 1.00a
* 東方地霊殿 ver. 1.00a
* 東方星蓮船 ver. 1.00b

以下作品については、本ツール作者の原作プレイ進捗状況に伴って今後対応予定です：

* ダブルスポイラー
* 妖精大戦争
* 東方神霊廟
* 東方輝針城

以下作品についても上記と同様ですが、本ツール作者は格ゲーが弾幕 STG 以上に下手なので、後回しです：

* 東方萃夢想
* 東方緋想天
* 東方非想天則
* 東方心綺楼

----------------------------------------

## Installation {: #Instllation }

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
    2. ベストショットファイル（bs\_01\_1.dat など）が保存されているフォルダを選択する。（東方文花帖のみ）
        * 「ベストショット(B):」欄右側の「開く(O)...」ボタンを押し、フォルダ選択から選択する。
        * 「ベストショット(B):」欄にそのフォルダを Drag & Drop する。
    3. [テンプレートファイル](manual/format.html#AboutTemplateFile)を選択する。（複数選択可）
        * 「テンプレート(T):」欄右側の「追加(A)...」「削除(C)」「全て削除(L)」ボタンを使って選択する。
        * 「テンプレート(T):」欄にテンプレートファイルを Drag & Drop する。（複数可）
    4. 変換後のファイルの出力先フォルダを選択する。
        * 「出力先(O):」欄右側の「開く(O)...」ボタンを押し、フォルダ選択ダイアログから選択する。
        * 「出力先(O):」欄にそのフォルダを Drag & Drop する。
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

----------------------------------------

## Notes {: #Notes }

以下の注意点は、本ツールの今後の更新により変更される可能性があります。

* 基本的に、スコアファイルに記録されているものを、誤植なども含めてそのまま出力します。  
（スペルカード名などの一覧を本ツールの内部データとして全部持っておく、といったことはあまりしたくないので。）
* 東方文花帖のベストショット出力のためにプラグインを別途用意する必要は*ありません*。
* ソースコードの公開は、対応予定作品（[Supported Titles](#SupportedTitles) 参照）への対応が一通り済んでから、と考えています。  
（本ツール作者がこれらの作品を未プレイ and/or プレイ中の間に、他の誰かに先を越されたらちょっと悔しいので…。）
* フォント設定によっては、ボタンなどの文字列が一部見切れる場合があります。  
（ボタンなどのサイズを全て、メイリオ 12pt を前提として固定しているため。）
* フォントに対してアンチエイリアシングが常に有効になる（ＭＳ Ｐゴシックを選択すると分かり易い）のは、.NET Framework 3.x の仕様のようです。

----------------------------------------

## Copyrights {: #Copyrights }

* 東方 Project の各作品の著作権は[上海アリス幻樂団][ZUN]様にあります。本ツール作者とは一切関係がありません。
* 東方メモリマネージャーの著作権は [sue445][sue445] 様にあります。本ツール作者とは一切関係がありません。
* 本ツールの著作権は IIHOSHI Yoshinori ([Web site][MyWeb], [Twitter][MyTwitter]) にあります。

  [ZUN]: http://www16.big.or.jp/~zun/ "上海アリス幻樂団"
  [sue445]: http://www.sue445.net/ "sue445.NET"
  [MyWeb]: http://www.colorless-sight.jp "Colorless Sight"
  [MyTwitter]: http://twitter.com/iihoshi

----------------------------------------

## License {: #License }

Copyright (c) 2013, IIHOSHI Yoshinori  
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

----------------------------------------

## ChangeLog {: #ChangeLog }

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
