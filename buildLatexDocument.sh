#!/usr/bin/env bash

# === CONFIG ===
BASE_DIR="$(pwd)"
SRC_DIR="$BASE_DIR/src"
OUT_DIR="$BASE_DIR/out"
MAIN_TEX="$SRC_DIR/main.tex"
JOBNAME="Samenvatting_OICT"

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

# === STEP 3: First LaTeX run ===
echo "🚀 Running pdflatex (1/3) ..."
pdflatex \
  -file-line-error \
  -interaction=batchmode \
  -synctex=1 \
  -output-format=pdf \
  "-output-directory=$OUT_DIR" \
  -jobname="$JOBNAME" \
  "$MAIN_TEX"

# === STEP 4: Build glossary ===
echo "📖 Building glossary ..."
cd "$OUT_DIR" || exit 1

if command -v makeglossaries >/dev/null 2>&1; then
  makeglossaries "$JOBNAME"
else
  echo "⚠️  makeglossaries not found, trying makeindex fallback"
  makeindex -s "$JOBNAME".ist -t "$JOBNAME".glg -o "$JOBNAME".gls "$JOBNAME".glo
fi

cd "$BASE_DIR" || exit 1

# === STEP 5: Final LaTeX runs ===
echo "🚀 Running pdflatex (2/3) ..."
pdflatex \
  -interaction=batchmode \
  "-output-directory=$OUT_DIR" \
  -jobname="$JOBNAME" \
  "$MAIN_TEX"

echo "🚀 Running pdflatex (3/3) ..."
pdflatex \
  -interaction=batchmode \
  "-output-directory=$OUT_DIR" \
  -jobname="$JOBNAME" \
  "$MAIN_TEX"

echo
echo "✅ LaTeX build (including glossary) completed successfully!"
