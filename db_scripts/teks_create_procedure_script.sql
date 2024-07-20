
CREATE PROCEDURE teks.GetTeksByChapter @TeksChapter NVARCHAR(max) AS 
BEGIN
    SET NOCOUNT ON
    DROP TABLE IF EXISTS #teks_temp
    CREATE TABLE #teks_temp
    (
        id UNIQUEIDENTIFIER NOT NULL,
        parent_id UNIQUEIDENTIFIER,
        human_coding_scheme NVARCHAR(max),
        min_code NVARCHAR(max),   
        list_enumeration INT,
        full_statement NVARCHAR(max),
        generation_number INT
    );

    --recursively group TEKS by parent_id
    WITH generation as (
        select 
            id,
            parent_id,
            human_coding_scheme,
            full_statement,
            list_enumeration,
            item_type_id,
            0 as generation_number
        from teks.teks_items
        where parent_id is null
    UNION ALL
        select 
            ti.id,
            ti.parent_id,
            ti.human_coding_scheme,
            ti.full_statement,
            ti.list_enumeration,
            ti.item_type_id,
            generation_number+1 as generation_number
        from teks.teks_items ti
        join generation g on g.id = ti.parent_id
    ) 
    --populate temp table for next step
    INSERT INTO #teks_temp 
    select 
        g.id, 
        g.parent_id, 
        g.human_coding_scheme, 
        REPLACE(g.human_coding_scheme, @TeksChapter, '') as min_code,
        g.list_enumeration, 
        g.full_statement, 
        g.generation_number
    from generation g
    join teks.teks_item_types typ on g.item_type_id = typ.id
    where g.human_coding_scheme like @TeksChapter+'%'
    and typ.title <> 'Introduction'
    order by g.human_coding_scheme, g.list_enumeration;

    --new temp table with split teks admin code
    DROP TABLE IF EXISTS #teks_temp_split_code
    CREATE TABLE #teks_temp_split_code
    (
        id UNIQUEIDENTIFIER NOT NULL,
        parent_id UNIQUEIDENTIFIER,
        human_coding_scheme NVARCHAR(max),
        code1 INT,
        code2 NVARCHAR(max),
        code3 NVARCHAR(max),
        list_enumeration INT,
        full_statement NVARCHAR(max),
        generation_number INT
    )

    INSERT INTO #teks_temp_split_code
        select 
            t.id, 
            t.parent_id, 
            t.human_coding_scheme, 
            CAST((select value from STRING_SPLIT(t.min_code, '.', 1) where ordinal = 2) AS INT),
            (select value from STRING_SPLIT(t.min_code, '.', 1) where ordinal = 3),
            (select value from STRING_SPLIT(t.min_code, '.', 1) where ordinal = 4),
            t.list_enumeration, 
            t.full_statement, 
            t.generation_number
        from #teks_temp t

    -- return sorted and filtered TEKS
    select tc.human_coding_scheme as 'TAC Code', tc.full_statement as 'TEK' from #teks_temp_split_code tc
    order by tc.code1, tc.list_enumeration
END;

EXEC teks.GetTeksByChapter N'128.3.b';

--DROP PROCEDURE teks.GetTeksByChapter



CREATE PROCEDURE teks.GetNonMatchingTeksByChapter 
    @TeksChapter1 NVARCHAR(MAX),
    @TeksChapter2 NVARCHAR(MAX)
AS 
BEGIN
    SET NOCOUNT ON;
    
    -- Drop temporary tables if they exist
    DROP TABLE IF EXISTS #teks_temp1;
    DROP TABLE IF EXISTS #teks_temp2;
    DROP TABLE IF EXISTS #teks_temp_split_code1;
    DROP TABLE IF EXISTS #teks_temp_split_code2;

    -- Create temporary tables for each TEKS Chapter
    CREATE TABLE #teks_temp1
    (
        id UNIQUEIDENTIFIER NOT NULL,
        parent_id UNIQUEIDENTIFIER,
        human_coding_scheme NVARCHAR(MAX),
        min_code NVARCHAR(MAX),   
        list_enumeration INT,
        full_statement NVARCHAR(MAX),
        generation_number INT
    );

    CREATE TABLE #teks_temp2
    (
        id UNIQUEIDENTIFIER NOT NULL,
        parent_id UNIQUEIDENTIFIER,
        human_coding_scheme NVARCHAR(MAX),
        min_code NVARCHAR(MAX),   
        list_enumeration INT,
        full_statement NVARCHAR(MAX),
        generation_number INT
    );

    -- Create temporary tables with split TEKS admin codes
    CREATE TABLE #teks_temp_split_code1
    (
        id UNIQUEIDENTIFIER NOT NULL,
        parent_id UNIQUEIDENTIFIER,
        human_coding_scheme NVARCHAR(MAX),
        code1 INT,
        code2 NVARCHAR(MAX),
        code3 NVARCHAR(MAX),
        list_enumeration INT,
        full_statement NVARCHAR(MAX),
        generation_number INT
    );

    CREATE TABLE #teks_temp_split_code2
    (
        id UNIQUEIDENTIFIER NOT NULL,
        parent_id UNIQUEIDENTIFIER,
        human_coding_scheme NVARCHAR(MAX),
        code1 INT,
        code2 NVARCHAR(MAX),
        code3 NVARCHAR(MAX),
        list_enumeration INT,
        full_statement NVARCHAR(MAX),
        generation_number INT
    );

    -- Recursively group TEKS by parent_id for the first chapter
    WITH generation1 AS (
        SELECT 
            id,
            parent_id,
            human_coding_scheme,
            full_statement,
            list_enumeration,
            item_type_id,
            0 AS generation_number
        FROM teks.teks_items
        WHERE parent_id IS NULL
    UNION ALL
        SELECT 
            ti.id,
            ti.parent_id,
            ti.human_coding_scheme,
            ti.full_statement,
            ti.list_enumeration,
            ti.item_type_id,
            generation_number + 1 AS generation_number
        FROM teks.teks_items ti
        JOIN generation1 g ON g.id = ti.parent_id
    ) 
    -- Populate temp table for the first chapter
    INSERT INTO #teks_temp1 
    SELECT 
        g.id, 
        g.parent_id, 
        g.human_coding_scheme, 
        REPLACE(g.human_coding_scheme, @TeksChapter1, '') AS min_code,
        g.list_enumeration, 
        g.full_statement, 
        g.generation_number
    FROM generation1 g
    JOIN teks.teks_item_types typ ON g.item_type_id = typ.id
    WHERE g.human_coding_scheme LIKE @TeksChapter1 + '%'
    AND typ.title <> 'Introduction'
    ORDER BY g.human_coding_scheme, g.list_enumeration;

    -- Populate split code table for the first chapter
    INSERT INTO #teks_temp_split_code1
    SELECT 
        t.id, 
        t.parent_id, 
        t.human_coding_scheme, 
        CAST((SELECT value FROM STRING_SPLIT(t.min_code, '.', 1) WHERE ordinal = 2) AS INT),
        (SELECT value FROM STRING_SPLIT(t.min_code, '.', 1) WHERE ordinal = 3),
        (SELECT value FROM STRING_SPLIT(t.min_code, '.', 1) WHERE ordinal = 4),
        t.list_enumeration, 
        t.full_statement, 
        t.generation_number
    FROM #teks_temp1 t;

    -- Recursively group TEKS by parent_id for the second chapter
    WITH generation2 AS (
        SELECT 
            id,
            parent_id,
            human_coding_scheme,
            full_statement,
            list_enumeration,
            item_type_id,
            0 AS generation_number
        FROM teks.teks_items
        WHERE parent_id IS NULL
    UNION ALL
        SELECT 
            ti.id,
            ti.parent_id,
            ti.human_coding_scheme,
            ti.full_statement,
            ti.list_enumeration,
            ti.item_type_id,
            generation_number + 1 AS generation_number
        FROM teks.teks_items ti
        JOIN generation2 g ON g.id = ti.parent_id
    ) 
    -- Populate temp table for the second chapter
    INSERT INTO #teks_temp2 
    SELECT 
        g.id, 
        g.parent_id, 
        g.human_coding_scheme, 
        REPLACE(g.human_coding_scheme, @TeksChapter2, '') AS min_code,
        g.list_enumeration, 
        g.full_statement, 
        g.generation_number
    FROM generation2 g
    JOIN teks.teks_item_types typ ON g.item_type_id = typ.id
    WHERE g.human_coding_scheme LIKE @TeksChapter2 + '%'
    AND typ.title <> 'Introduction'
    ORDER BY g.human_coding_scheme, g.list_enumeration;

    -- Populate split code table for the second chapter
    INSERT INTO #teks_temp_split_code2
    SELECT 
        t.id, 
        t.parent_id, 
        t.human_coding_scheme, 
        CAST((SELECT value FROM STRING_SPLIT(t.min_code, '.', 1) WHERE ordinal = 2) AS INT),
        (SELECT value FROM STRING_SPLIT(t.min_code, '.', 1) WHERE ordinal = 3),
        (SELECT value FROM STRING_SPLIT(t.min_code, '.', 1) WHERE ordinal = 4),
        t.list_enumeration, 
        t.full_statement, 
        t.generation_number
    FROM #teks_temp2 t;

    -- Calculate similarity and return non-matching TEKS
    SELECT 
        tc1.human_coding_scheme AS 'TAC Code', 
        tc1.full_statement AS 'TEK' 
    FROM #teks_temp_split_code1 tc1
    LEFT JOIN #teks_temp_split_code2 tc2 
    ON tc1.human_coding_scheme = tc2.human_coding_scheme
    WHERE dbo.Levenshtein(tc1.full_statement, tc2.full_statement) > 0.5 * LEN(tc1.full_statement)

    UNION

    SELECT 
        tc2.human_coding_scheme AS 'TAC Code', 
        tc2.full_statement AS 'TEK' 
    FROM #teks_temp_split_code2 tc2
    LEFT JOIN #teks_temp_split_code1 tc1 
    ON tc2.human_coding_scheme = tc1.human_coding_scheme
    WHERE dbo.Levenshtein(tc2.full_statement, tc1.full_statement) > 0.5 * LEN(tc2.full_statement);
END;



