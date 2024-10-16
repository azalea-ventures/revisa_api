USE revisa_db

-- Drop existing schema, if it exists
IF EXISTS (SELECT schema_name
FROM information_schema.schemata
WHERE schema_name = 'language_supports')
BEGIN
    -- Drop all tables in the schema
    DROP TABLE IF EXISTS language_supports.cognates
    DROP TABLE IF EXISTS language_supports.languages
    
    -- Drop the schema
    DROP SCHEMA IF EXISTS language_supports;
END
GO

-- Create new schema
CREATE SCHEMA language_supports;
GO

DROP TABLE language_supports.iclos;
DROP TABLE language_supports.content_teks_subjects;
DROP TABLE language_supports.support_packages;
DROP TABLE language_supports.lesson_schedule;


CREATE TABLE language_supports.lesson_schedule(
    id INT PRIMARY KEY IDENTITY(1,1),
    delivery_date DATE NOT NULL,
    lesson_order INT NOT NULL,
    cycle_number TINYINT NOT NULL,
    week_number TINYINT NOT NULL,
);

CREATE TABLE language_supports.support_packages
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

CREATE TABLE language_supports.languages
(
    id INT PRIMARY KEY IDENTITY(1,1),
    language_short VARCHAR(4),
    language_name VARCHAR(32) NOT NULL
);
GO

CREATE TABLE language_supports.translation_pairs
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

CREATE TABLE language_supports.elps_supports(
    lesson_schedule_id INT NOT NULL,
    strategy_objective_id INT NOT NULL, 
    grade_id INT NOT NULL,
    subject_id INT NOT NULL,
    FOREIGN KEY (lesson_schedule_id) REFERENCES language_supports.lesson_schedule(id),
    FOREIGN KEY (strategy_objective_id) REFERENCES elps.strategies_objectives(id),
    FOREIGN KEY (grade_id) REFERENCES content.grades(id),
    FOREIGN KEY (subject_id) REFERENCES content.subjects(id)
)


--sample data for testing
BEGIN TRANSACTION
INSERT INTO language_supports.lesson_schedule
    (delivery_date, lesson_order, cycle_number, week_number)
VALUES
    (Parse('2024-08-12' as date), 1, 1, 1),
    (Parse('2024-08-13' as date), 2, 1, 1),
    (Parse('2024-08-14' as date), 3, 1, 1),
    (Parse('2024-08-15' as date), 4, 1, 1),
    (Parse('2024-08-16' as date), 5, 1, 1),
    (Parse('2024-08-19' as date), 6, 1, 2),
    (Parse('2024-08-20' as date), 7, 1, 2),
    (Parse('2024-08-21' as date), 8, 1, 2),
    (Parse('2024-08-22' as date), 9, 1, 2),
    (Parse('2024-08-23' as date), 1, 1, 2),
    (Parse('2024-08-26' as date), 11, 1, 3),
    (Parse('2024-08-27' as date), 12, 1, 3),
    (Parse('2024-08-28' as date), 13, 1, 3),
    (Parse('2024-08-29' as date), 14, 1, 3),
    (Parse('2024-08-30' as date), 15, 1, 3),
    (Parse('2024-09-04' as date), 16, 1, 4),
    (Parse('2024-09-05' as date), 17, 1, 4),
    (Parse('2024-09-06' as date), 18, 1, 4),
    (Parse('2024-09-09' as date), 19, 1, 5),
    (Parse('2024-09-10' as date), 20, 1, 5),
    (Parse('2024-09-11' as date), 21, 1, 5),
    (Parse('2024-09-12' as date), 22, 1, 5),
    (Parse('2024-09-13' as date), 23, 1, 5),
    (Parse('2024-09-16' as date), 24, 1, 6),
    (Parse('2024-09-17' as date), 25, 1, 6),
    (Parse('2024-09-18' as date), 26, 1, 6),
    (Parse('2024-09-19' as date), 27, 1, 6),
    (Parse('2024-09-20' as date), 28, 1, 6),
    (Parse('2024-09-23' as date), 29, 2, 7),
    (Parse('2024-09-24' as date), 30, 2, 7),
    (Parse('2024-09-25' as date), 31, 2, 7),
    (Parse('2024-09-26' as date), 32, 2, 7),
    (Parse('2024-09-27' as date), 33, 2, 7),
    (Parse('2024-09-30' as date), 34, 2, 8),
    (Parse('2024-10-01' as date), 35, 2, 8),
    (Parse('2024-10-02' as date), 36, 2, 8),
    (Parse('2024-10-07' as date), 37, 2, 9),
    (Parse('2024-10-08' as date), 38, 2, 9),
    (Parse('2024-10-09' as date), 39, 2, 9),
    (Parse('2024-10-10' as date), 40, 2, 9),
    (Parse('2024-10-11' as date), 41, 2, 9),
    (Parse('2024-10-14' as date), 42, 2, 10),
    (Parse('2024-10-15' as date), 43, 2, 10),
    (Parse('2024-10-16' as date), 44, 2, 10),
    (Parse('2024-10-17' as date), 45, 2, 10),
    (Parse('2024-10-18' as date), 46, 2, 10),
    (Parse('2024-10-21' as date), 47, 2, 11),
    (Parse('2024-10-22' as date), 48, 2, 11),
    (Parse('2024-10-23' as date), 49, 2, 11),
    (Parse('2024-10-24' as date), 50, 2, 11),
    (Parse('2024-10-25' as date), 51, 2, 11),
    (Parse('2024-10-28' as date), 52, 2, 12),
    (Parse('2024-10-29' as date), 53, 2, 12),
    (Parse('2024-10-30' as date), 54, 2, 12),
    (Parse('2024-10-31' as date), 55, 2, 12),
    (Parse('2024-11-01' as date), 56, 2, 12),
    (Parse('2024-11-04' as date), 57, 3, 13),
    (Parse('2024-11-05' as date), 58, 3, 13),
    (Parse('2024-11-06' as date), 59, 3, 13),
    (Parse('2024-11-07' as date), 60, 3, 13),
    (Parse('2024-11-11' as date), 61, 3, 14),
    (Parse('2024-11-12' as date), 62, 3, 14),
    (Parse('2024-11-13' as date), 63, 3, 14),
    (Parse('2024-11-14' as date), 64, 3, 14),
    (Parse('2024-11-15' as date), 65, 3, 14),
    (Parse('2024-11-18' as date), 66, 3, 15),
    (Parse('2024-11-19' as date), 67, 3, 15),
    (Parse('2024-11-20' as date), 68, 3, 15),
    (Parse('2024-11-21' as date), 69, 3, 15),
    (Parse('2024-11-22' as date), 70, 3, 15),
    (Parse('2024-12-02' as date), 71, 3, 16),
    (Parse('2024-12-03' as date), 72, 3, 16),
    (Parse('2024-12-04' as date), 73, 3, 16),
    (Parse('2024-12-05' as date), 74, 3, 16),
    (Parse('2024-12-06' as date), 75, 3, 16),
    (Parse('2024-12-09' as date), 76, 3, 17),
    (Parse('2024-12-10' as date), 77, 3, 17),
    (Parse('2024-12-11' as date), 78, 3, 17),
    (Parse('2024-12-12' as date), 79, 3, 17),
    (Parse('2024-12-13' as date), 80, 3, 17),
    (Parse('2024-12-16' as date), 81, 3, 18),
    (Parse('2024-12-17' as date), 82, 3, 18),
    (Parse('2024-12-18' as date), 83, 3, 18),
    (Parse('2024-12-19' as date), 84, 3, 18),
    (Parse('2024-12-20' as date), 85, 3, 18),
    (Parse('2025-01-07' as date), 86, 4, 19),
    (Parse('2025-01-08' as date), 87, 4, 19),
    (Parse('2025-01-09' as date), 88, 4, 19),
    (Parse('2025-01-10' as date), 89, 4, 19),
    (Parse('2025-01-13' as date), 90, 4, 20),
    (Parse('2025-01-14' as date), 91, 4, 21),
    (Parse('2025-01-15' as date), 92, 4, 21),
    (Parse('2025-01-16' as date), 93, 4, 21),
    (Parse('2025-01-17' as date), 94, 4, 21),
    (Parse('2025-01-21' as date), 95, 4, 22),
    (Parse('2025-01-22' as date), 96, 4, 22),
    (Parse('2025-01-23' as date), 97, 4, 22),
    (Parse('2025-01-24' as date), 98, 4, 22),
    (Parse('2025-01-27' as date), 99, 4, 23),
    (Parse('2025-01-28' as date), 100, 4, 23),
    (Parse('2025-01-29' as date), 101, 4, 23),
    (Parse('2025-01-30' as date), 102, 4, 23),
    (Parse('2025-01-31' as date), 103, 4, 23),
    (Parse('2025-02-03' as date), 104, 4, 24),
    (Parse('2025-02-04' as date), 105, 4, 24),
    (Parse('2025-02-05' as date), 106, 4, 24),
    (Parse('2025-02-06' as date), 107, 4, 24),
    (Parse('2025-02-07' as date), 108, 4, 24),
    (Parse('2025-02-10' as date), 109, 4, 25),
    (Parse('2025-02-11' as date), 110, 4, 25),
    (Parse('2025-02-12' as date), 111, 4, 25),
    (Parse('2025-02-13' as date), 112, 4, 25),
    (Parse('2025-02-18' as date), 113, 4, 26),
    (Parse('2025-02-19' as date), 114, 4, 26),
    (Parse('2025-02-20' as date), 116, 4, 26),
    (Parse('2025-02-21' as date), 117, 4, 26),
    (Parse('2025-02-24' as date), 117, 5, 27),
    (Parse('2025-02-25' as date), 118, 5, 27),
    (Parse('2025-02-26' as date), 119, 5, 27),
    (Parse('2025-02-27' as date), 120, 5, 27),
    (Parse('2025-02-28' as date), 121, 5, 27),
    (Parse('2025-03-03' as date), 122, 5, 28),
    (Parse('2025-03-04' as date), 123, 5, 28),
    (Parse('2025-03-05' as date), 124, 5, 28),
    (Parse('2025-03-06' as date), 125, 5, 28),
    (Parse('2025-03-07' as date), 126, 5, 28),
    (Parse('2025-03-17' as date), 127, 5, 29),
    (Parse('2025-03-18' as date), 128, 5, 29),
    (Parse('2025-03-19' as date), 129, 5, 29),
    (Parse('2025-03-20' as date), 130, 5, 29),
    (Parse('2025-03-21' as date), 131, 5, 29),
    (Parse('2025-03-24' as date), 132, 5, 30),
    (Parse('2025-03-25' as date), 133, 5, 30),
    (Parse('2025-03-26' as date), 134, 5, 30),
    (Parse('2025-03-27' as date), 135, 5, 30),
    (Parse('2025-03-28' as date), 136, 5, 30),
    (Parse('2025-04-01' as date), 137, 5, 31),
    (Parse('2025-04-02' as date), 138, 5, 31),
    (Parse('2025-04-03' as date), 139, 5, 31),
    (Parse('2025-04-04' as date), 140, 5, 31),
    (Parse('2025-04-07' as date), 141, 5, 32),
    (Parse('2025-04-08' as date), 142, 5, 32),
    (Parse('2025-04-09' as date), 143, 5, 32),
    (Parse('2025-04-10' as date), 144, 5, 32),
    (Parse('2025-04-11' as date), 145, 5, 32),
    (Parse('2025-04-14' as date), 146, 5, 33),
    (Parse('2025-04-15' as date), 147, 5, 33),
    (Parse('2025-04-16' as date), 148, 5, 33),
    (Parse('2025-04-17' as date), 149, 5, 33),
    (Parse('2025-04-21' as date), 150, 6, 34),
    (Parse('2025-04-22' as date), 151, 6, 34),
    (Parse('2025-04-23' as date), 152, 6, 34),
    (Parse('2025-04-24' as date), 153, 6, 34),
    (Parse('2025-04-25' as date), 154, 6, 34),
    (Parse('2025-04-28' as date), 155, 6, 35),
    (Parse('2025-04-29' as date), 156, 6, 35),
    (Parse('2025-04-30' as date), 157, 6, 35),
    (Parse('2025-05-01' as date), 158, 6, 35),
    (Parse('2025-05-05' as date), 159, 6, 36),
    (Parse('2025-05-06' as date), 160, 6, 36),
    (Parse('2025-05-07' as date), 161, 6, 36),
    (Parse('2025-05-08' as date), 162, 6, 36),
    (Parse('2025-05-09' as date), 163, 6, 36),
    (Parse('2025-05-12' as date), 164, 6, 37),
    (Parse('2025-05-13' as date), 165, 6, 37),
    (Parse('2025-05-14' as date), 166, 6, 37),
    (Parse('2025-05-15' as date), 167, 6, 37),
    (Parse('2025-05-16' as date), 168, 6, 37),
    (Parse('2025-05-19' as date), 169, 6, 38),
    (Parse('2025-05-20' as date), 170, 6, 38),
    (Parse('2025-05-21' as date), 171, 6, 38),
    (Parse('2025-05-22' as date), 172, 6, 38),
    (Parse('2025-05-23' as date), 173, 6, 38),
    (Parse('2025-05-27' as date), 174, 6, 39),
    (Parse('2025-05-28' as date), 175, 6, 39),
    (Parse('2025-05-29' as date), 176, 6, 39),
    (Parse('2025-05-30' as date), 177, 6, 39),
    (Parse('2025-06-02' as date), 178, 6, 40),
    (Parse('2025-06-03' as date), 179, 6, 40),
    (Parse('2025-06-04' as date), 180, 6, 40)

INSERT INTO language_supports.support_packages (content_language_id, grade_id, subject_id, lesson_schedule_id)
    SELECT
        cd.language_id, 
        cd.grade_id, 
        cd.subject_id, 
        (select ls.id
        from language_supports.lesson_schedule ls
        where ls.delivery_date = cd.delivery_date)
    FROM content.content_details cd




COMMIT;

