#!/bin/bash

set -e

trap 'echo -e "\033[33mFrontend process ended\033[0m"' EXIT

echo -e "\033[36mStarting Frontend (Svelte)...\033[0m"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
FRONTEND_PATH="$SCRIPT_DIR/src/client"

if [ ! -d "$FRONTEND_PATH" ]; then
    echo -e "\033[31mERROR: Frontend path not found: $FRONTEND_PATH\033[0m"
    exit 1
fi

cd "$FRONTEND_PATH"
echo -e "\033[37mFrontend directory: $(pwd)\033[0m"

# Check if node_modules exists
if [ ! -d "node_modules" ]; then
    echo -e "\033[33mInstalling dependencies...\033[0m"
    if ! npm install; then
        echo -e "\033[31mERROR: npm install failed\033[0m"
        exit 1
    fi
fi

# Start the development server
echo -e "\033[32mStarting Svelte dev server...\033[0m"
if ! npm run dev; then
    echo -e "\033[31mERROR: Failed to start frontend\033[0m"
    exit 1
fi
