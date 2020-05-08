DROP TABLE DocumentCategory;

DROP TABLE DocumentColumnValues;

DROP TABLE CategoryClasses;

DROP TABLE CategoryClassCategory;

CREATE TABLE DynamicColumns
(
	[ID] AUTOINCREMENT PRIMARY KEY,
	[Title] TEXT(255),
	[Type] INTEGER,
	[Order] INTEGER,
	[Properties] MEMO
);

CREATE TABLE DynamicColumnCategories
(
	[ID] AUTOINCREMENT PRIMARY KEY,
	[DynamicColumnID] INTEGER,
	[Title] TEXT(255),
	[Number] INTEGER
);

DELETE FROM ColRegExp;

ALTER TABLE Categories ADD COLUMN [IsSelected] YESNO;
UPDATE Categories SET [IsSelected] = 1;