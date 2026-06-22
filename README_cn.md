<p align="center">
  <img src="assets/Textures/DeepJebLogo.png" alt="DeepJeb Logo" width="256">
</p>

<h1 align="center">DeepJeb</h1>

<p align="center">
  <strong>Kerbal Space Program AI 助手</strong><br>
  一个内嵌于游戏中的 AI 聊天窗口，能读、能写、能帮你建造。<br>
  <em>KSP 1.12.5 · Unity 2019.2 · C# 7.3 · 零依赖</em>
</p>

<p align="center">
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="MIT License"></a>
  <a href="#"><img src="https://img.shields.io/badge/KSP-1.12.5-blue" alt="KSP 1.12.5"></a>
  <a href="#"><img src="https://img.shields.io/badge/version-0.5.0-green" alt="v0.5.0"></a>
</p>

<p align="center">
  <a href="README.md">English</a> | <strong>中文</strong>
</p>

---

## DeepJeb 是什么？

DeepJeb 将 AI 聊天窗口直接嵌入 KSP。你可以让它编写 Module Manager 补丁、配置模组、解释轨道力学、调试 kOS 脚本，或者设计一枚不会在半空中翻跟头的火箭。

DeepJeb 内置了全面的 **KSP 世界知识库**：游戏机制、轨道物理学（开普勒定律、delta-V、引力弹弓）、所有原版天体的参数、航天器设计原则、DLC 内容、模组开发惯例和社区资源。但它真正的威力在于**技能系统**——你可以将任何 `SKILL.md` 文档放入 `Skills/` 文件夹，AI 就会将其加载为领域知识。教它你最喜欢的模组、你的自定义行星包，或者你个人的建造规范。技能格式是开放且有文档的——你的专业知识，你的规则。

> **你需要自备 API Key。** DeepJeb 不包含也不提供任何 AI 服务——你将它连接到你自己的 OpenAI、Anthropic、Google Gemini、DeepSeek 或其他兼容的 API 账户。所有 API 流量直接从你的机器发送到你配置的提供商。你也可以将其指向本地部署的大语言模型（通过 Ollama、vLLM 或任何兼容 OpenAI 接口的端点），将一切完全掌控在自己手中。

---

## 能做什么？

DeepJeb 自带 **6 个内置知识库**（Claude 技能）和 **7 个文件系统工具**，AI 可以直接使用。

### 知识库（技能）

| 技能 | 描述 |
|------|------|
| **KSP 世界知识** | 原版游戏机制、轨道物理学、天体数据、航天器设计、DLC 内容、模组开发技巧、社区资源 |
| **Module Manager** | MM 补丁语法、运算符、排序指令、NEEDS/DEPENDS 检查、变量、常用模式 |
| **kOS 编程** | KerboScript 语言参考、飞行控制、机动节点、启动文件、触发器 |
| **kRPC 编程** | 架构、Python/C#/Lua 客户端 API、SpaceCenter、AutoPilot、飞船控制、流式数据 |
| **MechJeb** | 全部引导模块、生涯模式集成、通过 kRPC/kOS 实时修改参数 |
| **Realism Overhaul** | RO/RP-1/RSS 模组套件综合指南 — 68 个仓库、安装、部件、引擎、生命支持、历史航天器、故障排查 |

技能会根据你问题的关键词自动匹配——匹配度最高的前 2 个会被注入为上下文。

### AI 代理工具

| 工具 | AI 可以做什么 |
|------|-------------|
| `read_file` | 读取 GameData 中的任意文件 |
| `write_file` | 创建或覆盖文件（自动创建目录，备份旧版本） |
| `delete_file` | 删除文件（先创建带时间戳的 `.bak` 备份） |
| `list_directory` | 列出目录内容，包含文件大小和修改时间 |
| `file_exists` | 检查文件或目录是否存在 |
| `backup_file` | 创建带时间戳的 `.bak` 快照，不修改原文件 |
| `get_game_state` | 报告当前 KSP 游戏状态（飞船、轨道、生物群系、资源） |

AI 可以读取 Squad/SquadExpansion 目录下的文件，但**无法修改或删除**它们。

---

## 支持的服务商

**12 个内置预设 + 自定义服务商支持**，覆盖 3 个协议族：

| 协议 | 内置预设 |
|------|---------|
| **OpenAI 兼容** | OpenAI、DeepSeek、OpenRouter、Grok (xAI)、Mistral、Together AI、Perplexity、Groq、Ollama、vLLM、自定义 |
| **Anthropic** | Anthropic (Claude) |
| **Google Gemini** | Google (Gemini) |

自定义端点、API Key、模型列表和服务商名称都可以在游戏内通过设置窗口配置。模型列表从各 API 实时获取。

---

## 什么是 Claude 技能？

DeepJeb 的知识库是 **Claude 技能**——一种将领域专业知识打包给 AI 助手的标准格式。每个技能是一个 `SKILL.md` 文件，包含 YAML 前置元数据（名称、描述、触发条件）和包含知识内容的 Markdown 正文。技能放置在 `Skills/` 目录下，启动时加载。

### 技能如何工作

- **[Claude Code 技能文档](https://docs.anthropic.com/en/docs/claude-code/skills)** — 官方指南
- **[创建自定义技能](https://docs.anthropic.com/en/docs/claude-code/skills#creating-custom-skills)** — 编写指南

要向 DeepJeb 添加你自己的技能，在 `GameData/DeepJeb/Skills/{分类}/{名称}/` 中创建一个 `SKILL.md` 文件：

```yaml
---
name: 你的技能名称
description: >
  这个技能涵盖的内容。
---
# 你的知识内容
```

参考文件（脚本、表格、示例）可以放在 `references/` 子目录中——匹配时会随技能一起注入。

### 条件激活技能

你可以使用 `when_to_use` 前置字段，让技能仅在特定模组存在时才激活。AI 代理可以检查 `GameData/` 来确认模组是否安装：

```yaml
---
name: my-mod-guide
description: >
  MyMod 的知识库。仅在模组已安装时激活。
when_to_use: |
  当用户的 GameData 文件夹中包含 "MyMod" 时触发。
condition: file_exists("MyMod/") -> true
---
# MyMod 配置指南
```

使用 `file_exists` 或 `list_directory` 工具调用作为条件来控制技能加载——这样 DeepJeb 就不会为你未安装的模组加载无关知识。

---

## 安装

1. 将 `DeepJeb/` 文件夹复制到你的 KSP `GameData/` 目录
2. 启动 KSP——DeepJeb 工具栏图标会出现在所有场景中
3. 点击图标打开聊天窗口
4. 打开设置，配置 API 服务商和模型
5. 开始聊天

**要求：** KSP 1.12.0+（在 1.12.5 上测试）。无需额外模组或依赖。

---

## 已知问题

- **模型可用性**：模型列表从各 API 服务商实时获取。如果 API 不可达，模型下拉菜单会显示上次缓存的列表或持续显示"加载中…"。请检查你的 API Key 和网络连接。
- **上下文截断**：当接近模型的上下文窗口限制时，过长的对话可能会丢失较早的消息。长时间会话建议定期使用 `/clear`。
- **UI 缩放**：聊天窗口使用固定像素尺寸（默认 600×500）。在非常小或非常大的屏幕上，布局可能无法理想缩放。
- **KSP 场景切换**：聊天窗口在场景切换时保持存在，但进行中的 AI 生成会在场景加载时停止。
- **Google Gemini API Key**：由于 Gemini API 使用查询参数方式认证，使用 Alt+F12 调试时 API Key 可能出现在 KSP 的调试控制台日志中。
- **流式性能**：对于非常长的 AI 回复，逐 token 渲染过程中可能出现轻微的 UI 帧率波动。
- **ClickThroughBlocker**：如果你安装了 ClickThroughBlocker，可能需要点击两次 DeepJeb 图标才能打开或关闭窗口。这是正常现象——DeepJeb 使用自己的点击穿透检测机制。

---

## 许可证

[MIT License](LICENSE)

Copyright © 2026 Acea - 使用 Claude Code / DeepSeek V4 Pro 开发

MiniJSON 基于 Calvin Rien 的公有领域实现。

---

<p align="center">
  <sub>为 Kerbal Space Program 模组社区而建。Fly safe.</sub>
</p>
