<p align="center">
  <img src="../assets/Textures/DeepJebLogo.png" alt="DeepJeb Logo" width="256">
</p>

<h1 align="center">DeepJeb</h1>

<p align="center">
  <strong>Assistente AI per Kerbal Space Program</strong><br>
  Una finestra di chat AI/LLM integrata nel gioco che legge, scrive e ti aiuta a costruire.<br>
  <em>KSP 1.12.5 · Unity 2019.2 · C# 7.3 · Zero Dipendenze</em>
</p>

<p align="center">
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="Licenza MIT"></a>
  <a href="#"><img src="https://img.shields.io/badge/KSP-1.12.5-blue" alt="KSP 1.12.5"></a>
  <a href="#"><img src="https://img.shields.io/badge/version-0.5.4-green" alt="v0.5.4"></a>
</p>

<p align="center">
  <a href="../README.md">English</a> | <a href="README_cn.md">简体中文</a> | <a href="README_de.md">Deutsch</a> | <a href="README_fr.md">Français</a> | <strong>Italiano</strong> | <a href="README_ja.md">日本語</a> | <a href="README_pt.md">Português</a> | <a href="README_ru.md">Русский</a> | <a href="README_es.md">Español</a>
</p>

---

## Cos'è DeepJeb?

DeepJeb integra una finestra di chat AI direttamente in KSP. Chiedigli qualsiasi cosa — scrivi patch Module Manager, configura mod, spiega la meccanica orbitale, debugga uno script kOS o progetta un razzo che non si ribalti in ascesa.

DeepJeb include una completa **base di conoscenza del mondo KSP**: meccaniche di gioco, fisica orbitale (leggi di Keplero, delta-V, fionde gravitazionali), tutti i corpi celesti stock, principi di progettazione di veicoli spaziali, contenuti DLC, convenzioni di modding e risorse della comunità. Ma la sua vera potenza è il **sistema di competenze** — puoi inserire qualsiasi documento `SKILL.md` nella cartella `Skills/` e l'IA lo caricherà come conoscenza di dominio. Insegnagli il tuo mod preferito, il tuo pacchetto di pianeti personalizzato o le tue convenzioni di costruzione. Il formato delle competenze è aperto e documentato — la tua esperienza, le tue regole.

> **Hai bisogno della tua chiave API.** DeepJeb non include né fornisce alcun servizio AI — lo connetti al tuo account OpenAI, Anthropic, Google Gemini, DeepSeek o altro API compatibile. Tutto il traffico API va direttamente dal tuo computer al provider che configuri. Puoi anche puntarlo a un LLM distribuito localmente (via Ollama, vLLM o qualsiasi endpoint compatibile con OpenAI) per mantenere tutto completamente sotto il tuo controllo.
>
> **Come viene memorizzata la tua chiave API.** In memoria, la chiave è conservata in chiaro (necessario per l'autenticazione API). Su disco, le chiavi sono cifrate usando offuscamento XOR con codifica Base64 — non vengono mai scritte in chiaro nel file di configurazione. Per OpenAI e Anthropic, le chiavi API vengono inviate come intestazioni HTTP Bearer token che la console di debug di KSP non registra. L'API Google Gemini è l'eccezione — vedi Problemi Noti qui sotto.

---

## Cosa Può Fare?

DeepJeb viene fornito con **7 basi di conoscenza integrate** (Agent Skills) e **7 strumenti di filesystem** che l'IA può utilizzare.

### Basi di Conoscenza (Competenze)

| Competenza | Descrizione |
|-------|-------------|
| **Conoscenza del mondo KSP** | Meccaniche di gioco stock, fisica orbitale, corpi celesti, progettazione di veicoli, contenuti DLC, consigli di modding, risorse della comunità |
| **File Craft KSP** | Formato file .craft, campi PART, rotazione quaternioni, nodi di attacco, simmetria speculare, dimensioni radiali, riferimento parti stock, strumenti di analisi, risoluzione problemi |
| **Module Manager** | Sintassi patch MM, operatori, direttive di ordinamento, controllo NEEDS/DEPENDS, variabili, schemi comuni |
| **Programmazione kOS** | Riferimento linguaggio KerboScript, controllo di volo, nodi di manovra, file di avvio, trigger |
| **Programmazione kRPC** | Architettura, API client Python/C#/Lua, SpaceCenter, AutoPilot, controllo navicella, dati in streaming |
| **MechJeb** | Tutti i moduli di guida, integrazione carriera, modifica valori in tempo reale via kRPC/kOS |
| **Realism Overhaul** | Guida completa alla suite di mod RO/RP-1/RSS — 68 repository, installazione, parti, motori, supporto vitale, veicoli storici, risoluzione problemi |

Le competenze vengono abbinate automaticamente per sovrapposizione di parole chiave con la tua domanda — i 2 migliori risultati vengono iniettati come contesto.

### Strumenti dell'Agente AI

| Strumento | Cosa può fare l'IA |
|------|-------------------|
| `read_file` | Leggere qualsiasi file in GameData |
| `write_file` | Creare o sovrascrivere un file (crea automaticamente directory, salva la versione precedente) |
| `delete_file` | Eliminare un file (crea prima un backup `.bak` con timestamp) |
| `list_directory` | Elencare il contenuto della directory con dimensioni e date di modifica |
| `file_exists` | Verificare se un file o directory esiste |
| `backup_file` | Creare un'istantanea `.bak` con timestamp senza modificare l'originale |
| `get_game_state` | Riportare lo stato attuale del gioco KSP (navicella, orbita, bioma, risorse) |

L'IA può leggere i file Squad/SquadExpansion ma **non può modificarli o eliminarli**.

### Comandi Slash

Digita `/` nel campo di input per eseguire comandi localmente — senza round-trip dell'IA:

| Comando | Funzione |
|--------|---------|
| `/retry` | Reinviare il tuo ultimo messaggio all'IA |
| `/undo` | Rimuovere l'ultima coppia di scambio dalla sessione |
| `/help` | Elencare tutti i comandi disponibili |
| `/session` | Mostrare info sulla sessione (provider, modello, conteggio messaggi) |
| `/game` | Visualizzare lo stato attuale del gioco KSP (scena, navicella, orbita, bioma, risorse) |

---

## Provider Supportati

**12 preset integrati + supporto provider personalizzato** su 3 famiglie di protocolli:

| Protocollo | Preset Integrati |
|----------|-----------------|
| **Compatibile OpenAI** | OpenAI, DeepSeek, OpenRouter, Grok (xAI), Mistral, Together AI, Perplexity, Groq, Ollama, vLLM, Personalizzato |
| **Anthropic** | Anthropic (Claude) |
| **Google Gemini** | Google (Gemini) |

Endpoint personalizzati, chiavi API, elenchi di modelli e nomi dei provider sono tutti configurabili in gioco tramite la finestra Impostazioni. Gli elenchi dei modelli vengono recuperati in tempo reale da ciascuna API.

---

## Cos'è un Agent Skill?

Le basi di conoscenza di DeepJeb sono **Agent Skills** — un formato standard per impacchettare competenze di dominio con un assistente AI. Ogni competenza è un file `SKILL.md` con frontmatter YAML (nome, descrizione, trigger) e un corpo Markdown contenente la conoscenza. Le competenze sono collocate nella directory `Skills/` e caricate all'avvio.

### Come Funzionano le Competenze

- **[Documentazione Agent Skills](https://docs.anthropic.com/it/docs/claude-code/skills)** — guida ufficiale (dalla documentazione Claude Code)
- **[Creare competenze personalizzate](https://docs.anthropic.com/it/docs/claude-code/skills#creating-custom-skills)** — guida alla creazione (dalla documentazione Claude Code)

Per aggiungere la tua competenza a DeepJeb, crea un file `SKILL.md` in `GameData/DeepJeb/Skills/{categoria}/{nome}/` con:

```yaml
---
name: nome-della-tua-competenza
description: >
  Cosa copre questa competenza.
---
# Il tuo contenuto di conoscenza qui
```

I file di riferimento (script, tabelle, esempi) possono essere inseriti in una sottodirectory `references/` — verranno iniettati insieme alla competenza quando abbinata.

### Attivazione Condizionale delle Competenze

Puoi usare il campo frontmatter `when_to_use` per far sì che una competenza si attivi solo quando un mod specifico è presente. L'agente IA può controllare `GameData/` per i mod installati prima di caricare la competenza:

```yaml
---
name: guida-mio-mod
description: >
  Base di conoscenza per MyMod. Si attiva solo quando il mod è installato.
when_to_use: |
  Si attiva quando la cartella GameData dell'utente contiene "MyMod".
condition: file_exists("MyMod/") -> true
---
# Guida alla configurazione di MyMod
```

Usa le chiamate agli strumenti `file_exists` o `list_directory` come condizioni per controllare il caricamento delle competenze — così DeepJeb non caricherà conoscenze irrilevanti per mod che non hai installato.

---

## Installazione

1. Copia la cartella `DeepJeb/` nella tua directory KSP `GameData/`
2. Avvia KSP — l'icona DeepJeb nella barra degli strumenti appare in tutte le scene
3. Clicca l'icona per aprire la finestra di chat
4. Apri Impostazioni per configurare un provider API e un modello
5. Inizia a chattare

> **Suggerimento:** Premi Invio per inviare, **Ctrl+Invio** o **Shift+Invio** per andare a capo.

**Requisiti:** KSP 1.12.0+ (testato su 1.12.5). Nessun mod o dipendenza aggiuntiva richiesta.

---

## Problemi Noti

- **Disponibilità dei modelli**: Gli elenchi dei modelli vengono recuperati in tempo reale da ciascun provider API. Se l'API non è raggiungibile, il menu a discesa dei modelli mostra l'ultimo elenco memorizzato o "Caricamento..." all'infinito. Controlla la tua chiave API e la connessione di rete.
- **Troncamento del contesto**: Conversazioni molto lunghe potrebbero perdere i messaggi più vecchi quando si avvicinano al limite della finestra di contesto del modello. Usa `/clear` periodicamente per sessioni lunghe.
- **Ridimensionamento UI**: La finestra di chat utilizza dimensioni fisse in pixel (600×500 predefinito). Su schermi molto piccoli o molto grandi, il layout potrebbe non adattarsi idealmente.
- **Transizioni di scena KSP**: La finestra di chat persiste attraverso i cambi di scena, ma la generazione AI in corso viene interrotta al caricamento della scena.
- **Chiave API Google Gemini**: L'API Gemini richiede che la chiave API venga passata come parametro di query URL (questo è il design di Google, non una scelta di DeepJeb). Di conseguenza, se usi il provider Google Gemini, la tua chiave API **potrebbe apparire in chiaro** nei log della console di debug di KSP quando usi il debug Alt+F12. Le chiavi per OpenAI e Anthropic vengono inviate come intestazioni HTTP e non vengono registrate.
- **Prestazioni di streaming**: Risposte AI molto lunghe potrebbero causare lievi fluttuazioni del frame rate durante il rendering token per token.
- **ClickThroughBlocker**: Se hai ClickThroughBlocker installato, potresti dover cliccare due volte sull'icona DeepJeb per aprire o chiudere la finestra. È normale — DeepJeb usa il proprio rilevamento click-through.

---

## Licenza

[Licenza MIT](LICENSE)

Copyright © 2026 Acea - sviluppato con Claude Code / DeepSeek V4 Pro

MiniJSON è basato sull'implementazione di pubblico dominio di Calvin Rien.

I tre file Python nella competenza `ksp-craft-files` (`ksparser.py`, `import_craft.py`, `part_dict.py`) derivano da [io_kspblender](https://github.com/spencerarrasmith/io_kspblender) di Spencer Arrasmith, con licenza [GPL-2.0](https://www.gnu.org/licenses/old-licenses/gpl-2.0.html). Sono inclusi solo come riferimento e dimostrazione.

---

<p align="center">
  <sub>Costruito per la comunità di modding di Kerbal Space Program. Vola sicuro.</sub>
</p>
