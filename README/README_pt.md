<p align="center">
  <img src="../assets/Textures/DeepJebLogo.png" alt="Logo do DeepJeb" width="256">
</p>

<h1 align="center">DeepJeb</h1>

<p align="center">
  <strong>Assistente IA para Kerbal Space Program</strong><br>
  Uma janela de chat IA/LLM integrada ao jogo que lê, escreve e ajuda você a construir.<br>
  <em>KSP 1.12.5 · Unity 2019.2 · C# 7.3 · Zero Dependências</em>
</p>

<p align="center">
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="Licença MIT"></a>
  <a href="#"><img src="https://img.shields.io/badge/KSP-1.12.5-blue" alt="KSP 1.12.5"></a>
  <a href="#"><img src="https://img.shields.io/badge/version-0.5.6-green" alt="v0.5.6"></a>
</p>

<p align="center">
  <a href="../README.md">English</a> | <a href="README_cn.md">简体中文</a> | <a href="README_de.md">Deutsch</a> | <a href="README_fr.md">Français</a> | <a href="README_it.md">Italiano</a> | <a href="README_ja.md">日本語</a> | <strong>Português</strong> | <a href="README_ru.md">Русский</a> | <a href="README_es.md">Español</a>
</p>

---

## O Que é o DeepJeb?

O DeepJeb integra uma janela de chat IA diretamente no KSP. Pergunte qualquer coisa — escreva patches do Module Manager, configure mods, explique mecânica orbital, depure um script kOS ou projete um foguete que não capote na subida.

O DeepJeb vem com uma abrangente **base de conhecimento do mundo KSP**: mecânicas de jogo, física orbital (leis de Kepler, delta-V, assistência gravitacional), todos os corpos celestes originais, princípios de design de naves, conteúdo de DLC, convenções de modding e recursos da comunidade. Mas seu verdadeiro poder é o **sistema de habilidades** — você pode colocar qualquer documento `SKILL.md` na pasta `Skills/` e a IA o carregará como conhecimento de domínio. Ensine a ela seu mod favorito, seu pacote de planetas personalizado ou suas convenções pessoais de construção. O formato de habilidade é aberto e documentado — sua experiência, suas regras.

> **Você precisa da sua própria chave de API.** O DeepJeb não inclui nem fornece nenhum serviço de IA — você o conecta à sua própria conta OpenAI, Anthropic, Google Gemini, DeepSeek ou outra API compatível. Todo o tráfego de API vai diretamente da sua máquina para o provedor que você configurar. Você também pode apontá-lo para um LLM implantado localmente (via Ollama, vLLM ou qualquer endpoint compatível com OpenAI) para manter tudo completamente sob seu controle.
>
> **Como sua chave de API é armazenada.** Na memória, sua chave é mantida em texto simples (necessário para autenticação da API). No disco, as chaves são criptografadas usando ofuscação XOR com codificação Base64 — elas nunca são gravadas no arquivo de configuração em texto simples. Para OpenAI e Anthropic, as chaves de API são enviadas como cabeçalhos HTTP Bearer token que o console de depuração do KSP não registra. A API Google Gemini é a exceção — veja Problemas Conhecidos abaixo.

---

## O Que Ele Pode Fazer?

O DeepJeb vem com **7 bases de conhecimento integradas** (Agent Skills) e **7 ferramentas de sistema de arquivos** que a IA pode usar.

### Bases de Conhecimento (Habilidades)

| Habilidade | Descrição |
|-------|-------------|
| **Conhecimento do Mundo KSP** | Mecânicas de jogo originais, física orbital, corpos celestes, design de naves, conteúdo DLC, dicas de modding, recursos da comunidade |
| **Arquivos Craft KSP** | Formato de arquivo .craft, campos PART, rotação quaternion, nós de fixação, simetria espelhada, tamanhos radiais, referência de peças originais, ferramentas de análise, solução de problemas |
| **Module Manager** | Sintaxe de patch MM, operadores, diretivas de ordenação, verificação NEEDS/DEPENDS, variáveis, padrões comuns |
| **Programação kOS** | Referência da linguagem KerboScript, controle de voo, nós de manobra, arquivos de inicialização, gatilhos |
| **Programação kRPC** | Arquitetura, APIs cliente Python/C#/Lua, SpaceCenter, AutoPilot, controle de nave, dados em streaming |
| **MechJeb** | Todos os módulos de orientação, integração de carreira, modificação de valores em tempo real via kRPC/kOS |
| **Realism Overhaul** | Guia abrangente do conjunto de mods RO/RP-1/RSS — 68 repositórios, instalação, peças, motores, suporte de vida, naves históricas, solução de problemas |

As habilidades são combinadas automaticamente por sobreposição de palavras-chave com sua pergunta — os 2 melhores resultados são injetados como contexto.

### Ferramentas do Agente IA

| Ferramenta | O que a IA pode fazer |
|------|-------------------|
| `read_file` | Ler qualquer arquivo dentro do GameData |
| `write_file` | Criar ou sobrescrever um arquivo (cria diretórios automaticamente, faz backup da versão anterior) |
| `delete_file` | Excluir um arquivo (cria backup `.bak` com timestamp primeiro) |
| `list_directory` | Listar conteúdo do diretório com tamanhos e datas de modificação |
| `file_exists` | Verificar se um arquivo ou diretório existe |
| `backup_file` | Criar um snapshot `.bak` com timestamp sem modificar o original |
| `get_game_state` | Relatar o estado atual do jogo KSP (nave, órbita, bioma, recursos) |
| `web_search` | Pesquisa na web por informacoes e documentacao atualizadas |
| `fetch_url` | Le o conteudo de texto de uma pagina web |

A IA pode ler arquivos Squad/SquadExpansion, mas **não pode modificá-los ou excluí-los**.

### Comandos Slash

Digite `/` no campo de entrada para executar comandos localmente — sem ida e volta da IA:

| Comando | Função |
|--------|---------|
| `/retry` | Reenviar sua última mensagem para a IA |
| `/undo` | Remover o último par de troca da sessão |
| `/help` | Listar todos os comandos disponíveis |
| `/session` | Mostrar informações da sessão atual (provedor, modelo, contagem de mensagens) |
| `/game` | Exibir o estado atual do jogo KSP (cena, nave, órbita, bioma, recursos) |

---

## Provedores Suportados

**12 predefinições integradas + suporte a provedor personalizado** em 3 famílias de protocolos:

| Protocolo | Predefinições Integradas |
|----------|-----------------|
| **Compatível com OpenAI** | OpenAI, DeepSeek, OpenRouter, Grok (xAI), Mistral, Together AI, Perplexity, Groq, Ollama, vLLM, Personalizado |
| **Anthropic** | Anthropic (Claude) |
| **Google Gemini** | Google (Gemini) |

Endpoints personalizados, chaves de API, listas de modelos e nomes de provedores são todos configuráveis no jogo através da janela de Configurações. As listas de modelos são obtidas ao vivo de cada API.

---

## O Que é uma Habilidade de Agente?

As bases de conhecimento do DeepJeb são **Agent Skills** — um formato padrão para empacotar experiência de domínio com um assistente de IA. Cada habilidade é um arquivo `SKILL.md` com frontmatter YAML (nome, descrição, gatilhos) e um corpo Markdown contendo o conhecimento. As habilidades são colocadas no diretório `Skills/` e carregadas na inicialização.

### Como as Habilidades Funcionam

- **[Documentação Agent Skills](https://docs.anthropic.com/pt-BR/docs/claude-code/skills)** — guia oficial (da documentação do Claude Code)
- **[Criando habilidades personalizadas](https://docs.anthropic.com/pt-BR/docs/claude-code/skills#creating-custom-skills)** — guia de criação (da documentação do Claude Code)

Para adicionar sua própria habilidade ao DeepJeb, crie um arquivo `SKILL.md` em `GameData/DeepJeb/Skills/{categoria}/{nome}/` com:

```yaml
---
name: nome-da-sua-habilidade
description: >
  O que esta habilidade cobre.
---
# Seu conteúdo de conhecimento aqui
```

Arquivos de referência (scripts, tabelas, exemplos) podem ser colocados em um subdiretório `references/` — eles serão injetados junto com a habilidade quando combinados.

### Ativação Condicional de Habilidades

Você pode usar o campo frontmatter `when_to_use` para fazer uma habilidade ser ativada apenas quando um mod específico está presente. O agente IA pode verificar `GameData/` por mods instalados antes de carregar a habilidade:

```yaml
---
name: guia-meu-mod
description: >
  Base de conhecimento para MyMod. Ativar apenas quando o mod estiver instalado.
when_to_use: |
  Acionar quando a pasta GameData do usuário contiver "MyMod".
condition: file_exists("MyMod/") -> true
---
# Guia de configuração do MyMod
```

Use chamadas de ferramenta `file_exists` ou `list_directory` como condições para controlar o carregamento de habilidades — assim o DeepJeb não carregará conhecimento irrelevante para mods que você não tem instalados.

---

## Instalação

1. Copie a pasta `DeepJeb/` para o diretório `GameData/` do KSP
2. Inicie o KSP — o ícone da barra de ferramentas do DeepJeb aparece em todas as cenas
3. Clique no ícone para abrir a janela de chat
4. Abra as Configurações para configurar um provedor de API e modelo
5. Comece a conversar

> **Dica:** Pressione Enter para enviar, **Ctrl+Enter** ou **Shift+Enter** para inserir uma nova linha.

**Requisitos:** KSP 1.12.0+ (testado na 1.12.5). Nenhum mod ou dependência adicional necessária.

---

## Problemas Conhecidos

- **Disponibilidade de modelos**: As listas de modelos são obtidas ao vivo de cada provedor de API. Se a API estiver inacessível, o menu suspenso de modelos mostra a última lista em cache ou "Carregando..." indefinidamente. Verifique sua chave de API e conexão de rede.
- **Truncamento de contexto**: Conversas muito longas podem perder mensagens mais antigas ao se aproximar do limite da janela de contexto do modelo. Use `/clear` periodicamente para sessões longas.
- **Escala da IU**: A janela de chat usa dimensões fixas em pixels (600×500 padrão). Em telas muito pequenas ou muito grandes, o layout pode não escalar idealmente.
- **Transições de cena do KSP**: A janela de chat persiste através das mudanças de cena, mas a geração IA em andamento é interrompida no carregamento da cena.
- **Chave de API Google Gemini**: A API Gemini exige que a chave de API seja passada como parâmetro de consulta URL (este é o design do Google, não uma escolha do DeepJeb). Como resultado, se você usar o provedor Google Gemini, sua chave de API **pode aparecer em texto simples** nos logs do console de depuração do KSP ao usar a depuração Alt+F12. Chaves para OpenAI e Anthropic são enviadas como cabeçalhos HTTP e não são registradas.
- **Desempenho de streaming**: Respostas IA muito longas podem causar pequenas flutuações na taxa de quadros da IU durante a renderização token a token.
- **ClickThroughBlocker**: Se você tiver o ClickThroughBlocker instalado, pode precisar clicar duas vezes no ícone do DeepJeb para abrir ou fechar a janela. Isso é esperado — o DeepJeb usa sua própria detecção de click-through.

---

## Licença

[Licença MIT](LICENSE)

Copyright © 2026 Acea - desenvolvido com Claude Code / DeepSeek V4 Pro

MiniJSON é baseado na implementação de domínio público de Calvin Rien.

Os três arquivos Python na habilidade `ksp-craft-files` (`ksparser.py`, `import_craft.py`, `part_dict.py`) são derivados de [io_kspblender](https://github.com/spencerarrasmith/io_kspblender) por Spencer Arrasmith, licenciado sob [GPL-2.0](https://www.gnu.org/licenses/old-licenses/gpl-2.0.html). Eles estão incluídos apenas para referência e demonstração.

---

<p align="center">
  <sub>Construído para a comunidade de modding do Kerbal Space Program. Voe seguro.</sub>
</p>
