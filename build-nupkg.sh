#!/bin/sh

# ForestLog - A minimalist logger interface.
# Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
#
# Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

echo
echo "==========================================================="
echo "Build ForestLog"
echo

# git clean -xfd

dotnet build -p:Configuration=Release -p:Platform=AnyCPU ForestLog/ForestLog.csproj
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts ForestLog.Core/ForestLog.Core.csproj
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts ForestLog/ForestLog.csproj
