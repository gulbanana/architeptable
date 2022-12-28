#!/bin/sh
[ -e artifacts ] && rm -r artifacts
dotnet ef dbcontext optimize --output-dir Data/Precompiled/ --namespace Architeptable.Data.Precompiled
dotnet publish -c Release -o artifacts/ -r win10-x64 -p SelfContained=false 