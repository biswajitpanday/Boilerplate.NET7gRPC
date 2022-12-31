#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GRPC.NET7.ProfileApi/GRPC.NET7.Profile.Api.csproj", "GRPC.NET7.ProfileApi/"]
COPY ["GRPC.NET7.Profile.Service/GRPC.NET7.Profile.Service.csproj", "GRPC.NET7.Profile.Service/"]
COPY ["GRPC.NET7.Profile.Repository/GRPC.NET7.Profile.Repository.csproj", "GRPC.NET7.Profile.Repository/"]
COPY ["GRPC.NET7.Profile.Core/GRPC.NET7.Profile.Core.csproj", "GRPC.NET7.Profile.Core/"]
COPY ["GRPC.NET7.Proto/GRPC.NET7.Proto.csproj", "GRPC.NET7.Proto/"]
RUN dotnet restore "GRPC.NET7.ProfileApi/GRPC.NET7.Profile.Api.csproj"
COPY . .
WORKDIR "/src/GRPC.NET7.ProfileApi"
RUN dotnet build "GRPC.NET7.Profile.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GRPC.NET7.Profile.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GRPC.NET7.Profile.Api.dll"]