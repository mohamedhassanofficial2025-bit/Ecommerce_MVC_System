#second stage 
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app

#downlaod pakage manager 
RUN apk add --no-cache icu-libs icu-data-full
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 8080

#first stage base
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

COPY ["EcommerceSystem.PL/EcommerceSystem.PL.csproj", "EcommerceSystem.PL/"]
COPY ["EcommerceSystem.BLL/EcommerceSystem.BLL.csproj", "EcommerceSystem.BLL/"]
COPY ["EcommerceSystem.DAL/EcommerceSystem.DAL.csproj", "EcommerceSystem.DAL/"]
COPY ["EcommerceSystem/EcommerceSystem.csproj", "EcommerceSystem/"]


#dependency restore
RUN dotnet restore "EcommerceSystem.PL/EcommerceSystem.PL.csproj"

COPY . .
WORKDIR /src/EcommerceSystem.PL

RUN dotnet publish "EcommerceSystem.PL.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EcommerceSystem.PL.dll"]
