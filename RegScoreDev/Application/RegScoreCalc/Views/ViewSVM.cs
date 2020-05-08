using RegScoreCalc.Forms;
using System;
using System.ComponentModel;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

namespace RegScoreCalc
{
    public class ViewSVM : View
    {
        #region Data members

        public PaneSVM _paneSVM;

        public TextBox notificationTextBox;
        public RibbonButton btnReviewResults;

        protected string processName;
        SVMProcessDBService.SVMServiceClient client;
        InstanceContext ins;
        ViewsManager viewsManager;

        FormGenerateSVMProgress fgSVM;
        BackgroundWorker downloadWorker;

        #endregion

        #region Ctors

        public ViewSVM(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
            : base(viewtype, strTitle, views, objArgument)
        {

            viewsManager = views;








            //ribbon = new Ribbon();
            //RibbonTab ribonTab = new RibbonTab(ribbon, "tab1");
            //RibbonPanel ribbonPanel = new RibbonPanel("pane;1");

            //RibbonButton rb = new RibbonButton("dasdas");
            //ribbonPanel.Items.Add(rb);


            //ribonTab.Panels.Add(ribbonPanel);
            //ribbon.Tabs.Add(ribonTab);

            //this._paneSVM.Controls.Add(ribbon);


        }


        #endregion

        #region Events

        protected void OnDataModified(object sender, EventArgs e)
        {
            UpdateView();
        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (client != null)
                {
                    processName = Properties.Settings.Default.ProcessName.ToString();
                    client.Disconnect(processName);
                }
            }
            catch { }
        }

        void _paneSVM_StartProcess(object sender, EventArgs e)
        {
            StartProcess();
        }

        public void StartProcess()
        {
            try
            {

                notificationTextBox.AppendText("Process at server is starting!" + Environment.NewLine + Environment.NewLine);
                processName = Properties.Settings.Default.ProcessName.ToString();

                if (!String.IsNullOrEmpty(processName))
                {
                    //Create and start process
                    StartService();

                    client.StartProcess(processName);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string reviewResultsCurrentProcess = "";
        public void ReviewResults(string process = null)
        {
            //process can have 1,2 or 3 value
            reviewResultsCurrentProcess = process;
            var name = "ReviewML" + reviewResultsCurrentProcess;
            _views.MainForm.ReviewMLOpenedFromStart = false;


            int viewIndex = viewsManager.GetViewIndex(name);
            if (viewIndex < 0)
            {
                //All ok

                downloadWorker = new BackgroundWorker();
                downloadWorker.WorkerSupportsCancellation = true;
                downloadWorker.WorkerReportsProgress = true;
                downloadWorker.RunWorkerCompleted += downloadWorker_RunWorkerCompleted;
                downloadWorker.ProgressChanged += downloadWorker_ProgressChanged;

                downloadWorker.DoWork += downloadWorker_DoWork;

                fgSVM = new FormGenerateSVMProgress();
                fgSVM.btnCancel.Enabled = false;

                downloadWorker.RunWorkerAsync();

                fgSVM.ShowDialog();
            }
            else
            {
                viewsManager.ActivateView(viewIndex);
            }


        }

        void downloadWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 100)
            {
                fgSVM.UpdateProgress(e.ProgressPercentage);
            }
            else
            {
                fgSVM.UpdateProgress(100);
            }
        }

        void downloadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (fgSVM != null)
            {
                fgSVM.Close();
            }

            if (e.Error != null)
            {
                MessageBox.Show(fgSVM, "Error while downloading file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                var name = "ReviewML" + reviewResultsCurrentProcess;
               
                int viewIndex = viewsManager.GetViewIndex(name);
                if (viewIndex < 0)
                {
                    //Open the new VIEW if it doesnt exist
                    viewsManager.AddViewType(name, typeof(ViewReviewMLOld), false, false, false);
                    viewsManager.CreateView(name, true, true);
                }
                else
                {
                    viewsManager.ActivateView(viewIndex);
                }
            }

        }

        void downloadWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            UploadFileToServer.FileTransferServiceClient client = new UploadFileToServer.FileTransferServiceClient();

            string filename = Properties.Settings.Default.ProcessName;

            // kill target file, if already exists
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "DataFromServer");

            //Check if the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            //Check if file already exists and if does delete it
            string filePath = "";
            if (filename[0] == '1' || filename[0] == '3')
            {
                filePath = Path.Combine(folderPath, "output.csv");
            }
            else if (filename[0] == '2')
            {
                filePath = Path.Combine(folderPath, "ranks.dat");
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }


            // get stream from server
            Stream inputStream = null;
            long length = 0;

            try
            {
                client.DownloadFile(ref filename, out length, out inputStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

       
            filePath = Path.Combine(folderPath, filename);

            // write server stream to disk
            using (System.IO.FileStream writeStream = new FileStream(filePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write))
            {
                int chunkSize = 2048;
                byte[] buffer = new byte[chunkSize];
           
                do
                {
                    // read bytes from input stream
                    int bytesRead = inputStream.Read(buffer, 0, chunkSize);
                    if (bytesRead == 0) break;

                    //// write bytes to output stream
                    writeStream.Write(buffer, 0, bytesRead);

                    // report progress from time to time
                    downloadWorker.ReportProgress((int)(writeStream.Position * 100 / length));

                } while (true);


                writeStream.Close();
            }

            // close service client
            inputStream.Dispose();
            client.Close();

        }

        void btnReconnect_Click(object sender, EventArgs e)
        {
            fgSVM = new FormGenerateSVMProgress();
            fgSVM.SetProgressStyle(ProgressBarStyle.Marquee);
           

            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.DoWork += (senderM, eM) =>
                {
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        var processName = Properties.Settings.Default.ProcessName.ToString();

                        if (!String.IsNullOrEmpty(processName))
                        {
                            StartService();

                            bool finished = client.Reconnect(processName);

                            if (finished)
                            {
                                _paneSVM.EnableReviewButton(processName[0].ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "No process to reconect to!", "Process don't exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch
                    {
                        MessageBox.Show(this, "Unable to reconnect to server!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                bw.RunWorkerCompleted += (senderM, em) =>
                    {
                        this.Cursor = Cursors.Default;
                        fgSVM.Close();
                        fgSVM.Dispose();
                    };


                bw.RunWorkerAsync();
                fgSVM.ShowDialog();

            };


        }



        #endregion

        #region Overrides

        protected override void InitViewCommands(RibbonPanel panel)
        {


            RibbonButton btnReconnect = new RibbonButton("Reconnect");
            panel.Items.Add(btnReconnect);
            btnReconnect.Image = Properties.Resources.reconnectStrong;
            btnReconnect.SmallImage = Properties.Resources.reconnectStrong;
            btnReconnect.Click += btnReconnect_Click;

            btnReconnect.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

        }

        protected override void InitViewPanes(RibbonTab tab)
        {
            this.Orientation = Orientation.Horizontal;


            this.SplitterWidth = 5;
            this.SplitterDistance = this.Height - 150;
            this.Panel1MinSize = 0;
            this.Panel2MinSize = 0;


            _paneSVM = new PaneSVM();
            _paneSVM._viewSVM = this;

			_paneSVM.InitPane(_views, this, this.Panel1, tab);

            this.Panel1.Controls.Add(_paneSVM);
            _paneSVM.ShowPane();

            //////////////////////////////////////////////////////////////////////////



            notificationTextBox = new TextBox();
            notificationTextBox.Height = 200;
            notificationTextBox.Multiline = true;
            notificationTextBox.Name = "txtNotification";
            notificationTextBox.Margin = new Padding(0, 2, 0, 0);
            notificationTextBox.Dock = DockStyle.Fill;
            notificationTextBox.ScrollBars = ScrollBars.Vertical;
            notificationTextBox.ReadOnly = true;
            notificationTextBox.ForeColor = System.Drawing.Color.FromArgb(163, 163, 163);
            notificationTextBox.BackColor = System.Drawing.Color.FromArgb(249, 252, 255);


            this.Panel2.Controls.Add(notificationTextBox);
            this.Panel2Collapsed = false;

            _paneSVM.notificationTextBox = notificationTextBox;

            ResetView();

        }

        public override void UpdateView()
        {

            if (_paneSVM != null)
                _paneSVM.UpdatePane();

        }

        #endregion

        #region Implementation

        private void StartService()
        {
            Application.ApplicationExit += Application_ApplicationExit;

            if (ins == null)
            {
                ins = new InstanceContext(new RegScoreCalc.SVMClient.SVMClientCallback(this));
            }
            if (client == null)
            {
                client = new SVMProcessDBService.SVMServiceClient(ins);
            }
        }

        protected void MaximizeNotesView()
        {
            this.Panel2Collapsed = true;
        }

        protected void ResetView()
        {
            this.Panel1Collapsed = false;
        }

        #endregion
    }
}
