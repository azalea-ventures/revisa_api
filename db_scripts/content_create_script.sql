USE revisa_db

-- Drop existing schema, if it exists
IF EXISTS (SELECT schema_name
FROM information_schema.schemata
WHERE schema_name = 'content')
BEGIN
    -- Drop all tables in the schema
    DROP TABLE IF EXISTS content.content_txt;
    DROP TABLE IF EXISTS content.content_group;
    DROP TABLE IF EXISTS content.content_versions;
    DROP TABLE IF EXISTS content.content_details;
    DROP TABLE IF EXISTS content.content_type;
    DROP TABLE IF EXISTS content.clients;
    DROP TABLE IF EXISTS content.grades;
    DROP TABLE IF EXISTS content.subjects;
    DROP TABLE IF EXISTS content.users;

    -- Drop the schema
    DROP SCHEMA IF EXISTS content;
END
GO

-- Create new schema
CREATE SCHEMA content;
GO

-- Create clients table
CREATE TABLE content.clients
(
    id INT PRIMARY KEY IDENTITY(1,1),
    client_name NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- Create grades table
CREATE TABLE content.grades
(
    id INT PRIMARY KEY IDENTITY(1,1),
    grade NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- Create subjects table
CREATE TABLE content.subjects
(
    id INT PRIMARY KEY IDENTITY(1,1),
    [subject] NVARCHAR(100) NOT NULL
);
GO

-- Create users table
CREATE TABLE content.users
(
    id INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL
);
GO

-- Create content_details table with owner_id column
CREATE TABLE content.content_details
(
    id INT PRIMARY KEY IDENTITY(1,1),
    client_id INT REFERENCES content.clients(id) NOT NULL,
    grade_id INT REFERENCES content.grades(id)NOT NULL,
    subject_id INT REFERENCES content.subjects(id)NOT NULL,
    delivery_date DATE NOT NULL,
    owner_id INT REFERENCES content.users(id) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);
GO

-- Create content_type table
CREATE TABLE content.content_type
(
    id INT PRIMARY KEY IDENTITY(1,1),
    content_type NVARCHAR(100) NOT NULL
);
GO

-- Create content_versions table
CREATE TABLE content.content_versions
(
    id INT PRIMARY KEY IDENTITY(1,1),
    [version] INT NOT NULL,
    owner_id INT REFERENCES content.users(id) NOT NULL,
    content_details_id INT REFERENCES content.content_details(id) NOT NULL,
    is_latest TINYINT DEFAULT 1,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT CHK_IsLatest CHECK (is_latest IN (0, 1))
    -- Check constraint for is_latest column
);
GO


-- Create content_group table
CREATE TABLE content.content_group
(
    id INT PRIMARY KEY IDENTITY(1,1),
    content_version_id INT REFERENCES content.content_versions(id) NOT NULL
);
GO

CREATE TABLE content.content_txt
(
    id INT PRIMARY KEY IDENTITY(1,1),
    object_id VARCHAR(50) NOT NULL,
    content_group_id INT REFERENCES content.content_group(id) NOT NULL,
    txt TEXT
);
GO

BEGIN TRANSACTION
-- Insert data into Grades table
INSERT INTO content.grades
    (grade)
VALUES
    ('K'),
    ('1'),
    ('2'),
    ('3'),
    ('4'),
    ('5'),
    ('6'),
    ('7'),
    ('8'),
    ('9'),
    ('10'),
    ('11'),
    ('12'),
    ('primary'),
    ('secondary'),
    ('all');
COMMIT;
GO


CREATE TABLE content.content_file
(
    id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    file_name NVARCHAR(MAX),
    file_id NVARCHAR(MAX),
    source_file_id NVARCHAR(MAX)
);

GO;

CREATE TABLE content.content_status
(
    id INT PRIMARY KEY NOT NULL,
    status VARCHAR(12)
)

INSERT INTO content.content_status
    (id, status)
VALUES
    (0, 'NONE'),
    (1, 'IMPORTED'),
    (2, 'PROCESSED'),
    (3, 'EXPORTED'),
    (4, 'ARCHIVED'),
    (5, 'TRANSLATED');
COMMIT;
GO

CREATE TABLE content.content_language
(
    id INT PRIMARY KEY NOT NULL,
    language VARCHAR(32),
    abbreviation VARCHAR(4)
)
GO

INSERT INTO content.content_language
    (id, language, abbreviation)
VALUES
    (1, 'ENGLISH', 'EN'),
    (2, 'SPANISH', 'ESP')
GO

BEGIN TRANSACTION
INSERT INTO content.content_file
    (id)
VALUES
    ('00000000-0000-0000-0000-000000000000')

ALTER TABLE content.content_details
ADD file_id UNIQUEIDENTIFIER DEFAULT '00000000-0000-0000-0000-000000000000' NULL REFERENCES content.content_file(id);

UPDATE content.content_details
SET file_id = '00000000-0000-0000-0000-000000000000';
COMMIT;
GO;

ALTER TABLE content.content_details
ADD language_id INT NOT NULL DEFAULT 1;
GO;

ALTER TABLE content.content_details
ADD CONSTRAINT FK_content_d_lang
FOREIGN KEY (language_id) REFERENCES content.content_language(id);
GO;

-- Trigger to insert into content_versions
CREATE OR ALTER TRIGGER trg_insert_content_version
ON content.content_details
AFTER INSERT
AS
BEGIN
    DECLARE @owner_id INT;
    DECLARE @id INT;

    SELECT @owner_id = inserted.owner_id
    FROM inserted;

    SELECT @id = inserted.id
    FROM inserted;


    -- Update previous latest version to set is_latest to 0
    UPDATE content.content_versions
    SET is_latest = 0
    WHERE content_details_id = @id;

    -- Create new content_versions record
    INSERT INTO content.content_versions
        ([version], owner_id, is_latest, content_details_id)
    VALUES
        ((SELECT ISNULL(MAX([version]), 0)
            FROM content.content_versions
            WHERE owner_id = @owner_id) + 1, @owner_id, 1, @id);


END;
GO

CREATE OR ALTER TRIGGER trg_upper_client_name
ON [content].[clients]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @client_name NVARCHAR(100);
    DECLARE @id INT;

    SELECT @client_name = UPPER(inserted.client_name)
    FROM inserted;

    SELECT @id = inserted.id
    FROM inserted;

    UPDATE [content].[clients]
    SET client_name = @client_name
    WHERE id = @id;

END;

