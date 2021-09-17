# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# 基于 `mcr.microsoft.com/dotnet/aspnet:5.0 AS base` 来构建镜像
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# 设置工作目录,如不存在会被创建
WORKDIR /app
# Copy release文件夹内容到工作目录app
COPY . /app
# 运行.dll
ENTRYPOINT ["dotnet", "Product.Api.dll"]
ENV ASPNETCORE_URLS=http://+:8090