FROM node:12-alpine as build
COPY . /src
WORKDIR /src
RUN npm ci
ENV NODE_ENV=production
RUN npm run build

FROM node:12-alpine
WORKDIR /usr/app
ENV NODE_ENV=production
COPY --from=build /src/node_modules /usr/app/node_modules
COPY --from=build /src/build /usr/app/build
COPY --from=build /src/package.json /usr/app/package.json
LABEL traefik.enable=true
LABEL traefik.http.routers.sand-head-live-frontend.rule="Host(`sand-head.live`)"
LABEL traefik.http.routers.sand-head-live-frontend.tls=true
LABEL traefik.http.routers.sand-head-live-frontend.tls.certresolver="le"
LABEL traefik.http.routers.sand-head-live-frontend.tls.domains[0].main="sand-head.live"
LABEL traefik.http.routers.sand-head-live-frontend.tls.domains[0].sans="www.sand-head.live"
EXPOSE 3000
CMD ["npm", "run", "start:prod"]