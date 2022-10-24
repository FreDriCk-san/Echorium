#!/bin/bash

# Move to upper folder
echo "/---------------------Move to upper folder--------------------/"
cd ..

# Move to main project
echo "/---------------------Move to main project--------------------/"
cd Echorium

# Clean project files
echo "/---------------------Clean project files--------------------/"
dotnet clean

# Build project files
echo "/---------------------Build project files--------------------/"
dotnet build

# Run program
echo "/---------------------Run program--------------------/"
dotnet run

