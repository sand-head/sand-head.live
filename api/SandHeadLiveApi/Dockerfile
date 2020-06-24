FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY ./ ./
LABEL traefik.enable=true
LABEL traefik.http.routers.sand-head-live-api.rule="Host(`sand-head.live`) && Path(`/api`)"
LABEL traefik.http.routers.sand-head-live-api.tls=true
LABEL traefik.http.routers.sand-head-live-api.tls.certresolver="le"
LABEL traefik.http.routers.sand-head-live-api.tls.domains[0].main="sand-head.live"
LABEL traefik.http.routers.sand-head-live-api.tls.domains[0].sans="www.sand-head.live"
ENTRYPOINT ["dotnet", "SandHeadLiveApi.dll"]