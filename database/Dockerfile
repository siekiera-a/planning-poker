FROM mcr.microsoft.com/mssql/server:latest

USER root

RUN apt-get -y update && \
      mkdir /usr/db

COPY all_in_one.sql *.sh /usr/db/

WORKDIR /usr/db

RUN chmod +x import-data.sh && \
      chmod +x entrypoint.sh

USER mssql

ENV ACCEPT_EULA Y
ENV SA_PASSWORD p#bqaOAh!b

EXPOSE 1433

ENTRYPOINT /bin/bash ./entrypoint.sh
