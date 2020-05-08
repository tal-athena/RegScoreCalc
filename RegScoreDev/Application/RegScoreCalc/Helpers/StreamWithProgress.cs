﻿using System;
using System.IO;

namespace RegScoreCalc.Helpers
{
    public class StreamWithProgress : Stream
    {
        private readonly FileStream file;
        private readonly long length;

        public class ProgressChangedEventArgs : EventArgs
        {
            public long BytesRead;
            public long Length;

            public ProgressChangedEventArgs(long BytesRead, long Length)
            {
                this.BytesRead = BytesRead;
                this.Length = Length;
            }
        }

        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        private long bytesRead;

        public StreamWithProgress(FileStream file)
        {
            this.file = file;
            length = file.Length;
            bytesRead = 0;
            if (ProgressChanged != null) ProgressChanged(this, new ProgressChangedEventArgs(bytesRead, length));
        }

        public FileStream GetStream()
        {
            return file;
        }

        public double GetProgress()
        {
            return ((double)bytesRead) / file.Length;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush() { }

        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override long Position
        {
            get { return bytesRead; }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = file.Read(buffer, offset, count);
            bytesRead += result;
            if (ProgressChanged != null) ProgressChanged(this, new ProgressChangedEventArgs(bytesRead, length));
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            //throw new Exception("The method or operation is not implemented.");
            return file.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            //throw new Exception("The method or operation is not implemented.");
            file.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            file.Write(buffer, offset, count);
        }
    }
}
