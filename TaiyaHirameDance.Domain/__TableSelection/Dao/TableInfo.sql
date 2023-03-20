SELECT
    S.schema_id AS SchemaId
  , T.object_id AS TableId
  , S.name      AS SchemaName
  , T.name      AS TableName
  , E.value     AS TableComment
  , P.rows      AS TableRowCount 
FROM
  sys.tables T 
  INNER JOIN sys.schemas S 
    ON T.schema_id = S.schema_id 
  LEFT JOIN sys.extended_properties E 
    ON T.object_id = E.major_id 
    AND E.minor_id = 0 
  INNER JOIN sys.partitions P 
    ON T.object_id = P.object_id 
    AND P.index_id < 2 
ORDER BY
  S.schema_id
  , T.name
