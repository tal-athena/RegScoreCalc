SELECT        %placeholder%, 

			  Categories.Category as Category,

			  DocumentColumnValues.ColumnID, DocumentColumnValues.TextValue, DocumentColumnValues.NumericValue, DocumentColumnValues.DateTimeValue,
						 
			  CategoryClasses.CategoryClassID, CategoryClasses.ClassName, CategoryClasses.Type,

			  DocumentCategory.CategoryID

FROM          ((((Documents LEFT OUTER JOIN
                         DocumentColumnValues ON Documents.ED_ENC_NUM = DocumentColumnValues.DocumentID) LEFT OUTER JOIN
                         CategoryClasses ON DocumentColumnValues.ColumnID = CategoryClasses.CategoryClassID) LEFT OUTER JOIN
                         Categories ON Documents.category = Categories.ID) LEFT OUTER JOIN
                         DocumentCategory ON Documents.ED_ENC_NUM = DocumentCategory.DocumentID)

ORDER BY      Documents.ED_ENC_NUM, DocumentColumnValues.ColumnID