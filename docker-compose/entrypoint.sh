#!/bin/bash
set -e

echo "Starting SQL Server..."
/opt/mssql/bin/sqlservr &

echo "Waiting for SQL Server to be ready..."
until /opt/mssql-tools/bin/sqlcmd -S spacex-sql -U sa -P "${DB_PASSWORD}" -Q "SELECT 1"; do
    sleep 5
done

echo "SQL Server is ready!"

if [ -f /data/application/spacex-db.sql ]; then
    echo "Executing database setup..."
    /opt/mssql-tools/bin/sqlcmd -S spacex-sql -U sa -P "${DB_PASSWORD}" -i /data/application/spacex-db.sql
else
    echo "ERROR: SQL script not found!"
fi

# Keep SQL Server running
wait
