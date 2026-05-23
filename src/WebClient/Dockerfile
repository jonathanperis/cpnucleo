FROM node:22-alpine AS build
WORKDIR /app
COPY WebClient/package*.json ./
RUN npm ci
COPY WebClient/ ./
RUN npm run build

FROM nginx:1.27-alpine AS runtime
RUN mkdir -p /var/run \
    && touch /var/run/nginx.pid \
    && chown -R nginx:nginx /var/cache/nginx /var/log/nginx /var/run/nginx.pid /usr/share/nginx/html /etc/nginx/conf.d
COPY --chown=nginx:nginx WebClient/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build --chown=nginx:nginx /app/dist/ /usr/share/nginx/html/
USER nginx
EXPOSE 5030
HEALTHCHECK --interval=30s --timeout=5s --retries=3 CMD wget -qO- http://localhost:5030/healthz >/dev/null || exit 1
