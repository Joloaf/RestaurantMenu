#!/bin/bash

set -e

trap 'echo -e "\033[33mBackend process ended\033[0m"' EXIT

echo -e "\033[36mStarting Backend (C# .NET)...\033[0m"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BACKEND_PATH="$SCRIPT_DIR/src/server/RestaurantMenu.API"

if [ ! -d "$BACKEND_PATH" ]; then
    echo -e "\033[31mERROR: Backend path not found: $BACKEND_PATH\033[0m"
    exit 1
fi

cd "$BACKEND_PATH"
echo -e "\033[37mBackend directory: $(pwd)\033[0m"

# Run the .NET application
if ! dotnet run; then
    echo -e "\033[31mERROR: Failed to start backend\033[0m"
    exit 1
fi
