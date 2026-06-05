-- ako pokretanje skripte baci exception pokreni ROLLBACK;
-- popravi gresku i pokusaj ponovo
BEGIN;

INSERT INTO users (id, username, user_role, password, name, surname) VALUES
(1,  'milan.jovanovic',    'building_manager',         '123', 'Milan',      'Jovanović'),
(2,  'dragan.petrovic',    'building_manager',         '123', 'Dragan',     'Petrović'),
(3,  'ivana.markovic',     'building_manager',         '123', 'Ivana',      'Marković'),
(4,  'nenad.stankovic',    'building_manager',         '123', 'Nenad',      'Stanković'),
(5,  'jelena.nikolic',     'building_manager',         '123', 'Jelena',     'Nikolić'),
(6,  'ana.ivanovic',       'building_resident',        '123', 'Ana',        'Ivanović'),
(7,  'marko.pavlovic',     'building_resident',        '123', 'Marko',      'Pavlović'),
(8,  'tamara.milosevic',   'building_resident',        '123', 'Tamara',     'Milošević'),
(9,  'nikola.djordjevic',  'building_resident',        '123', 'Nikola',     'Đorđević'),
(10, 'marija.ilic',        'building_resident',        '123', 'Marija',     'Ilić'),
(11, 'aleksandar.savic',   'neighborhood_coordinator', '123', 'Aleksandar', 'Savić'),
(12, 'katarina.matic',     'neighborhood_coordinator', '123', 'Katarina',   'Matić'),
(13, 'bojan.ristic',       'neighborhood_coordinator', '123', 'Bojan',      'Ristić'),
(14, 'sofija.lazic',       'neighborhood_coordinator', '123', 'Sofija',     'Lazić'),
(15, 'vladimir.kostic',    'neighborhood_coordinator', '123', 'Vladimir',   'Kostić'),
(16, 'ivan.zoric',         'building_resident',        '123', 'Ivan',       'Zorić'),
(17, 'teodora.knezevic',   'building_resident',        '123', 'Teodora',    'Knežević'),
(18, 'stefan.matic',       'building_resident',        '123', 'Stefan',     'Matić'),
(19, 'jelena.rakic',       'building_resident',        '123', 'Jelena',     'Rakić'),
(20, 'nemanja.panic',      'building_resident',        '123', 'Nemanja',    'Panić');

INSERT INTO countries (id, name) VALUES
(1, 'Srbija'),
(2, 'Bosna i Hercegovina'),
(3, 'Crna Gora'),
(4, 'Hrvatska'),
(5, 'Severna Makedonija'),
(6, 'Slovenija'),
(7, 'Mađarska'),
(8, 'Rumunija'),
(9, 'Bugarska'),
(10, 'Albanija'),
(11, 'Grčka'),
(12, 'Austrija'),
(13, 'Nemačka'),
(14, 'Italija'),
(15, 'Francuska');

INSERT INTO cities (id, name, country_id) VALUES
(1,  'Beograd',      1),
(2,  'Novi Sad',     1),
(3,  'Niš',          1),
(4,  'Kragujevac',   1),
(5,  'Subotica',     1),
(6,  'Pančevo',      1),
(7,  'Čačak',        1),
(8,  'Zrenjanin',    1),
(9,  'Sombor',       1),
(10, 'Leskovac',     1),
(11, 'Užice',        1),
(12, 'Valjevo',      1),
(13, 'Kruševac',     1),
(14, 'Šabac',        1),
(15, 'Vranje',       1);

INSERT INTO neighborhoods (id, name, description, budget, city_id, coordinator_id) VALUES
(1,  'Vračar',                 'Gradsko naselje sa razvijenom infrastrukturom i aktivnim učešćem stanara.', 850000, 1, 11),
(2,  'Blok 45',                'Veliko urbano naselje sa višespratnicama i zelenim površinama.', 1200000, 1, 12),
(3,  'Grbavica',               'Gusto naseljeni deo Novog Sada sa kombinacijom porodičnih i kolektivnih objekata.', 640000, 2, 13),
(4,  'Centar',                 'Centralna gradska zona sa intenzivnim komunalnim aktivnostima.', 710000, 3, 14),
(5,  'Aerodrom',               'Savremeno gradsko naselje sa stalnim ulaganjima u infrastrukturu.', 930000, 4, 15),
(6,  'Mali Bajmok',            'Mirnije naselje sa pretežno porodičnim objektima i manjim stambenim blokovima.', 420000, 5, 11),
(7,  'Strelište',              'Popularno gradsko naselje sa mešovitom namenom i razvijenim sadržajima.', 510000, 6, 12),
(8,  'Ljubić',                 'Stambeno naselje sa porodičnim kućama i novijim višeporodičnim zgradama.', 460000, 7, 13),
(9,  'Bagljaš',                'Veliko stambeno naselje sa brojnim zgradama i komunalnim potrebama.', 550000, 8, 14),
(10, 'Nova Selenča',           'Uređeno gradsko naselje sa stabilnom infrastrukturom.', 390000, 9, 15),
(11, 'Dubočica',               'Naselje sa značajnim brojem stanovnika i rastućim potrebama za održavanjem.', 470000, 10, 11),
(12, 'Krčagovo',               'Brdsko-gradsko naselje sa mešovitom strukturom stanovanja.', 360000, 11, 12),
(13, 'Peti puk',               'Gradska zona sa višespratnicama i porodičnim objektima.', 440000, 12, 13),
(14, 'Rasadnik',               'Stambeni deo grada sa izraženim potrebama za lokalnom koordinacijom.', 410000, 13, 14),
(15, 'Trkalište',              'Urbano naselje sa aktivnom zajednicom stanara.', 530000, 14, 15);

INSERT INTO streets (id, name, city_id) VALUES
(1,  'Njegoševa',                    1),
(2,  'Jurija Gagarina',              1),
(3,  'Bulevar Oslobođenja',          2),
(4,  'Vožda Karađorđa',              3),
(5,  'Grada Sirena',                 4),
(6,  'Bulevar kralja Aleksandra',    1),
(7,  'Dušanova',                     3),
(8,  'Lepenički bulevar',            4),
(9,  'Matije Gupca',                 5),
(10, 'Žarka Zrenjanina',             6),
(11, 'Cara Dušana',                  7),
(12, 'Cara Lazara',                  8),
(13, 'Venac Radomira Putnika',       9),
(14, 'Bulevar Nikole Pašića',        10),
(15, 'Knez Mihailova',               11);

INSERT INTO street_sections (id, start_number, end_number, street_id, neighborhood_id, side) VALUES
(1,  1,  58,  1,  1,  'both'),
(2,  1,  241, 2,  2,  'both'),
(3,  1,  126, 3,  3,  'both'),
(4,  1,  48,  4,  4,  'both'),
(5,  1,  92,  5,  5,  'both'),
(6,  1,  44,  9,  6,  'both'),
(7,  1,  80,  10, 7,  'both'),
(8,  1,  67,  11, 8,  'both'),
(9,  1,  120, 12, 9,  'both'),
(10, 1,  56,  13, 10, 'both'),
(11, 1,  88,  14, 11, 'both'),
(12, 1,  34,  15, 12, 'both'),
(13, 1,  73,  6,  13, 'even'),
(14, 2,  65,  7,  14, 'odd'),
(15, 1,  98,  8,  15, 'both');

INSERT INTO addresses (id, street_number, neighborhood, city_id, street_id) VALUES
(1,  '17', 'Vračar',         1,  6),
(2,  '128', 'Blok 45',       1,  2),
(3,  '97',  'Grbavica',      2,  3),
(4,  '14',  'Centar',        3,  7),
(5,  '32', 'Aerodrom',       4,  8),
(6,  '18',  'Mali Bajmok',   5,  9),
(7,  '44',  'Strelište',     6,  10),
(8,  '27',  'Ljubić',        7,  11),
(9,  '51',  'Bagljaš',       8,  12),
(10, '12',  'Nova Selenča',  9,  13),
(11, '84',  'Dubočica',      10, 14),
(12, '7',   'Krčagovo',      11, 15),
(13, '29',  'Peti puk',      12, 6),
(14, '63',  'Rasadnik',      13, 7),
(15, '41',  'Trkalište',     14, 8);

INSERT INTO buildings (id, floor_count, apartment_count, building_manager_id, address_id) VALUES
(1,  6,  24, 1,  1),
(2,  9,  45, 2,  2),
(3,  5,  15, 3,  3),
(4,  4,  12, 4,  4),
(5,  7,  28, 5,  5),
(6,  3,  6,  1,  6),
(7,  8,  32, 2,  7),
(8,  5,  15, 3,  8),
(9,  6,  24, 4,  9),
(10, 4,  8,  5,  10),
(11, 7,  35, 1,  11),
(12, 5,  15, 2,  12),
(13, 6,  18, 3,  13),
(14, 4,  8,  4,  14),
(15, 9,  54, 5,  15);


INSERT INTO apartments (apartment_number, floor_number, building_id) VALUES
(101,1,1),(102,1,1),(103,1,1),(104,1,1),
(201,2,1),(202,2,1),(203,2,1),(204,2,1),
(301,3,1),(302,3,1),(303,3,1),(304,3,1),
(401,4,1),(402,4,1),(403,4,1),(404,4,1),
(501,5,1),(502,5,1),(503,5,1),(504,5,1),
(601,6,1),(602,6,1),(603,6,1),(604,6,1),
(101,1,2),(102,1,2),(103,1,2),(104,1,2),(105,1,2),
(201,2,2),(202,2,2),(203,2,2),(204,2,2),(205,2,2),
(301,3,2),(302,3,2),(303,3,2),(304,3,2),(305,3,2),
(401,4,2),(402,4,2),(403,4,2),(404,4,2),(405,4,2),
(501,5,2),(502,5,2),(503,5,2),(504,5,2),(505,5,2),
(601,6,2),(602,6,2),(603,6,2),(604,6,2),(605,6,2),
(701,7,2),(702,7,2),(703,7,2),(704,7,2),(705,7,2),
(801,8,2),(802,8,2),(803,8,2),(804,8,2),(805,8,2),
(901,9,2),(902,9,2),(903,9,2),(904,9,2),(905,9,2),
(101,1,3),(102,1,3),(103,1,3),
(201,2,3),(202,2,3),(203,2,3),
(301,3,3),(302,3,3),(303,3,3),
(401,4,3),(402,4,3),(403,4,3),
(501,5,3),(502,5,3),(503,5,3),
(101,1,4),(102,1,4),(103,1,4),
(201,2,4),(202,2,4),(203,2,4),
(301,3,4),(302,3,4),(303,3,4),
(401,4,4),(402,4,4),(403,4,4),
(101,1,5),(102,1,5),(103,1,5),(104,1,5),
(201,2,5),(202,2,5),(203,2,5),(204,2,5),
(301,3,5),(302,3,5),(303,3,5),(304,3,5),
(401,4,5),(402,4,5),(403,4,5),(404,4,5),
(501,5,5),(502,5,5),(503,5,5),(504,5,5),
(601,6,5),(602,6,5),(603,6,5),(604,6,5),
(701,7,5),(702,7,5),(703,7,5),(704,7,5),
(101,1,6),(102,1,6),
(201,2,6),(202,2,6),
(301,3,6),(302,3,6),
(101,1,7),(102,1,7),(103,1,7),(104,1,7),
(201,2,7),(202,2,7),(203,2,7),(204,2,7),
(301,3,7),(302,3,7),(303,3,7),(304,3,7),
(401,4,7),(402,4,7),(403,4,7),(404,4,7),
(501,5,7),(502,5,7),(503,5,7),(504,5,7),
(601,6,7),(602,6,7),(603,6,7),(604,6,7),
(701,7,7),(702,7,7),(703,7,7),(704,7,7),
(801,8,7),(802,8,7),(803,8,7),(804,8,7),
(101,1,8),(102,1,8),(103,1,8),
(201,2,8),(202,2,8),(203,2,8),
(301,3,8),(302,3,8),(303,3,8),
(401,4,8),(402,4,8),(403,4,8),
(501,5,8),(502,5,8),(503,5,8),
(101,1,9),(102,1,9),(103,1,9),(104,1,9),
(201,2,9),(202,2,9),(203,2,9),(204,2,9),
(301,3,9),(302,3,9),(303,3,9),(304,3,9),
(401,4,9),(402,4,9),(403,4,9),(404,4,9),
(501,5,9),(502,5,9),(503,5,9),(504,5,9),
(601,6,9),(602,6,9),(603,6,9),(604,6,9),
(101,1,10),(102,1,10),
(201,2,10),(202,2,10),
(301,3,10),(302,3,10),
(401,4,10),(402,4,10),
(101,1,11),(102,1,11),(103,1,11),(104,1,11),(105,1,11),
(201,2,11),(202,2,11),(203,2,11),(204,2,11),(205,2,11),
(301,3,11),(302,3,11),(303,3,11),(304,3,11),(305,3,11),
(401,4,11),(402,4,11),(403,4,11),(404,4,11),(405,4,11),
(501,5,11),(502,5,11),(503,5,11),(504,5,11),(505,5,11),
(601,6,11),(602,6,11),(603,6,11),(604,6,11),(605,6,11),
(701,7,11),(702,7,11),(703,7,11),(704,7,11),(705,7,11),
(101,1,12),(102,1,12),(103,1,12),
(201,2,12),(202,2,12),(203,2,12),
(301,3,12),(302,3,12),(303,3,12),
(401,4,12),(402,4,12),(403,4,12),
(501,5,12),(502,5,12),(503,5,12),
(101,1,13),(102,1,13),(103,1,13),
(201,2,13),(202,2,13),(203,2,13),
(301,3,13),(302,3,13),(303,3,13),
(401,4,13),(402,4,13),(403,4,13),
(501,5,13),(502,5,13),(503,5,13),
(601,6,13),(602,6,13),(603,6,13),
(101,1,14),(102,1,14),
(201,2,14),(202,2,14),
(301,3,14),(302,3,14),
(401,4,14),(402,4,14),
(101,1,15),(102,1,15),(103,1,15),(104,1,15),(105,1,15),(106,1,15),
(201,2,15),(202,2,15),(203,2,15),(204,2,15),(205,2,15),(206,2,15),
(301,3,15),(302,3,15),(303,3,15),(304,3,15),(305,3,15),(306,3,15),
(401,4,15),(402,4,15),(403,4,15),(404,4,15),(405,4,15),(406,4,15),
(501,5,15),(502,5,15),(503,5,15),(504,5,15),(505,5,15),(506,5,15),
(601,6,15),(602,6,15),(603,6,15),(604,6,15),(605,6,15),(606,6,15),
(701,7,15),(702,7,15),(703,7,15),(704,7,15),(705,7,15),(706,7,15),
(801,8,15),(802,8,15),(803,8,15),(804,8,15),(805,8,15),(806,8,15),
(901,9,15),(902,9,15),(903,9,15),(904,9,15),(905,9,15),(906,9,15);

INSERT INTO apartment_access_requests (creation_date, status, comment, apartment_id, applicant_id) VALUES
('2026-03-01', 'accepted', NULL,                                  16,  6),
('2026-03-02', 'waiting',  NULL,                                  17,  7),
('2026-03-03', 'rejected', 'Dokumentacija nije kompletna.',       18,  8),
('2026-03-04', 'accepted', NULL,                                  19,  9),
('2026-03-05', 'waiting',  NULL,                                  20,  10),
('2026-03-06', 'rejected', 'Podnosilac ne ispunjava uslove.',     21,  16),
('2026-03-07', 'accepted', NULL,                                  22,  17),
('2026-03-08', 'waiting',  NULL,                                  23,  18),
('2026-03-09', 'accepted', NULL,                                  125, 19),
('2026-03-10', 'waiting',  NULL,                                  126, 20),
('2026-03-11', 'rejected', 'Neusaglašeni podaci.',                127,  6),
('2026-03-12', 'accepted', NULL,                                  128,  7),
('2026-03-13', 'waiting',  NULL,                                  129,  8),
('2026-03-14', 'accepted', NULL,                                  210,  9),
('2026-03-15', 'waiting',  NULL,                                  211,  10),
('2026-03-16', 'rejected', 'Stan već ima aktivnog stanara.',      212,  16),
('2026-03-17', 'accepted', NULL,                                  213,  17),
('2026-03-18', 'waiting',  NULL,                                  214,  18),
('2026-03-19', 'accepted', NULL,                                  215,  19),
('2026-03-20', 'rejected', 'Zahtev nije potpisan.',               216,  20);

INSERT INTO apartment_memberships (acceptance_date, apartment_id, building_resident_id) VALUES
('2026-03-01', 16,  6),
('2026-03-04', 19,  9),
('2026-03-07', 22,  17),
('2026-03-09', 125, 19),
('2026-03-12', 128, 7),
('2026-03-14', 210, 9),
('2026-03-17', 213, 17),
('2026-03-19', 215, 19);

INSERT INTO neighborhood_memberships (id, acceptance_date, neighborhood_id) VALUES
(1,  '2025-08-20', 1),
(2,  '2025-08-22', 3),
(3,  '2025-08-25', 3),
(4,  '2025-08-28', 4),
(5,  '2025-08-30', 1),
(6,  '2025-09-01', 6),
(7,  '2025-09-02', 2),
(8,  '2025-09-03', 8),
(9,  '2025-09-04', 4),
(10, '2025-09-05', 10),
(11, '2025-09-06', 1),
(12, '2025-09-06', 2),
(13, '2025-09-06', 3),
(14, '2025-09-06', 4),
(15, '2025-09-06', 5);

INSERT INTO citizens (id, username, user_role, password, name, surname, address_id, membership_id) VALUES
(101, 'petar.simic',       'citizen', '123', 'Petar',   'Simić',       1,  1),
(102, 'milica.todorovic',  'citizen', '123', 'Milica',  'Todorović',   2,  2),
(103, 'uros.blagojevic',   'citizen', '123', 'Uroš',    'Blagojević',  3,  3),
(104, 'andjela.zivkovic',  'citizen', '123', 'Anđela',  'Živković',    4,  4),
(105, 'dusan.radulovic',   'citizen', '123', 'Dušan',   'Radulović',   5,  5),
(106, 'marko.zivkovic',    'citizen', '123', 'Marko',   'Živković',    6,  6),
(107, 'ana.pavlovic',      'citizen', '123', 'Ana',     'Pavlović',    7,  7),
(108, 'luka.savic',        'citizen', '123', 'Luka',    'Savić',       8,  8),
(109, 'mina.stojic',       'citizen', '123', 'Mina',    'Stojić',      9,  9),
(110, 'nikola.kostic',     'citizen', '123', 'Nikola',  'Kostić',      10, 10),
(111, 'sara.vasic',        'citizen', '123', 'Sara',    'Vasić',       11, NULL),
(112, 'ognjen.mitrovic',   'citizen', '123', 'Ognjen',  'Mitrović',    12, NULL),
(113, 'maja.jaksic',       'citizen', '123', 'Maja',    'Jaksić',      13, NULL),
(114, 'filip.atanasov',    'citizen', '123', 'Filip',   'Atanasov',    14, NULL),
(115, 'jana.popovic',      'citizen', '123', 'Jana',    'Popović',     15, NULL),
(116, 'stefan.milovanovic', 'citizen', '123', 'Stefan',  'Milovanović', 1, 11),
(117, 'marina.knezevic',   'citizen', '123', 'Marina',  'Knežević',     2, 12),
(118, 'nemanja.stojanovic', 'citizen', '123', 'Nemanja', 'Stojanović',  3, 13),
(119, 'teodora.ristic',    'citizen', '123', 'Teodora', 'Ristić',       4, 14),
(120, 'aleksandar.ivanovic','citizen','123','Aleksandar','Ivanović',    5, 15);

INSERT INTO neighborhood_access_requests (citizen_id, neighborhood_id, status, creation_date, comment) VALUES
(101, 2,  'waiting',  '2026-03-01', NULL),
(102, 3,  'accepted', '2026-03-02', NULL),
(103, 4,  'rejected', '2026-03-04', 'Adresa prebivališta ne pripada traženom naselju.'),
(104, 5,  'waiting',  '2026-03-06', NULL),
(105, 1,  'accepted', '2026-03-07', NULL),
(106, 1,  'waiting',  '2026-03-10', NULL),
(107, 2,  'accepted', '2026-03-11', NULL),
(108, 3,  'waiting',  '2026-03-12', NULL),
(109, 4,  'accepted', '2026-03-13', NULL),
(110, 5,  'waiting',  '2026-03-14', NULL),
(111, 6,  'waiting',  '2026-03-20', NULL),
(112, 7,  'waiting',  '2026-03-21', NULL),
(113, 8,  'rejected', '2026-03-22', 'Nepotpuna prijava'),
(114, 9,  'rejected', '2026-03-23', 'Nepotpuna prijava.'),
(115, 10, 'rejected', '2026-03-24', 'Neusaglašeni podaci o prebivalištu.');

INSERT INTO community_events (id, name, description, event_date, start_time, duration_minutes, min_volunteers, status, neighborhood_id, organizer_citizen_id) VALUES
(1, 'Žurka', 'Zabavna mesečna proslava u parku.', '2026-05-20', '09:00', 180, 10, 'preparation', 1, 101),
(2, 'Bazar', 'Ulični bazar za razmenu i prodaju raznih stvari.', '2026-05-15', '10:00', 240, 20, 'preparation', 2, 102),
(3, 'Seminar o reciklaži', 'Edukativni seminar za građane o pravilnom odvajanju otpada.', '2026-04-01', '18:00', 90, 5, 'finished', 3, 103),
(4, 'Humanitarna akcija', 'Prikupljanje i pakovanje pomoći za porodice u potrebi.', '2026-05-30', '08:00', 300, 30, 'preparation', 4, 104),
(5, 'Letnja proslava', 'Druženje i kulturni program na otvorenom za stanovnike naselja.', '2026-07-10', '19:00', 240, 15, 'cancelled', 5, 105),
(6, 'Sportski dan', 'Organizacija sportskih aktivnosti i turnira za sve uzraste.', '2026-06-15', '14:00', 180, 25, 'preparation', 1, 116),
(7, 'Kreativne radionice', 'Radionice za decu i odrasle sa različitim temama i tehnikama.', '2026-05-25', '16:00', 120, 10, 'preparation', 2, 117),
(8, 'Filmsko veče', 'Projekcija filmova na otvorenom uz diskusiju i druženje.', '2026-06-20', '20:00', 150, 20, 'preparation', 3, 118),
(9, 'Zdravstveni seminar', 'Besplatne zdravstvene provere i edukacija o zdravlju.', '2026-07-05', '09:00', 240, 15, 'preparation', 4, 119),
(10, 'Eko akcija', 'Čišćenje i uređenje javnih površina u naselju.', '2026-06-10', '08:00', 180, 30, 'preparation', 5, 120);

INSERT INTO event_items (id, name, event_id) VALUES
(1, 'Ozvučenje', 1),
(2, 'Stolice', 1),
(3, 'Voda', 1),
(4, 'Hrana', 1),
(5, 'Sok', 1),
(6, 'Šatori', 2),
(7, 'Stolovi', 2),
(8, 'Projektor', 3),
(9, 'Letci o reciklaži', 3),
(10, 'Kutije za pakovanje', 4),
(11, 'Transportne torbe', 4),
(12, 'Bina', 5),
(13, 'Osvetljenje', 5),
(14, 'Sportska oprema', 6),
(15, 'Označavanje terena', 6),
(16, 'Osveženje', 6),
(17, 'Lopte', 6),
(18, 'Radni materijal', 7),
(19, 'Platno za projekciju', 8),
(20, 'Projektor', 8),
(21, 'Edukativni materijali', 9),
(22, 'Vreće za otpad', 10),
(23, 'Medicinski sto', 9);

INSERT INTO event_registrations (id, attended, event_id, citizen_id) VALUES
(1, NULL, 2, 107),
(2, NULL, 7, 107),
(3, TRUE, 3, 102),
(4, FALSE,3, 103),
(5, NULL, 8, 102),
(6, NULL, 8, 103),
(7, NULL, 4, 104),
(8, NULL, 4, 109),
(9, NULL, 9, 104),
(10, NULL, 9, 109);

INSERT INTO event_item_assignments (id, registration_id, event_item_id) VALUES
(1, 1, 6),
(2, 2, 18),
(3, 3, 8),
(4, 4, 9),
(5, 5, 19),
(6, 6, 20),
(7, 7, 10),
(8, 8, 11),
(9, 9, 21),
(10, 10, 23);

INSERT INTO users (id, username, user_role, password, name, surname) VALUES
(21, 'zoran.milanovic',   'neighborhood_service_provider', '123', 'Zoran',   'Milanović'),
(22, 'ana.djordjevic',   'neighborhood_service_provider', '123', 'Ana',    'Đorđević'),
(23, 'una.jacimovic',    'neighborhood_service_provider', '123', 'Una',   'Jaćimović'),
(24, 'nina.petrovic',     'neighborhood_service_provider', '123', 'Nina',    'Petrović'),
(25, 'boris.todorovic',   'neighborhood_service_provider', '123', 'Boris',   'Todorović');
 
INSERT INTO service_providers (id, city_id, country_id) VALUES
(21, 1, 1),
(22, 1, 1),
(23, 2, 1),
(24, 3, 1),
(25, 4, 1);
 
INSERT INTO job_listings (id, service_name, description, billing_type, city_id, country_id, provider_id) VALUES
(1,  'Održavanje lifta',          'Redovno servisiranje i popravka liftova u stambenim zgradama.',                          'per_job',  1, 1, 21),
(2,  'Košenje trave',             'Košenje i uređenje travnatih površina u dvorištima i parkovima.',                        'per_hour', 1, 1, 21),
(3,  'Popravka igrališta',        'Servisiranje i zamena dotrajalih delova na dečijim igralištima.',                        'per_job',  2, 1, 23);
 
INSERT INTO price_entries (id, amount, valid_from, valid_to, job_listing_id) VALUES
(1,  15000.00, '2025-01-01', '2026-05-24', 1),
(2,  16500.00, '2026-05-25', '2026-12-31', 1),
(3,   1200.00, '2025-01-01', '2026-05-24', 2),
(4,   1400.00, '2026-05-25', '2026-12-31', 2),
(5,  12000.00, '2025-01-01', '2026-05-24', 3),
(6, 13500.00, '2026-05-25', '2026-12-31', 3);
 
INSERT INTO booked_jobs (id, scheduled_date, scheduled_time, duration_hours, unit_price, is_substitution, job_listing_id, neighborhood_id, substituting_provider_id) VALUES
(1,  '2026-02-10', NULL,    NULL, 15000.00, FALSE, 1,  1,  NULL),
(2,  '2026-03-05', NULL,    NULL, 15000.00, FALSE, 1,  2,  NULL),
(3,  '2026-03-20', '09:00', 3,    1200.00,  FALSE, 2,  1,  NULL),
(4,  '2026-04-10', '08:00', 4,    1200.00,  FALSE, 2,  2,  NULL),
(8,  '2026-02-20', NULL,    NULL, 12000.00, FALSE, 3,  3,  NULL),
(9,  '2026-04-01', NULL,    NULL, 12000.00, FALSE, 3,  4,  NULL);
 
INSERT INTO leave_requests (id, start_date, end_date, status, created_at, provider_id) VALUES
(1, '2026-04-20', '2026-04-27', 'accepted', '2026-04-05 10:00:00', 21),
(2, '2026-05-05', '2026-05-09', 'waiting',  '2026-04-15 09:30:00', 22),
(3, '2026-04-28', '2026-05-02', 'rejected', '2026-04-12 14:00:00', 23);
 
INSERT INTO coverage_offers (id, status, leave_request_id, booked_job_id, covering_provider_id) VALUES
(1, 'accepted', 1, 3, 22),
(2, 'waiting',  2, 4, 21),
(3, 'rejected', 3, 8, 25);
 
INSERT INTO provider_forums (id, title, description, is_closed, created_at, created_by_id) VALUES
(1, 'Iskustva sa održavanjem liftova',
   'Delim iskustvo sa ugovaranjem servisa liftova u višespratnicama — šta je važno proveriti pre potpisivanja ugovora sa kvartalnom koordinacijom.',
   FALSE, '2026-01-15 10:30:00', 21),
(2, 'Pitanje oko cenovnika za 2026.',
   'Kako se nosite sa rastom cena materijala? Da li ste povećali cene usluga klijentima za ovu godinu?',
   FALSE, '2026-02-03 09:15:00', 23),
(3, 'Arhiva: Zimske intervencije',
   'Forum o iskustvima iz zimskog perioda — tema je zatvorena jer je sezona prošla.',
   TRUE,  '2025-12-01 08:00:00', 21),
(4, 'Kako se organizujete za zamenu kolega?',
   'Zanima me kako ostali rešavaju situacije kada moraju da pokriju kolegu na odmoru. Tražim savete za organizaciju.',
   FALSE, '2026-02-20 11:00:00', 24);
 
INSERT INTO forum_posts (id, content, posted_at, forum_id, author_id) VALUES
(1,  'Uvek tražim pisanu specifikaciju rezervnih delova pre potpisivanja. Koordinatori cene transparentnost.',
     '2026-01-16 09:00:00', 1, 22),
(2,  'Slažem se. Kod mene su ključni rokovi odaziva — zapisujem u ugovor max 4 sata za hitne kvarove.',
     '2026-01-17 11:30:00', 1, 23),
(3,  'Koristim foto-dokumentaciju pre i posle svakog servisa. Izbegavam sporove sa koordinatorima.',
     '2026-01-18 15:00:00', 1, 24),
(4,  'Povećao sam cene za 15% od januara. Objasnio koordinatorima rast cena energenata i materijala.',
     '2026-02-04 09:30:00', 2, 21),
(5,  'Ja sam zadržao iste cene ali uveo minimalnu naknadu za izlazak — 30 minuta uvek se naplaćuje.',
     '2026-02-05 12:00:00', 2, 22),
(6,  'Slično kao Boris — uveo sam putni trošak za kvartove van centra grada.',
     '2026-02-06 16:00:00', 2, 25),
(7,  'Trudim se da imam uvek slobodan dan rezervisan kada znam da kolega planira odmor. Komunikacija je ključ.',
     '2026-02-21 10:00:00', 4, 22),
(8, 'Napravio sam grupni chat sa kolegama na istoj lokaciji. Brže se reaguje nego čekanjem sistema.',
     '2026-02-22 14:00:00', 4, 23);
 
INSERT INTO post_reactions (id, is_upvote, post_id, reactor_id) VALUES
(1,  TRUE,  1,  21),
(2,  TRUE,  1,  23),
(3,  TRUE,  2,  21),
(4,  FALSE, 2,  25),
(5,  TRUE,  3,  22),
(6,  TRUE,  3,  25),
(7,  TRUE,  4,  22),
(8,  TRUE,  4,  23),
(9,  FALSE, 5,  21),
(10, TRUE,  6,  24),
(11, TRUE,  6,  25),
(12, TRUE,  7,  21),
(13, TRUE,  7,  24),
(14, FALSE, 8,  22);
 
INSERT INTO provider_reviews (id, quality_rating, communication_rating, comment, created_at, updated_at, booked_job_id, coordinator_id) VALUES
(1,  5, 5, 'Odličan servis lifta, stigli tačno na vreme i sve popravili u jednoj poseti.',
    '2026-02-15 12:00:00', NULL, 1, 11),
(2,  4, 5, 'Brzo i uredno košenje. Malo skuplje nego prošle godine, ali kvalitet je tu.',
    '2026-03-28 10:30:00', NULL, 3, 12),
(3,  5, 5, 'Fantastičan posao na igralištu. Deca su oduševljena novom opremom.',
    '2026-03-02 11:00:00', NULL, 8, 13);
 
INSERT INTO common_rooms (building_id, name, description, floor_number, rental_type) VALUES
(1,  'Sala za sastanke',      'Manja sala za sastanke kucnog saveta i okupljanja stanara.', 1, 'one_day'),
(1,  'Teretana',              'Zajednička teretana za stanare zgrade.', 1, 'multi_day'),
(3,  'Vešernica',             'Prostorija za sušenje vesa i odlaganje daski za peglanje.', 1, 'multi_day'),
(4,  'Ostava',                'Zajednička ostava za odlaganje sezonskih stvari.', 1, 'multi_day'),
(5,  'Hobi radionica',        'Prostor za sitne popravke, rad sa alatom i uradi sam aktivnosti.', 1, 'one_day'),
(6,  'Prostorija za bicikle', 'Zaključana prostorija za odlaganje bicikala.', 1, 'multi_day'),
(7,  'Soba za proslave',      'Zajednička prostorija za proslave rođendana i okupljanja.', 1, 'one_day'),
(8,  'Čitaonica',             'Mirna prostorija namenjena učenju i radu.', 1, 'one_day'),
(9,  'Multimedijalna sala',   'Sala sa projektorom za prezentacije i zajedničke aktivnosti.', 1, 'one_day'),
(10, 'Ostava za alat',        'Prostorija za čuvanje zajedničkog alata i opreme.', 1, 'multi_day'),
(11, 'Prostorija za sastanke','Veća prostorija za sastanke stanara i radionice.', 1, 'one_day'),
(12, 'Vešernica',             'Zajednička vešernica za sušenje.', 1, 'multi_day'),
(13, 'Klub stanara',          'Prostorija za druzenje, sah i manje dogadjaje.', 1, 'one_day'),
(14, 'Prostorija za bicikle', 'Prostor za bicikle..', 1, 'multi_day'),
(15, 'Soba za proslave',      'Sala namenjena manjim porodicnim proslavama.', 1, 'one_day');

INSERT INTO common_room_rental_requests
(common_room_id, requested_start_date, requested_end_date, proposed_start_date, proposed_end_date, rental_status, resident_id, assigned_date) VALUES
(2, '2026-05-12', '2026-05-15', NULL, NULL, 'waiting', 6, NULL),
(2, '2026-05-18', '2026-05-20', NULL, NULL, 'waiting', 6, NULL),
(2, '2026-05-15', '2026-05-16', NULL, NULL, 'waiting', 6, NULL),
(1, '2026-05-18', '2026-05-20', NULL, NULL, 'waiting', 6, NULL),
(1, '2026-05-21', '2026-05-21', NULL, NULL, 'waiting', 6, NULL),
(1, '2026-05-19', '2026-05-22', NULL, NULL, 'waiting', 6, NULL),
(1, '2026-05-19', '2026-05-20', NULL, NULL, 'waiting', 6, NULL);

INSERT INTO ads
(building_id, resident_id, ad_type, ad_category, ad_status, description, start_date, end_date) VALUES
(1,  6, 'offer',   'moving',       'active',   'Mogu da pomognem oko iznosenja kutija i lakseg namestaja tokom popodneva.', '2026-05-05', '2026-05-08'),
(1,  7, 'request', 'moving',       'active',   'Potrebna pomoc da se prenese ugaona garnitura iz kombija do stana na drugom spratu.', '2026-05-06', '2026-05-07'),
(1,  8, 'offer',   'cleaning',     'active',   'Mogu da pomognem oko generalnog ciscenja stana i pranja prozora.', '2026-05-08', '2026-05-10'),
(1,  9, 'request', 'cleaning',     'active',   'Potrebna pomoc oko ciscenja stana posle molerskih radova.', '2026-05-09', '2026-05-10'),
(1, 10, 'offer',   'repairs',      'active',   'Mogu da pogledam manje kvarove na slavini, vodokotlicu i bravama.', '2026-05-11', '2026-05-14'),
(1, 16, 'request', 'repairs',      'active',   'Vrata od kupatila se tesko zatvaraju i potrebna je manja popravka.', '2026-05-12', '2026-05-13'),
(1, 17, 'offer',   'shopping',     'active',   'Mogu da odem u nabavku za komsije kojima treba pomoc oko nosenja kesa.', '2026-05-07', '2026-05-09'),
(1, 18, 'request', 'shopping',     'active',   'Potrebna pomoc oko kupovine namirnica i donosenja do stana.', '2026-05-08', '2026-05-08'),
(1, 19, 'offer',   'tech_support', 'active',   'Pomazem oko podesavanja rutera, stampaca i osnovnih problema sa laptopom.', '2026-05-15', '2026-05-17'),
(1, 20, 'request', 'tech_support', 'active',   'Potrebna pomoc oko povezivanja televizora na internet i aplikacije.', '2026-05-16', '2026-05-17'),
(1,  6, 'request', 'borrowing',    'active',   'Potreban mi je udarni busac na jedno popodne za montazu polica.', '2026-05-18', '2026-05-19'),
(1,  7, 'offer',   'borrowing',    'active',   'Mogu da pozajmim merdevine i osnovni alat za sitne radove u stanu.', '2026-05-18', '2026-05-20'),
(1,  8, 'request', 'pet_care',     'active',   'Potrebno je da neko proseta psa u subotu uvece zbog puta van grada.', '2026-05-20', '2026-05-21'),
(1,  9, 'offer',   'pet_care',     'active',   'Mogu da prosetam psa ili nahranim macku kada komsije nisu kod kuce.', '2026-05-20', '2026-05-22'),
(1, 10, 'request', 'delivery',     'active',   'Treba mi pomoc da se preuzme paket sa pumpe i donese do zgrade.', '2026-05-23', '2026-05-23'),
(1, 16, 'offer',   'delivery',     'active',   'Mogu da preuzmem manje pakete i ostavim ih komsijama u dogovoreno vreme.', '2026-05-23', '2026-05-24'),
(1, 17, 'request', 'tutoring',     'active',   'Potrebna pomoc detetu oko matematike za kontrolni zadatak iz razlomaka.', '2026-05-26', '2026-05-28'),
(1, 18, 'offer',   'tutoring',     'active',   'Mogu da drzim casove matematike za osnovce u vecernjim terminima.', '2026-05-26', '2026-05-29'),
(1, 19, 'request', 'transport',    'active',   'Treba mi pomoc da se prevezu dve vece kutije do novog stana.', '2026-05-29', '2026-05-30'),
(1, 20, 'offer',   'transport',    'active',   'Mogu da pomognem prevozom manjih stvari po gradu vikendom.', '2026-05-29', '2026-05-31'),
(1,  6, 'offer',   'gardening',    'archived', 'Pomagala sam oko sredjivanja saksija i cveca na ulazu zgrade.', '2026-04-01', '2026-04-03'),
(1, 17, 'offer',   'cleaning',     'archived', 'Mogla sam da pomognem oko sredjivanja podruma pre vikenda.', '2026-04-10', '2026-04-12');

INSERT INTO ad_time_slots (ad_id, start_time, end_time) VALUES
(1, '2026-05-05 16:00:00', '2026-05-05 17:00:00'),
(1, '2026-05-05 17:00:00', '2026-05-05 18:00:00'),
(1, '2026-05-05 18:00:00', '2026-05-05 19:00:00'),
(1, '2026-05-05 19:00:00', '2026-05-05 20:00:00'),
(1, '2026-05-06 16:00:00', '2026-05-06 17:00:00'),
(1, '2026-05-06 17:00:00', '2026-05-06 18:00:00'),
(1, '2026-05-06 18:00:00', '2026-05-06 19:00:00'),
(1, '2026-05-06 19:00:00', '2026-05-06 20:00:00'),
(1, '2026-05-07 16:00:00', '2026-05-07 17:00:00'),
(1, '2026-05-07 17:00:00', '2026-05-07 18:00:00'),
(1, '2026-05-07 18:00:00', '2026-05-07 19:00:00'),
(1, '2026-05-07 19:00:00', '2026-05-07 20:00:00'),
(1, '2026-05-08 16:00:00', '2026-05-08 17:00:00'),
(1, '2026-05-08 17:00:00', '2026-05-08 18:00:00'),
(1, '2026-05-08 18:00:00', '2026-05-08 19:00:00'),
(1, '2026-05-08 19:00:00', '2026-05-08 20:00:00'),
(2, '2026-05-06 16:00:00', '2026-05-06 17:00:00'),
(2, '2026-05-06 17:00:00', '2026-05-06 18:00:00'),
(2, '2026-05-06 18:00:00', '2026-05-06 19:00:00'),
(2, '2026-05-06 19:00:00', '2026-05-06 20:00:00'),
(2, '2026-05-07 16:00:00', '2026-05-07 17:00:00'),
(2, '2026-05-07 17:00:00', '2026-05-07 18:00:00'),
(2, '2026-05-07 18:00:00', '2026-05-07 19:00:00'),
(2, '2026-05-07 19:00:00', '2026-05-07 20:00:00'),
(3, '2026-05-08 16:00:00', '2026-05-08 17:00:00'),
(3, '2026-05-08 17:00:00', '2026-05-08 18:00:00'),
(3, '2026-05-08 18:00:00', '2026-05-08 19:00:00'),
(3, '2026-05-08 19:00:00', '2026-05-08 20:00:00'),
(3, '2026-05-09 16:00:00', '2026-05-09 17:00:00'),
(3, '2026-05-09 17:00:00', '2026-05-09 18:00:00'),
(3, '2026-05-09 18:00:00', '2026-05-09 19:00:00'),
(3, '2026-05-09 19:00:00', '2026-05-09 20:00:00'),
(3, '2026-05-10 16:00:00', '2026-05-10 17:00:00'),
(3, '2026-05-10 17:00:00', '2026-05-10 18:00:00'),
(3, '2026-05-10 18:00:00', '2026-05-10 19:00:00'),
(3, '2026-05-10 19:00:00', '2026-05-10 20:00:00'),
(4, '2026-05-09 16:00:00', '2026-05-09 17:00:00'),
(4, '2026-05-09 17:00:00', '2026-05-09 18:00:00'),
(4, '2026-05-09 18:00:00', '2026-05-09 19:00:00'),
(4, '2026-05-09 19:00:00', '2026-05-09 20:00:00'),
(4, '2026-05-10 16:00:00', '2026-05-10 17:00:00'),
(4, '2026-05-10 17:00:00', '2026-05-10 18:00:00'),
(4, '2026-05-10 18:00:00', '2026-05-10 19:00:00'),
(4, '2026-05-10 19:00:00', '2026-05-10 20:00:00'),
(5, '2026-05-11 16:00:00', '2026-05-11 17:00:00'),
(5, '2026-05-11 17:00:00', '2026-05-11 18:00:00'),
(5, '2026-05-11 18:00:00', '2026-05-11 19:00:00'),
(5, '2026-05-11 19:00:00', '2026-05-11 20:00:00'),
(5, '2026-05-12 16:00:00', '2026-05-12 17:00:00'),
(5, '2026-05-12 17:00:00', '2026-05-12 18:00:00'),
(5, '2026-05-12 18:00:00', '2026-05-12 19:00:00'),
(5, '2026-05-12 19:00:00', '2026-05-12 20:00:00'),
(5, '2026-05-13 16:00:00', '2026-05-13 17:00:00'),
(5, '2026-05-13 17:00:00', '2026-05-13 18:00:00'),
(5, '2026-05-13 18:00:00', '2026-05-13 19:00:00'),
(5, '2026-05-13 19:00:00', '2026-05-13 20:00:00'),
(5, '2026-05-14 16:00:00', '2026-05-14 17:00:00'),
(5, '2026-05-14 17:00:00', '2026-05-14 18:00:00'),
(5, '2026-05-14 18:00:00', '2026-05-14 19:00:00'),
(5, '2026-05-14 19:00:00', '2026-05-14 20:00:00'),
(6, '2026-05-12 16:00:00', '2026-05-12 17:00:00'),
(6, '2026-05-12 17:00:00', '2026-05-12 18:00:00'),
(6, '2026-05-12 18:00:00', '2026-05-12 19:00:00'),
(6, '2026-05-12 19:00:00', '2026-05-12 20:00:00'),
(6, '2026-05-13 16:00:00', '2026-05-13 17:00:00'),
(6, '2026-05-13 17:00:00', '2026-05-13 18:00:00'),
(6, '2026-05-13 18:00:00', '2026-05-13 19:00:00'),
(6, '2026-05-13 19:00:00', '2026-05-13 20:00:00'),
(7, '2026-05-07 16:00:00', '2026-05-07 17:00:00'),
(7, '2026-05-07 17:00:00', '2026-05-07 18:00:00'),
(7, '2026-05-07 18:00:00', '2026-05-07 19:00:00'),
(7, '2026-05-07 19:00:00', '2026-05-07 20:00:00'),
(7, '2026-05-08 16:00:00', '2026-05-08 17:00:00'),
(7, '2026-05-08 17:00:00', '2026-05-08 18:00:00'),
(7, '2026-05-08 18:00:00', '2026-05-08 19:00:00'),
(7, '2026-05-08 19:00:00', '2026-05-08 20:00:00'),
(7, '2026-05-09 16:00:00', '2026-05-09 17:00:00'),
(7, '2026-05-09 17:00:00', '2026-05-09 18:00:00'),
(7, '2026-05-09 18:00:00', '2026-05-09 19:00:00'),
(7, '2026-05-09 19:00:00', '2026-05-09 20:00:00'),
(8, '2026-05-08 16:00:00', '2026-05-08 17:00:00'),
(8, '2026-05-08 17:00:00', '2026-05-08 18:00:00'),
(8, '2026-05-08 18:00:00', '2026-05-08 19:00:00'),
(8, '2026-05-08 19:00:00', '2026-05-08 20:00:00'),
(9, '2026-05-15 16:00:00', '2026-05-15 17:00:00'),
(9, '2026-05-15 17:00:00', '2026-05-15 18:00:00'),
(9, '2026-05-15 18:00:00', '2026-05-15 19:00:00'),
(9, '2026-05-15 19:00:00', '2026-05-15 20:00:00'),
(9, '2026-05-16 16:00:00', '2026-05-16 17:00:00'),
(9, '2026-05-16 17:00:00', '2026-05-16 18:00:00'),
(9, '2026-05-16 18:00:00', '2026-05-16 19:00:00'),
(9, '2026-05-16 19:00:00', '2026-05-16 20:00:00'),
(9, '2026-05-17 16:00:00', '2026-05-17 17:00:00'),
(9, '2026-05-17 17:00:00', '2026-05-17 18:00:00'),
(9, '2026-05-17 18:00:00', '2026-05-17 19:00:00'),
(9, '2026-05-17 19:00:00', '2026-05-17 20:00:00'),
(10, '2026-05-16 16:00:00', '2026-05-16 17:00:00'),
(10, '2026-05-16 17:00:00', '2026-05-16 18:00:00'),
(10, '2026-05-16 18:00:00', '2026-05-16 19:00:00'),
(10, '2026-05-16 19:00:00', '2026-05-16 20:00:00'),
(10, '2026-05-17 16:00:00', '2026-05-17 17:00:00'),
(10, '2026-05-17 17:00:00', '2026-05-17 18:00:00'),
(10, '2026-05-17 18:00:00', '2026-05-17 19:00:00'),
(10, '2026-05-17 19:00:00', '2026-05-17 20:00:00'),
(11, '2026-05-18 16:00:00', '2026-05-18 17:00:00'),
(11, '2026-05-18 17:00:00', '2026-05-18 18:00:00'),
(11, '2026-05-18 18:00:00', '2026-05-18 19:00:00'),
(11, '2026-05-18 19:00:00', '2026-05-18 20:00:00'),
(11, '2026-05-19 16:00:00', '2026-05-19 17:00:00'),
(11, '2026-05-19 17:00:00', '2026-05-19 18:00:00'),
(11, '2026-05-19 18:00:00', '2026-05-19 19:00:00'),
(11, '2026-05-19 19:00:00', '2026-05-19 20:00:00'),
(12, '2026-05-18 16:00:00', '2026-05-18 17:00:00'),
(12, '2026-05-18 17:00:00', '2026-05-18 18:00:00'),
(12, '2026-05-18 18:00:00', '2026-05-18 19:00:00'),
(12, '2026-05-18 19:00:00', '2026-05-18 20:00:00'),
(12, '2026-05-19 16:00:00', '2026-05-19 17:00:00'),
(12, '2026-05-19 17:00:00', '2026-05-19 18:00:00'),
(12, '2026-05-19 18:00:00', '2026-05-19 19:00:00'),
(12, '2026-05-19 19:00:00', '2026-05-19 20:00:00'),
(12, '2026-05-20 16:00:00', '2026-05-20 17:00:00'),
(12, '2026-05-20 17:00:00', '2026-05-20 18:00:00'),
(12, '2026-05-20 18:00:00', '2026-05-20 19:00:00'),
(12, '2026-05-20 19:00:00', '2026-05-20 20:00:00'),
(13, '2026-05-20 16:00:00', '2026-05-20 17:00:00'),
(13, '2026-05-20 17:00:00', '2026-05-20 18:00:00'),
(13, '2026-05-20 18:00:00', '2026-05-20 19:00:00'),
(13, '2026-05-20 19:00:00', '2026-05-20 20:00:00'),
(13, '2026-05-21 16:00:00', '2026-05-21 17:00:00'),
(13, '2026-05-21 17:00:00', '2026-05-21 18:00:00'),
(13, '2026-05-21 18:00:00', '2026-05-21 19:00:00'),
(13, '2026-05-21 19:00:00', '2026-05-21 20:00:00'),
(14, '2026-05-20 16:00:00', '2026-05-20 17:00:00'),
(14, '2026-05-20 17:00:00', '2026-05-20 18:00:00'),
(14, '2026-05-20 18:00:00', '2026-05-20 19:00:00'),
(14, '2026-05-20 19:00:00', '2026-05-20 20:00:00'),
(14, '2026-05-21 16:00:00', '2026-05-21 17:00:00'),
(14, '2026-05-21 17:00:00', '2026-05-21 18:00:00'),
(14, '2026-05-21 18:00:00', '2026-05-21 19:00:00'),
(14, '2026-05-21 19:00:00', '2026-05-21 20:00:00'),
(14, '2026-05-22 16:00:00', '2026-05-22 17:00:00'),
(14, '2026-05-22 17:00:00', '2026-05-22 18:00:00'),
(14, '2026-05-22 18:00:00', '2026-05-22 19:00:00'),
(14, '2026-05-22 19:00:00', '2026-05-22 20:00:00'),
(15, '2026-05-23 16:00:00', '2026-05-23 17:00:00'),
(15, '2026-05-23 17:00:00', '2026-05-23 18:00:00'),
(15, '2026-05-23 18:00:00', '2026-05-23 19:00:00'),
(15, '2026-05-23 19:00:00', '2026-05-23 20:00:00'),
(16, '2026-05-23 16:00:00', '2026-05-23 17:00:00'),
(16, '2026-05-23 17:00:00', '2026-05-23 18:00:00'),
(16, '2026-05-23 18:00:00', '2026-05-23 19:00:00'),
(16, '2026-05-23 19:00:00', '2026-05-23 20:00:00'),
(16, '2026-05-24 16:00:00', '2026-05-24 17:00:00'),
(16, '2026-05-24 17:00:00', '2026-05-24 18:00:00'),
(16, '2026-05-24 18:00:00', '2026-05-24 19:00:00'),
(16, '2026-05-24 19:00:00', '2026-05-24 20:00:00'),
(17, '2026-05-26 16:00:00', '2026-05-26 17:00:00'),
(17, '2026-05-26 17:00:00', '2026-05-26 18:00:00'),
(17, '2026-05-26 18:00:00', '2026-05-26 19:00:00'),
(17, '2026-05-26 19:00:00', '2026-05-26 20:00:00'),
(17, '2026-05-27 16:00:00', '2026-05-27 17:00:00'),
(17, '2026-05-27 17:00:00', '2026-05-27 18:00:00'),
(17, '2026-05-27 18:00:00', '2026-05-27 19:00:00'),
(17, '2026-05-27 19:00:00', '2026-05-27 20:00:00'),
(17, '2026-05-28 16:00:00', '2026-05-28 17:00:00'),
(17, '2026-05-28 17:00:00', '2026-05-28 18:00:00'),
(17, '2026-05-28 18:00:00', '2026-05-28 19:00:00'),
(17, '2026-05-28 19:00:00', '2026-05-28 20:00:00'),
(18, '2026-05-26 16:00:00', '2026-05-26 17:00:00'),
(18, '2026-05-26 17:00:00', '2026-05-26 18:00:00'),
(18, '2026-05-26 18:00:00', '2026-05-26 19:00:00'),
(18, '2026-05-26 19:00:00', '2026-05-26 20:00:00'),
(18, '2026-05-27 16:00:00', '2026-05-27 17:00:00'),
(18, '2026-05-27 17:00:00', '2026-05-27 18:00:00'),
(18, '2026-05-27 18:00:00', '2026-05-27 19:00:00'),
(18, '2026-05-27 19:00:00', '2026-05-27 20:00:00'),
(18, '2026-05-28 16:00:00', '2026-05-28 17:00:00'),
(18, '2026-05-28 17:00:00', '2026-05-28 18:00:00'),
(18, '2026-05-28 18:00:00', '2026-05-28 19:00:00'),
(18, '2026-05-28 19:00:00', '2026-05-28 20:00:00'),
(18, '2026-05-29 16:00:00', '2026-05-29 17:00:00'),
(18, '2026-05-29 17:00:00', '2026-05-29 18:00:00'),
(18, '2026-05-29 18:00:00', '2026-05-29 19:00:00'),
(18, '2026-05-29 19:00:00', '2026-05-29 20:00:00'),
(19, '2026-05-29 16:00:00', '2026-05-29 17:00:00'),
(19, '2026-05-29 17:00:00', '2026-05-29 18:00:00'),
(19, '2026-05-29 18:00:00', '2026-05-29 19:00:00'),
(19, '2026-05-29 19:00:00', '2026-05-29 20:00:00'),
(19, '2026-05-30 16:00:00', '2026-05-30 17:00:00'),
(19, '2026-05-30 17:00:00', '2026-05-30 18:00:00'),
(19, '2026-05-30 18:00:00', '2026-05-30 19:00:00'),
(19, '2026-05-30 19:00:00', '2026-05-30 20:00:00'),
(20, '2026-05-29 16:00:00', '2026-05-29 17:00:00'),
(20, '2026-05-29 17:00:00', '2026-05-29 18:00:00'),
(20, '2026-05-29 18:00:00', '2026-05-29 19:00:00'),
(20, '2026-05-29 19:00:00', '2026-05-29 20:00:00'),
(20, '2026-05-30 16:00:00', '2026-05-30 17:00:00'),
(20, '2026-05-30 17:00:00', '2026-05-30 18:00:00'),
(20, '2026-05-30 18:00:00', '2026-05-30 19:00:00'),
(20, '2026-05-30 19:00:00', '2026-05-30 20:00:00'),
(20, '2026-05-31 16:00:00', '2026-05-31 17:00:00'),
(20, '2026-05-31 17:00:00', '2026-05-31 18:00:00'),
(20, '2026-05-31 18:00:00', '2026-05-31 19:00:00'),
(20, '2026-05-31 19:00:00', '2026-05-31 20:00:00'),
(21, '2026-04-01 16:00:00', '2026-04-01 17:00:00'),
(21, '2026-04-01 17:00:00', '2026-04-01 18:00:00'),
(21, '2026-04-01 18:00:00', '2026-04-01 19:00:00'),
(21, '2026-04-01 19:00:00', '2026-04-01 20:00:00'),
(21, '2026-04-02 16:00:00', '2026-04-02 17:00:00'),
(21, '2026-04-02 17:00:00', '2026-04-02 18:00:00'),
(21, '2026-04-02 18:00:00', '2026-04-02 19:00:00'),
(21, '2026-04-02 19:00:00', '2026-04-02 20:00:00'),
(21, '2026-04-03 16:00:00', '2026-04-03 17:00:00'),
(21, '2026-04-03 17:00:00', '2026-04-03 18:00:00'),
(21, '2026-04-03 18:00:00', '2026-04-03 19:00:00'),
(21, '2026-04-03 19:00:00', '2026-04-03 20:00:00'),
(22, '2026-04-10 16:00:00', '2026-04-10 17:00:00'),
(22, '2026-04-10 17:00:00', '2026-04-10 18:00:00'),
(22, '2026-04-10 18:00:00', '2026-04-10 19:00:00'),
(22, '2026-04-10 19:00:00', '2026-04-10 20:00:00'),
(22, '2026-04-11 16:00:00', '2026-04-11 17:00:00'),
(22, '2026-04-11 17:00:00', '2026-04-11 18:00:00'),
(22, '2026-04-11 18:00:00', '2026-04-11 19:00:00'),
(22, '2026-04-11 19:00:00', '2026-04-11 20:00:00'),
(22, '2026-04-12 16:00:00', '2026-04-12 17:00:00'),
(22, '2026-04-12 17:00:00', '2026-04-12 18:00:00'),
(22, '2026-04-12 18:00:00', '2026-04-12 19:00:00'),
(22, '2026-04-12 19:00:00', '2026-04-12 20:00:00');

INSERT INTO ad_time_slot_reservations (offer_ad_time_slot_id, request_ad_time_slot_id) VALUES
(1,  5),   
(3,  6),   
(7,  10), 
(12, 15), 
(13, 16),  
(17, 19),  
(20, 22), 
(21, 23), 
(24, 25), 
(26, 27), 
(28, 30),  
(31, 32);

INSERT INTO reported_issues (description, reported_date, priority, status, resident_id, building_id) VALUES
('Lift ne radi već 3 dana, stanari sa višim spratovima imaju problem.', '2026-03-10', 'urgent', 'open', 1, 1),
('Sijalica u hodniku na 2. spratu je pregorela.', '2026-03-18', 'can_wait', 'resolved', 2, 1),
('Curenje vode iz cevi u podrumu, mokri zidovi.', '2026-04-01', 'should_be_resolved_soon', 'potentially_resolved', 3, 1),
('Interfon na glavnom ulazu ne funkcioniše.', '2026-04-15', 'should_be_resolved_soon', 'open', 4, 1),
('Pukotine na fasadi vidljive sa ulice.', '2026-04-22', 'can_wait', 'open', 5, 1),
('Ne radi rasveta u hodniku na 1. spratu.', '2026-05-02', 'should_be_resolved_soon', 'open', 7, 6),
('Ulazna vrata zgrade se tesko zatvaraju i brava zapinje.', '2026-05-06', 'can_wait', 'open', 9, 6),
('U podrumu se oseca jak miris vlage i vide se tragovi curenja.', '2026-05-11', 'urgent', 'potentially_resolved', 17, 6),
('Interfon na glavnom ulazu ne radi za nekoliko stanova.', '2026-05-19', 'should_be_resolved_soon', 'open', 19, 6);

INSERT INTO assemblies (id, building_id, starts_at, status) VALUES
(1,  1,  '2026-06-10 18:00:00', 'scheduled'),
(2,  2,  '2026-06-12 19:00:00', 'confirmed'),
(3,  3,  '2026-06-08 17:30:00', 'cancelled'),
(4,  4,  '2026-06-15 18:30:00', 'scheduled'),
(5,  5,  '2026-06-18 20:00:00', 'confirmed'),
(6,  6,  '2026-06-07 18:00:00', 'scheduled'),
(7,  7,  '2026-06-20 19:00:00', 'scheduled'),
(8,  8,  '2026-06-22 18:00:00', 'confirmed'),
(9,  9,  '2026-06-25 19:30:00', 'scheduled'),
(10, 10, '2026-06-28 17:00:00', 'cancelled'),
(12,  1,  '2026-05-30 10:00:00', 'cancelled');

INSERT INTO assembly_attendances (assembly_id, apartment_id) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 5),
(1, 7),
(1, 8),
(1, 10),
(1, 12),
(1, 15),
(1, 18),
(1, 20);

INSERT INTO assembly_attendances (assembly_id, apartment_id) VALUES
(2, 25),
(2, 26),
(2, 27),
(2, 28),
(2, 29),
(2, 30),
(2, 31),
(2, 32),
(2, 33),
(2, 34),
(2, 35),
(2, 36),
(2, 37),
(2, 38),
(2, 39),
(2, 40),
(2, 41),
(2, 42),
(2, 43),
(2, 44),
(2, 45),
(2, 46),
(2, 47);

INSERT INTO assembly_attendances (assembly_id, apartment_id) VALUES
(3, 70),
(3, 71),
(3, 72),
(3, 73),
(3, 74);

INSERT INTO assembly_attendances (assembly_id, apartment_id) VALUES 
(6, 125),
(6, 129),
(6, 130);


INSERT INTO assembly_topics
(description, assembly_id, author_id, topic_type) VALUES
('Pregled troskova odrzavanja zgrade', 1, 1, 'accepted'),
('Plan redovnog servisa lifta', 1, 1, 'accepted'),
('Uredjenje ulaza i zajednickih hodnika', 1, 1, 'accepted'),
('Popravka interfona na glavnom ulazu', 1, 6, 'suggested'),
('Predlog dnevnog reda i organizacije sastanka', 6, 1, 'accepted'),
('Servis protivpozarnog aparata', 6, 16, 'suggested'),
('Zamena sijalica u hodnicima', 1, 9, 'suggested'),
('Postavljanje dodatnih kamera u ulazu', 1, 17, 'suggested'),
('Sanacija vlage u podrumu', 1, 6, 'accepted'),
('Popravka ulaznih vrata', 2, 7, 'accepted'),
('Organizacija dezinfekcije zgrade', 3, 8, 'accepted'),
('Zamena tabli sa obavestenjima', 4, 9, 'accepted'),
('Ciscenje podruma', 5, 10, 'accepted'),
('Farbanje ograde', 7, 17, 'accepted'),
('Postavljanje novog zvonceta', 8, 18, 'accepted'),
('Uredjenje dvorista', 9, 19, 'accepted'),
('Uvodjenje video nadzora', 10, 20, 'accepted'),
('Pregled troskova odrzavanja zgrade', 12, 1, 'accepted'),
('Popravka lifta', 6, 16, 'suggested');


INSERT INTO images (path, building_id) VALUES
('/Resources/Images/building_0.jpg', 1),
('/Resources/Images/building_1.jpg', 1),
('/Resources/Images/building_2.jpg', 1),
('/Resources/Images/building_0.jpg', 2),
('/Resources/Images/building_1.jpg', 2),
('/Resources/Images/building_2.jpg', 2),
('/Resources/Images/building_0.jpg', 3),
('/Resources/Images/building_1.jpg', 3),
('/Resources/Images/building_2.jpg', 3),
('/Resources/Images/building_0.jpg', 4),
('/Resources/Images/building_1.jpg', 4),
('/Resources/Images/building_2.jpg', 4),
('/Resources/Images/building_0.jpg', 5),
('/Resources/Images/building_1.jpg', 5),
('/Resources/Images/building_2.jpg', 5),
('/Resources/Images/building_0.jpg', 6),
('/Resources/Images/building_1.jpg', 6),
('/Resources/Images/building_2.jpg', 6),
('/Resources/Images/building_0.jpg', 7),
('/Resources/Images/building_1.jpg', 7),
('/Resources/Images/building_2.jpg', 7),
('/Resources/Images/building_0.jpg', 8),
('/Resources/Images/building_1.jpg', 8),
('/Resources/Images/building_2.jpg', 8),
('/Resources/Images/building_0.jpg', 9),
('/Resources/Images/building_1.jpg', 9),
('/Resources/Images/building_2.jpg', 9),
('/Resources/Images/building_0.jpg', 10),
('/Resources/Images/building_1.jpg', 10),
('/Resources/Images/building_2.jpg', 10),
('/Resources/Images/building_0.jpg', 11),
('/Resources/Images/building_1.jpg', 11),
('/Resources/Images/building_2.jpg', 11),
('/Resources/Images/building_0.jpg', 12),
('/Resources/Images/building_1.jpg', 12),
('/Resources/Images/building_2.jpg', 12),
('/Resources/Images/building_0.jpg', 13),
('/Resources/Images/building_1.jpg', 13),
('/Resources/Images/building_2.jpg', 13),
('/Resources/Images/building_0.jpg', 14),
('/Resources/Images/building_1.jpg', 14),
('/Resources/Images/building_2.jpg', 14),
('/Resources/Images/building_0.jpg', 15),
('/Resources/Images/building_1.jpg', 15),
('/Resources/Images/building_2.jpg', 15);

INSERT INTO images (path, neighborhood_id) VALUES
('/Resources/Images/neighborhood_0.jpg', 1),
('/Resources/Images/neighborhood_1.jpg', 1),
('/Resources/Images/neighborhood_2.jpg', 1),
('/Resources/Images/neighborhood_0.jpg', 2),
('/Resources/Images/neighborhood_1.jpg', 2),
('/Resources/Images/neighborhood_2.jpg', 2),
('/Resources/Images/neighborhood_0.jpg', 3),
('/Resources/Images/neighborhood_1.jpg', 3),
('/Resources/Images/neighborhood_2.jpg', 3),
('/Resources/Images/neighborhood_0.jpg', 4),
('/Resources/Images/neighborhood_1.jpg', 4),
('/Resources/Images/neighborhood_2.jpg', 4),
('/Resources/Images/neighborhood_0.jpg', 5),
('/Resources/Images/neighborhood_1.jpg', 5),
('/Resources/Images/neighborhood_2.jpg', 5),
('/Resources/Images/neighborhood_0.jpg', 6),
('/Resources/Images/neighborhood_1.jpg', 6),
('/Resources/Images/neighborhood_2.jpg', 6),
('/Resources/Images/neighborhood_0.jpg', 7),
('/Resources/Images/neighborhood_1.jpg', 7),
('/Resources/Images/neighborhood_2.jpg', 7),
('/Resources/Images/neighborhood_0.jpg', 8),
('/Resources/Images/neighborhood_1.jpg', 8),
('/Resources/Images/neighborhood_2.jpg', 8),
('/Resources/Images/neighborhood_0.jpg', 9),
('/Resources/Images/neighborhood_1.jpg', 9),
('/Resources/Images/neighborhood_2.jpg', 9),
('/Resources/Images/neighborhood_0.jpg', 10),
('/Resources/Images/neighborhood_1.jpg', 10),
('/Resources/Images/neighborhood_2.jpg', 10),
('/Resources/Images/neighborhood_0.jpg', 11),
('/Resources/Images/neighborhood_1.jpg', 11),
('/Resources/Images/neighborhood_2.jpg', 11),
('/Resources/Images/neighborhood_0.jpg', 12),
('/Resources/Images/neighborhood_1.jpg', 12),
('/Resources/Images/neighborhood_2.jpg', 12),
('/Resources/Images/neighborhood_0.jpg', 13),
('/Resources/Images/neighborhood_1.jpg', 13),
('/Resources/Images/neighborhood_2.jpg', 13),
('/Resources/Images/neighborhood_0.jpg', 14),
('/Resources/Images/neighborhood_1.jpg', 14),
('/Resources/Images/neighborhood_2.jpg', 14),
('/Resources/Images/neighborhood_0.jpg', 15),
('/Resources/Images/neighborhood_1.jpg', 15),
('/Resources/Images/neighborhood_2.jpg', 15);

INSERT INTO neighborhood_meetings (id, theme, meeting_time, date_range_start, date_range_end, scheduled_date, status, neighborhood_id) VALUES
(1,  'welcome',    '19:00', '2026-05-01', '2026-05-05', '2026-05-03', 'scheduled',      1),
(2,  'motivation', '18:00', '2026-05-10', '2026-05-14', '2026-05-12', 'scheduled',      2),
(3,  'welcome',    '20:00', '2026-05-15', '2026-05-19', NULL,         'in_preparation', 3),
(4,  'motivation', '17:00', '2026-05-20', '2026-05-24', NULL,         'in_preparation', 4),
(5,  'welcome',    '19:00', '2026-04-01', '2026-04-05', '2026-04-02', 'scheduled',      5),
(6,  'motivation', '18:00', '2026-03-10', '2026-03-14', NULL,         'cancelled',      6),
(7,  'welcome',    '20:00', '2026-05-22', '2026-05-26', NULL,         'in_preparation', 7),
(8,  'motivation', '19:00', '2026-05-01', '2026-05-05', '2026-05-04', 'scheduled',      8),
(9,  'welcome',    '17:00', '2026-06-01', '2026-06-05', NULL,         'in_preparation', 9),
(10, 'motivation', '20:00', '2026-04-15', '2026-04-19', '2026-04-17', 'scheduled',      10);

INSERT INTO meeting_date_votes (id, voted_date, meeting_id, citizen_id) VALUES
(1, '2026-05-03', 1, 105),
(2, '2026-05-03', 1, 116),
(3, '2026-05-12', 2, 117),
(4, '2026-05-15', 3, 118),
(5, '2026-05-20', 4, 104),
(6, '2026-05-20', 4, 119),
(7, '2026-04-02', 5, 105),
(8, '2026-03-12', 6, 106),
(9, '2026-05-22', 7, 107),
(10, '2026-05-04', 8, 108);

INSERT INTO city_facilities (id, name, city_id) VALUES
(1,  'Bioskop Fontana',         1),
(2,  'Gradski bazen Tašmajdan', 1),
(3,  'Luna park Ušće',          1),
(4,  'Gradska biblioteka NS',   2),
(5,  'Sportski centar Vojvodina',2),
(6,  'Zoološki vrt Niš',        3),
(7,  'Aqua park Kragujevac',    4),
(8,  'Planetarijum Subotica',   5),
(9,  'Gradski park Pančevo',    6),
(10, 'Kulturni centar Čačak',   7);

INSERT INTO facility_reservations (id, start_datetime, end_datetime, facility_id, neighborhood_id) VALUES
(1,  '2026-03-05 10:00:00', '2026-03-05 13:00:00', 1,  1),
(2,  '2026-03-12 14:00:00', '2026-03-12 17:00:00', 2,  2),
(3,  '2026-03-20 09:00:00', '2026-03-20 12:00:00', 3,  3),
(4,  '2026-04-02 11:00:00', '2026-04-02 14:00:00', 4,  4),
(5,  '2026-04-10 15:00:00', '2026-04-10 18:00:00', 5,  5),
(6,  '2026-04-18 10:00:00', '2026-04-18 12:00:00', 1,  6),
(7,  '2026-04-25 13:00:00', '2026-04-25 16:00:00', 6,  7),
(8,  '2026-05-03 09:00:00', '2026-05-03 11:00:00', 7,  8),
(9,  '2026-05-14 14:00:00', '2026-05-14 17:00:00', 8,  9),
(10, '2026-05-22 10:00:00', '2026-05-22 13:00:00', 9,  10),
(11, '2026-06-01 11:00:00', '2026-06-01 14:00:00', 2,  1),
(12, '2026-06-10 15:00:00', '2026-06-10 18:00:00', 3,  2);

INSERT INTO budget_categories (id, name) VALUES
(1, 'Uređenje ulica'),
(2, 'Uređenje parkova'),
(3, 'Rasveta'),
(4, 'Čistoća'),
(5, 'Zelenilo'),
(6, 'Hitne popravke'),
(7, 'Dečija igrališta'),
(8, 'Sportski tereni');
 
INSERT INTO budget_category_balances (category_id, neighborhood_id, total_amount) VALUES
(1, 1, 200000.00),
(2, 1, 150000.00),
(3, 1,  80000.00),
(4, 1, 120000.00),
(5, 1, 100000.00),
(6, 1, 100000.00),
(7, 1,  50000.00),
(8, 1,  50000.00),
(1, 6, 100000.00),
(2, 6,  50000.00),
(3, 6,  40000.00),
(4, 6,  80000.00),
(5, 6,  50000.00),
(6, 6,  60000.00),
(7, 6,  20000.00),
(8, 6,  20000.00),
(1, 11, 120000.00),
(2, 11,  60000.00),
(3, 11,  50000.00),
(4, 11,  70000.00),
(5, 11,  50000.00),
(6, 11,  80000.00),
(7, 11,  20000.00),
(8, 11,  20000.00);
 
INSERT INTO budget_expenses (amount, description, expense_date, budget_category_balance_id, coordinator_id) VALUES
(12500.50, 'Popravka ulične svetiljke u Njegoševoj', '2026-04-20', 3,  11),
(45000.00, 'Asfaltiranje udarne rupe kod hrama',     '2026-04-25', 1,  11),
( 8200.00, 'Nove sadnice za Karađorđev park',        '2026-05-01', 5,  11),
( 5600.00, 'Zamena peska u peskarniku na igralištu', '2026-04-15', 7,  11),
(15000.00, 'Hitna sanacija vodovodne cevi',          '2026-04-28', 6,  11),
(22000.00, 'Farbanje klupa u glavnom parku',         '2026-05-02', 2,  11),
( 3100.00, 'Nabavka kesa za komunalni otpad',        '2026-04-10', 4,  11);

INSERT INTO neighborhood_memberships (id, acceptance_date, neighborhood_id) VALUES
(16, '2026-04-01', 1), (17, '2026-04-01', 1), (18, '2026-04-01', 1), (19, '2026-04-01', 1), (20, '2026-04-01', 1), (21, '2026-04-01', 1),
(22, '2026-04-01', 6), (23, '2026-04-01', 6), (24, '2026-04-01', 6), (25, '2026-04-01', 6), (26, '2026-04-01', 6), (27, '2026-04-01', 6),
(28, '2026-04-01', 11), (29, '2026-04-01', 11), (30, '2026-04-01', 11), (31, '2026-04-01', 11), (32, '2026-04-01', 11), (33, '2026-04-01', 11);

INSERT INTO citizens (id, username, user_role, password, name, surname, address_id, membership_id) VALUES
(121, 'c1.1', 'citizen', '123', 'Ivan', 'Ivić', 1, 16), (122, 'c1.2', 'citizen', '123', 'Ana', 'Anić', 1, 17), (123, 'c1.3', 'citizen', '123', 'Luka', 'Lukić', 1, 18), (124, 'c1.4', 'citizen', '123', 'Maja', 'Majić', 1, 19), (125, 'c1.5', 'citizen', '123', 'Sava', 'Savić', 1, 20), (126, 'c1.6', 'citizen', '123', 'Sara', 'Sarić', 1, 21),
(127, 'c6.1', 'citizen', '123', 'Marko', 'Marković', 6, 22), (128, 'c6.2', 'citizen', '123', 'Jana', 'Janić', 6, 23), (129, 'c6.3', 'citizen', '123', 'Igor', 'Igorić', 6, 24), (130, 'c6.4', 'citizen', '123', 'Tea', 'Teić', 6, 25), (131, 'c6.5', 'citizen', '123', 'Vuk', 'Vukić', 6, 26), (132, 'c6.6', 'citizen', '123', 'Mina', 'Minić', 6, 27),
(133, 'c11.1', 'citizen', '123', 'Pera', 'Perić', 11, 28), (134, 'c11.2', 'citizen', '123', 'Neda', 'Nedić', 11, 29), (135, 'c11.3', 'citizen', '123', 'Uroš', 'Urošić', 11, 30), (136, 'c11.4', 'citizen', '123', 'Mira', 'Mirić', 11, 31), (137, 'c11.5', 'citizen', '123', 'Bane', 'Banić', 11, 32), (138, 'c11.6', 'citizen', '123', 'Ena', 'Enić', 11, 33);

INSERT INTO neighborhood_access_requests (citizen_id, neighborhood_id, status, creation_date) VALUES
(121, 1, 'accepted', '2026-03-25'), (122, 1, 'accepted', '2026-03-25'), (123, 1, 'accepted', '2026-03-25'), (124, 1, 'accepted', '2026-03-25'), (125, 1, 'accepted', '2026-03-25'), (126, 1, 'accepted', '2026-03-25'),
(127, 6, 'accepted', '2026-03-25'), (128, 6, 'accepted', '2026-03-25'), (129, 6, 'accepted', '2026-03-25'), (130, 6, 'accepted', '2026-03-25'), (131, 6, 'accepted', '2026-03-25'), (132, 6, 'accepted', '2026-03-25'),
(133, 11, 'accepted', '2026-03-25'), (134, 11, 'accepted', '2026-03-25'), (135, 11, 'accepted', '2026-03-25'), (136, 11, 'accepted', '2026-03-25'), (137, 11, 'accepted', '2026-03-25'), (138, 11, 'accepted', '2026-03-25');

INSERT INTO neighborhood_meetings (id, theme, meeting_time, date_range_start, date_range_end, scheduled_date, status, neighborhood_id) VALUES
(11, 'welcome', '20:00', '2026-06-05', '2026-06-09', NULL, 'in_preparation', 1),
(12, 'motivation', '20:00', '2026-06-05', '2026-06-09', NULL, 'in_preparation', 1),
(13, 'welcome', '20:00', '2026-06-04', '2026-06-09', NULL, 'in_preparation', 1),
(14, 'welcome', '20:00', '2026-06-05', '2026-06-09', NULL, 'in_preparation', 6),
(15, 'motivation', '20:00', '2026-06-05', '2026-06-09', NULL, 'in_preparation', 6),
(16, 'welcome', '20:00', '2026-06-04', '2026-06-08', NULL, 'in_preparation', 6),
(17, 'welcome', '20:00', '2026-06-05', '2026-06-09', NULL, 'in_preparation', 11),
(18, 'motivation', '20:00', '2026-06-05', '2026-06-09', NULL, 'in_preparation', 11),
(19, 'welcome', '20:00', '2026-06-04', '2026-06-09', NULL, 'in_preparation', 11);

INSERT INTO meeting_date_votes (id, voted_date, meeting_id, citizen_id) VALUES
(11, '2026-06-05', 11, 121), (12, '2026-06-06', 11, 122),
(13, '2026-06-04', 12, 123), (14, '2026-06-07', 12, 124),
(15, '2026-06-04', 13, 125), (16, '2026-06-05', 13, 126),
(17, '2026-06-05', 14, 127), (18, '2026-06-06', 14, 128),
(19, '2026-06-05', 15, 129), (20, '2026-06-07', 15, 130),
(21, '2026-06-04', 16, 131), (22, '2026-06-05', 16, 132),
(23, '2026-06-05', 17, 133), (24, '2026-06-07', 17, 134),
(25, '2026-06-05', 18, 135), (26, '2026-06-07', 18, 136),
(27, '2026-06-04', 19, 137), (28, '2026-06-24', 19, 138);

INSERT INTO budget_donations (id, amount, donation_date, neighborhood_id, budget_category_id, citizen_id) VALUES
(1, 50.00, '2026-04-15', 1, 1, 121),
(2, 100.00, '2026-04-20', 1, 2, 122),
(3, 75.50, '2026-05-05', 1, 3, 123),
(4, 200.00, '2026-05-10', 1, 4, 124),
(5, 150.00, '2026-05-15', 1, 5, 125),
(6, 80.00, '2026-04-18', 6, 1, 127),
(7, 120.00, '2026-05-01', 6, 2, 128),
(8, 95.00, '2026-05-12', 6, 3, 129),
(9, 300.00, '2026-05-20', 6, 4, 130),
(10, 175.00, '2026-06-01', 6, 5, 131),
(11, 60.00, '2026-04-25', 11, 1, 133),
(12, 140.00, '2026-05-08', 11, 2, 134),
(13, 85.00, '2026-05-18', 11, 3, 135),
(14, 250.00, '2026-06-05', 11, 4, 136);

INSERT INTO neighborhood_memberships (id, acceptance_date, neighborhood_id) VALUES
(34, '2026-04-15', 1),
(35, '2026-04-20', 1),
(36, '2026-05-01', 1),
(37, '2026-05-10', 1);
 
INSERT INTO citizens (id, username, user_role, password, name, surname, address_id, membership_id) VALUES
(139, 'c1.7', 'citizen', '123', 'Jovan',   'Jovanić',   1, 34),
(140, 'c1.8', 'citizen', '123', 'Milena',  'Milenić',   1, 35),
(141, 'c1.9', 'citizen', '123', 'Darko',   'Darkić',    1, 36),
(142, 'c1.10','citizen', '123', 'Vesna',   'Vesnić',    1, 37);
 
INSERT INTO neighborhood_access_requests (citizen_id, neighborhood_id, status, creation_date) VALUES
(139, 1, 'accepted', '2026-04-10'),
(140, 1, 'accepted', '2026-04-15'),
(141, 1, 'accepted', '2026-04-25'),
(142, 1, 'accepted', '2026-05-05');
 
INSERT INTO neighborhood_memberships (id, acceptance_date, neighborhood_id) VALUES
(38, '2024-03-01', 6),
(39, '2024-04-01', 6),
(40, '2024-05-01', 6),
(41, '2024-06-01', 6),
(42, '2024-07-01', 6),
(43, '2024-08-01', 6),
(44, '2024-08-01', 6),
(45, '2024-08-01', 6),
(46, '2024-08-01', 6);
 
INSERT INTO citizens (id, username, user_role, password, name, surname, address_id, membership_id) VALUES
(143, 'c6.7',  'citizen', '123', 'Dragan',  'Draganić',  6, 38),
(144, 'c6.8',  'citizen', '123', 'Bojana',  'Bojanić',   6, 39),
(145, 'c6.9',  'citizen', '123', 'Nikola',  'Nikolić',   6, 40),
(146, 'c6.10', 'citizen', '123', 'Tamara',  'Tamarić',   6, 41),
(147, 'c6.11', 'citizen', '123', 'Goran',   'Goranić',   6, 42),
(148, 'c6.12', 'citizen', '123', 'Sonja',   'Sonjić',    6, 43),
(149, 'c6.12', 'citizen', '123', 'Sonja',   'Sonjić',    6, 44),
(150, 'c6.12', 'citizen', '123', 'Sonja',   'Sonjić',    6, 45),
(151, 'c6.12', 'citizen', '123', 'Sonja',   'Sonjić',    6, 46);
 
INSERT INTO neighborhood_access_requests (citizen_id, neighborhood_id, status, creation_date) VALUES
(143, 6, 'accepted', '2024-02-20'),
(144, 6, 'accepted', '2024-03-20'),
(145, 6, 'accepted', '2024-04-20'),
(146, 6, 'accepted', '2024-05-20'),
(147, 6, 'accepted', '2024-06-20'),
(148, 6, 'accepted', '2024-07-20'),
(149, 6, 'accepted', '2024-07-20'),
(150, 6, 'accepted', '2024-07-20'),
(151, 6, 'accepted', '2024-07-20');

INSERT INTO neighborhood_memberships (id, acceptance_date, neighborhood_id) VALUES
(47, '2024-02-01', 11),
(48, '2024-03-01', 11),
(49, '2024-04-01', 11),
(50, '2024-05-01', 11),
(51, '2024-05-01', 11),
(52, '2024-05-01', 11),
(53, '2024-05-01', 11);
 
INSERT INTO citizens (id, username, user_role, password, name, surname, address_id, membership_id) VALUES
(152, 'c11.7',  'citizen', '123', 'Zoran',   'Zoranić',  11, 47),
(153, 'c11.8',  'citizen', '123', 'Ivana',   'Ivanić',   11, 48),
(154, 'c11.9',  'citizen', '123', 'Mirko',   'Mirkić',   11, 49),
(155, 'c11.10', 'citizen', '123', 'Jelena',  'Jelenić',  11, 50),
(156, 'c11.10', 'citizen', '123', 'Jelena',  'Jelenić',  11, 51),
(157, 'c11.10', 'citizen', '123', 'Jelena',  'Jelenić',  11, 52),
(158, 'c11.10', 'citizen', '123', 'Jelena',  'Jelenić',  11, 53);
 
INSERT INTO neighborhood_access_requests (citizen_id, neighborhood_id, status, creation_date) VALUES
(152, 11, 'accepted', '2024-01-20'),
(153, 11, 'accepted', '2024-02-20'),
(154, 11, 'accepted', '2024-03-20'),
(155, 11, 'accepted', '2024-04-20'),
(156, 11, 'accepted', '2024-04-20'),
(157, 11, 'accepted', '2024-04-20'),
(158, 11, 'accepted', '2024-04-20');
 
INSERT INTO community_events (id, name, description, event_date, start_time, duration_minutes, min_volunteers, status, neighborhood_id, organizer_citizen_id) VALUES
(11, 'Čišćenje parka',    'Akcija čišćenja', '2025-06-01', '09:00', 120, 5, 'finished', 11, 152),
(12, 'Eko akcija 11',     'Eko akcija',      '2025-06-15', '10:00', 90,  5, 'finished', 11, 152),
(13, 'Sportski dan 11',   'Sport',           '2025-07-01', '14:00', 180, 5, 'finished', 11, 152),
(14, 'Kulturni program',  'Kultura',         '2025-08-01', '18:00', 120, 5, 'finished', 11, 152),
(15, 'Humanitarna 11',    'Humanitarna',     '2025-09-01', '09:00', 240, 5, 'finished', 11, 152),
(16, 'Proslava naselja',  'Proslava',        '2025-10-01', '17:00', 180, 5, 'finished', 11, 153),
(17, 'Čišćenje parka',    'Akcija čišćenja', '2025-06-01', '10:00', 120, 5, 'finished', 11, 153),
(18, 'Eko akcija 11',     'Eko akcija',      '2025-06-15', '11:00', 90,  5, 'finished', 11, 153),
(19, 'Sportski dan 11',   'Sport',           '2025-07-01', '15:00', 180, 5, 'finished', 11, 153),
(20, 'Kulturni program',  'Kultura',         '2025-08-01', '19:00', 120, 5, 'finished', 11, 153),
(21, 'Humanitarna 11',    'Humanitarna',     '2025-09-01', '12:00', 240, 5, 'finished', 11, 154),
(22, 'Proslava naselja',  'Proslava',        '2025-10-01', '19:00', 180, 5, 'finished', 11, 154),
(23, 'Čišćenje parka',    'Akcija čišćenja', '2025-06-01', '10:00', 120, 5, 'finished', 11, 154),
(24, 'Eko akcija 11',     'Eko akcija',      '2025-06-15', '10:00', 90,  5, 'finished', 11, 154),
(25, 'Sportski dan 11',   'Sport',           '2025-07-01', '14:00', 180, 5, 'finished', 11, 154),
(26, 'Kulturni program',  'Kultura',         '2025-08-01', '18:00', 120, 5, 'finished', 11, 155),
(27, 'Humanitarna 11',    'Humanitarna',     '2025-09-01', '09:00', 240, 5, 'finished', 11, 155),
(28, 'Proslava naselja',  'Proslava',        '2025-10-01', '17:00', 180, 5, 'finished', 11, 155),
(29, 'Čišćenje parka',    'Akcija čišćenja', '2025-06-01', '09:00', 120, 5, 'finished', 11, 155),
(30, 'Eko akcija 11',     'Eko akcija',      '2025-06-15', '10:00', 90,  5, 'finished', 11, 155),
(31, 'Sportski dan 11',   'Sport',           '2025-07-01', '14:00', 180, 5, 'finished', 11, 156),
(32, 'Kulturni program',  'Kultura',         '2025-08-01', '18:00', 120, 5, 'finished', 11, 156),
(33, 'Humanitarna 11',    'Humanitarna',     '2025-09-01', '09:00', 240, 5, 'finished', 11, 156),
(34, 'Proslava naselja',  'Proslava',        '2025-10-01', '17:00', 180, 5, 'finished', 11, 156),
(35, 'Proslava naselja',  'Proslava',        '2025-10-01', '17:00', 180, 5, 'finished', 11, 156);
 
INSERT INTO event_registrations (id, attended, event_id, citizen_id) VALUES
(11, TRUE, 11, 157),
(12, TRUE, 12, 157),
(13, TRUE, 13, 157),
(14, TRUE, 14, 157),
(15, TRUE, 15, 157),
(16, TRUE, 16, 158),
(17, TRUE, 17, 158),
(18, TRUE, 18, 158),
(19, TRUE, 19, 158),
(20, TRUE, 20, 158);

INSERT INTO facility_votes (id, facility_id, citizen_id) VALUES
(1, 1, 101),
(2, 2, 101),
(3, 1, 105),
(4, 3, 105),
(5, 2, 111),
(6, 3, 111),
(7, 1, 102),
(8, 3, 102),
(9, 2, 112),
(10, 4, 103),
(11, 5, 103),
(12, 4, 113),
(13, 6, 104),
(14, 7, 114),
(15, 8, 106),
(16, 9, 107),
(17, 10, 108);

INSERT INTO coordinator_reviews (id, rating, comment, created_at, neighborhood_id, reviewer_id, is_removed) VALUES
(1, 5, NULL, '2026-02-15 10:30:00', 1, 101, false),
(2, 4, NULL, '2026-02-20 14:45:00', 1, 105, false),
(3, 2, 'Koordinator nije dostupan.', '2026-03-01 09:15:00', 1, 111, false),
(4, 5, NULL, '2026-02-18 16:20:00', 2, 102, false),
(5, 3, 'Spora komunikacija sa stanovnicima.', '2026-02-25 11:00:00', 2, 112, false),
(6, 4, NULL, '2026-03-05 13:30:00', 2, 117, false),
(7, 5, NULL, '2026-02-22 10:00:00', 3, 103, false),
(8, 5, NULL, '2026-02-28 15:45:00', 3, 113, false),
(9, 4, NULL, '2026-03-03 12:15:00', 3, 118, false),
(10, 5, NULL, '2026-02-17 09:30:00', 4, 104, false),
(11, 4, NULL, '2026-02-24 14:00:00', 4, 119, false),
(12, 5, NULL, '2026-03-02 11:20:00', 4, 109, false),
(13, 4, NULL, '2026-02-19 10:45:00', 5, 120, false),
(14, 5, NULL, '2026-02-26 16:30:00', 5, 110, false);

INSERT INTO review_reports (id, review_id, reporter_id, created_at) VALUES
(1, 3, 105, '2026-03-02 10:00:00'),
(2, 3, 112, '2026-03-03 11:30:00'),
(3, 5, 102, '2026-03-01 09:45:00');

SELECT setval(
    pg_get_serial_sequence('public.users', 'id'),
    COALESCE((SELECT MAX(id) FROM public.users), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.countries', 'id'),
    COALESCE((SELECT MAX(id) FROM public.countries), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.cities', 'id'),
    COALESCE((SELECT MAX(id) FROM public.cities), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.neighborhoods', 'id'),
    COALESCE((SELECT MAX(id) FROM public.neighborhoods), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.streets', 'id'),
    COALESCE((SELECT MAX(id) FROM public.streets), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.street_sections', 'id'),
    COALESCE((SELECT MAX(id) FROM public.street_sections), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.addresses', 'id'),
    COALESCE((SELECT MAX(id) FROM public.addresses), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.buildings', 'id'),
    COALESCE((SELECT MAX(id) FROM public.buildings), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.neighborhood_memberships', 'id'),
    COALESCE((SELECT MAX(id) FROM public.neighborhood_memberships), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.citizens', 'id'),
    COALESCE((SELECT MAX(id) FROM public.citizens), 0),
    true
);

SELECT setval( 
    pg_get_serial_sequence('public.community_events', 'id'),
    COALESCE((SELECT MAX(id) FROM public.community_events), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.event_items', 'id'),
    COALESCE((SELECT MAX(id) FROM public.event_items), 0), 
    true
);

SELECT setval(
    pg_get_serial_sequence('public.event_registrations', 'id'),
    COALESCE((SELECT MAX(id) FROM public.event_registrations), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.event_item_assignments', 'id'),
    COALESCE((SELECT MAX(id) FROM public.event_item_assignments), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.meeting_date_votes', 'id'),
    COALESCE((SELECT MAX(id) FROM public.meeting_date_votes), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.service_providers', 'id'),
    COALESCE((SELECT MAX(id) FROM public.service_providers), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.job_listings', 'id'),
    COALESCE((SELECT MAX(id) FROM public.job_listings), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.price_entries', 'id'),
    COALESCE((SELECT MAX(id) FROM public.price_entries), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.booked_jobs', 'id'),
    COALESCE((SELECT MAX(id) FROM public.booked_jobs), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.leave_requests', 'id'),
    COALESCE((SELECT MAX(id) FROM public.leave_requests), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.coverage_offers', 'id'),
    COALESCE((SELECT MAX(id) FROM public.coverage_offers), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.provider_forums', 'id'),
    COALESCE((SELECT MAX(id) FROM public.provider_forums), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.forum_posts', 'id'),
    COALESCE((SELECT MAX(id) FROM public.forum_posts), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.post_reactions', 'id'),
    COALESCE((SELECT MAX(id) FROM public.post_reactions), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.provider_reviews', 'id'),
    COALESCE((SELECT MAX(id) FROM public.provider_reviews), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.common_rooms', 'id'),
    COALESCE((SELECT MAX(id) FROM public.common_rooms), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.common_room_rental_requests', 'id'),
    COALESCE((SELECT MAX(id) FROM public.common_room_rental_requests), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.ads', 'id'),
    COALESCE((SELECT MAX(id) FROM public.ads), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.ad_time_slots', 'id'),
    COALESCE((SELECT MAX(id) FROM public.ad_time_slots), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.ad_time_slot_reservations', 'id'),
    COALESCE((SELECT MAX(id) FROM public.ad_time_slot_reservations), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.reported_issues', 'id'),
    COALESCE((SELECT MAX(id) FROM public.reported_issues), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.assemblies', 'id'),
    COALESCE((SELECT MAX(id) FROM public.assemblies), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.images', 'id'),
    COALESCE((SELECT MAX(id) FROM public.images), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.neighborhood_meetings', 'id'),
    COALESCE((SELECT MAX(id) FROM public.neighborhood_meetings), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.facility_reservations', 'id'),
    COALESCE((SELECT MAX(id) FROM public.facility_reservations), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.facility_votes', 'id'),
    COALESCE((SELECT MAX(id) FROM public.facility_votes), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.coordinator_reviews', 'id'),
    COALESCE((SELECT MAX(id) FROM public.coordinator_reviews), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.review_reports', 'id'),
    COALESCE((SELECT MAX(id) FROM public.review_reports), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.budget_categories', 'id'),
    COALESCE((SELECT MAX(id) FROM public.budget_categories), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.budget_category_balances', 'id'),
    COALESCE((SELECT MAX(id) FROM public.budget_category_balances), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.budget_expenses', 'id'),
    COALESCE((SELECT MAX(id) FROM public.budget_expenses), 0),
    true
);

SELECT setval(
    pg_get_serial_sequence('public.budget_donations', 'id'),
    COALESCE((SELECT MAX(id) FROM public.budget_donations), 0),
    true
);

COMMIT;