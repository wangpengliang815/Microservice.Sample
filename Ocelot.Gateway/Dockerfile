# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# ���� `mcr.microsoft.com/dotnet/aspnet:5.0 AS base` ����������
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# ���ù���Ŀ¼,�粻���ڻᱻ����
WORKDIR /app
# Copy release�ļ������ݵ�����Ŀ¼app
COPY . /app
# ����.dll
ENTRYPOINT ["dotnet", "Ocelot.Gateway.dll"]
