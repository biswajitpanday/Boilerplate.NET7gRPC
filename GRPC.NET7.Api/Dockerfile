#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GRPC.NET7.Api/GRPC.NET7.Api.csproj", "GRPC.NET7.Api/"]
COPY ["GRPC.NET7.Proto/GRPC.NET7.Proto.csproj", "GRPC.NET7.Proto/"]
COPY ["GRPC.NET7.Profile.Core/GRPC.NET7.Profile.Core.csproj", "GRPC.NET7.Profile.Core/"]
COPY ["GRPC.NET7.Service/GRPC.NET7.Service.csproj", "GRPC.NET7.Service/"]
COPY ["GRPC.NET7.Repository/GRPC.NET7.Repository.csproj", "GRPC.NET7.Repository/"]
COPY ["GRPC.NET7.Core/GRPC.NET7.Core.csproj", "GRPC.NET7.Core/"]
RUN dotnet restore "GRPC.NET7.Api/GRPC.NET7.Api.csproj"
COPY . .
WORKDIR "/src/GRPC.NET7.Api"
RUN dotnet build "GRPC.NET7.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GRPC.NET7.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GRPC.NET7.Api.dll"]