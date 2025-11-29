#nullable enable
// RebirthLauncher/ViewModels/InitializingDialogViewModel.cs
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RebirthLauncher.ViewModels
{
    /// <summary>
    /// ViewModel for the initializing dialog that collects the install path.
    /// The actual folder picker is provided by the view or bootstrapper via <see cref="FolderPicker"/>.
    /// </summary>
    public class InitializingDialogViewModel : INotifyPropertyChanged
    {
        private string _installPath = string.Empty;

        /// <summary>
        /// The chosen install path. Bind this to the TextBox in the dialog.
        /// </summary>
        public string InstallPath
        {
            get => _installPath;
            set
            {
                if (value == _installPath) return;
                _installPath = value ?? string.Empty;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanOk));
                _okCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// True when InstallPath is non-empty and appears valid for enabling OK.
        /// </summary>
        public bool CanOk => !string.IsNullOrWhiteSpace(InstallPath);

        /// <summary>
        /// Optional delegate the view or bootstrapper should set to show a folder picker and return the selected path.
        /// If null, BrowseCommand will do nothing; wiring this keeps the VM UI-agnostic and testable.
        /// </summary>
        public Func<string?>? FolderPicker { get; set; }

        public ICommand BrowseCommand { get; }
        public ICommand OkCommand => _okCommand;
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Raised when the VM requests the dialog to close. The bool? payload
        /// should be applied to Window.DialogResult (true for OK, false for Cancel).
        /// </summary>
        public event Action<bool?>? RequestClose;

        private readonly DelegateCommand _okCommand;

        public InitializingDialogViewModel()
        {
            BrowseCommand = new DelegateCommand(_ => OnBrowse());
            _okCommand = new DelegateCommand(_ => RequestClose?.Invoke(true), _ => CanOk);
            CancelCommand = new DelegateCommand(_ => RequestClose?.Invoke(false));
        }

        private void OnBrowse()
        {
            try
            {
                var picked = FolderPicker?.Invoke();
                if (!string.IsNullOrWhiteSpace(picked))
                {
                    InstallPath = picked!;
                }
            }
            catch
            {
                // Swallow exceptions from the picker delegate to keep VM robust.
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name ?? string.Empty));
        #endregion

        #region DelegateCommand (simple ICommand with RaiseCanExecuteChanged)
        private class DelegateCommand : ICommand
        {
            private readonly Action<object?> _execute;
            private readonly Func<object?, bool>? _canExecute;

            public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
            public void Execute(object? parameter) => _execute(parameter);
            public event EventHandler? CanExecuteChanged;
            public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}