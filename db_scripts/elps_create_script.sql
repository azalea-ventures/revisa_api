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
    strategy NVARCHAR(MAX)
);

-- Learning Strategies Modifications
CREATE TABLE elps.learning_strategies_mods (
    id INT IDENTITY(1,1) PRIMARY KEY,
    learning_strategy_id INT NOT NULL,
    strategy NVARCHAR(MAX),
    image_file_id VARCHAR(255),
    FOREIGN KEY (learning_strategy_id) REFERENCES elps.learning_strategies(id)
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

CREATE TABLE elps.strategies_objectives (
    id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    strategy_mod_id INT NOT NULL,
    domain_objective_id INT NOT NULL,
    FOREIGN KEY (strategy_mod_id) REFERENCES elps.learning_strategies_mods(id),
    FOREIGN KEY (domain_objective_id) REFERENCES elps.domain_objectives(id)
);


-- **WIPE AND RESTART elps schema** 

-- USE revisa_db;
-- GO
-- DROP TABLE elps.strategies_objectives;
-- DROP TABLE elps.learning_strategies_mods;
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


