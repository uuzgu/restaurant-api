-- First check if the column doesn't exist
SELECT CASE 
    WHEN NOT EXISTS (
        SELECT 1 
        FROM pragma_table_info('items') 
        WHERE name = 'is_available'
    ) THEN
        -- Add the column if it doesn't exist
        'ALTER TABLE items ADD COLUMN is_available INTEGER NOT NULL DEFAULT 1;'
    ELSE
        -- Do nothing if the column exists
        'SELECT 1;'
END;

-- Update all existing items to be available
UPDATE items SET is_available = 1; 