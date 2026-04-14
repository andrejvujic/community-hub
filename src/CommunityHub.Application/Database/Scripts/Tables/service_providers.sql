CREATE TABLE service_providers (
    city_id BIGINT REFERENCES cities(id) NOT NULL,
    country_id BIGINT REFERENCES countries(id) NOT NULL,
    PRIMARY KEY (id)
) INHERITS (users);