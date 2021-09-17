#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Web.Client/Web.Client.csproj", "Web.Client/"]
RUN dotnet restore "Web.Client/Web.Client.csproj"
COPY . .
WORKDIR "/src/Web.Client"
RUN dotnet build "Web.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.Client.dll"]