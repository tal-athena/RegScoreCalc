using System;
using System.Data;

namespace RegExpLib
{
	internal class DataRowAdapter : IDataRecord
	{
		#region Fields

		protected DataRow _row;

		#endregion

		#region Properties

		public DataRow Row
		{
			get { return _row; }
		}

		#endregion

		#region Ctors

		public DataRowAdapter(DataRow row)
		{
			_row = row;
		}

		#endregion

		#region Implementation: IDataRecord

		public object this[string name]
		{
			get { return _row[name]; }
		}

		public object this[int i]
		{
			get { return _row[i]; }
		}

		public int FieldCount
		{
			get { return _row.Table.Columns.Count; }
		}

		public bool GetBoolean(int i)
		{
			return Convert.ToBoolean(_row[i]);
		}

		public byte GetByte(int i)
		{
			return Convert.ToByte(_row[i]);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotSupportedException("GetBytes is not supported.");
		}

		public char GetChar(int i)
		{
			return Convert.ToChar(_row[i]);
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotSupportedException("GetChars is not supported.");
		}

		public IDataReader GetData(int i)
		{
			throw new NotSupportedException("GetData is not supported.");
		}

		public string GetDataTypeName(int i)
		{
			return _row[i].GetType()
			              .Name;
		}

		public DateTime GetDateTime(int i)
		{
			return Convert.ToDateTime(_row[i]);
		}

		public decimal GetDecimal(int i)
		{
			return Convert.ToDecimal(_row[i]);
		}

		public double GetDouble(int i)
		{
			return Convert.ToDouble(_row[i]);
		}

		public Type GetFieldType(int i)
		{
			return _row[i].GetType();
		}

		public float GetFloat(int i)
		{
			return Convert.ToSingle(_row[i]);
		}

		public Guid GetGuid(int i)
		{
			return (Guid) _row[i];
		}

		public short GetInt16(int i)
		{
			return Convert.ToInt16(_row[i]);
		}

		public int GetInt32(int i)
		{
			return Convert.ToInt32(_row[i]);
		}

		public long GetInt64(int i)
		{
			return Convert.ToInt64(_row[i]);
		}

		public string GetName(int i)
		{
			return _row.Table.Columns[i].ColumnName;
		}

		public int GetOrdinal(string name)
		{
			return _row.Table.Columns.IndexOf(name);
		}

		public string GetString(int i)
		{
			return _row[i].ToString();
		}

		public object GetValue(int i)
		{
			return _row[i];
		}

		public int GetValues(object[] values)
		{
			values = _row.ItemArray;
			return _row.ItemArray.GetLength(0);
		}

		public bool IsDBNull(int i)
		{
			return Convert.IsDBNull(_row[i]);
		}

		#endregion
	}
}
