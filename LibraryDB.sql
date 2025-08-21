-- Creating Tables
DROP TABLE IF EXISTS RentDetails;
DROP TABLE IF EXISTS Rent;
DROP TABLE IF EXISTS Book;
DROP TABLE IF EXISTS Member;
DROP TABLE IF EXISTS Librarians;
DROP TABLE IF EXISTS Genre;
DROP TABLE IF EXISTS Publish;

-- 1) Member(reader) tables
CREATE TABLE Member (
    MemberId    INTEGER PRIMARY KEY AUTOINCREMENT,
    MemberName  TEXT NOT NULL
);

-- 2) Librarian Table
CREATE TABLE Librarians (
    LibrarianId   INTEGER PRIMARY KEY AUTOINCREMENT,
    LibrarianName TEXT NOT NULL
);

-- 3) Genre Table
CREATE TABLE Genre (
    GenreId    INTEGER PRIMARY KEY AUTOINCREMENT,
    GenreName  TEXT NOT NULL UNIQUE
);

--4) Publisher Table
CREATE TABLE Publish (
    PublishId   INTEGER PRIMARY KEY AUTOINCREMENT,
    PublishName TEXT NOT NULL UNIQUE
);

-- 5) Book table
CREATE TABLE Book (
    BookId      INTEGER PRIMARY KEY AUTOINCREMENT,
    BookName    TEXT NOT NULL,
    AuthorName  TEXT NOT NULL,
    GenreId     INTEGER NOT NULL,
    PublishId   INTEGER NOT NULL,
    Amount      INTEGER NOT NULL DEFAULT 0,
    Availability INTEGER AS (CASE WHEN Amount > 0 THEN 1 ELSE 0 END) STORED,

    FOREIGN KEY (GenreId)  REFERENCES Genre(GenreId),
    FOREIGN KEY (PublishId) REFERENCES Publish(PublishId)
);

-- 6) Rent table
CREATE TABLE Rent (
    RentId      INTEGER PRIMARY KEY AUTOINCREMENT,
    RentDate    TEXT NOT NULL DEFAULT (datetime('now')),
    MemberId    INTEGER NOT NULL,
    LibrarianId INTEGER NOT NULL,

    FOREIGN KEY (MemberId)    REFERENCES Member(MemberId),
    FOREIGN KEY (LibrarianId) REFERENCES Librarians(LibrarianId)
);

-- 7) RentDetails table
CREATE TABLE RentDetails (
    RentDetailId INTEGER PRIMARY KEY AUTOINCREMENT,
    RentId       INTEGER NOT NULL,
    BookId       INTEGER NOT NULL,
    DueDate      TEXT NOT NULL,     -- YYYY-MM-DD format
    ReturnDate   TEXT,              -- can be NULL

    FOREIGN KEY (RentId) REFERENCES Rent(RentId),
    FOREIGN KEY (BookId) REFERENCES Book(BookId)
);


-- Inserting Data
INSERT INTO Member (MemberName) VALUES
('Aryan Sharma'),
('Emily Chen'),
('Liam Brown'),
('Noah Wilson'),
('Olivia Davis'),
('Ava Patel'),
('Mia Thompson'),
('Ethan Garcia'),
('Amelia Singh'),
('James Walker'),
('Sophia Lee'),
('Benjamin Clark');

/* ===== Librarians ===== */
INSERT INTO Librarians (LibrarianName) VALUES
('Sarah Reed'),
('Mark Johnston'),
('Danielle Fox'),
('Oscar Hall'),
('Priya Kapoor');

/* ===== Genres ===== */
INSERT INTO Genre (GenreName) VALUES
('Fiction'),
('Non-Fiction'),
('Science'),
('Fantasy'),
('History'),
('Programming'),
('Biography'),
('Mystery');

/* ===== Publishers ("Publish") ===== */
INSERT INTO Publish (PublishName) VALUES
('Penguin Random House'),
('HarperCollins'),
('Macmillan'),
('Hachette'),
('Simon & Schuster'),
('O''Reilly Media'),
('Packt Publishing');

/* ===== Books ===== */
INSERT INTO Book (BookName, AuthorName, GenreId, PublishId, Amount) VALUES
('The Silent Patient',       'Alex Michaelides', 8, 1, 5),
('Atomic Habits',            'James Clear',      2, 5, 3),
('A Brief History of Time',  'Stephen Hawking',  3, 3, 2),
('The Hobbit',               'J.R.R. Tolkien',   4, 2, 0),
('Sapiens',                  'Yuval Noah Harari',5, 4, 4),
('Clean Code',               'Robert C. Martin', 6, 6, 6),
('Introduction to Algorithms','CLRS',            6, 3, 2),
('Becoming',                 'Michelle Obama',   7, 5, 3),
('Gone Girl',                'Gillian Flynn',    8, 2, 1),
('The Pragmatic Programmer', 'Andrew Hunt',      6, 6, 5),
('Deep Work',                'Cal Newport',      2, 4, 2),
('Dune',                     'Frank Herbert',    4, 1, 0),
('Educated',                 'Tara Westover',    7, 5, 2),
('Data Structures in C#',    'Jon Skeet',        6, 7, 3),
('The Wright Brothers',      'David McCullough', 5, 5, 1);

/* ===== Sample Rents and Details ===== */

/* Rent #1 */
INSERT INTO Rent (MemberId, LibrarianId, RentDate)
VALUES (
    (SELECT MemberId FROM Member WHERE MemberName='Aryan Sharma' LIMIT 1),
    (SELECT LibrarianId FROM Librarians WHERE LibrarianName='Sarah Reed' LIMIT 1),
    datetime('now')
);

INSERT INTO RentDetails (RentId, BookId, DueDate, ReturnDate)
VALUES
(
    last_insert_rowid(),
    (SELECT BookId FROM Book WHERE BookName='Clean Code'),
    date('now','+14 day'),
    NULL
),
(
    last_insert_rowid(),
    (SELECT BookId FROM Book WHERE BookName='Atomic Habits'),
    date('now','+14 day'),
    NULL
);

/* Rent #2 */
INSERT INTO Rent (MemberId, LibrarianId, RentDate)
VALUES (
    (SELECT MemberId FROM Member WHERE MemberName='Emily Chen' LIMIT 1),
    (SELECT LibrarianId FROM Librarians WHERE LibrarianName='Mark Johnston' LIMIT 1),
    date('now','-3 day')
);

INSERT INTO RentDetails (RentId, BookId, DueDate, ReturnDate)
VALUES
(
    last_insert_rowid(),
    (SELECT BookId FROM Book WHERE BookName='A Brief History of Time'),
    date('now','+10 day'),
    NULL
),
(
    last_insert_rowid(),
    (SELECT BookId FROM Book WHERE BookName='Gone Girl'),
    date('now','+10 day'),
    NULL
);

/* Rent #3 (already returned) */
INSERT INTO Rent (MemberId, LibrarianId, RentDate)
VALUES (
    (SELECT MemberId FROM Member WHERE MemberName='Liam Brown' LIMIT 1),
    (SELECT LibrarianId FROM Librarians WHERE LibrarianName='Danielle Fox' LIMIT 1),
    date('now','-20 day')
);

INSERT INTO RentDetails (RentId, BookId, DueDate, ReturnDate)
VALUES
(
    last_insert_rowid(),
    (SELECT BookId FROM Book WHERE BookName='The Pragmatic Programmer'),
    date('now','-6 day'),
    date('now','-5 day')
),
(
    last_insert_rowid(),
    (SELECT BookId FROM Book WHERE BookName='Sapiens'),
    date('now','-6 day'),
    date('now','-6 day')
);