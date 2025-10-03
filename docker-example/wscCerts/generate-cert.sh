#!/usr/bin/env bash
set -e
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
KEY_FILE="$SCRIPT_DIR/key.pem"
CERT_FILE="$SCRIPT_DIR/certificate.cer"

[ -f "$KEY_FILE" ] || { echo "❌ key.pem mangler i $SCRIPT_DIR"; exit 1; }

# Selvsigneret cert med SHA-256 som certifikat-signatur og korrekt DN-orden
openssl req -new -x509 -sha256 \
  -key "$KEY_FILE" \
  -out "$CERT_FILE" \
  -days 365 \
  -subj "/C=dk/ST=aarhus/L=aarhus/O=signatur"

echo "✅ Nyt cert:"
openssl x509 -in "$CERT_FILE" -noout -subject -issuer -dates -sigopt rsa_padding_mode:pss 2>/dev/null || true
