<p align="center">
  <img src="../assets/Textures/DeepJebLogo.png" alt="DeepJeb Logo" width="256">
</p>

<h1 align="center">DeepJeb</h1>

<p align="center">
  <strong>Assistant IA pour Kerbal Space Program</strong><br>
  Une fenêtre de chat IA/LLM intégrée au jeu qui lit, écrit et vous aide à construire.<br>
  <em>KSP 1.12.5 · Unity 2019.2 · C# 7.3 · Zéro Dépendance</em>
</p>

<p align="center">
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="Licence MIT"></a>
  <a href="#"><img src="https://img.shields.io/badge/KSP-1.12.5-blue" alt="KSP 1.12.5"></a>
  <a href="#"><img src="https://img.shields.io/badge/version-0.5.5-green" alt="v0.5.5"></a>
</p>

<p align="center">
  <a href="../README.md">English</a> | <a href="README_cn.md">简体中文</a> | <a href="README_de.md">Deutsch</a> | <strong>Français</strong> | <a href="README_it.md">Italiano</a> | <a href="README_ja.md">日本語</a> | <a href="README_pt.md">Português</a> | <a href="README_ru.md">Русский</a> | <a href="README_es.md">Español</a>
</p>

---

## Qu'est-ce que DeepJeb ?

DeepJeb intègre une fenêtre de chat IA directement dans KSP. Posez-lui n'importe quelle question — écrivez des correctifs Module Manager, configurez des mods, expliquez la mécanique orbitale, débuggez un script kOS ou concevez une fusée qui ne bascule pas au décollage.

DeepJeb est livré avec une **base de connaissances KSP** complète : mécaniques de jeu, physique orbitale (lois de Kepler, delta-V, assistance gravitationnelle), tous les corps célestes de base, principes de conception d'engins spatiaux, contenu DLC, conventions de modding et ressources communautaires. Mais sa véritable puissance réside dans son **système de compétences** — vous pouvez déposer n'importe quel document `SKILL.md` dans le dossier `Skills/` et l'IA le chargera comme connaissance de domaine. Enseignez-lui votre mod préféré, votre pack de planètes personnalisé ou vos propres conventions de construction. Le format de compétence est ouvert et documenté — votre expertise, vos règles.

> **Vous avez besoin de votre propre clé API.** DeepJeb n'inclut ni ne fournit aucun service IA — vous le connectez à votre propre compte OpenAI, Anthropic, Google Gemini, DeepSeek ou autre API compatible. Tout le trafic API va directement de votre machine au fournisseur que vous configurez. Vous pouvez également le diriger vers un LLM déployé localement (via Ollama, vLLM ou tout point de terminaison compatible OpenAI) pour garder tout entièrement sous votre contrôle.
>
> **Comment votre clé API est stockée.** En mémoire, votre clé est conservée en clair (nécessaire pour l'authentification API). Sur le disque, les clés sont chiffrées avec un chiffrement XOR et un encodage Base64 — elles ne sont jamais écrites en clair dans le fichier de configuration. Pour OpenAI et Anthropic, les clés API sont envoyées en tant qu'en-têtes HTTP Bearer token que la console de débogage KSP n'enregistre pas. L'API Google Gemini est l'exception — voir Problèmes Connus ci-dessous.

---

## Que peut-il faire ?

DeepJeb est livré avec **7 bases de connaissances intégrées** (Agent Skills) et **7 outils de système de fichiers** que l'IA peut utiliser.

### Bases de connaissances (Compétences)

| Compétence | Description |
|-------|-------------|
| **Connaissances du monde KSP** | Mécaniques de jeu, physique orbitale, corps célestes, conception d'engins, contenu DLC, astuces de modding, ressources communautaires |
| **Fichiers Craft KSP** | Format de fichier .craft, champs PART, rotation quaternion, nœuds d'attache, symétrie miroir, tailles radiales, référence de pièces stock, outils d'analyse, dépannage |
| **Module Manager** | Syntaxe des correctifs MM, opérateurs, directives d'ordre, vérification NEEDS/DEPENDS, variables, motifs courants |
| **Programmation kOS** | Référence du langage KerboScript, contrôle de vol, nœuds de manœuvre, fichiers de démarrage, déclencheurs |
| **Programmation kRPC** | Architecture, API client Python/C#/Lua, SpaceCenter, AutoPilot, contrôle de vaisseau, données en streaming |
| **MechJeb** | Tous les modules de guidage, intégration carrière, modification de valeurs en temps réel via kRPC/kOS |
| **Realism Overhaul** | Guide complet de la suite de mods RO/RP-1/RSS — 68 dépôts, installation, pièces, moteurs, support de vie, vaisseaux historiques, dépannage |

Les compétences sont associées automatiquement par chevauchement de mots-clés avec votre question — les 2 meilleurs résultats sont injectés comme contexte.

### Outils de l'Agent IA

| Outil | Ce que l'IA peut faire |
|------|-------------------|
| `read_file` | Lire n'importe quel fichier dans GameData |
| `write_file` | Créer ou écraser un fichier (crée automatiquement les répertoires, sauvegarde la version précédente) |
| `delete_file` | Supprimer un fichier (crée d'abord une sauvegarde `.bak` horodatée) |
| `list_directory` | Lister le contenu du répertoire avec tailles et dates de modification |
| `file_exists` | Vérifier si un fichier ou répertoire existe |
| `backup_file` | Créer un instantané `.bak` horodaté sans modifier l'original |
| `get_game_state` | Rapporter l'état actuel du jeu KSP (vaisseau, orbite, biome, ressources) |
| `web_search` | Recherche sur le web des informations et de la documentation a jour |
| `fetch_url` | Recupere le contenu textuel d'une page web |

L'IA peut lire les fichiers Squad/SquadExpansion mais **ne peut pas les modifier ni les supprimer**.

### Commandes Slash

Tapez `/` dans le champ de saisie pour exécuter des commandes localement — sans aller-retour IA :

| Commande | Fonction |
|---------|-------------|
| `/retry` | Renvoyer votre dernier message à l'IA |
| `/undo` | Supprimer la dernière paire d'échange de la session |
| `/help` | Lister toutes les commandes disponibles |
| `/session` | Afficher les infos de la session (fournisseur, modèle, nombre de messages) |
| `/game` | Afficher l'état actuel du jeu KSP (scène, vaisseau, orbite, biome, ressources) |

---

## Fournisseurs Supportés

**12 préréglages intégrés + support de fournisseur personnalisé** sur 3 familles de protocoles :

| Protocole | Préréglages intégrés |
|----------|-----------------|
| **Compatible OpenAI** | OpenAI, DeepSeek, OpenRouter, Grok (xAI), Mistral, Together AI, Perplexity, Groq, Ollama, vLLM, Personnalisé |
| **Anthropic** | Anthropic (Claude) |
| **Google Gemini** | Google (Gemini) |

Les points de terminaison, clés API, listes de modèles et noms de fournisseurs sont tous configurables en jeu via la fenêtre Paramètres. Les listes de modèles sont récupérées en direct de chaque API.

---

## Qu'est-ce qu'une Compétence d'Agent ?

Les bases de connaissances de DeepJeb sont des **Agent Skills** — un format standard pour empaqueter l'expertise de domaine avec un assistant IA. Chaque compétence est un fichier `SKILL.md` avec un frontmatter YAML (nom, description, déclencheurs) et un corps Markdown contenant les connaissances. Les compétences sont placées dans le répertoire `Skills/` et chargées au démarrage.

### Comment fonctionnent les compétences

- **[Documentation Agent Skills](https://docs.anthropic.com/fr/docs/claude-code/skills)** — guide officiel (via la documentation Claude Code)
- **[Créer des compétences personnalisées](https://docs.anthropic.com/fr/docs/claude-code/skills#creating-custom-skills)** — guide de création (via la documentation Claude Code)

Pour ajouter votre propre compétence à DeepJeb, créez un fichier `SKILL.md` dans `GameData/DeepJeb/Skills/{catégorie}/{nom}/` avec :

```yaml
---
name: nom-de-votre-compétence
description: >
  Ce que couvre cette compétence.
---
# Votre contenu de connaissances ici
```

Les fichiers de référence (scripts, tableaux, exemples) peuvent être placés dans un sous-répertoire `references/` — ils seront injectés avec la compétence lors de la correspondance.

### Activation Conditionnelle des Compétences

Vous pouvez utiliser le champ frontmatter `when_to_use` pour qu'une compétence ne s'active que lorsqu'un mod spécifique est présent. L'agent IA peut vérifier `GameData/` pour les mods installés avant de charger la compétence :

```yaml
---
name: guide-mon-mod
description: >
  Base de connaissances pour MyMod. Ne s'active que lorsque le mod est installé.
when_to_use: |
  Se déclenche quand le dossier GameData de l'utilisateur contient "MyMod".
condition: file_exists("MyMod/") -> true
---
# Guide de configuration MyMod
```

Utilisez les appels d'outils `file_exists` ou `list_directory` comme conditions pour contrôler le chargement des compétences — ainsi DeepJeb ne chargera pas de connaissances inutiles pour les mods que vous n'avez pas installés.

---

## Installation

1. Copiez le dossier `DeepJeb/` dans votre répertoire KSP `GameData/`
2. Lancez KSP — l'icône DeepJeb dans la barre d'outils apparaît dans toutes les scènes
3. Cliquez sur l'icône pour ouvrir la fenêtre de chat
4. Ouvrez les Paramètres pour configurer un fournisseur API et un modèle
5. Commencez à chatter

> **Astuce :** Appuyez sur Entrée pour envoyer, **Ctrl+Entrée** ou **Shift+Entrée** pour insérer un saut de ligne.

**Prérequis :** KSP 1.12.0+ (testé sur 1.12.5). Aucun mod ou dépendance supplémentaire requis.

---

## Problèmes Connus

- **Disponibilité des modèles** : Les listes de modèles sont récupérées en direct de chaque fournisseur API. Si l'API est inaccessible, le menu déroulant des modèles affiche la dernière liste en cache ou « Chargement... » indéfiniment. Vérifiez votre clé API et votre connexion réseau.
- **Troncature de contexte** : Les conversations très longues peuvent perdre des messages plus anciens en approchant de la limite de fenêtre de contexte du modèle. Utilisez `/clear` périodiquement pour les longues sessions.
- **Mise à l'échelle de l'interface** : La fenêtre de chat utilise des dimensions en pixels fixes (600×500 par défaut). Sur les très petits ou très grands écrans, la mise en page peut ne pas s'adapter idéalement.
- **Transitions de scène KSP** : La fenêtre de chat persiste à travers les changements de scène, mais la génération IA en cours est arrêtée lors du chargement d'une scène.
- **Clé API Google Gemini** : L'API Gemini exige que la clé API soit passée comme paramètre de requête URL (c'est la conception de Google, pas un choix de DeepJeb). Par conséquent, si vous utilisez le fournisseur Google Gemini, votre clé API **peut apparaître en clair** dans les journaux de la console de débogage KSP lors de l'utilisation du débogage Alt+F12. Les clés pour OpenAI et Anthropic sont envoyées comme en-têtes HTTP et ne sont pas journalisées.
- **Performances de streaming** : Les réponses IA très longues peuvent causer de légères fluctuations de fréquence d'images lors du rendu token par token.
- **ClickThroughBlocker** : Si vous avez ClickThroughBlocker installé, vous devrez peut-être cliquer deux fois sur l'icône DeepJeb pour ouvrir ou fermer la fenêtre. C'est normal — DeepJeb utilise sa propre détection de clic.

---

## Licence

[Licence MIT](LICENSE)

Copyright © 2026 Acea - développé avec Claude Code / DeepSeek V4 Pro

MiniJSON est basé sur l'implémentation du domaine public de Calvin Rien.

Les trois fichiers Python dans la compétence `ksp-craft-files` (`ksparser.py`, `import_craft.py`, `part_dict.py`) proviennent de [io_kspblender](https://github.com/spencerarrasmith/io_kspblender) par Spencer Arrasmith, sous licence [GPL-2.0](https://www.gnu.org/licenses/old-licenses/gpl-2.0.html). Ils sont inclus à titre de référence et de démonstration uniquement.

---

<p align="center">
  <sub>Construit pour la communauté de modding Kerbal Space Program. Bon vol.</sub>
</p>
