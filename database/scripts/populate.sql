-- AI-GENERATED TEST DATA

INSERT INTO "Schools" ("Name", "Acronym", "State", "City") VALUES
('Harvard University', 'HU', 'MA', 'Cambridge'),
('Stanford University', 'SU', 'CA', 'Stanford'),
('University of Texas', 'UT', 'TX', 'Austin'),
('University of Florida', 'UF', 'FL', 'Gainesville'),
('New York University', 'NYU', 'NY', 'New York');

INSERT INTO "Argon2idPasswords" ("Version", "MemoryCost", "Iterations", "Parallelism", "Salt", "Hash") VALUES
(19, 65536, 3, 1, '\x1234567890abcdef', '\xabcdef1234567890'),
(19, 65536, 3, 1, '\xabcdef1234567890', '\x1234567890abcdef'),
(19, 65536, 3, 1, '\xdeadbeefdeadbeef', '\xbeefdeadbeefdead'),
(19, 65536, 3, 1, '\xcafebabe12345678', '\x5678cafebabe1234'),
(19, 65536, 3, 1, '\xfeedfacefeedface', '\xfacefeedfacefeed');

INSERT INTO "Users" ("FirstName", "LastName", "MiddleName", "EmailAddress", "SchoolId", "Argon2idPasswordId", "CreatedAt", "Username") VALUES
('John', 'Doe', 'A', 'john.doe@example.com', 1, 1, '2026-01-01 10:00:00-08', 'johndoe'),
('Jane', 'Smith', NULL, 'jane.smith@example.com', 2, 2, '2026-01-02 11:00:00-08', 'janesmith'),
('Alice', 'Johnson', 'B', 'alice.johnson@example.com', 3, 3, '2026-01-03 12:00:00-08', 'alicejohnson'),
('Bob', 'Brown', NULL, 'bob.brown@example.com', 4, 4, '2026-01-04 13:00:00-08', 'bobbrown'),
('Charlie', 'Davis', 'C', 'charlie.davis@example.com', 5, 5, '2026-01-05 14:00:00-08', 'charliedavis');
