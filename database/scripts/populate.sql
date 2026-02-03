-- AI-GENERATED TEST DATA

BEGIN;

INSERT INTO "Schools" ("Name", "Acronym", "State", "City") VALUES
('Harvard University', 'HU', 'MA', 'Cambridge'),
('Stanford University', 'SU', 'CA', 'Stanford'),
('University of Texas', 'UT', 'TX', 'Austin'),
('University of Florida', 'UF', 'FL', 'Gainesville'),
('New York University', 'NYU', 'NY', 'New York');

INSERT INTO "Users" ("FirstName", "LastName", "MiddleName", "EmailAddress", "SchoolId", "CreatedAt", "Username", "IsAdmin") VALUES
('John', 'Doe', 'A', 'john.doe@example.com', 1, '2026-01-01 10:00:00-08', 'johndoe', false),
('Jane', 'Smith', NULL, 'jane.smith@example.com', 2, '2026-01-02 11:00:00-08', 'janesmith', false),
('Alice', 'Johnson', 'B', 'alice.johnson@example.com', 3, '2026-01-03 12:00:00-08', 'alicejohnson', false),
('Bob', 'Brown', NULL, 'bob.brown@example.com', 4, '2026-01-04 13:00:00-08', 'bobbrown', false),
('Charlie', 'Davis', 'C', 'charlie.davis@example.com', 5, '2026-01-05 14:00:00-08', 'charliedavis', false),
('Admin', 'Admin', 'Admin', 'admin@opentextbookexchange.shop', 1, '2026-02-05 14:00:00-08', 'admin', true);

-- passwords:
-- password1
-- password2
-- password3
-- password4
-- password5
INSERT INTO "Argon2idPasswords" ("Version", "MemoryCost", "Iterations", "Parallelism", "Salt", "Hash", "UserId") VALUES
(19, 65536, 3, 1, '\xf79ae07eebdd3fb92e2ca57bac09df98', '\xbd871bbb6e650731c415cbbf88a9636b', 1),
(19, 65536, 3, 1, '\xb8274135d88cc286ef80e1ffcef5266c', '\x91778658576832aa6bdfdee6712fcfae', 2),
(19, 65536, 3, 1, '\xde1ffc151aace8b8d09f3d4308de6822', '\x5999412962829ead028c6de82b579e4f', 3),
(19, 65536, 3, 1, '\xf95579c942a24cbde194038e916648ac', '\xd5de7532cc8998826ff9f0ef8aa73240', 4),
(19, 65536, 3, 1, '\x2690f4d242b6a7f1d36d54a9c6c28f85', '\x9d41c1b520848e59a9430ab1fbe0c3b9', 5);

INSERT INTO "SessionTokens" ("UserId", "CreatedAt", "ExpiresAt", "Token") VALUES
(6, '2026-02-05 14:00:00-08', '9026-02-05 14:00:00-08', '\x00');

COMMIT;
