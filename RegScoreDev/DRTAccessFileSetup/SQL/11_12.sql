CREATE TABLE ColRegExp
(
    [ID] AUTOINCREMENT PRIMARY KEY,
	[RegExp] TEXT,
	[Extract] MEMO,
	[Match] TEXT,
	[RegExpColor] INTEGER,
	[lookbehind] MEMO,
	[neg lookbehind] MEMO,
	[lookahead] MEMO,
	[neg lookahead] MEMO,
	[Exceptions] MEMO,
	[ColumnID] INTEGER
);