﻿using CMS.Common.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.ViewModel.Base
{
    abstract class EditableViewModel : BaseViewModel
    {
        public event EventHandler<SelectedModeEventArgs> SelectedModeChanged;

        protected void EditData()
        {
            Mode selectedMode = SelectedMode;

            switch (selectedMode)
            {
                case Mode.Add: CreateData();
                    break;
                case Mode.Edit: EditData();
                    break;
                default: return;
            }
        }

        abstract protected void SaveData();

        abstract protected void CreateData();

        /// <summary>
        /// Indicates if there was any changes
        /// </summary>
        public bool PendingChanges
        {
            get
            {
                return _pendingChanges;
            }
            set
            {
                if (_pendingChanges != value)
                {
                    _pendingChanges = value;
                }
            }
        }
        private bool _pendingChanges = false;

        /// <summary>
        /// Indicates if the selected mode allows to save data
        /// </summary>
        public bool CanSaveData
        {
            get
            {
                return
                    SelectedMode == Mode.Add ||
                    SelectedMode == Mode.Edit;
            }
        }

        #region Modes

        public Mode SelectedMode
        {
            get
            {
                return _selectedMode;
            }
            set
            {
                if (_selectedMode != value)
                {
                    _selectedMode = value;
                    OnPropertyChanged("SelectedMode");
                    OnPropertyChanged("CanSaveData");
                    OnSelectedModeChanged(new SelectedModeEventArgs(value));
                }
            }
        }

        private void OnSelectedModeChanged(SelectedModeEventArgs e)
        {
            EventHandler<SelectedModeEventArgs> selectedModeChanged = SelectedModeChanged;

            if (selectedModeChanged != null)
            {
                selectedModeChanged.BeginInvoke(this, e, null, null);
            }
        }

        private Mode _selectedMode = Mode.Read;        

        public ObservableCollection<Mode> Modes
        {
            get
            {
                return new ObservableCollection<Mode>
                {
                    Mode.Read,
                    Mode.Edit,
                    Mode.Add,
                    Mode.Remove
                };
            }
        }

        #endregion
    }

    class SelectedModeEventArgs : EventArgs
    {
        public Mode SelectedMode { get; private set; }

        public SelectedModeEventArgs(Mode mode)
        {
            SelectedMode = mode;
        }
    }
}
