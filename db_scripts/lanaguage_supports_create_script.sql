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

CREATE TABLE language_supports.lesson_schedule(
    id INT PRIMARY KEY IDENTITY(1,1),
    delivery_date DATE NOT NULL,
    lesson_order INT NOT NULL
);
GO

CREATE TABLE language_supports.iclos
(
    id INT PRIMARY KEY IDENTITY(1,1),
    iclo TEXT NOT NULL,
    strategy_objective_id INT REFERENCES elps.strategies_objectives(id) NOT NULL,
    teks_item_id UNIQUEIDENTIFIER REFERENCES teks.teks_items(id) NOT NULL,
    lesson_schedule_id INT REFERENCES language_supports.lesson_schedule(id) NOT NULL,
);
GO

-- maps content subjects to teks subjects
-- we don't always know a client's subject mapping so this
-- should be done at the db level for flexibility
CREATE TABLE language_supports.content_teks_subjects(
    content_subject_id INT REFERENCES content.subjects(id) NOT NULL,
    teks_subject_id UNIQUEIDENTIFIER REFERENCES teks.teks_subjects(id) NOT NULL,
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

CREATE TABLE [language_supports].[support_packages]
(
    id int PRIMARY KEY IDENTITY(1,1),
    content_language_id int not null,
    grade_id int not null,
    subject_id int not null,
    lesson_schedule_id int not null,
    is_active int not null default 1,
    elps_strategy_objective_id int null,
    cross_linguistic_connection VARCHAR(255) null,
    FOREIGN KEY (elps_strategy_objective_id) REFERENCES elps.strategies_objectives(id),
    FOREIGN KEY (content_language_id) REFERENCES content.content_language(id),
    FOREIGN KEY (grade_id) REFERENCES content.grades(id),
    FOREIGN KEY (subject_id) REFERENCES content.subjects(id),
    FOREIGN KEY (lesson_schedule_id) REFERENCES language_supports.lesson_schedule(id),
);
GO

--sample data for testing
BEGIN TRANSACTION
INSERT INTO language_supports.lesson_schedule
(delivery_date, lesson_order)
VALUES (Parse('2024-03-18' as date), 117);

BEGIN TRANSACTION
INSERT INTO language_supports.iclos
(iclo, strategy_objective_id, teks_item_id, lesson_schedule_id)
VALUES ('', 117, 'd3202c19-11b9-58a3-9c42-173e5c8d135f', 2)
COMMIT;

BEGIN TRANSACTION
INSERT INTO language_supports.lesson_schedule
    (delivery_date, lesson_order)
VALUES
    (Parse('2024-08-12' as date), 1),
    (Parse('2024-08-13' as date), 2),
    (Parse('2024-08-14' as date), 3),
    (Parse('2024-08-15' as date), 4),
    (Parse('2024-08-16' as date), 5),
    (Parse('2024-08-19' as date), 6),
    (Parse('2024-08-20' as date), 7),
    (Parse('2024-08-21' as date), 8),
    (Parse('2024-08-22' as date), 9),
    (Parse('2024-08-23' as date), 10),
    (Parse('2024-08-26' as date), 11),
    (Parse('2024-08-27' as date), 12),
    (Parse('2024-08-28' as date), 13),
    (Parse('2024-08-29' as date), 14),
    (Parse('2024-08-30' as date), 15),
    (Parse('2024-09-04' as date), 16),
    (Parse('2024-09-05' as date), 17),
    (Parse('2024-09-06' as date), 18),
    (Parse('2024-09-09' as date), 19),
    (Parse('2024-09-10' as date), 20),
    (Parse('2024-09-11' as date), 21),
    (Parse('2024-09-12' as date), 22),
    (Parse('2024-09-13' as date), 23),
    (Parse('2024-09-16' as date), 24),
    (Parse('2024-09-17' as date), 25),
    (Parse('2024-09-18' as date), 26),
    (Parse('2024-09-19' as date), 27),
    (Parse('2024-09-20' as date), 28),
    (Parse('2024-09-23' as date), 29),
    (Parse('2024-09-24' as date), 30),
    (Parse('2024-09-25' as date), 31),
    (Parse('2024-09-26' as date), 32),
    (Parse('2024-09-27' as date), 33),
    (Parse('2024-09-30' as date), 34),
    (Parse('2024-10-01' as date), 35),
    (Parse('2024-10-02' as date), 36),
    (Parse('2024-10-07' as date), 37),
    (Parse('2024-10-08' as date), 38),
    (Parse('2024-10-09' as date), 39),
    (Parse('2024-10-10' as date), 40),
    (Parse('2024-10-11' as date), 41),
    (Parse('2024-10-14' as date), 42),
    (Parse('2024-10-15' as date), 43),
    (Parse('2024-10-16' as date), 44),
    (Parse('2024-10-17' as date), 45),
    (Parse('2024-10-18' as date), 46),
    (Parse('2024-10-21' as date), 47),
    (Parse('2024-10-22' as date), 48),
    (Parse('2024-10-23' as date), 49),
    (Parse('2024-10-24' as date), 50),
    (Parse('2024-10-25' as date), 51),
    (Parse('2024-10-28' as date), 52),
    (Parse('2024-10-29' as date), 53),
    (Parse('2024-10-30' as date), 54),
    (Parse('2024-10-31' as date), 55),
    (Parse('2024-11-01' as date), 56),
    (Parse('2024-11-04' as date), 57),
    (Parse('2024-11-05' as date), 58),
    (Parse('2024-11-06' as date), 59),
    (Parse('2024-11-07' as date), 60),
    (Parse('2024-11-11' as date), 61),
    (Parse('2024-11-12' as date), 62),
    (Parse('2024-11-13' as date), 63),
    (Parse('2024-11-14' as date), 64),
    (Parse('2024-11-15' as date), 65),
    (Parse('2024-11-18' as date), 66),
    (Parse('2024-11-19' as date), 67),
    (Parse('2024-11-20' as date), 68),
    (Parse('2024-11-21' as date), 69),
    (Parse('2024-11-22' as date), 70),
    (Parse('2024-12-02' as date), 71),
    (Parse('2024-12-03' as date), 72),
    (Parse('2024-12-04' as date), 73),
    (Parse('2024-12-05' as date), 74),
    (Parse('2024-12-06' as date), 75),
    (Parse('2024-12-09' as date), 76),
    (Parse('2024-12-10' as date), 77),
    (Parse('2024-12-11' as date), 78),
    (Parse('2024-12-12' as date), 79),
    (Parse('2024-12-13' as date), 80),
    (Parse('2024-12-16' as date), 81),
    (Parse('2024-12-17' as date), 82),
    (Parse('2024-12-18' as date), 83),
    (Parse('2024-12-19' as date), 84),
    (Parse('2024-12-20' as date), 85),
    (Parse('2025-01-07' as date), 86),
    (Parse('2025-01-08' as date), 87),
    (Parse('2025-01-09' as date), 88),
    (Parse('2025-01-10' as date), 89),
    (Parse('2025-01-13' as date), 90),
    (Parse('2025-01-14' as date), 91),
    (Parse('2025-01-15' as date), 92),
    (Parse('2025-01-16' as date), 93),
    (Parse('2025-01-17' as date), 94),
    (Parse('2025-01-21' as date), 95),
    (Parse('2025-01-22' as date), 96),
    (Parse('2025-01-23' as date), 97),
    (Parse('2025-01-24' as date), 98),
    (Parse('2025-01-27' as date), 99),
    (Parse('2025-01-28' as date), 100),
    (Parse('2025-01-29' as date), 101),
    (Parse('2025-01-30' as date), 102),
    (Parse('2025-01-31' as date), 103),
    (Parse('2025-02-03' as date), 104),
    (Parse('2025-02-04' as date), 105),
    (Parse('2025-02-05' as date), 106),
    (Parse('2025-02-06' as date), 107),
    (Parse('2025-02-07' as date), 108),
    (Parse('2025-02-10' as date), 109),
    (Parse('2025-02-11' as date), 110),
    (Parse('2025-02-12' as date), 111),
    (Parse('2025-02-13' as date), 112),
    (Parse('2025-02-18' as date), 113),
    (Parse('2025-02-19' as date), 114),
    (Parse('2025-02-20' as date), 115),
    (Parse('2025-02-21' as date), 116),
    (Parse('2025-02-24' as date), 117),
    (Parse('2025-02-25' as date), 118),
    (Parse('2025-02-26' as date), 119),
    (Parse('2025-02-27' as date), 120),
    (Parse('2025-02-28' as date), 121),
    (Parse('2025-03-03' as date), 122),
    (Parse('2025-03-04' as date), 123),
    (Parse('2025-03-05' as date), 124),
    (Parse('2025-03-06' as date), 125),
    (Parse('2025-03-07' as date), 126),
    (Parse('2025-03-17' as date), 127),
    (Parse('2025-03-18' as date), 128),
    (Parse('2025-03-19' as date), 129),
    (Parse('2025-03-20' as date), 130),
    (Parse('2025-03-21' as date), 131),
    (Parse('2025-03-24' as date), 132),
    (Parse('2025-03-25' as date), 133),
    (Parse('2025-03-26' as date), 134),
    (Parse('2025-03-27' as date), 135),
    (Parse('2025-03-28' as date), 136),
    (Parse('2025-04-01' as date), 137),
    (Parse('2025-04-02' as date), 138),
    (Parse('2025-04-03' as date), 139),
    (Parse('2025-04-04' as date), 140),
    (Parse('2025-04-07' as date), 141),
    (Parse('2025-04-08' as date), 142),
    (Parse('2025-04-09' as date), 143),
    (Parse('2025-04-10' as date), 144),
    (Parse('2025-04-11' as date), 145),
    (Parse('2025-04-14' as date), 146),
    (Parse('2025-04-15' as date), 147),
    (Parse('2025-04-16' as date), 148),
    (Parse('2025-04-17' as date), 149),
    (Parse('2025-04-21' as date), 150),
    (Parse('2025-04-22' as date), 151),
    (Parse('2025-04-23' as date), 152),
    (Parse('2025-04-24' as date), 153),
    (Parse('2025-04-25' as date), 154),
    (Parse('2025-04-28' as date), 155),
    (Parse('2025-04-29' as date), 156),
    (Parse('2025-04-30' as date), 157),
    (Parse('2025-05-01' as date), 158),
    (Parse('2025-05-05' as date), 159),
    (Parse('2025-05-06' as date), 160),
    (Parse('2025-05-07' as date), 161),
    (Parse('2025-05-08' as date), 162),
    (Parse('2025-05-09' as date), 163),
    (Parse('2025-05-12' as date), 164),
    (Parse('2025-05-13' as date), 165),
    (Parse('2025-05-14' as date), 166),
    (Parse('2025-05-15' as date), 167),
    (Parse('2025-05-16' as date), 168),
    (Parse('2025-05-19' as date), 169),
    (Parse('2025-05-20' as date), 170),
    (Parse('2025-05-21' as date), 171),
    (Parse('2025-05-22' as date), 172),
    (Parse('2025-05-23' as date), 173),
    (Parse('2025-05-27' as date), 174),
    (Parse('2025-05-28' as date), 175),
    (Parse('2025-05-29' as date), 176),
    (Parse('2025-05-30' as date), 177),
    (Parse('2025-06-02' as date), 178),
    (Parse('2025-06-03' as date), 179),
    (Parse('2025-06-04' as date), 180)



COMMIT;

