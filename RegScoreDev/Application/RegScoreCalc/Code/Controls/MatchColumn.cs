using System;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public class MatchColumn : DataGridViewColumn
	{
		public MatchColumn()
			: base(new MatchCell())
		{
		}

		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				if (value != null &&
					!value.GetType().IsAssignableFrom(typeof(MatchColumn)))
				{
					throw new InvalidCastException("Must be a MatchColumn");
				}
				base.CellTemplate = value;
			}
		}
	}

	public class MatchCell : DataGridViewTextBoxCell
	{

		public MatchCell()
			: base()
		{
			
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

			MatchEditingControl ctl = DataGridView.EditingControl as MatchEditingControl;

			
		}

		public override Type EditType
		{
			get
			{
				return typeof(MatchEditingControl);
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(string);
			}
		}

		public override object DefaultNewRowValue
		{
			get
			{
				return null;
			}
		}
	}
}
