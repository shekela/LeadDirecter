set -e

echo "Waiting for database to be ready..."
until docker exec $(docker ps -q -f name=db) pg_isready -U leaduser -d leadsdb > /dev/null 2>&1; do
  echo "Waiting for Postgres..."
  sleep 2
done

echo "Running EF Core migrations..."
docker compose run --rm migrate

echo "Migrations applied successfully."
