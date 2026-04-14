CREATE TABLE neighborhood_access_requests (
	citizen_id BIGINT REFERENCES citizens(id),
	neighborhood_id BIGINT REFERENCES neighborhoods(id),
	status statuses NOT NULL,
	creation_date DATE NOT NULL,
	comment VARCHAR(300),
	PRIMARY KEY (citizen_id, neighborhood_id)
);