-- Insert brand menu item into main menu if it does not exist
DO $$
BEGIN
    -- Check if Main menu exists and brands menu item doesn't exist
    IF EXISTS (
        SELECT 1 FROM "MenuRecord" m 
        WHERE m."SystemName" = 'Main'
    ) AND NOT EXISTS (
        SELECT 1 FROM "MenuItemRecord" mi 
        JOIN "MenuRecord" m ON mi."MenuId" = m."Id"
        WHERE m."SystemName" = 'Main' 
        AND mi."ProviderName" = 'brands'
    ) THEN
        INSERT INTO "MenuItemRecord" 
        ("MenuId", "ParentItemId", "ProviderName", "Title", "Published", "DisplayOrder", "BeginGroup", "ShowExpanded", "NoFollow", "NewWindow", "LimitedToStores", "SubjectToAcl")
        SELECT 
            m."Id",
            0,
            'brands',
            'Brands',
            true,
            1000,
            false,
            false,
            false,
            false,
            false,
            false
        FROM "MenuRecord" m
        WHERE m."SystemName" = 'Main';
        
        RAISE NOTICE 'Brands menu item added to Main menu successfully.';
    ELSE
        RAISE NOTICE 'Brands menu item already exists or Main menu not found.';
    END IF;
END $$;