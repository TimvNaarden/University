#!/usr/bin/env bash
set -e  # stop on error

# === CONFIG ===
BASE_DIR="$(pwd)"
SRC_DIR="$BASE_DIR/src"
OUT_DIR="$BASE_DIR/out"
MAIN_BASENAME="main"  # corresponds to main.tex
BFC_FILE="$OUT_DIR/Samenvatting_Databases.bcf"

echo "=== Running biber ==="
echo "Working directory: $BASE_DIR"
echo

# === STEP 0: Set environment variables ===
export BIBINPUTS="$SRC_DIR;"
export BSTINPUTS="$SRC_DIR"
echo "📌 Set BIBINPUTS=$BIBINPUTS"
echo "📌 Set BSTINPUT=$BSTINPUTS"
echo

# === STEP 1: Check if main.bcf exists ===
if [ ! -f "$BFC_FILE" ]; then
  echo "⚠️  main.bcf not found in $OUT_DIR — running initial LaTeX build first..."
  bash "$BASE_DIR/buildLatexDocument.sh"
  echo "✅ LaTeX build completed."
  echo
else
  echo "✅ Found main.bcf in $OUT_DIR — skipping LaTeX build."
  echo
fi

# === STEP 2: Run biber ===
echo "🚀 Running biber in $OUT_DIR ..."
(
  cd "$OUT_DIR"
  biber \
    "--input-directory=$SRC_DIR" \
    --quiet \
    "$MAIN_BASENAME"
)

echo
echo "✅ Biber run completed!"
