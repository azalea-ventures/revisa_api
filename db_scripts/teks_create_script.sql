USE revisa_db;
GO

-- Create schema
CREATE SCHEMA teks;
GO

BEGIN TRANSACTION
-- Create tables

CREATE TABLE teks.teks_item_types (
    id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    title NVARCHAR(MAX)
);

CREATE TABLE teks.teks_subjects (
    id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    title NVARCHAR(MAX),
    human_coding_scheme VARCHAR(3) NOT NULL
);

CREATE TABLE teks.teks_items (
    id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    parent_id UNIQUEIDENTIFIER,
    list_enumeration INT,
    item_type_id UNIQUEIDENTIFIER,
    human_coding_scheme NVARCHAR(MAX),
    full_statement NVARCHAR(MAX),
    language NVARCHAR(MAX),
    last_change_tea DATETIME,
    uploaded_at DATETIME,
    FOREIGN KEY (parent_id) REFERENCES teks.teks_items(id),
    FOREIGN KEY (item_type_id) REFERENCES teks.teks_item_types(id)
);

CREATE TABLE teks.teks (
    id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    subject_id UNIQUEIDENTIFIER,
    title NVARCHAR(MAX),
    description NVARCHAR(MAX),
    adoption_status NVARCHAR(MAX),
    effective_year NVARCHAR(MAX),
    notes NVARCHAR(MAX),
    official_source_url NVARCHAR(MAX),
    language NVARCHAR(MAX),
    FOREIGN KEY (subject_id) REFERENCES teks.teks_subjects(id)
);

COMMIT;


-- DROP TABLE teks.teks;
-- DROP TABLE teks.teks_items;
-- DROP TABLE teks.teks_item_types;
-- DROP TABLE teks.teks_subjects;
-- DROP SCHEMA teks;