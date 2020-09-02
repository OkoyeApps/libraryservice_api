FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Library.Api/Library.Api.csproj", "Library.Api/"]
COPY ["Library.Domain/Library.Domain.csproj", "Library.Domain/"]
RUN dotnet restore "Library.Api/Library.Api.csproj"
COPY . .
WORKDIR "/src/Library.Api"
RUN dotnet build "Library.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Library.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN mkdir AppraisalResult 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Library.Api.dll"]
