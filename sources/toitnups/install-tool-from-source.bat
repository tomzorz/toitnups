cd toitnups
dotnet build -c release
dotnet pack -c release -o nupkg
dotnet tool uninstall -g toitnups
dotnet tool install --add-source .\nupkg -g toitnups
cd ..