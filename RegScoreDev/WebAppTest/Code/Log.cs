using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.IO;
using WebAppTest.Forms;

namespace WebAppTest
{
    public class TaskCompletionStatus
    {
        private TaskCompletionStatus(string value) { Value = value; }

        public string Value { get; set; }

        public static TaskCompletionStatus Passed { get { return new TaskCompletionStatus("Passed"); } }
        public static TaskCompletionStatus Failed { get { return new TaskCompletionStatus("Failed"); } }
        public static TaskCompletionStatus Warning { get { return new TaskCompletionStatus("Warning"); } }
        public static TaskCompletionStatus Error { get { return new TaskCompletionStatus("Error"); } }

        public Color color
        {
            get
            {
                switch (Value)
                {
                    case "Passed":
                        return Color.Green;
                    default:
                        return Color.Red;
                }
                    
            }
        }
    }
    public class LogTask
    {
        static string seperator = "\t";
        public string TaskName { get; set; }
        public TaskCompletionStatus status { get; set; }
        public string message { get; set; }
        public TimeSpan TimeNetto { get; set; }

        public string LogLine
        {
            get
            {
                StringBuilder sb = new StringBuilder(System.DateTime.Now.ToString("HH:mm:ss"), 128);  
                sb.Append(seperator);
                sb.Append(TimeNetto.ToString(@"hh\:mm\:ss"));
                sb.Append(seperator);
                sb.Append(TaskName);
                sb.Append(seperator);
                sb.Append(status.Value);
                sb.Append(seperator);
                sb.Append(message);
                return sb.ToString();
            }
        }

        
    }
    static class Log
    {
        private static RichTextBox txtLog;
        private static MainForm mainForm;
        private static LogFile logFile;

        public static void Reset(string logDir)
        {
            Close();
            logFile = new LogFile(logDir);
        }

        public static void Close()
        {
            if (null != logFile)
                logFile.Dispose();
        }

        public static void Initialize(RichTextBox textBox, MainForm MainForm)
        {
            txtLog = textBox;
            mainForm = MainForm;
        }

        delegate void WriteLogCallback(string text, Color color, bool addTimeStamp);

        public static void WriteLog(LogTask logMsg)
        {
            WriteLogEx(logMsg.LogLine, logMsg.status.color);
            if (null != logFile)
            {
                logFile.writeLine(logMsg.LogLine);
            }
        }

        public static void WriteLog(string text, Color color)
        {
            WriteLogEx(text, color);
        }

        public static void WriteLogEx(string text, Color color, bool addTimeStamp = false)
        {
            if (txtLog.InvokeRequired)
            {
                WriteLogCallback d = new WriteLogCallback(WriteLogEx);
                mainForm.Invoke(d, new object[] { text, color, addTimeStamp });
                return;
            }
	        try
	        {
                Int32 maxsize = 102400;
                Int32 dropsize = maxsize / 2;

                if (txtLog.Text.Length > maxsize)
                {
                    
                    Int32 endmarker = txtLog.Text.IndexOf('\n', dropsize) + 1;
                    if (endmarker < dropsize)
                        endmarker = dropsize;

                    txtLog.Select(0, endmarker);
                    txtLog.Cut();
                }

                
                
                txtLog.SelectionStart = txtLog.Text.Length;
                txtLog.SelectionLength = 0;
                txtLog.SelectionColor = color;
                txtLog.AppendText(
                    (addTimeStamp? (System.DateTime.Now.ToString("HH:mm:ss") + "\t") : string.Empty) + text);
               
				txtLog.AppendText(Environment.NewLine);

				//Scroll to bottom
				txtLog.SelectionStart = txtLog.Text.Length;
				txtLog.ScrollToCaret();
			}
	        catch (Exception ex)
	        {
		        MessageBox.Show(ex.Message, "Exception");
	        }
        }

        public static void Dump(string fname)
        {
            string log_file = System.Environment.CurrentDirectory + "\\" + "WebAppTestLog_.txt".AppendTimeStamp();
            txtLog.SaveFile(log_file, RichTextBoxStreamType.PlainText);
        }

    }

    public class LogFile : IDisposable
    {
        protected string logFilePath { get; set; }

        protected StreamWriter sw;

        public LogFile()
        {
            InitDefaultLog();
        }

        public LogFile(string dirPath)
        {
            if (!String.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
                throw new Exception("Log file directory: " + dirPath + " does not exist");
            if (String.IsNullOrEmpty(dirPath))
                InitDefaultLog();
            else
                logFilePath = dirPath + "\\" + "WebAppTestLog_.txt".AppendTimeStamp();
        }

        protected void InitDefaultLog()
        {
            logFilePath = System.Environment.CurrentDirectory + "\\" + "WebAppTestLog_.txt".AppendTimeStamp();
        }

        public void writeLine(string line)
        {
            if (sw == null)
            {
                sw = new StreamWriter(logFilePath);
            }
            sw.WriteLineAsync(line);
            sw.FlushAsync();
        }

        public void Dispose()
        {
            if (sw != null)
            { 
                sw.Flush();
                sw.Close();
            }
            sw = null;
        }
    }
}
