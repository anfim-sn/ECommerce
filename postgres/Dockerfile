#Base image for build stage
FROM alpine:latest AS builder
WORKDIR /app
COPY postgres-init ./
RUN apk add --no-cache openssl

#Base image for the final Postgres image
FROM postgres:latest

#Copy initialization scripts from the build stage
COPY --from=builder /app /docker-entrypoint-initdb.d

#Set the Postgres environment variables (replace with your actual values)
ENV POSTGRES_USERNAME=db
ENV POSTGRES_PASSWORD=1234
ENV POSTGRES_DB=ecommerce

#Expose the Postgres port
EXPOSE 5432