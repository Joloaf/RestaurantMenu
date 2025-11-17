#!/bin/bash

set -e

echo -e "\033[32mStarting Frontend and Backend...\033[0m"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Start backend in background
BACKEND_SCRIPT="$SCRIPT_DIR/start-backend.sh"
if [ ! -f "$BACKEND_SCRIPT" ]; then
    echo -e "\033[31mERROR: Backend script not found at $BACKEND_SCRIPT\033[0m"
    exit 1
fi

bash "$BACKEND_SCRIPT" &
BACKEND_PID=$!
echo -e "\033[32mBackend started (PID: $BACKEND_PID)\033[0m"

# Wait a moment before starting frontend
sleep 2

# Start frontend in background
FRONTEND_SCRIPT="$SCRIPT_DIR/start-frontend.sh"
if [ ! -f "$FRONTEND_SCRIPT" ]; then
    echo -e "\033[31mERROR: Frontend script not found at $FRONTEND_SCRIPT\033[0m"
    exit 1
fi

bash "$FRONTEND_SCRIPT" &
FRONTEND_PID=$!
echo -e "\033[32mFrontend started (PID: $FRONTEND_PID)\033[0m"

echo -e "\n\033[32mBoth services started in background\033[0m"
echo -e "\033[33mBackend PID: $BACKEND_PID\033[0m"
echo -e "\033[33mFrontend PID: $FRONTEND_PID\033[0m"
echo -e "\033[33mTo stop services, use: kill $BACKEND_PID $FRONTEND_PID\033[0m"

# Wait for both processes
wait
