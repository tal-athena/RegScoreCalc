CREATE TABLE CategoryClasses
(
    [CategoryClassID] AUTOINCREMENT PRIMARY KEY,
    [ClassName] TEXT(255)
);
CREATE TABLE CategoryClassCategory
(
    [CategoryID] AUTOINCREMENT PRIMARY KEY,
    [CategoryClassID] INTEGER,
    [CategoryName] TEXT(255),
    [CategoryNumber] INTEGER
);