#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Ocelot.Geteway/Ocelot.Geteway.csproj", "Ocelot.Geteway/"]
RUN dotnet restore "Ocelot.Geteway/Ocelot.Geteway.csproj"
COPY . .
WORKDIR "/src/Ocelot.Geteway"
RUN dotnet build "Ocelot.Geteway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ocelot.Geteway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ocelot.Geteway.dll"]