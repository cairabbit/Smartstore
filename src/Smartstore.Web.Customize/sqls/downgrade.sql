-- Delete brand menu item from main menu if it exists
DO $$
DECLARE
    deleted_count INTEGER;
BEGIN
    -- Check if brands menu item exists before attempting to delete
    IF EXISTS (
        SELECT 1 FROM "MenuItemRecord" 
        WHERE "ProviderName" = 'brands'
    ) THEN
        DELETE FROM "MenuItemRecord" 
        WHERE "ProviderName" = 'brands';
        
        GET DIAGNOSTICS deleted_count = ROW_COUNT;
        RAISE NOTICE 'Deleted % brands menu item(s) successfully.', deleted_count;
    ELSE
        RAISE NOTICE 'No brands menu items found to delete.';
    END IF;
END $$;