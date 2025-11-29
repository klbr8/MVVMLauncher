using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebirthLauncher.Models
{
    internal class DownloadProgress : INotifyPropertyChanged
    {
        private int _filesCompleted;
        private int _totalFiles;
        private long _totalBytesCompleted;
        private long _totalBytes;
        private int _overallProgressPercentage;
        private ObservableCollection<FileProgress> _fileProgresses = new ObservableCollection<FileProgress>();

        public int FilesCompleted
        {
            get => _filesCompleted;
            set
            {
                if (_filesCompleted != value)
                {
                    _filesCompleted = value;
                    OnPropertyChanged(nameof(FilesCompleted));
                }
            }
        }
        public int TotalFiles
        {
            get => _totalFiles;
            set
            {
                if (_totalFiles != value)
                {
                    _totalFiles = value;
                    OnPropertyChanged(nameof(TotalFiles));
                }
            }
        }
        public long TotalBytesCompleted
        {
            get => _totalBytesCompleted;
            set
            {
                if (_totalBytesCompleted != value)
                {
                    _totalBytesCompleted = value;
                    OnPropertyChanged(nameof(TotalBytesCompleted));
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
        public int OverallProgressPercentage
        {
            get => _overallProgressPercentage;
            set
            {
                if (_overallProgressPercentage != value)
                {
                    _overallProgressPercentage = value;
                    OnPropertyChanged(nameof(OverallProgressPercentage));
                }
            }
        }
        public ObservableCollection<FileProgress> FileProgresses
        {
            get => _fileProgresses;
            set
            {
                if (_fileProgresses != value)
                {
                    _fileProgresses = value;
                    OnPropertyChanged(nameof(FileProgresses));
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
