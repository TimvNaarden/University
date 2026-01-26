#!/usr/bin/env bash

# === CONFIG ===
BASE_DIR="$(pwd)"
SRC_DIR="$BASE_DIR/src"
OUT_DIR="$BASE_DIR/out"
MAIN_TEX="$SRC_DIR/main.tex"

echo "=== Building LaTeX document ==="
echo "Working directory: $BASE_DIR"
echo

# === STEP 1: Check if main.tex exists ===
if [ ! -f "$MAIN_TEX" ]; then
  echo "❌ ERROR: main.tex not found in $SRC_DIR"
  exit 1
fi

# === STEP 2: Ensure out directory exists ===
if [ ! -d "$OUT_DIR" ]; then
  echo "📁 Creating output directory: $OUT_DIR"
  mkdir -p "$OUT_DIR"
fi

# === STEP 3: Run pdflatex ===
echo "🚀 Running pdflatex ..."
pdflatex \
  -file-line-error \
  -interaction=nonstopmode \
  -synctex=1 \
  -output-format=pdf \
  "-output-directory=$OUT_DIR" \
  -interaction=batchmode \
  "$MAIN_TEX"

echo
echo "✅ LaTeX build completed successfully!"