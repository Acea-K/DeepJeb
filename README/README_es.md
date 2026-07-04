<p align="center">
  <img src="../assets/Textures/DeepJebLogo.png" alt="Logo de DeepJeb" width="256">
</p>

<h1 align="center">DeepJeb</h1>

<p align="center">
  <strong>Asistente IA para Kerbal Space Program</strong><br>
  Una ventana de chat IA/LLM integrada en el juego que lee, escribe y te ayuda a construir.<br>
  <em>KSP 1.12.5 · Unity 2019.2 · C# 7.3 · Cero Dependencias</em>
</p>

<p align="center">
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="Licencia MIT"></a>
  <a href="#"><img src="https://img.shields.io/badge/KSP-1.12.5-blue" alt="KSP 1.12.5"></a>
  <a href="#"><img src="https://img.shields.io/badge/version-0.5.4-green" alt="v0.5.4"></a>
</p>

<p align="center">
  <a href="../README.md">English</a> | <a href="README_cn.md">简体中文</a> | <a href="README_de.md">Deutsch</a> | <a href="README_fr.md">Français</a> | <a href="README_it.md">Italiano</a> | <a href="README_ja.md">日本語</a> | <a href="README_pt.md">Português</a> | <a href="README_ru.md">Русский</a> | <strong>Español</strong>
</p>

---

## ¿Qué es DeepJeb?

DeepJeb integra una ventana de chat con IA directamente en KSP. Pregúntale lo que quieras — escribe parches de Module Manager, configura mods, explica mecánica orbital, depura un script de kOS o diseña un cohete que no se voltee en el ascenso.

DeepJeb viene con una completa **base de conocimiento del mundo KSP**: mecánicas de juego, física orbital (leyes de Kepler, delta-V, asistencia gravitatoria), todos los cuerpos celestes originales, principios de diseño de naves, contenido de DLC, convenciones de modding y recursos de la comunidad. Pero su verdadero poder es el **sistema de habilidades** — puedes colocar cualquier documento `SKILL.md` en la carpeta `Skills/` y la IA lo cargará como conocimiento de dominio. Enséñale tu mod favorito, tu paquete de planetas personalizado o tus propias convenciones de construcción. El formato de habilidad es abierto y está documentado — tu experiencia, tus reglas.

> **Necesitas tu propia clave API.** DeepJeb no incluye ni proporciona ningún servicio de IA — lo conectas a tu propia cuenta de OpenAI, Anthropic, Google Gemini, DeepSeek u otra API compatible. Todo el tráfico API va directamente desde tu máquina al proveedor que configures. También puedes dirigirlo a un LLM desplegado localmente (a través de Ollama, vLLM o cualquier endpoint compatible con OpenAI) para mantener todo completamente bajo tu control.
>
> **Cómo se almacena tu clave API.** En memoria, tu clave se mantiene en texto plano (necesario para la autenticación API). En disco, las claves se cifran usando ofuscación XOR con codificación Base64 — nunca se escriben en el archivo de configuración en texto plano. Para OpenAI y Anthropic, las claves API se envían como cabeceras HTTP Bearer token que la consola de depuración de KSP no registra. La API de Google Gemini es la excepción — consulta Problemas Conocidos más abajo.

---

## ¿Qué Puede Hacer?

DeepJeb viene con **7 bases de conocimiento integradas** (Agent Skills) y **7 herramientas de sistema de archivos** que la IA puede usar.

### Bases de Conocimiento (Habilidades)

| Habilidad | Descripción |
|-------|-------------|
| **Conocimiento del mundo KSP** | Mecánicas de juego originales, física orbital, cuerpos celestes, diseño de naves, contenido DLC, consejos de modding, recursos de la comunidad |
| **Archivos Craft KSP** | Formato de archivo .craft, campos PART, rotación de cuaterniones, nodos de fijación, simetría especular, tamaños radiales, referencia de piezas originales, herramientas de análisis, solución de problemas |
| **Module Manager** | Sintaxis de parches MM, operadores, directivas de ordenación, verificación NEEDS/DEPENDS, variables, patrones comunes |
| **Programación kOS** | Referencia del lenguaje KerboScript, control de vuelo, nodos de maniobra, archivos de arranque, disparadores |
| **Programación kRPC** | Arquitectura, APIs cliente Python/C#/Lua, SpaceCenter, AutoPilot, control de nave, datos en streaming |
| **MechJeb** | Todos los módulos de guiado, integración profesional, modificación de valores en tiempo real vía kRPC/kOS |
| **Realism Overhaul** | Guía completa del conjunto de mods RO/RP-1/RSS — 68 repositorios, instalación, piezas, motores, soporte vital, naves históricas, solución de problemas |

Las habilidades se emparejan automáticamente por solapamiento de palabras clave con tu pregunta — las 2 mejores coincidencias se inyectan como contexto.

### Herramientas del Agente IA

| Herramienta | Lo que la IA puede hacer |
|------|-------------------|
| `read_file` | Leer cualquier archivo dentro de GameData |
| `write_file` | Crear o sobrescribir un archivo (crea directorios automáticamente, respalda la versión anterior) |
| `delete_file` | Eliminar un archivo (crea primero una copia de seguridad `.bak` con marca de tiempo) |
| `list_directory` | Listar el contenido del directorio con tamaños y fechas de modificación |
| `file_exists` | Verificar si un archivo o directorio existe |
| `backup_file` | Crear una instantánea `.bak` con marca de tiempo sin modificar el original |
| `get_game_state` | Informar del estado actual del juego KSP (nave, órbita, bioma, recursos) |

La IA puede leer archivos de Squad/SquadExpansion pero **no puede modificarlos ni eliminarlos**.

### Comandos Slash

Escribe `/` en el campo de entrada para ejecutar comandos localmente — sin ida y vuelta a la IA:

| Comando | Función |
|--------|---------|
| `/retry` | Reenviar tu último mensaje a la IA |
| `/undo` | Eliminar el último par de intercambio de la sesión |
| `/help` | Listar todos los comandos disponibles |
| `/session` | Mostrar información de la sesión actual (proveedor, modelo, número de mensajes) |
| `/game` | Mostrar el estado actual del juego KSP (escena, nave, órbita, bioma, recursos) |

---

## Proveedores Soportados

**12 preajustes integrados + soporte para proveedores personalizados** en 3 familias de protocolos:

| Protocolo | Preajustes Integrados |
|----------|-----------------|
| **Compatible con OpenAI** | OpenAI, DeepSeek, OpenRouter, Grok (xAI), Mistral, Together AI, Perplexity, Groq, Ollama, vLLM, Personalizado |
| **Anthropic** | Anthropic (Claude) |
| **Google Gemini** | Google (Gemini) |

Los endpoints personalizados, claves API, listas de modelos y nombres de proveedores son todos configurables en el juego a través de la ventana de Configuración. Las listas de modelos se obtienen en vivo de cada API.

---

## ¿Qué es una Habilidad de Agente?

Las bases de conocimiento de DeepJeb son **Agent Skills** — un formato estándar para empaquetar experiencia de dominio con un asistente de IA. Cada habilidad es un archivo `SKILL.md` con frontmatter YAML (nombre, descripción, disparadores) y un cuerpo Markdown que contiene el conocimiento. Las habilidades se colocan en el directorio `Skills/` y se cargan al inicio.

### Cómo Funcionan las Habilidades

- **[Documentación de Agent Skills](https://docs.anthropic.com/es/docs/claude-code/skills)** — guía oficial (de la documentación de Claude Code)
- **[Crear habilidades personalizadas](https://docs.anthropic.com/es/docs/claude-code/skills#creating-custom-skills)** — guía de creación (de la documentación de Claude Code)

Para añadir tu propia habilidad a DeepJeb, crea un archivo `SKILL.md` en `GameData/DeepJeb/Skills/{categoría}/{nombre}/` con:

```yaml
---
name: nombre-de-tu-habilidad
description: >
  Lo que cubre esta habilidad.
---
# Tu contenido de conocimiento aquí
```

Los archivos de referencia (scripts, tablas, ejemplos) se pueden colocar en un subdirectorio `references/` — se inyectarán junto con la habilidad cuando coincida.

### Activación Condicional de Habilidades

Puedes usar el campo frontmatter `when_to_use` para que una habilidad solo se active cuando un mod específico esté presente. El agente IA puede verificar `GameData/` en busca de mods instalados antes de cargar la habilidad:

```yaml
---
name: guia-mi-mod
description: >
  Base de conocimiento para MyMod. Solo activar cuando el mod esté instalado.
when_to_use: |
  Se activa cuando la carpeta GameData del usuario contiene "MyMod".
condition: file_exists("MyMod/") -> true
---
# Guía de configuración de MyMod
```

Usa llamadas a herramientas `file_exists` o `list_directory` como condiciones para controlar la carga de habilidades — así DeepJeb no cargará conocimiento irrelevante para mods que no tienes instalados.

---

## Instalación

1. Copia la carpeta `DeepJeb/` en tu directorio KSP `GameData/`
2. Inicia KSP — el icono de la barra de herramientas de DeepJeb aparece en todas las escenas
3. Haz clic en el icono para abrir la ventana de chat
4. Abre Configuración para configurar un proveedor API y un modelo
5. Empieza a chatear

> **Consejo:** Pulsa Enter para enviar, **Ctrl+Enter** o **Shift+Enter** para insertar un salto de línea.

**Requisitos:** KSP 1.12.0+ (probado en 1.12.5). No se requieren mods ni dependencias adicionales.

---

## Problemas Conocidos

- **Disponibilidad de modelos**: Las listas de modelos se obtienen en vivo de cada proveedor API. Si la API no es accesible, el menú desplegable de modelos muestra la última lista en caché o "Cargando..." indefinidamente. Verifica tu clave API y conexión de red.
- **Truncamiento de contexto**: Las conversaciones muy largas pueden perder mensajes anteriores al acercarse al límite de la ventana de contexto del modelo. Usa `/clear` periódicamente para sesiones largas.
- **Escalado de la interfaz**: La ventana de chat usa dimensiones fijas en píxeles (600×500 por defecto). En pantallas muy pequeñas o muy grandes, el diseño puede no escalar idealmente.
- **Transiciones de escena KSP**: La ventana de chat persiste a través de los cambios de escena, pero la generación IA en curso se detiene al cargar una escena.
- **Clave API de Google Gemini**: La API de Gemini requiere que la clave API se pase como parámetro de consulta URL (esto es diseño de Google, no una elección de DeepJeb). Como resultado, si usas el proveedor Google Gemini, tu clave API **puede aparecer en texto plano** en los registros de la consola de depuración de KSP al usar la depuración Alt+F12. Las claves para OpenAI y Anthropic se envían como cabeceras HTTP y no se registran.
- **Rendimiento de streaming**: Las respuestas IA muy largas pueden causar pequeñas fluctuaciones en la tasa de fotogramas durante la renderización token a token.
- **ClickThroughBlocker**: Si tienes ClickThroughBlocker instalado, puede que necesites hacer clic dos veces en el icono de DeepJeb para abrir o cerrar la ventana. Esto es esperado — DeepJeb usa su propia detección de clics.

---

## Licencia

[Licencia MIT](LICENSE)

Copyright © 2026 Acea - desarrollado con Codex / DeepSeek V4 Pro

MiniJSON está basado en la implementación de dominio público de Calvin Rien.

Los tres archivos Python en la habilidad `ksp-craft-files` (`ksparser.py`, `import_craft.py`, `part_dict.py`) derivan de [io_kspblender](https://github.com/spencerarrasmith/io_kspblender) por Spencer Arrasmith, bajo licencia [GPL-2.0](https://www.gnu.org/licenses/old-licenses/gpl-2.0.html). Se incluyen solo como referencia y demostración.

---

<p align="center">
  <sub>Construido para la comunidad de modding de Kerbal Space Program. Vuela seguro.</sub>
</p>
