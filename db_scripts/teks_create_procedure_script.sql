
DROP PROCEDURE teks.GetTeksByChapter;

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

-- EXEC teks.GetTeksByChapter N'128.3.b';