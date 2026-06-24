#!/bin/bash
# DeepJeb build script — compile all 4 assemblies and deploy to KSP GameData.
# Uses Roslyn csc.exe from Microsoft.Net.Compilers (C# 7.3).
# Usage: ./build.sh [Debug|Release]

set -e

CONFIG="${1:-Debug}"
PROJ="$(cd "$(dirname "$0")" && pwd)"

CSC="E:/ClaudeCode/packages/Microsoft.Net.Compilers.2.10.0/tools/csc.exe"
RF="/c/Windows/Microsoft.NET/Framework64/v4.0.30319"
KSP="E:/KSP_LMP/KSP_x64_Data/Managed"
DEPLOY="E:/KSP_LMP/GameData/DeepJeb"
PLUGINS="$PROJ/GameData/DeepJeb/Plugins"

FLAGS="-target:library -langversion:7.3"
SYS="-reference:$RF/System.dll -reference:$RF/System.Core.dll"

if [ "$CONFIG" = "Release" ]; then
    FLAGS="$FLAGS -optimize+"
else
    FLAGS="$FLAGS -optimize- -debug+"
fi

echo "=== DeepJeb Build ($CONFIG) ==="

# Ensure Plugins directory exists
mkdir -p "$PLUGINS"

# Core (no external deps)
echo "  Core..."
"$CSC" $FLAGS -out:"$PLUGINS/DeepJeb.Core.dll" $SYS -recurse:src/DeepJeb.Core/*.cs

# Protocol (depends on Core)
echo "  Protocol..."
"$CSC" $FLAGS -out:"$PLUGINS/DeepJeb.Protocol.dll" $SYS \
    -reference:"$PLUGINS/DeepJeb.Core.dll" \
    -recurse:src/DeepJeb.Protocol/*.cs

# Unity (depends on Core + Protocol + KSP)
echo "  Unity..."
"$CSC" $FLAGS -out:"$PLUGINS/DeepJeb.Unity.dll" $SYS \
    -reference:"$KSP/UnityEngine.dll" \
    -reference:"$KSP/UnityEngine.CoreModule.dll" \
    -reference:"$KSP/UnityEngine.IMGUIModule.dll" \
    -reference:"$KSP/UnityEngine.UI.dll" \
    -reference:"$KSP/UnityEngine.TextRenderingModule.dll" \
    -reference:"$KSP/UnityEngine.ImageConversionModule.dll" \
    -reference:"$KSP/Assembly-CSharp.dll" \
    -reference:"$KSP/Assembly-CSharp-firstpass.dll" \
    -reference:"$PLUGINS/DeepJeb.Core.dll" \
    -reference:"$PLUGINS/DeepJeb.Protocol.dll" \
    -recurse:src/DeepJeb.Unity/*.cs

# Mod (depends on everything)
echo "  Mod..."
"$CSC" $FLAGS -out:"$PLUGINS/DeepJeb.dll" $SYS \
    -reference:"$KSP/UnityEngine.dll" \
    -reference:"$KSP/UnityEngine.CoreModule.dll" \
    -reference:"$KSP/Assembly-CSharp.dll" \
    -reference:"$KSP/Assembly-CSharp-firstpass.dll" \
    -reference:"$PLUGINS/DeepJeb.Core.dll" \
    -reference:"$PLUGINS/DeepJeb.Protocol.dll" \
    -reference:"$PLUGINS/DeepJeb.Unity.dll" \
    -recurse:src/DeepJeb.Mod/*.cs

# Deploy
echo "  Deploying to $DEPLOY..."

# Create deploy directory structure
mkdir -p "$DEPLOY/Plugins"
mkdir -p "$DEPLOY/Localization"
mkdir -p "$DEPLOY/Sessions"
mkdir -p "$DEPLOY/Skills"
mkdir -p "$DEPLOY/Textures"

# Copy DLLs
cp "$PLUGINS/"*.dll "$DEPLOY/Plugins/"

# Copy static assets (Localization, Skills, Textures)
cp "$PROJ/GameData/DeepJeb/Localization/"* "$DEPLOY/Localization/" 2>/dev/null || true
cp -r "$PROJ/GameData/DeepJeb/Skills/." "$DEPLOY/Skills/" 2>/dev/null || true
cp -r "$PROJ/GameData/DeepJeb/Textures/." "$DEPLOY/Textures/" 2>/dev/null || true

# Copy version file
cp "$PROJ/GameData/DeepJeb/DeepJeb.version" "$DEPLOY/"

# Copy CHANGELOG
cp "$PROJ/CHANGELOG.md" "$DEPLOY/"

# Only copy DeepJeb.cfg if it doesn't exist in deploy (preserve user's runtime config)
if [ ! -f "$DEPLOY/DeepJeb.cfg" ]; then
    cp "$PROJ/GameData/DeepJeb/DeepJeb.cfg" "$DEPLOY/"
    echo "  (fresh DeepJeb.cfg)"
else
    echo "  (preserved existing DeepJeb.cfg)"
fi

echo "=== Done ==="
echo "  Plugins:"
ls -la "$PLUGINS"/*.dll 2>/dev/null || echo "  (none)"
echo "  Deploy:"
ls -la "$DEPLOY/Plugins/"*.dll 2>/dev/null || echo "  (none)"
