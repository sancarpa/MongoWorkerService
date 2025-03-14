# Usa l'immagine di runtime .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app

# Copia i file del progetto
COPY . .

# Pubblica l'applicazione
RUN dotnet publish -c Release -o out

# Usa l'immagine di runtime .NET
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=base /app/out ./

# Definisci il comando di avvio
ENTRYPOINT ["dotnet", "MongoWorkerService.dll"]