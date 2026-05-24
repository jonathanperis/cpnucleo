FROM node:22-alpine AS build
WORKDIR /app
RUN npm install -g bun@1.3.11
COPY WebClient/package.json WebClient/bun.lock ./
COPY WebClient/scripts ./scripts
RUN bun install --frozen-lockfile
COPY WebClient/ ./
ARG PUBLIC_WEBAPI_BASE_URL=https://api-cpnucleo.jonathanperis.tech/api
ARG PUBLIC_IDENTITY_API_BASE_URL=https://identity-cpnucleo.jonathanperis.tech/api
ARG PUBLIC_IDENTITY_API_ISSUER=https://identity-cpnucleo.jonathanperis.tech
ENV PUBLIC_WEBAPI_BASE_URL=$PUBLIC_WEBAPI_BASE_URL \
    PUBLIC_IDENTITY_API_BASE_URL=$PUBLIC_IDENTITY_API_BASE_URL \
    PUBLIC_IDENTITY_API_ISSUER=$PUBLIC_IDENTITY_API_ISSUER
RUN bun run build

FROM node:22-alpine AS runtime
WORKDIR /app
ENV NODE_ENV=Production \
    HOST=0.0.0.0 \
    PORT=5030
RUN npm install -g bun@1.3.11
COPY WebClient/package.json WebClient/bun.lock ./
COPY WebClient/scripts ./scripts
RUN bun install --frozen-lockfile --production
COPY --from=build /app/dist ./dist
USER node
EXPOSE 5030
HEALTHCHECK --interval=30s --timeout=5s --retries=3 CMD wget -qO- http://localhost:5030/healthz >/dev/null || exit 1
CMD ["bun", "run", "preview"]
