#!/bin/sh

# ForestLog - A minimalist logger interface.
# Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
#
# Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

echo ""
echo "==========================================================="
echo "Build ForestLog"
echo ""

# git clean -xfd

dotnet build -p:Configuration=Release -p:Platform="Any CPU"
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts ForestLog.Core/ForestLog.Core.csproj
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts ForestLog/ForestLog.csproj
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts ForestLog.JsonLines/ForestLog.JsonLines.csproj
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts ForestLog.Extensions.Logging/ForestLog.Extensions.Logging.csproj
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts ForestLog.MQTTnet31/ForestLog.MQTTnet31.csproj
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts FSharp.ForestLog/FSharp.ForestLog.fsproj
