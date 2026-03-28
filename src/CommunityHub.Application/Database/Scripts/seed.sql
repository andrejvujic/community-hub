-- Prvo je potrebno drop-ovati relacije posts i users.
-- DROP TABLE posts;
-- DROP TABLE users;

INSERT INTO users (username, user_role, password, name, surname) VALUES
('radovan.vitomirov', 'building_manager', '12345', 'Radovan', 'Vitomirov'),
('andrej.vujic', 'building_resident', '12345', 'Andrej', 'Vujić'),
('stefan.trkulja', 'neighborhood_coordinator', '12345', 'Stefan', 'Trkulja'),
('aleks.senji', 'citizen', '12345', 'Aleks', 'Senji');
