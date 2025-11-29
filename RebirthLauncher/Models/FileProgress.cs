using System.ComponentModel;

namespace RebirthLauncher.Models
{
    internal class FileProgress : INotifyPropertyChanged
    {
        private string _fileName = "";
        private long _totalBytes;
        private long _downloadedBytes;
        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }
        public long TotalBytes
        {
            get => _totalBytes;
            set
            {
                if (_totalBytes != value)
                {
                    _totalBytes = value;
                    OnPropertyChanged(nameof(TotalBytes));
                }
            }
        }
        public long DownloadedBytes
        {
            get => _downloadedBytes;
            set
            {
                if (_downloadedBytes != value)
                {
                    _downloadedBytes = value;
                    OnPropertyChanged(nameof(DownloadedBytes));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
