﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Askme.Reference.Backend.Api/Askme.Reference.Backend.Api.csproj", "Askme.Reference.Backend.Api/"]
RUN dotnet restore "Askme.Reference.Backend.Api/Askme.Reference.Backend.Api.csproj"
COPY . .
WORKDIR "/src/Askme.Reference.Backend.Api"
RUN dotnet build "Askme.Reference.Backend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Askme.Reference.Backend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Askme.Reference.Backend.Api.dll"]
