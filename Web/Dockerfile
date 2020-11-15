#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./MotoShop.WebAPI/MotoShop.WebAPI.csproj", "./MotoShop.WebAPI/"]
COPY ["./MotoShop.Services/MotoShop.Services.csproj", "./MotoShop.Services/"]
COPY ["./MotoShop.Data/MotoShop.Data.csproj", "./MotoShop.Data/"]
RUN dotnet restore "MotoShop.WebAPI/MotoShop.WebAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./MotoShop.WebAPI/MotoShop.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./MotoShop.WebAPI/MotoShop.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "./MotoShop.WebAPI/MotoShop.WebAPI.dll"]