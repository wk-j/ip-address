#!/usr/bin/env bash

# Exit on error
set -e

# Variables
NAME="IpAddress"
PACKAGE_ID="wk.IpAddress"
PROJECT_PATH="src/$NAME"
PUBLISH_DIR="publish"

# Get version from csproj file
VERSION=$(grep -o '<Version>[^<]*' "$PROJECT_PATH/$NAME.csproj" | sed 's/<Version>//')

# Function to pack the project
pack() {
    rm -rf "$PUBLISH_DIR"
    mkdir -p "$PUBLISH_DIR"
    dotnet pack "$PROJECT_PATH" -o "$PUBLISH_DIR"
}

# Function to publish to NuGet
publish_nuget() {
    if [ -z "$NPI" ]; then
        echo "Error: NuGet API key (NPI) is not set"
        exit 1
    fi

    pack
    PACKAGE=$(ls -t "$PUBLISH_DIR"/*.nupkg | head -n1)
    if [ -n "$PACKAGE" ]; then
        dotnet nuget push "$PACKAGE" --source "https://www.nuget.org/api/v2/package" --api-key "$NPI"
    else
        echo "Error: No package found in $PUBLISH_DIR"
        exit 1
    fi
}

# Function to install the tool globally
install() {
    pack
    dotnet tool uninstall -g "$PACKAGE_ID" || true
    dotnet tool install -g "$PACKAGE_ID" --add-source "$PUBLISH_DIR" --version "$VERSION"
}

# Parse command line arguments
COMMAND=${1:-pack}

case "$COMMAND" in
    "pack")
        pack
        ;;
    "publish-nuget")
        publish_nuget
        ;;
    "install")
        install
        ;;
    *)
        echo "Unknown command: $COMMAND"
        echo "Available commands: pack, publish-nuget, install"
        exit 1
        ;;
esac
