USE revisa_db;
GO


-- Create schema
CREATE SCHEMA elps;
GO

BEGIN TRANSACTION
-- Create tables

-- Learning Strategies
CREATE TABLE elps.learning_strategies (
    id INT IDENTITY(1,1) PRIMARY KEY,
    label VARCHAR(1),
    objective NVARCHAR(MAX)
);

-- Domains
CREATE TABLE elps.domains (
    id INT IDENTITY(1,1) PRIMARY KEY,
    domain VARCHAR(MAX),
    label VARCHAR(1)
);

-- Domain Objectives
CREATE TABLE elps.domain_objectives (
    id INT IDENTITY(1,1) PRIMARY KEY, 
    domain_id INT,
    label VARCHAR(1),
    objective NVARCHAR(MAX),
    FOREIGN KEY (domain_id) REFERENCES elps.domains (id)
);

-- Levels
CREATE TABLE elps.levels (
    id INT IDENTITY(1,1) PRIMARY KEY,
    lvl VARCHAR(32)
);

-- Grades
CREATE TABLE elps.grades (
    id INT IDENTITY(1,1) PRIMARY KEY,
    grade VARCHAR(12)
);

-- Domain Levels
CREATE TABLE elps.domain_levels (
    id INT IDENTITY(1,1) PRIMARY KEY,
    domain_id INT,
    level_id INT,
    details NVARCHAR(MAX),
    FOREIGN KEY (domain_id) REFERENCES elps.domains (id),
    FOREIGN KEY (level_id) REFERENCES elps.levels (id),
);

-- Attribute Types
CREATE TABLE elps.attr_type (
    id INT IDENTITY(1,1) PRIMARY KEY,
    typ NVARCHAR(MAX)
);

-- Domain Level Attributes
CREATE TABLE elps.domain_lvl_attr (
    id INT IDENTITY(1,1) PRIMARY KEY,
    domain_level_id INT,
    grade_id INT,
    attr VARCHAR(MAX),
    attr_type_id INT,
    FOREIGN KEY (domain_level_id) REFERENCES elps.domain_levels (id),
    FOREIGN KEY (grade_id) REFERENCES elps.grades (id),
    FOREIGN KEY (attr_type_id) REFERENCES elps.attr_type (id)
);

-- Domain Level Attribute Items
CREATE TABLE elps.domain_lvl_attr_item (
    id INT IDENTITY(1,1) PRIMARY KEY,
    domain_lvl_attr_id INT,
    item VARCHAR(MAX),
    FOREIGN KEY (domain_lvl_attr_id) REFERENCES elps.domain_lvl_attr (id)
);


-- **WIPE AND RESTART elps schema** 

-- USE revisa_db;
-- GO
-- DROP TABLE elps.learning_strategies;
-- DROP TABLE elps.domain_lvl_attr_item;
-- DROP TABLE elps.domain_lvl_attr;
-- DROP TABLE elps.attr_type;
-- DROP TABLE elps.domain_levels;
-- DROP TABLE elps.grades;
-- DROP TABLE elps.levels;
-- DROP TABLE elps.domain_objectives;
-- DROP TABLE elps.domains;
-- DROP SCHEMA elps;


