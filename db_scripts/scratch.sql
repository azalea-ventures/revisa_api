CREATE TABLE content.content_file
(
    id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    file_name NVARCHAR(MAX),
    file_id NVARCHAR(MAX),
    source_file_id NVARCHAR(MAX),
    current_folder_id NVARCHAR(MAX),
    outbound_file_id NVARCHAR(MAX),
    outbound_folder_id NVARCHAR(MAX),
    outbound_path NVARCHAR(MAX),
    created_at DATETIME
);
DROP TABLE content.content_file;
ALTER TABLE content.content_details
ADD CONSTRAINT FK_content_d_file
FOREIGN KEY (file_id) REFERENCES content.content_file(id);

DELETE FROM content.content_txt;
DELETE FROM content.content_teks;
DELETE FROM content.content_group;
DELETE FROM content.content_versions;
DELETE FROM content.content_details;
DELETE FROM content.content_file;

CREATE TABLE content.content_status
(
    id INT PRIMARY KEY NOT NULL,
    status VARCHAR(12)
)

INSERT INTO content.content_status
    (id, status)
VALUES
    (0, 'NONE'),
    (1, 'INBOUND'),
    (2, 'PROCESSING'),
    (3, 'OUTBOUND'),
    (4, 'ARCHIVED');


ALTER TABLE content.content_details
ADD status_id INT DEFAULT 0 REFERENCES content.content_status(id);


--SWAP THE ELPS STRATEGY FILE IDS
-- SELECT *
-- FROM [elps].[learning_strategies_mods] lsm
--     join elps.learning_strategies lm on lsm.learning_strategy_id = lm.id;

-- alter table elps.learning_strategies_mods 
-- add strategy_file_id varchar(255);

-- update elps.learning_strategies_mods 
-- set strategy_file_id = case id 
-- when 20 then '1K4t5Iwb-u491gAw_6-Zjr3hqRiMZYhqAycRqXiyc_pY'
-- when 19 then '1VgGhUUSDehYPDjp09cBT02j66Zj8EpEeod9dMFMQmZ8'
-- when 18 then '1ka7aB0HVeb_TZRSV3FSTbiwQKKvMVWUGGQhcelwpJyo'
-- when 17 then '1K6rtrcX6fTJjoAX40e7lgBHiAu-HF2UroAH0_Eki-o0'
-- when 16 then '1dQ-XECXnFO_bzpnOJG1hG3BpkXN2mzATjGlv53HO6Hs'
-- when 15 then '1ZQE0R7ulVlnhPGBJHHGd_ISMA7jiFwC-z93q3Ps3QYk'
-- when 14 then '1V7H0ikM9BmTpAaRRAKcai1YLJzL6TPaRYDo-0LeUJn8'
-- when 13 then '14QU45fO3w6uX0VHX1HVBA4sl9IXxxdDrMo-TcF65YB4'
-- when 12 then '1UdAwMhsG4dL8mJP84n4DLlas2gkjFhOGyUuZTrmn918'
-- when 11 then '1JYv0SL5uAvFzt5drllO5PskJRAfVeI0262WnZwtXV60'
-- when 10 then '1IZ2KG64cdQ9J44VgHDsEiUqBOJxbSjoyDWt2P-H6Fuc'
-- when 9 then '1hhQJ8-xRHITUdAGaN-yyv5E8gPRebhFOgv71g_IrXqU'
-- when 8 then '1wFQD3zLbdAROE5pL0Ly4S7Chw4UdFZH_Uz6xwiRQjaA'
-- when 7 then '16YdjmsBNw0bMqlalrd9zjKcIohqMj3OOyrsuWpvE3_4'
-- when 6 then '1VzD_QCzjIei-YmI9NAXXMwia6M1T8X5fTbIwCeyLCFg'
-- when 5 then '1vX5V_887dnOZhHKqs8T_teBXnKf6m6JsbGfoMH4h-OA'
-- when 4 then '1LE3Br0HIo5IG2xxlztPmDpYcYBI6vJYtWud0G5c0L4g'
-- when 3 then '1nOp9vmKpNpCwEZ0JP2skWd_NJQMgAqtH9N1sYP0_bGM'
-- when 2 then '1eKmYm9WQH8_FvwbiK3HINdm7AEsH2oIIct55Tq5ZSdw'
-- when 1 then '1VVV8LVfbEGunidx92b7jibWSI3IurPJZ3nZLQQ3WCkQ'
-- end

-- insert into elps.learning_strategies_mods (learning_strategy_id, strategy)
-- values (3, 'use strategic learning techniques such as memorizing to acquire basic and grade-level vocabulary')



-- INSERT INTO language_supports.support_packages
--     (
--     content_language_id,
--     grade_id,
--     subject_id,
--     lesson_schedule_id,
--     is_active,
--     elps_strategy_objective_id,
--     cross_linguistic_connection
--     )
  
-- SELECT 
--     cd.language_id, 
--     cd.grade_id, 
--     cd.subject_id, 
--     (
--         SELECT TOP 1 ls.id
--         FROM language_supports.lesson_schedule ls
--         WHERE ls.delivery_date = cd.delivery_date
--     ), 
--     1, 
--     null, 
--     null
-- FROM content.content_details cd;


