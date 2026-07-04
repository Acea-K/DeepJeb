<p align="center">
  <img src="../assets/Textures/DeepJebLogo.png" alt="DeepJeb Logo" width="256">
</p>

<h1 align="center">DeepJeb</h1>

<p align="center">
  <strong>KI-Assistent für Kerbal Space Program</strong><br>
  Ein integriertes KI/LLM-Chatfenster, das liest, schreibt und beim Bauen hilft.<br>
  <em>KSP 1.12.5 · Unity 2019.2 · C# 7.3 · Keine Abhängigkeiten</em>
</p>

<p align="center">
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="MIT License"></a>
  <a href="#"><img src="https://img.shields.io/badge/KSP-1.12.5-blue" alt="KSP 1.12.5"></a>
  <a href="#"><img src="https://img.shields.io/badge/version-0.5.4-green" alt="v0.5.4"></a>
</p>

<p align="center">
  <a href="../README.md">English</a> | <a href="README_cn.md">简体中文</a> | <strong>Deutsch</strong> | <a href="README_fr.md">Français</a> | <a href="README_it.md">Italiano</a> | <a href="README_ja.md">日本語</a> | <a href="README_pt.md">Português</a> | <a href="README_ru.md">Русский</a> | <a href="README_es.md">Español</a>
</p>

---

## Was ist DeepJeb?

DeepJeb bettet ein KI-Chatfenster direkt in KSP ein. Du kannst es alles fragen — schreibe Module Manager-Patches, konfiguriere Mods, erkläre Orbitalmechanik, debugge ein kOS-Skript oder entwerfe eine Rakete, die beim Aufstieg nicht abkippt.

DeepJeb enthält eine umfassende **KSP-Weltwissensbasis**: Spielmechaniken, Orbitalphysik (Keplersche Gesetze, Delta-V, Gravity Assists), alle Stock-Himmelskörper, Raumfahrzeugdesign-Prinzipien, DLC-Inhalte, Modding-Konventionen und Community-Ressourcen. Aber seine wahre Stärke ist das **Skill-System** — du kannst jedes `SKILL.md`-Dokument in den `Skills/`-Ordner legen und die KI lädt es als Domänenwissen. Bringe ihr deinen Lieblingsmod, dein benutzerdefiniertes Planetensystem oder deine persönlichen Baukonventionen bei. Das Skill-Format ist offen und dokumentiert — dein Fachwissen, deine Regeln.

> **Du benötigst deinen eigenen API-Schlüssel.** DeepJeb enthält oder bietet keinen KI-Dienst — du verbindest es mit deinem eigenen OpenAI-, Anthropic-, Google Gemini-, DeepSeek- oder anderem kompatiblen API-Konto. Der gesamte API-Verkehr geht direkt von deinem Rechner zum konfigurierten Anbieter. Du kannst es auch auf ein lokal betriebenes LLM (via Ollama, vLLM oder einen beliebigen OpenAI-kompatiblen Endpunkt) richten, um alles vollständig unter deiner Kontrolle zu behalten.
>
> **Wie dein API-Schlüssel gespeichert wird.** Im Speicher wird dein Schlüssel im Klartext aufbewahrt (für die API-Authentifizierung erforderlich). Auf der Festplatte werden Schlüssel mit XOR-Verschleierung und Base64-Kodierung verschlüsselt — sie werden niemals im Klartext in die Konfigurationsdatei geschrieben. Bei OpenAI und Anthropic werden API-Schlüssel als HTTP-Bearer-Token-Header gesendet, die die KSP-Debug-Konsole nicht protokolliert. Die Google Gemini API ist die Ausnahme — siehe Bekannte Probleme unten.

---

## Was kann es?

DeepJeb wird mit **7 integrierten Wissensbasen** (Agent Skills) und **7 Dateisystem-Tools** ausgeliefert, die die KI nutzen kann.

### Wissensbasen (Skills)

| Skill | Beschreibung |
|-------|-------------|
| **KSP Weltwissen** | Stock-Spielmechaniken, Orbitalphysik, Himmelskörper, Raumfahrzeugdesign, DLC-Inhalte, Modding-Tipps, Community-Ressourcen |
| **KSP Craft-Dateien** | .craft-Dateiformat, PART-Felder, Quaternionenrotation, Befestigungsknoten, Spiegelsymmetrie, radiale Größen, Stock-Teile-Referenz, Parser-Tools, Fehlerbehebung |
| **Module Manager** | MM-Patch-Syntax, Operatoren, Sortieranweisungen, NEEDS/DEPENDS-Prüfung, Variablen, gängige Muster |
| **kOS-Programmierung** | KerboScript-Sprachreferenz, Flugsteuerung, Manöverknoten, Boot-Dateien, Trigger |
| **kRPC-Programmierung** | Architektur, Python/C#/Lua-Client-APIs, SpaceCenter, AutoPilot, Schiffssteuerung, Streaming-Daten |
| **MechJeb** | Alle Leitmodule, Karriere-Integration, Echtzeit-Wertänderung via kRPC/kOS |
| **Realism Overhaul** | Umfassender RO/RP-1/RSS-Mod-Suite-Guide — 68 Repos, Installation, Teile, Triebwerke, Lebenserhaltung, historische Raumfahrzeuge, Fehlerbehebung |

Skills werden automatisch durch Schlüsselwortüberschneidung mit deiner Frage zugeordnet — die besten 2 Treffer werden als Kontext eingefügt.

### KI-Agenten-Tools

| Tool | Was die KI tun kann |
|------|-------------------|
| `read_file` | Jede Datei in GameData lesen |
| `write_file` | Datei erstellen oder überschreiben (erstellt automatisch Verzeichnisse, sichert vorherige Version) |
| `delete_file` | Datei löschen (erstellt zuerst eine zeitgestempelte `.bak`-Sicherung) |
| `list_directory` | Verzeichnisinhalte mit Dateigrößen und Änderungszeiten auflisten |
| `file_exists` | Prüfen, ob eine Datei oder ein Verzeichnis existiert |
| `backup_file` | Zeitgestempelte `.bak`-Momentaufnahme ohne Änderung der Originaldatei erstellen |
| `get_game_state` | Aktuellen KSP-Spielzustand melden (Schiff, Orbit, Biom, Ressourcen) |

Die KI kann Squad/SquadExpansion-Dateien lesen, aber **nicht ändern oder löschen**.

### Slash-Befehle

Gib `/` im Chat-Eingabefeld ein, um Befehle lokal auszuführen — kein KI-Roundtrip:

| Befehl | Funktion |
|--------|---------|
| `/retry` | Deine letzte Nachricht erneut an die KI senden |
| `/undo` | Das letzte Austauschpaar aus der Sitzung entfernen |
| `/help` | Alle verfügbaren Befehle auflisten |
| `/session` | Aktuelle Sitzungsinfo anzeigen (Anbieter, Modell, Nachrichtenanzahl) |
| `/game` | Aktuellen KSP-Spielzustand anzeigen (Szene, Schiff, Orbit, Biom, Ressourcen) |

---

## Unterstützte Anbieter

**12 integrierte Voreinstellungen + benutzerdefinierte Anbieter** über 3 Protokollfamilien:

| Protokoll | Integrierte Voreinstellungen |
|----------|-----------------|
| **OpenAI-kompatibel** | OpenAI, DeepSeek, OpenRouter, Grok (xAI), Mistral, Together AI, Perplexity, Groq, Ollama, vLLM, Benutzerdefiniert |
| **Anthropic** | Anthropic (Claude) |
| **Google Gemini** | Google (Gemini) |

Benutzerdefinierte Endpunkte, API-Schlüssel, Modelllisten und Anbieternamen sind alle im Spiel über das Einstellungsfenster konfigurierbar. Modelllisten werden live von jeder API abgerufen.

---

## Was ist ein Agent Skill?

DeepJebs Wissensbasen sind **Agent Skills** — ein Standardformat zur Verpackung von Domänenexpertise mit einem KI-Assistenten. Jeder Skill ist eine `SKILL.md`-Datei mit YAML-Frontmatter (Name, Beschreibung, Auslöser) und einem Markdown-Body, der das Wissen enthält. Skills werden im `Skills/`-Verzeichnis abgelegt und beim Start geladen.

### Wie Skills funktionieren

- **[Agent Skills-Dokumentation](https://docs.anthropic.com/de/docs/claude-code/skills)** — offizieller Leitfaden (aus der Claude Code-Dokumentation)
- **[Eigene Skills erstellen](https://docs.anthropic.com/de/docs/claude-code/skills#creating-custom-skills)** — Anleitung zum Erstellen (aus der Claude Code-Dokumentation)

Um deinen eigenen Skill zu DeepJeb hinzuzufügen, erstelle eine `SKILL.md`-Datei in `GameData/DeepJeb/Skills/{Kategorie}/{Name}/` mit:

```yaml
---
name: dein-skill-name
description: >
  Was dieser Skill abdeckt.
---
# Dein Wissensinhalt hier
```

Referenzdateien (Skripte, Tabellen, Beispiele) können in einem `references/`-Unterverzeichnis abgelegt werden — sie werden bei Übereinstimmung zusammen mit dem Skill eingefügt.

### Bedingte Skill-Aktivierung

Du kannst das `when_to_use`-Frontmatter-Feld verwenden, um einen Skill nur dann zu aktivieren, wenn ein bestimmter Mod vorhanden ist. Der KI-Agent kann `GameData/` auf installierte Mods prüfen, bevor der Skill geladen wird:

```yaml
---
name: mein-mod-guide
description: >
  Wissensbasis für MyMod. Nur aktivieren, wenn der Mod installiert ist.
when_to_use: |
  Auslösen, wenn der GameData-Ordner des Benutzers "MyMod" enthält.
condition: file_exists("MyMod/") -> true
---
# MyMod-Konfigurationsleitfaden
```

Verwende `file_exists`- oder `list_directory`-Toolaufrufe als Bedingungen, um das Laden von Skills zu steuern — so lädt DeepJeb kein irrelevanties Wissen für Mods, die du nicht installiert hast.

---

## Installation

1. Kopiere den `DeepJeb/`-Ordner in dein KSP-`GameData/`-Verzeichnis
2. Starte KSP — das DeepJeb-Symbolleisten-Icon erscheint in allen Szenen
3. Klicke auf das Icon, um das Chatfenster zu öffnen
4. Öffne die Einstellungen, um einen API-Anbieter und ein Modell zu konfigurieren
5. Beginne zu chatten

> **Tipp:** Drücke Enter zum Senden, **Strg+Enter** oder **Shift+Enter** für einen Zeilenumbruch.

**Anforderungen:** KSP 1.12.0+ (getestet auf 1.12.5). Keine zusätzlichen Mods oder Abhängigkeiten erforderlich.

---

## Bekannte Probleme

- **Modellverfügbarkeit**: Modelllisten werden live von jedem API-Anbieter abgerufen. Wenn die API nicht erreichbar ist, zeigt das Modell-Dropdown die letzte zwischengespeicherte Liste oder dauerhaft "Laden..." an. Überprüfe deinen API-Schlüssel und deine Netzwerkverbindung.
- **Kontextkürzung**: Sehr lange Konversationen können ältere Nachrichten verlieren, wenn das Kontextfensterlimit des Modells erreicht wird. Verwende `/clear` regelmäßig für lange Sitzungen.
- **UI-Skalierung**: Das Chatfenster verwendet feste Pixelmaße (Standard 600×500). Auf sehr kleinen oder sehr großen Bildschirmen skaliert das Layout möglicherweise nicht ideal.
- **KSP-Szenenwechsel**: Das Chatfenster bleibt bei Szenenwechseln erhalten, aber laufende KI-Generierung wird beim Laden einer neuen Szene gestoppt.
- **Google Gemini API-Schlüssel**: Die Gemini-API erfordert, dass der API-Schlüssel als URL-Abfrageparameter übergeben wird (dies ist Googles Design, nicht DeepJebs Wahl). Wenn du den Google Gemini-Anbieter verwendest, kann dein API-Schlüssel **im Klartext** in den KSP-Debug-Konsolenprotokollen erscheinen, wenn du Alt+F12-Debugging verwendest. Schlüssel für OpenAI und Anthropic werden als HTTP-Header gesendet und nicht protokolliert.
- **Streaming-Leistung**: Sehr lange KI-Antworten können bei der Token-für-Token-Darstellung zu geringfügigen UI-Bildratenschwankungen führen.
- **ClickThroughBlocker**: Wenn du ClickThroughBlocker installiert hast, musst du möglicherweise zweimal auf das DeepJeb-Toolbar-Icon klicken, um das Fenster zu öffnen oder zu schließen. Dies ist erwartet — DeepJeb verwendet seine eigene Click-Through-Erkennung.

---

## Lizenz

[MIT License](LICENSE)

Copyright © 2026 Acea - entwickelt mit Codex / DeepSeek V4 Pro

MiniJSON basiert auf der Public-Domain-Implementierung von Calvin Rien.

Die drei Python-Dateien im `ksp-craft-files`-Skill (`ksparser.py`, `import_craft.py`, `part_dict.py`) stammen von [io_kspblender](https://github.com/spencerarrasmith/io_kspblender) von Spencer Arrasmith, lizenziert unter [GPL-2.0](https://www.gnu.org/licenses/old-licenses/gpl-2.0.html). Sie sind nur als Referenz und Demonstration enthalten.

---

<p align="center">
  <sub>Für die Kerbal Space Program Modding-Community gebaut. Fly safe.</sub>
</p>
