.. _introduction:

イントロダクション
==================

これは何？
----------

:term:`ThScoreFileConverter` (以下、 :term:`ThSFC`) は、 東方 Project
の各作品のスコアファイルからランキングや御札戦歴などのデータを読み込み、
任意のテキストファイルに書き出すことのできるツールです。
文花帖形式のベストショットの現像 (PNG 画像出力) も可能です。

:term:`東方メモリマネージャー` の代替ツールとなることを目指して開発中です。
特に、 風神録までの :term:`テンプレート書式` については互換性を維持しています。

前提条件
--------

ThSFC を利用するには .NET Framework 4.5 以降が必要です。

.NET Framework のバージョン間の互換性やインストール方法などについては、
下記リンク先の記事が参考になります:

* `.NET Frameworkのバージョンを整理する (1/2)：Tech TIPS - ＠IT
  <https://www.atmarkit.co.jp/ait/articles/1211/16/news093.html>`_

2019 年 7 月現在、 ThSFC は下記環境でのみ開発及び動作確認を実施しています。
下記以外の環境で動作しないなどのご報告を受けても対応できない可能性があります。

* Windows 10 Pro Version 1903 (64bit)
* .NET Framework 4.8
* Visual Studio Community 2019 Version 16.2.0
* Python 3.7.4

免責事項
--------

**ThSFC の利用は自己責任の下で実施して下さい。**

ThSFC の免責事項などの詳細は以下の通りです。
(つまり BSD 2-Clause License です。)

.. literalinclude:: ../LICENSE.txt
   :language: none

著作権
------

* 東方 Project の各作品の著作権は
  `上海アリス幻樂団 <https://www16.big.or.jp/~zun/>`_ 様にあります。
  ThSFC 作者とは一切関係がありません。
* 東方萃夢想、 東方緋想天、 東方非想天則、 東方心綺楼、 東方深秘録、
  東方憑依華の著作権は
  `黄昏フロンティア <https://www.tasofro.net/>`_ 様及び
  `上海アリス幻樂団 <https://www16.big.or.jp/~zun/>`_ 様にあります。
  ThSFC 作者とは一切関係がありません。
* 東方メモリマネージャーの著作権は
  `sue445 <http://www.sue445.net/>`_ 様にあります。
  ThSFC 作者とは一切関係がありません。
* ThSFC の著作権は IIHOSHI Yoshinori
  (`Web サイト <https://www.colorless-sight.jp/>`_,
  `Twitter <https://twitter.com/iihoshi>`_) にあります。
