using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
	public partial class Performance : Form
	{
		#region Fields

		protected readonly ViewsManager _views;

		#endregion

		#region Ctors

		public Performance(ViewsManager views)
		{
			InitializeComponent();

			_views = views;
		}

		#endregion

		#region Events

		private void Performance_Load(object sender, EventArgs e)
		{
			try
			{
				UpdateIndexName();

				var style = new FastColoredTextBoxNS.TextStyle(Brushes.Black, Brushes.Green, FontStyle.Regular);

				//textBox1.Text = _views.MainForm.datasetMain.Documents.First().NOTE_TEXT;
				textBox1.ShowLineNumbers = false;
				textBox1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular);

				for (var i = 0; i < textBox1.LinesCount; i += 3)
				{
					var range = textBox1.GetLine(i);
					range.SetStyle(style);
				}

				textBox1.Refresh();
			}
			catch (Exception ex)
			{
				lblIndexName.Text = ex.Message;
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			try
			{
				gridResults.Rows.Clear();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnDeleteIndex_Click(object sender, EventArgs e)
		{
			try
			{
				var connection = GetConnection();

				var indexName = GetIndexName(connection);
				DeleteIndex(connection, indexName);

				UpdateIndexName();

				ShowSuccessMessage();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCreateIndex_Click(object sender, EventArgs e)
		{
			try
			{
				RunMethod(UpdateMethod.CreateIndex);

				UpdateIndexName();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSimpleUpdate_Click(object sender, EventArgs e)
		{
			try
			{
				RunMethod(UpdateMethod.SimpleUpdate);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnStoredProcedure_Click(object sender, EventArgs e)
		{
			try
			{
				RunMethod(UpdateMethod.StoredProcedure);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCreateProcedure_Click(object sender, EventArgs e)
		{
			try
			{
				CreateStoredProcedure();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation: general

		protected void RunMethod(UpdateMethod method)
		{
			var args = new MethodArgs
			           {
				           Method = method,
				           MaxRowsToUpdate = Convert.ToInt64(txtMaxRowsToUpdate.Value),
						   PrepareCommand = chkbPrepareCommand.Checked
			           };

			///////////////////////////////////////////////////////////////////////////////

			var progressForm = new FormGenericProgress(method.ToString(), PerformOperation, args, true);
			progressForm.ShowDialog();

			///////////////////////////////////////////////////////////////////////////////

			ShowResults(args);
		}

		protected void ShowResults(MethodArgs args)
		{
			var elapsedPerRecord = args.TotalElapsed.TotalMilliseconds / args.TotalRowsUpdated;

			gridResults.Rows.Add(args.Method.ToString(), args.TotalRowsUpdated.ToString(""), args.TotalElapsed.ToString(@"mm\:ss\.ff"), elapsedPerRecord.ToString("F3") + " ms");
		}

		protected bool PerformOperation(BackgroundWorker worker, object objArgument)
		{
			try
			{
				var args = (MethodArgs) objArgument;

				///////////////////////////////////////////////////////////////////////////////

				var connection = GetConnection();

				///////////////////////////////////////////////////////////////////////////////

				var table = args.Method == UpdateMethod.CreateIndex ? null : PrepareOperation(connection, args);

				var sw = Stopwatch.StartNew();

				///////////////////////////////////////////////////////////////////////////////

				switch (args.Method)
				{
					case UpdateMethod.SimpleUpdate:

						RunMethod_SimpleUpdate(connection, table, args, worker);
						break;

					case UpdateMethod.StoredProcedure:

						RunMethod_StoredProcedure(connection, table, args, worker);
						break;

					case UpdateMethod.CreateIndex:

						RunMethod_CreateIndex(connection);
						break;
				}

				///////////////////////////////////////////////////////////////////////////////

				args.TotalElapsed = new TimeSpan(sw.Elapsed.Ticks);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);

				return false;
			}

			return true;
		}

		protected DataTable PrepareOperation(OleDbConnection connection, MethodArgs args)
		{
			var cmd = new OleDbCommand(String.Format("SELECT TOP {0} ED_ENC_NUM FROM Documents", args.MaxRowsToUpdate), connection);
			var adapter = new OleDbDataAdapter(cmd);

			var table = new DataTable();

			adapter.Fill(table);

			args.MaxRowsToUpdate = args.MaxRowsToUpdate < table.Rows.Count ? args.MaxRowsToUpdate : table.Rows.Count;

			return table;
		}

		protected void UpdateIndexName()
		{
			var connection = GetConnection();
			var indexName = GetIndexName(connection);
			if (String.IsNullOrEmpty(indexName))
				lblIndexName.Text = "INDEX NOT EXIST";
			else
				lblIndexName.Text = "INDEX NAME: " + indexName;
		}

		protected bool ReportProgress(MethodArgs args, BackgroundWorker worker)
		{
			if (worker.CancellationPending)
				return false;

			///////////////////////////////////////////////////////////////////////////////

			var progressPercentage = (int) ((double) args.TotalRowsUpdated / args.MaxRowsToUpdate * 100d);

			worker.ReportProgress(progressPercentage);

			return true;
		}

		#endregion

		#region Implementation: database helpers

		protected OleDbConnection GetConnection()
		{
			var connection = _views.MainForm.adapterDocuments.Connection;
			if (connection.State != ConnectionState.Open)
				connection.Open();

			return connection;
		}

		protected string GetIndexName(OleDbConnection connection)
		{
			var table = connection.GetSchema("Indexes");
			var indexRow = table.Rows
			                    .Cast<DataRow>()
			                    .FirstOrDefault(x => x.Field<string>("TABLE_NAME") == "Documents" && x.Field<string>("COLUMN_NAME") == "ED_ENC_NUM");

			if (indexRow != null)
				return (string) indexRow["INDEX_NAME"];

			return string.Empty;
		}

		protected void DeleteIndex(OleDbConnection connection, string indexName)
		{
			var query = string.Format("DROP INDEX {0} ON Documents", indexName);

			var cmd = new OleDbCommand(query, connection);
			cmd.ExecuteNonQuery();
		}

		protected void CreateIndex(OleDbConnection connection)
		{
			var query = "CREATE INDEX INDEX_DOCUMENTS ON Documents (ED_ENC_NUM)";

			var cmd = new OleDbCommand(query, connection);
			cmd.ExecuteNonQuery();
		}

		protected void CreateStoredProcedure()
		{
			var query = "Create Procedure UpdateDocument ( ScoreVal INTEGER, IdVal DOUBLE ) AS UPDATE Documents SET Score = ScoreVal WHERE ED_ENC_NUM = IdVal";

			var cmd = new OleDbCommand(query, GetConnection());
			cmd.ExecuteNonQuery();
		}

		#endregion

		#region Implementation: methods

		protected void RunMethod_SimpleUpdate(OleDbConnection connection, DataTable table, MethodArgs args, BackgroundWorker worker)
		{
			var query = "UPDATE Documents SET Score = @Score WHERE ED_ENC_NUM = @ED_ENC_NUM";

			var cmd = new OleDbCommand(query, connection);

			cmd.Parameters.AddWithValue("@Score", 0).DbType = DbType.Int32;
			cmd.Parameters.AddWithValue("@ED_ENC_NUM", 0).DbType = DbType.Double;

			if (args.PrepareCommand)
				cmd.Prepare();

			///////////////////////////////////////////////////////////////////////////////

			foreach (var row in table.Rows.Cast<DataRow>())
			{
				var id = (double) row.ItemArray[0];

				cmd.Parameters[0].Value = Convert.ToInt32(id);
				cmd.Parameters[1].Value = id;

				///////////////////////////////////////////////////////////////////////////////

				var rowsUpdated = cmd.ExecuteNonQuery();
				if (rowsUpdated != 1)
					throw new Exception();

				///////////////////////////////////////////////////////////////////////////////

				args.TotalRowsUpdated++;
				if (args.TotalRowsUpdated >= args.MaxRowsToUpdate)
					break;

				if (args.TotalRowsUpdated % 50 == 0)
				{
					if (!ReportProgress(args, worker))
						break;
				}
			}
		}

		protected void RunMethod_StoredProcedure(OleDbConnection connection, DataTable table, MethodArgs args, BackgroundWorker worker)
		{
			var query = "UpdateDocument";

			var cmd = new OleDbCommand(query, connection);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.AddWithValue("ScoreVal", 0).DbType = DbType.Int32;
			cmd.Parameters.AddWithValue("IdVal", 0).DbType = DbType.Double;

			if (args.PrepareCommand)
				cmd.Prepare();

			///////////////////////////////////////////////////////////////////////////////

			foreach (var row in table.Rows.Cast<DataRow>())
			{
				var id = (double)row.ItemArray[0];

				cmd.Parameters[0].Value = Convert.ToInt32(id);
				cmd.Parameters[1].Value = id;

				///////////////////////////////////////////////////////////////////////////////

				var rowsUpdated = cmd.ExecuteNonQuery();
				if (rowsUpdated != 1)
					throw new Exception();

				///////////////////////////////////////////////////////////////////////////////

				args.TotalRowsUpdated++;
				if (args.TotalRowsUpdated >= args.MaxRowsToUpdate)
					break;

				if (args.TotalRowsUpdated % 50 == 0)
				{
					if (!ReportProgress(args, worker))
						break;
				}
			}
		}

		protected void RunMethod_CreateIndex(OleDbConnection connection)
		{
			CreateIndex(connection);
		}

		#endregion

		#region Helpers

		protected void ShowSuccessMessage()
		{
			MessageBox.Show("Success", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		#endregion
	}

	public class MethodArgs
	{
		#region Fields

		public UpdateMethod Method { get; set; }
		public bool PrepareCommand { get; set; }
		public long MaxRowsToUpdate { get; set; }

		public TimeSpan TotalElapsed { get; set; }
		public long TotalRowsUpdated { get; set; }

		#endregion
	}

	public enum UpdateMethod
	{
		#region Constants

		SimpleUpdate,
		StoredProcedure,
		CreateIndex

		#endregion
	}
}