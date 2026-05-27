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
(1, '2026-02-05 14:00:00-08', '9026-02-05 14:00:00-08', '\x00'),
(2, '2026-02-05 14:00:00-08', '9026-02-05 14:00:00-08', '\x00'),
(3, '2026-02-05 14:00:00-08', '9026-02-05 14:00:00-08', '\x00'),
(4, '2026-02-05 14:00:00-08', '9026-02-05 14:00:00-08', '\x00'),
(5, '2026-02-05 14:00:00-08', '9026-02-05 14:00:00-08', '\x00'),
(6, '2026-02-05 14:00:00-08', '9026-02-05 14:00:00-08', '\x00');

INSERT INTO "Books" ("ISBN", "Title", "Authors", "Publishers", "Description", "PublishDate", "CreatedAt") VALUES
('12345', 'Tales of Foo', 'Mr. Foo', 'Foo Corp', 'Foo goes on an adventure', '1995-01-01 01:00:00-08', '2026-01-01 01:00:00-08'),
('12346', 'Tales of Bar', 'Mr. Bar', 'Bar Corp', 'Bar goes on an adventure', '1996-01-01 01:00:00-08', '2026-01-02 01:00:00-08'),
('12347', 'Tales of Baz', 'Mr. Baz', 'Baz Corp', 'Baz goes on an adventure', '1997-01-01 01:00:00-08', '2026-01-03 01:00:00-08'),
('12348', 'Tales of Quz', 'Mr. Quz', 'Quz Corp', 'Quz goes on an adventure', '1998-01-01 01:00:00-08', '2026-01-04 01:00:00-08'),
('12349', 'Tales of Quuz', 'Mr. Quuz', 'Quuz Corp', 'Quuz goes on an adventure', '1999-01-01 01:00:00-08', '2026-01-05 01:00:00-08');

INSERT INTO "BookListings" ("Condition", "PurchaseType", "Price", "CreatedAt", "UserId", "ISBN") VALUES
('Good', 'Buy', '$19.99', '2026-01-01 01:00:00-08', 1, '12345'),
('Good', 'Rent', '$29.99', '2026-01-02 01:00:00-08', 1, '12346'),
('Used', 'Buy', '$39.99', '2026-01-03 01:00:00-08', 1, '12347'),
('Used', 'Rent', '$49.99', '2026-01-04 01:00:00-08', 2, '12348'),
('Good', 'Buy', '$59.99', '2026-01-05 01:00:00-08', 2, '12349');

COMMIT;
