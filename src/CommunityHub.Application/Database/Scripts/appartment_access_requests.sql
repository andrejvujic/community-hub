CREATE TABLE appartment_access_requests (
	creation_date DATE NOT NULL,
	status statuses NOT NULL,
	comment VARCHAR(300),
	appartment_id BIGINT REFERENCES appartments(id) NOT NULL,
	building_resident_id BIGINT REFERENCES users(id) NOT NULL,
	PRIMARY KEY (appartment_id, building_resident_id)
);