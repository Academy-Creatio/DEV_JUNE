UPDATE "AdminUnitFeatureState" 
    SET "FeatureState" = 1 
WHERE "FeatureId" = ( 
    SELECT 
        "Id" 
    FROM "Feature" 
    WHERE "Code" = 'OAuth20Integration')