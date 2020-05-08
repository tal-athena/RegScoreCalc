CREATE TABLE DocumentColumnValues
(
    [ID] AUTOINCREMENT PRIMARY KEY,
    [DocumentID] DOUBLE,
    [ColumnID] INTEGER,
    [TextValue] TEXT,
    [NumericValue] DOUBLE,
    [DateTimeValue] DATETIME
);
ALTER TABLE CategoryClasses ADD COLUMN [Type] INTEGER;
ALTER TABLE CategoryClasses ADD COLUMN [Order] INTEGER;
ALTER TABLE CategoryClasses ADD COLUMN [Properties] TEXT;
UPDATE CategoryClasses SET [Type] = 0;
UPDATE CategoryClasses SET [Order] = 0;
UPDATE CategoryClasses SET [Properties] = '';