using System;
using System.IO;
using System.Threading;

namespace RegScoreCalc.Code
{
	public class NotebooksWatcher : IDisposable
	{
		protected FileSystemWatcher _watcher;
		protected SynchronizationContext _syncContext;

		public event EventHandler FolderChanged;

		public NotebooksWatcher(string folder)
		{
			_watcher = new FileSystemWatcher(folder)
			{
				EnableRaisingEvents = true,
				Filter = "*.ipynb"
			};

			_watcher.Created += watcher_Changed;
			_watcher.Renamed += watcher_Changed;
			_watcher.Deleted += watcher_Changed;

			_syncContext = SynchronizationContext.Current;
		}

		private void watcher_Changed(object sender, FileSystemEventArgs e)
		{
			try
			{
				_syncContext.Post((state) =>
				                  {
					                  try
					                  {
						                  InvokeEvent_FolderChanged();
					                  }
					                  catch (Exception ex)
					                  {
										  MainForm.ShowErrorToolTip(ex.Message);
									  }
				                  }, null);
			}
			catch (Exception ex)
			{
				MainForm.ShowErrorToolTip(ex.Message);
			}
		}

		protected virtual void InvokeEvent_FolderChanged()
		{
			FolderChanged?.Invoke(this, EventArgs.Empty);
		}

		public void Dispose()
		{
			_watcher?.Dispose();
		}
	}
}
