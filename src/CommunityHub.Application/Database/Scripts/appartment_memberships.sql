CREATE TABLE appartment_memberships (
	acceptance_date DATE NOT NULL,
	appartment_id BIGINT REFERENCES appartments(id) NOT NULL,
	building_resident_id BIGINT REFERENCES users(id) NOT NULL,
	PRIMARY KEY (appartment_id, building_resident_id)
);