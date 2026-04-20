-- ako pokretanje skripte baci exception pokreni ROLLBACK;
-- popravi gresku i pokusaj ponovo
BEGIN;

-- DROP SCHEMA public CASCADE;
-- CREATE SCHEMA public;

DROP TABLE IF EXISTS provider_reviews;
DROP TABLE IF EXISTS post_reactions;
DROP TABLE IF EXISTS forum_posts;
DROP TABLE IF EXISTS provider_forums;
DROP TABLE IF EXISTS coverage_offers;
DROP TABLE IF EXISTS leave_requests;
DROP TABLE IF EXISTS booked_jobs;
DROP TABLE IF EXISTS price_entries;
DROP TABLE IF EXISTS job_listings;
DROP TABLE IF EXISTS service_providers;

DROP TABLE IF EXISTS neighborhood_access_requests;
DROP TABLE IF EXISTS citizens;
DROP TABLE IF EXISTS neighborhood_memberships;
DROP TABLE IF EXISTS street_sections;

DROP TABLE IF EXISTS apartment_access_requests;
DROP TABLE IF EXISTS apartment_memberships;
DROP TABLE IF EXISTS apartments;

DROP TABLE IF EXISTS common_rooms;
DROP TABLE IF EXISTS common_room_rental_requests;
DROP TABLE IF EXISTS ads;
DROP TABLE IF EXISTS ad_time_slots;
DROP TABLE IF EXISTS ad_time_slot_reservations;

DROP TABLE IF EXISTS images;

DROP TABLE IF EXISTS neighborhoods;
DROP TABLE IF EXISTS buildings;

DROP TABLE IF EXISTS addresses;
DROP TABLE IF EXISTS streets;
DROP TABLE IF EXISTS cities;
DROP TABLE IF EXISTS countries;

DROP TABLE IF EXISTS users;

DROP TYPE IF EXISTS statuses;
DROP TYPE IF EXISTS roles;
DROP TYPE IF EXISTS parity_types;
DROP TYPE IF EXISTS billing_types;

COMMIT;