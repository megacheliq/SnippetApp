FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY Simpl.Snippets.Service/*.csproj ./Simpl.Snippets.Service/
RUN dotnet restore ./Simpl.Snippets.Service/Simpl.Snippets.Service.csproj

COPY . .
WORKDIR /app/Simpl.Snippets.Service
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/Simpl.Snippets.Service/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "Simpl.Snippets.Service.dll"]