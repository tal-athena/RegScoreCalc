CREATE TABLE Entities
(
    [ID] AUTOINCREMENT PRIMARY KEY,	
	[Entity] MEMO,
	[EntityColor] INTEGER,
	[Description] MEMO,
	[TotalMatches] INTEGER,
	[TotalDocuments] INTEGER,
	[TotalRecords] INTEGER,
	[TotalCategorized] INTEGER
);

ALTER TABLE Documents ADD COLUMN [NOTE_ENTITIES] MEMO;