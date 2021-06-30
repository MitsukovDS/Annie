#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["../Annie.Web/Annie.Web.csproj", "../Annie.Web/"]
RUN dotnet restore "../Annie.Web/Annie.Web.csproj"
COPY . .
WORKDIR "/src/../Annie.Web"
RUN dotnet build "Annie.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Annie.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Annie.Web.dll"]