#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["Tgs.Game.Type.Round.Aggregate.Service/Tgs.Game.Type.Round.Aggregate.Service.csproj", "Tgs.Game.Type.Round.Aggregate.Service/"]
COPY ["Tgs.Game.Type.Round.Aggregate.Core/Tgs.Game.Type.Round.Aggregate.Core.csproj", "Tgs.Game.Type.Round.Aggregate.Core/"]
COPY ["Tgs.Game.Type.Round.Aggregate.Repositories/Tgs.Game.Type.Round.Aggregate.Repositories.csproj", "Tgs.Game.Type.Round.Aggregate.Repositories/"]
RUN dotnet restore "Tgs.Game.Type.Round.Aggregate.Service/Tgs.Game.Type.Round.Aggregate.Service.csproj"
COPY . .
WORKDIR "/src/Tgs.Game.Type.Round.Aggregate.Service"
RUN dotnet build "Tgs.Game.Type.Round.Aggregate.Service.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.Game.Type.Round.Aggregate.Service.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.Game.Type.Round.Aggregate.Service.dll"]