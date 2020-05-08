ALTER TABLE Categories ADD COLUMN [Color] INTEGER;

CREATE TABLE Columns
(
    [ID] AUTOINCREMENT PRIMARY KEY,
    [Name] TEXT,
    [Order] INTEGER,
	[IsVisible] YESNO
);
