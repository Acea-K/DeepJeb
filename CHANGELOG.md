# Changelog

## v0.5.3.1

- Added `craft` to brace auto-completion language set (code blocks tagged ` ```craft ` or auto-detected by `PART`/`ship =` now get bracket auto-close)

## v0.5.3

- Completed localization entries for Settings form, model dropdown, and connection status
- Added KSP Craft Files skill: .craft format reference with PART fields, quaternion rotation, attachment nodes, mirror symmetry, radial sizes, stock part data, parser tools, and troubleshooting
- Removed .craft file restriction from system prompt (now aligns with new Craft Files skill)

## v0.5.2

**Streaming & Rendering**
- Scroll height system rewritten: self-correcting actual-rendered-height feedback replaces CalcHeight measurement — zero approximation
- Code blocks use per-line `CalcHeight` instead of fixed 16px (CJK/box-drawing chars now correctly measured)
- Horizontal rule (`---` / `***` / `___`) recognized and rendered as separator line
- Role label height fixed at 18px (was leaking stale `lineH` from previous message)

**Session & Security**
- System prompt zero-storage: never saved to session file, injected fresh at conversation start
- Session load: strips all old System messages, prepends current prompt, immediately usable
- Fixed 400 error on session load — Assistant messages with `tool_calls` no longer saved without Tool results
- `.craft` file generation restricted

**Skills**
- Added Realism Overhaul skill

**Fixes**
- Settings: model checkbox list scrollable; scrollbar state persisted
- Loaded session auto-scrolls to bottom
- Shift+Enter added as newline shortcut (alongside Ctrl+Enter)
- ClickThroughBlocker compatibility noted in documentation

## v0.5.1

**Display & Markdown**
- Heading H4 (`####`) now recognized and rendered correctly
- All heading (H1-H4), text, list, and table row heights measured via `GUIStyle.CalcHeight` at precise per-type widths — zero hardcoded heights
- Content height measurement passes match rendering passes exactly; margin formula `min(measured×10%, 200px)` applied only when content exceeds viewport
- Scroll-to-bottom uses continuous follow mode, auto-releases when user scrolls up
- Leading empty parsed lines trimmed to prevent unwanted top gaps
- `<` escaped with zero-width space to prevent Unity rich-text tag corruption in plain text

**Settings Window**
- Model checkbox list made scrollable — no longer overflows and blocks Save/Close buttons
- Presets panel height fills available form space instead of fixed 120px
- Presets scrollbar position persisted across frames

**Session & Security**
- System prompt never displayed in chat — all System messages filtered from render output
- Loading a session replaces old system prompt with current version
- Provider switch on session load validates model against provider's enabled list (prevents 400 Bad Request)

**Streaming**
- Trailing tokens from a streaming round flushed before sentinel-triggered finalization (prevents round-boundary token loss)

**UI**
- Chat and Settings windows centered on first open, adaptive to any resolution

## v0.5.0

- Initial release
