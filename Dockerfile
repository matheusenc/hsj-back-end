# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

# Os .csproj vêm antes do código para o restore virar camada cacheada:
# só refaz o download de pacotes quando uma referência muda.
COPY src/Backend/HospitalSaoJose.Api/HospitalSaoJose.Api.csproj                        src/Backend/HospitalSaoJose.Api/
COPY src/Backend/HospitalSaoJose.Application/HospitalSaoJose.Application.csproj        src/Backend/HospitalSaoJose.Application/
COPY src/Backend/HospitalSaoJose.Domain/HospitalSaoJose.Domain.csproj                  src/Backend/HospitalSaoJose.Domain/
COPY src/Backend/HospitalSaoJose.Infrastructure/HospitalSaoJose.Infrastructure.csproj  src/Backend/HospitalSaoJose.Infrastructure/
COPY src/Shared/HospitalSaoJose.Communication/HospitalSaoJose.Communication.csproj     src/Shared/HospitalSaoJose.Communication/
COPY src/Shared/HospitalSaoJose.Exception/HospitalSaoJose.Exception.csproj             src/Shared/HospitalSaoJose.Exception/

RUN dotnet restore src/Backend/HospitalSaoJose.Api/HospitalSaoJose.Api.csproj

COPY src/ src/

RUN dotnet publish src/Backend/HospitalSaoJose.Api/HospitalSaoJose.Api.csproj \
    --configuration Release \
    --no-restore \
    --output /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# gcServer=0 (workstation GC) porque a VPS é de 1 vCPU: o server GC reserva
# heap por core e desperdiça memória num host desse tamanho.
ENV ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_gcServer=0

COPY --from=build /app .

# O volume de documentos é montado neste caminho. Criar o diretório já com o dono
# certo faz o Docker propagar a permissão para o volume nomeado na primeira subida.
RUN mkdir -p /var/hsj/storage && chown -R app:app /var/hsj/storage

USER app
EXPOSE 8080

ENTRYPOINT ["dotnet", "HospitalSaoJose.Api.dll"]