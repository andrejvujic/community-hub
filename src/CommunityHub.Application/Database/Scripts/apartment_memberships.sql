CREATE TABLE apartment_memberships (
	acceptance_date DATE NOT NULL,
	apartment_id BIGINT REFERENCES apartments(id) NOT NULL,
	building_resident_id BIGINT REFERENCES users(id) NOT NULL,
	PRIMARY KEY (apartment_id, building_resident_id)
);