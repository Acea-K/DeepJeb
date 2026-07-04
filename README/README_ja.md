<p align="center">
  <img src="../assets/Textures/DeepJebLogo.png" alt="DeepJeb ロゴ" width="256">
</p>

<h1 align="center">DeepJeb</h1>

<p align="center">
  <strong>Kerbal Space Program AI アシスタント</strong><br>
  読み取り、書き込み、構築を支援するゲーム内蔵の AI/LLM チャットウィンドウ。<br>
  <em>KSP 1.12.5 · Unity 2019.2 · C# 7.3 · ゼロ依存</em>
</p>

<p align="center">
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="MIT ライセンス"></a>
  <a href="#"><img src="https://img.shields.io/badge/KSP-1.12.5-blue" alt="KSP 1.12.5"></a>
  <a href="#"><img src="https://img.shields.io/badge/version-0.5.4-green" alt="v0.5.4"></a>
</p>

<p align="center">
  <a href="../README.md">English</a> | <a href="README_cn.md">简体中文</a> | <a href="README_de.md">Deutsch</a> | <a href="README_fr.md">Français</a> | <a href="README_it.md">Italiano</a> | <strong>日本語</strong> | <a href="README_pt.md">Português</a> | <a href="README_ru.md">Русский</a> | <a href="README_es.md">Español</a>
</p>

---

## DeepJeb とは？

DeepJeb は AI チャットウィンドウを KSP に直接組み込みます。Module Manager パッチの作成、MOD の設定、軌道力学の説明、kOS スクリプトのデバッグ、上昇時に転倒しないロケットの設計など、何でも尋ねられます。

DeepJeb には包括的な **KSP ワールド知識ベース**が付属しています：ゲームメカニクス、軌道物理学（ケプラーの法則、デルタ V、重力アシスト）、全てのストック天体、宇宙機設計原則、DLC コンテンツ、MOD 開発の慣例、コミュニティリソース。しかし真の力は**スキルシステム**にあります — 任意の `SKILL.md` ドキュメントを `Skills/` フォルダに配置するだけで、AI がドメイン知識として読み込みます。お気に入りの MOD、カスタム惑星パック、独自の建造ルールを教えられます。スキル形式はオープンで文書化されています — あなたの専門知識、あなたのルール。

> **独自の API キーが必要です。** DeepJeb は AI サービスを含まず、提供もしません — お客様自身の OpenAI、Anthropic、Google Gemini、DeepSeek、またはその他の互換 API アカウントに接続します。全ての API トラフィックは、お客様のマシンから設定されたプロバイダーに直接送信されます。Ollama、vLLM、または任意の OpenAI 互換エンドポイントを介してローカルに展開された LLM に接続し、全てを完全に制御下に置くこともできます。
>
> **API キーの保存方法。** メモリ上では、キーは平文で保持されます（API 認証に必要）。ディスク上では、キーは XOR 難読化と Base64 エンコーディングを使用して暗号化され、設定ファイルに平文で書き込まれることはありません。OpenAI と Anthropic の場合、API キーは HTTP Bearer トークンヘッダーとして送信され、KSP デバッグコンソールには記録されません。Google Gemini API は例外です — 下記の既知の問題を参照してください。

---

## できること

DeepJeb には **7 つの組み込み知識ベース**（Agent スキル）と AI が使用できる **7 つのファイルシステムツール**が付属しています。

### 知識ベース（スキル）

| スキル | 説明 |
|-------|-------------|
| **KSP ワールド知識** | ストックゲームメカニクス、軌道物理学、天体、宇宙機設計、DLC コンテンツ、MOD 開発のヒント、コミュニティリソース |
| **KSP クラフトファイル** | .craft ファイル形式、PARTフィールド、クォータニオン回転、接続ノード、ミラー対称、ラジアルサイズ、ストックパーツ参照、パーサーツール、トラブルシューティング |
| **Module Manager** | MM パッチ構文、演算子、順序指定子、NEEDS/DEPENDS チェック、変数、一般的なパターン |
| **kOS プログラミング** | KerboScript 言語リファレンス、フライト制御、マニューバノード、ブートファイル、トリガー |
| **kRPC プログラミング** | アーキテクチャ、Python/C#/Lua クライアント API、SpaceCenter、AutoPilot、船体制御、ストリーミングデータ |
| **MechJeb** | 全ての誘導モジュール、キャリア統合、kRPC/kOS 経由のリアルタイム値変更 |
| **Realism Overhaul** | RO/RP-1/RSS MOD スイートの包括的ガイド — 68 リポジトリ、インストール、パーツ、エンジン、生命維持、歴史的宇宙機、トラブルシューティング |

スキルは質問とのキーワード重複によって自動的にマッチングされ — 上位 2 件がコンテキストとして注入されます。

### AI エージェントツール

| ツール | AI ができること |
|------|-------------------|
| `read_file` | GameData 内の任意のファイルを読み取る |
| `write_file` | ファイルを作成または上書き（ディレクトリを自動作成し、以前のバージョンをバックアップ） |
| `delete_file` | ファイルを削除（タイムスタンプ付き `.bak` バックアップを先に作成） |
| `list_directory` | ファイルサイズと変更時刻付きでディレクトリ内容を一覧表示 |
| `file_exists` | ファイルまたはディレクトリの存在を確認 |
| `backup_file` | 元ファイルを変更せずにタイムスタンプ付き `.bak` スナップショットを作成 |
| `get_game_state` | 現在の KSP ゲーム状態を報告（船体、軌道、バイオーム、リソース） |

AI は Squad/SquadExpansion ファイルを読み取れますが、**変更または削除はできません**。

### スラッシュコマンド

チャット入力に `/` を入力してローカルでコマンドを実行 — AI の往復なし：

| コマンド | 機能 |
|--------|---------|
| `/retry` | 最後のメッセージを AI に再送信 |
| `/undo` | セッションから最後の交換ペアを削除 |
| `/help` | 利用可能なすべてのコマンドを一覧表示 |
| `/session` | 現在のセッション情報を表示（プロバイダー、モデル、メッセージ数） |
| `/game` | 現在の KSP ゲーム状態を表示（シーン、船体、軌道、バイオーム、リソース） |

---

## 対応プロバイダー

**12 の組み込みプリセット + カスタムプロバイダー対応**、3 つのプロトコルファミリー：

| プロトコル | 組み込みプリセット |
|----------|-----------------|
| **OpenAI 互換** | OpenAI、DeepSeek、OpenRouter、Grok (xAI)、Mistral、Together AI、Perplexity、Groq、Ollama、vLLM、カスタム |
| **Anthropic** | Anthropic (Claude) |
| **Google Gemini** | Google (Gemini) |

カスタムエンドポイント、API キー、モデルリスト、プロバイダー名はすべて、ゲーム内の設定ウィンドウから設定可能です。モデルリストは各 API からライブで取得されます。

---

## Agent スキルとは？

DeepJeb の知識ベースは **Agent スキル**です — AI アシスタントと共にドメイン専門知識をパッケージ化するための標準形式です。各スキルは、YAML フロントマター（名前、説明、トリガー）と知識を含む Markdown 本文を持つ `SKILL.md` ファイルです。スキルは `Skills/` ディレクトリに配置され、起動時に読み込まれます。

### スキルの仕組み

- **[Agent スキルドキュメント](https://docs.anthropic.com/ja/docs/claude-code/skills)** — 公式ガイド（Claude Code ドキュメントより）
- **[カスタムスキルの作成](https://docs.anthropic.com/ja/docs/claude-code/skills#creating-custom-skills)** — 作成ガイド（Claude Code ドキュメントより）

DeepJeb に独自のスキルを追加するには、`GameData/DeepJeb/Skills/{カテゴリ}/{名前}/` に `SKILL.md` ファイルを作成します：

```yaml
---
name: あなたのスキル名
description: >
  このスキルがカバーする内容。
---
# ここに知識コンテンツを記述
```

参照ファイル（スクリプト、表、例）は `references/` サブディレクトリに配置できます — マッチ時にスキルと共に注入されます。

### 条件付きスキルアクティベーション

`when_to_use` フロントマターフィールドを使用して、特定の MOD が存在する場合にのみスキルをアクティブにすることができます。AI エージェントはスキルを読み込む前に `GameData/` でインストール済み MOD を確認できます：

```yaml
---
name: my-mod-guide
description: >
  MyMod の知識ベース。MOD がインストールされている場合のみアクティブ化。
when_to_use: |
  ユーザーの GameData フォルダに "MyMod" が含まれている場合にトリガー。
condition: file_exists("MyMod/") -> true
---
# MyMod 設定ガイド
```

`file_exists` または `list_directory` ツール呼び出しを条件として使用してスキルの読み込みを制御します — これにより、DeepJeb はインストールされていない MOD の無関係な知識を読み込みません。

---

## インストール

1. `DeepJeb/` フォルダを KSP の `GameData/` ディレクトリにコピー
2. KSP を起動 — DeepJeb ツールバーアイコンが全シーンに表示されます
3. アイコンをクリックしてチャットウィンドウを開く
4. 設定を開いて API プロバイダーとモデルを設定
5. チャットを開始

> **ヒント:** Enter で送信、**Ctrl+Enter** または **Shift+Enter** で改行。

**要件:** KSP 1.12.0+（1.12.5 でテスト済み）。追加の MOD や依存関係は不要です。

---

## 既知の問題

- **モデルの可用性**: モデルリストは各 API プロバイダーからライブで取得されます。API に到達できない場合、モデルドロップダウンは最後にキャッシュされたリストを表示するか、「読み込み中...」と表示され続けます。API キーとネットワーク接続を確認してください。
- **コンテキストの切り詰め**: 非常に長い会話は、モデルのコンテキストウィンドウ制限に近づくと古いメッセージを失う可能性があります。長いセッションでは定期的に `/clear` を使用してください。
- **UI スケーリング**: チャットウィンドウは固定ピクセル寸法（デフォルト 600×500）を使用します。非常に小さいまたは非常に大きい画面では、レイアウトが理想的にスケールしない場合があります。
- **KSP シーン遷移**: チャットウィンドウはシーン変更をまたいで存続しますが、進行中の AI 生成はシーン読み込み時に停止します。
- **Google Gemini API キー**: Gemini API では、API キーを URL クエリパラメータとして渡す必要があります（これは Google の設計であり、DeepJeb の選択ではありません）。そのため、Google Gemini プロバイダーを使用する場合、Alt+F12 デバッグ使用時に API キーが KSP デバッグコンソールログに**平文で表示される可能性**があります。OpenAI と Anthropic のキーは HTTP ヘッダーとして送信され、ログに記録されません。
- **ストリーミングパフォーマンス**: 非常に長い AI 応答は、トークンごとのレンダリング中に UI フレームレートのわずかな変動を引き起こす可能性があります。
- **ClickThroughBlocker**: ClickThroughBlocker がインストールされている場合、DeepJeb ツールバーアイコンを 2 回クリックしてウィンドウを開閉する必要がある場合があります。これは想定通りです — DeepJeb は独自のクリックスルー検出を使用しています。

---

## ライセンス

[MIT ライセンス](LICENSE)

Copyright © 2026 Acea - Codex / DeepSeek V4 Pro を使用して開発

MiniJSON は Calvin Rien によるパブリックドメイン実装に基づいています。

`ksp-craft-files` スキル内の 3 つの Python ファイル（`ksparser.py`、`import_craft.py`、`part_dict.py`）は、Spencer Arrasmith による [io_kspblender](https://github.com/spencerarrasmith/io_kspblender) から派生したもので、[GPL-2.0](https://www.gnu.org/licenses/old-licenses/gpl-2.0.html) ライセンスの下で提供されています。参照とデモンストレーションのみを目的として含まれています。

---

<p align="center">
  <sub>Kerbal Space Program MOD コミュニティのために構築。Fly safe.</sub>
</p>
