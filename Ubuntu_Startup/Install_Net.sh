#!/bin/bash

# Before you install .NET, run the following commands to add the Microsoft package signing key to your list of trusted keys and add the package repository
echo "Add the Microsoft package signing key to your list of trusted keys and add the package repository:"
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install the .NET SDK
echo "Install\Update the .NET SDK"
sudo apt-get update && \
  sudo apt-get install -y dotnet6
  


#sudo apt install aspnetcore-runtime-6.0=6.0.8-1 dotnet-apphost-pack-6.0=6.0.8-1 dotnet-host=6.0.8-1 dotnet-hostfxr-6.0=6.0.8-1 dotnet-runtime-6.0=6.0.8-1 dotnet-sdk-6.0=6.0.400-1 dotnet-targeting-pack-6.0=6.0.8-1

# Clear dotnet package
#echo "Clear dotnet package:"
#sudo apt remove dotnet*

# Clear aspnetcore package
#echo "Clear aspnetcore package:"
#sudo apt remove aspnetcore*

# Clear netstandart package
#echo "Clear netstandart package:"
#sudo apt remove netstandart*

# Remove Microsoft repo
#echo "Remove Microsoft repo:"
#sudo rm /etc/apt/sources.list.d/microsoft-prod.list

# Restore Microsoft packages
#echo "Restore Microsoft packages:"

#echo "apt update:"
#sudo apt update

#echo "apt install dotnet-sdk-6.0:"
#sudo apt install dotnet-sdk-6.0
