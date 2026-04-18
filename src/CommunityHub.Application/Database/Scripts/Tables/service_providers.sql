CREATE TABLE service_providers (
    id BIGINT PRIMARY KEY REFERENCES users(id) ON DELETE CASCADE,
    city_id BIGINT REFERENCES cities(id) NOT NULL,
    country_id BIGINT REFERENCES countries(id) NOT NULL
);