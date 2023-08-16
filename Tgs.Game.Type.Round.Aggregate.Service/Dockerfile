#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Tgs.Game.Round.Aggregate.Service/Tgs.Game.Round.Aggregate.Service.csproj", "Tgs.Game.Round.Aggregate.Service/"]
RUN dotnet restore "Tgs.Game.Round.Aggregate.Service/Tgs.Game.Round.Aggregate.Service.csproj"
COPY . .
WORKDIR "/src/Tgs.Game.Round.Aggregate.Service"
RUN dotnet build "Tgs.Game.Round.Aggregate.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tgs.Game.Round.Aggregate.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.Game.Round.Aggregate.Service.dll"]