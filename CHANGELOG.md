# Changelog

## v0.5.1

**Display & Markdown**
- Heading H4 (`####`) support added
- All line heights (H1-H4, Text, ListItem, OrderedItem, TableRow) measured via `GUIStyle.CalcHeight` at precise render-per-pass widths — no hardcoded heights
- Content height measurement matches rendering pass exactly (initial offset, label height, per-line width)
- Cumulative height drift eliminated; margin formula `min(measured×10%, 200px)` only applied when content exceeds viewport
- Scroll-to-bottom uses continuous follow mode, auto-releases on manual scroll-up
- Leading empty parsed lines trimmed, trailing preserved for message separation
- `<` escaped with ZWS to prevent Unity rich-text tag corruption

**Security**
- System prompt never displayed in chat — all System/Tool messages filtered at render time
- Session load replaces old system prompt with current pipeline version
- Provider switch on session load validates model against provider's enabled list (prevents 400)

**Provider & HTTP**
- `ExtraHeaders` moved from global static to per-request parameter (prevents cross-provider header leak)
- `onRequestCreated` callback chain enables `HttpWebRequest.Abort()` on Stop generation
- `_activeHttpRequest` volatile for thread-safe abort
- Anthropic streaming error events handled
- GoogleClient `functionCall` parts emitted for assistant tool calls

**Fixes**
- InputLock released on window hide (prevents permanent control lockout)
- `TrimDisplayMessages` preserves system + recent non-system messages
- PathSandbox directory-boundary check prevents same-prefix escape
- ContextManager uses `ConcurrentDictionary` + `OrderByDescending` for deterministic model matching
- `maxTokens` guarded against negative; `reservedTokens` accounts for user message in truncation
- Force-summary tool calls stripped from conversation (no orphan tool calls)
- `.bak` pruning keeps last 3 per file; `bytes_written` uses UTF-8 byte count
- `ParseArgs`/`GetString` deduplicated into `ToolJson` shared helper
- `AiResponse` moved to `DeepJeb.Core.Models`
- Windows centered on first open
- Settings presets scrollbar state persisted; model checkbox list scrollable

## v0.5.0

- Initial release
