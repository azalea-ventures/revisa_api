USE revisa_db

-- Drop existing schema, if it exists
IF EXISTS (SELECT schema_name
FROM information_schema.schemata
WHERE schema_name = 'language_supports')
BEGIN
    -- Drop all tables in the schema
    DROP TABLE IF EXISTS language_supports.cognates
    DROP TABLE IF EXISTS language_supports.languages
    DROP TABLE IF EXISTS language_supports.iclos
    
    -- Drop the schema
    DROP SCHEMA IF EXISTS language_supports;
END
GO

-- Create new schema
CREATE SCHEMA language_supports;
GO
CREATE TABLE language_supports.iclos
(
    id INT PRIMARY KEY IDENTITY(1,1),
    iclo TEXT NOT NULL,
    strategy_objective_id INT REFERENCES elps.strategies_objectives(id) NOT NULL,
    teks_item_id UNIQUEIDENTIFIER REFERENCES teks.teks_items(id) NOT NULL,
    content_txt_id INT REFERENCES content.content_txt(id)
);
GO

CREATE TABLE language_supports.languages
(
    id INT PRIMARY KEY IDENTITY(1,1),
    language_short VARCHAR(4),
    language_name VARCHAR(32) NOT NULL
);
GO

CREATE TABLE language_supports.cognates
(
    id INT PRIMARY KEY IDENTITY(1,1),
    language_origin_id INT REFERENCES language_supports.languages(id),
    language_origin_text VARCHAR(56) NOT NULL,
    language_target_id INT REFERENCES language_supports.languages(id),
    language_target_text VARCHAR(56) NOT NULL,
    language_target_meaning TEXT,
    content_txt_id INT REFERENCES content.content_txt(id)
);
GO
