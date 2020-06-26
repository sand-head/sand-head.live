FROM node:12-alpine
ENV NODE_ENV=production
COPY . /usr/app
WORKDIR /usr/app
LABEL traefik.enable=true
LABEL traefik.http.routers.sand-head-live-frontend.rule="Host(`sand-head.live`)"
LABEL traefik.http.routers.sand-head-live-frontend.tls=true
LABEL traefik.http.routers.sand-head-live-frontend.tls.certresolver="le"
LABEL traefik.http.routers.sand-head-live-frontend.tls.domains[0].main="sand-head.live"
LABEL traefik.http.routers.sand-head-live-frontend.tls.domains[0].sans="www.sand-head.live"
EXPOSE 3000
CMD ["npm", "run", "start:prod"]