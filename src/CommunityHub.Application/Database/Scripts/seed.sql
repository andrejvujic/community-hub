INSERT INTO users (id, username, user_role, password, name, surname) VALUES
-- Upravnici zgrada
(1,  'milan.jovanovic',   'building_manager', '123', 'Milan',    'Jovanović'),
(2,  'dragan.petrovic',   'building_manager', '123', 'Dragan',   'Petrović'),
(3,  'ivana.markovic',    'building_manager', '123', 'Ivana',    'Marković'),
(4,  'nenad.stankovic',   'building_manager', '123', 'Nenad',    'Stanković'),
(5,  'jelena.nikolic',    'building_manager', '123', 'Jelena',   'Nikolić'),

-- Stanari
(6,  'ana.ivanovic',      'building_resident', '123', 'Ana',      'Ivanović'),
(7,  'marko.pavlovic',    'building_resident', '123', 'Marko',    'Pavlović'),
(8,  'tamara.milosevic',  'building_resident', '123', 'Tamara',   'Milošević'),
(9,  'nikola.djordjevic', 'building_resident', '123', 'Nikola',   'Đorđević'),
(10, 'marija.ilic',       'building_resident', '123', 'Marija',   'Ilić'),

-- Koordinatori naselja
(11, 'aleksandar.savic',  'neighborhood_coordinator', '123', 'Aleksandar', 'Savić'),
(12, 'katarina.matic',    'neighborhood_coordinator', '123', 'Katarina',   'Matić'),
(13, 'bojan.ristic',      'neighborhood_coordinator', '123', 'Bojan',      'Ristić'),
(14, 'sofija.lazic',      'neighborhood_coordinator', '123', 'Sofija',     'Lazić'),
(15, 'vladimir.kostic',   'neighborhood_coordinator', '123', 'Vladimir',   'Kostić');


INSERT INTO countries (id, name) VALUES
(1, 'Srbija'),
(2, 'Bosna i Hercegovina'),
(3, 'Crna Gora'),
(4, 'Hrvatska'),
(5, 'Severna Makedonija');

INSERT INTO cities (id, name, country_id) VALUES
(1, 'Beograd', 1),
(2, 'Novi Sad', 1),
(3, 'Niš', 1),
(4, 'Kragujevac', 1),
(5, 'Subotica', 1);


INSERT INTO addresses (id, street_name, street_number, neighborhood, city_id) VALUES
(1, 'Bulevar kralja Aleksandra', '243', 'Vračar', 1),
(2, 'Jurija Gagarina', '128', 'Novi Beograd', 1),
(3, 'Bulevar oslobođenja', '97', 'Grbavica', 2),
(4, 'Dušanova', '14', 'Centar', 3),
(5, 'Lepenički bulevar', '32A', 'Aerodrom', 4);

INSERT INTO buildings (id, floor_count, building_manager_id, address_id) VALUES
(1, 6, 1, 1),
(2, 9, 2, 2),
(3, 5, 3, 3),
(4, 4, 4, 4),
(5, 7, 5, 5);

INSERT INTO images (id, path, resource_type, owner_id) VALUES
(1, '/uploads/zgrade/zgrada-vracar-243.jpg', 'buildings', 1),
(2, '/uploads/zgrade/zgrada-nbg-128.jpg', 'buildings', 2),
(3, '/uploads/zgrade/zgrada-grbavica-97.jpg', 'buildings', 3),
(4, '/uploads/naselja/vracar.jpg', 'neighborhoods', 1),
(5, '/uploads/naselja/novi-beograd.jpg', 'neighborhoods', 2);

INSERT INTO appartments (id, appartment_number, floor_number, building_id) VALUES
(1, 12, 1, 1),
(2, 27, 2, 2),
(3, 15, 1, 3),
(4, 8,  2, 4),
(5, 34, 3, 5);

INSERT INTO appartment_memberships (acceptance_date, appartment_id, building_resident_id) VALUES
('2025-09-01', 1, 6),
('2025-09-03', 2, 7),
('2025-09-05', 3, 8),
('2025-09-10', 4, 9),
('2025-09-12', 5, 10);

INSERT INTO appartment_access_requests (creation_date, status, comment, appartment_id, building_resident_id) VALUES
('2026-02-03', 'waiting',  'Podnet zahtev za pristup zbog promene prebivališta.', 2, 6),
('2026-02-05', 'accepted', 'Dostavljen ugovor o zakupu i potvrda upravnika.',    3, 7),
('2026-02-08', 'rejected', 'Priložena dokumentacija nije bila potpuna.',         4, 8),
('2026-02-11', 'waiting',  'Stanar je u postupku prijave boravišta.',            5, 9),
('2026-02-14', 'accepted', 'Nakon provere vlasništva zahtev je odobren.',        1, 10);

INSERT INTO neighborhoods (id, name, description, budget, city_id, coordinator_id) VALUES
(1, 'Vračar', 'Gradsko naselje sa pretežno stambenim zgradama, razvijenom infrastrukturom i aktivnim učešćem stanara u lokalnim inicijativama.', 850000, 1, 11),
(2, 'Novi Beograd - Blok 45', 'Veliko urbano naselje sa višespratnicama, uređenim zelenim površinama i izraženim potrebama za komunalnom koordinacijom.', 1200000, 1, 12),
(3, 'Grbavica', 'Gusto naseljeni deo Novog Sada sa kombinacijom porodičnih i kolektivnih stambenih objekata.', 640000, 2, 13),
(4, 'Centar', 'Centralna gradska zona sa velikim brojem stanara, poslovnih objekata i intenzivnim komunalnim aktivnostima.', 710000, 3, 14),
(5, 'Aerodrom', 'Savremeno gradsko naselje sa višeporodičnim zgradama i stalnim ulaganjima u saobraćajnu i komunalnu infrastrukturu.', 930000, 4, 15);

INSERT INTO streets (id, name, start_number, end_number, neighborhood_id) VALUES
(1, 'Njegoševa', 1, 58, 1),
(2, 'Jurija Gagarina', 1, 241, 2),
(3, 'Bulevar oslobođenja', 1, 126, 3),
(4, 'Vožda Karađorđa', 1, 48, 4),
(5, 'Grada Sirena', 1, 92, 5);

INSERT INTO neighborhood_memberships (id, acceptance_date, neighborhood_id) VALUES
(1, '2025-08-20', 1),
(2, '2025-08-22', 2),
(3, '2025-08-25', 3),
(4, '2025-08-28', 4),
(5, '2025-08-30', 5);

INSERT INTO citizens (id, username, user_role, password, name, surname, address_id, membership_id) VALUES
(101, 'petar.simic',      'citizen', '123', 'Petar',    'Simić',      1, 1),
(102, 'milica.todorovic', 'citizen', '123', 'Milica',   'Todorović',  2, 2),
(103, 'uros.blagojevic',  'citizen', '123', 'Uroš',     'Blagojević', 3, 3),
(104, 'andjela.zivkovic', 'citizen', '123', 'Anđela',   'Živković',   4, 4),
(105, 'dusan.radulovic',  'citizen', '123', 'Dušan',    'Radulović',  5, 5);

INSERT INTO neighborhood_access_requests (citizen_id, neighborhood_id, status, creation_date, comment) VALUES
(101, 2, 'waiting',  '2026-03-01', 'Podnet zahtev nakon preseljenja na novu adresu.'),
(102, 3, 'accepted', '2026-03-02', 'Zahtev odobren nakon potvrde prebivališta.'),
(103, 4, 'rejected', '2026-03-04', 'Adresa prebivališta ne pripada traženom naselju.'),
(104, 5, 'waiting',  '2026-03-06', 'U toku je provera prijavljene adrese stanovanja.'),
(105, 1, 'accepted', '2026-03-07', 'Članstvo odobreno na osnovu validne dokumentacije.');