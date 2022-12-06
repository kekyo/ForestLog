@echo off

rem ForestLog - A minimalist logger interface.
rem Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
rem
rem Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

echo.
echo "==========================================================="
echo "Build ForestLog"
echo.

rem git clean -xfd

dotnet build -p:Configuration=Release -p:Platform=AnyCPU ForestLog\ForestLog.csproj
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts ForestLog.Core\ForestLog.Core.csproj
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts ForestLog\ForestLog.csproj
