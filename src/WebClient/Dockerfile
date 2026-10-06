# syntax=docker/dockerfile:1

FROM node:26.9.0-alpine AS build
WORKDIR /app
RUN npm install -g bun@1.4.2
COPY WebClient/package.json WebClient/bun.lock ./
COPY WebClient/scripts ./scripts
RUN bun install --frozen-lockfile
COPY WebClient/ ./
# PUBLIC_* values are compiled into the static assets at build time. The URLs the
# browser calls default to the local lab ports; the release workflow passes the
# production URLs explicitly. The issuer is not a URL the browser contacts: it
# must equal the IdentityApi `Jwt:Issuer` claim value, which every appsettings
# file (Development included) sets to the identity domain below.
ARG PUBLIC_WEBAPI_BASE_URL=http://localhost:5100/api
ARG PUBLIC_IDENTITY_API_BASE_URL=http://localhost:5200/api
ARG PUBLIC_IDENTITY_API_ISSUER=https://identity-cpnucleo.jonathanperis.tech
ENV PUBLIC_WEBAPI_BASE_URL=$PUBLIC_WEBAPI_BASE_URL \
    PUBLIC_IDENTITY_API_BASE_URL=$PUBLIC_IDENTITY_API_BASE_URL \
    PUBLIC_IDENTITY_API_ISSUER=$PUBLIC_IDENTITY_API_ISSUER
RUN bun run build

# Production dependencies only (OpenTelemetry for the preview server). Bun is
# needed to honor bun.lock, but it stays out of the runtime image.
FROM node:26.9.0-alpine AS prod-deps
WORKDIR /app
RUN npm install -g bun@1.4.2
COPY WebClient/package.json WebClient/bun.lock ./
COPY WebClient/scripts ./scripts
RUN bun install --frozen-lockfile --production

FROM node:26.9.0-alpine AS runtime
WORKDIR /app
ENV NODE_ENV=production \
    HOST=0.0.0.0 \
    PORT=5030
COPY WebClient/package.json ./
COPY WebClient/scripts ./scripts
COPY --from=prod-deps /app/node_modules ./node_modules
COPY --from=build /app/dist ./dist
USER node
EXPOSE 5030
HEALTHCHECK --interval=1m --timeout=5s --start-period=1m --start-interval=10s --retries=3 CMD wget -qO- http://localhost:5030/healthz >/dev/null || exit 1
# scripts/preview.mjs is a plain Node HTTP server that loads scripts/otel.mjs.
CMD ["node", "scripts/preview.mjs"]
