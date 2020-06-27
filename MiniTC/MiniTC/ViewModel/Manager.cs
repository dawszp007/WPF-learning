using MiniTC.Model;
using MiniTC.ViewModel.BaseTC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiniTC.ViewModel
{
    class Manager : ViewModelBase
    {

        public string LeftPath {
            get
            {
                return Model.CurrentPath[0];
            }
            set
            {
                Model.CurrentPath[0] = value;
            }
        }

        public string RightPath
        {
            get
            {
                return Model.CurrentPath[1];
            }
            set
            {
                Model.CurrentPath[1] = value;
            }
        }

        public string[] Drives
        {
            get
            {
                return Model.Drives;
            }
        }

        public List<string> LeftFiles
        {
            get
            {
                List<string> files = Model.Directories[0];
                try
                {
                    foreach (var item in Model.Files[0])
                        files.Add(item);
                }
                catch { }
                return files;
            }
        }

        public List<string> RightFiles
        {
            get
            {
                List<string> files = Model.Directories[1];
                try
                {
                    foreach (var item in Model.Files[1])
                        files.Add(item);
                }
                catch { }
                return files;
            }
        }

        public int LeftDrive
        {
            get
            {
                return Model.Drive[0];
            }
            set
            {
                Model.Drive[0] = value;
            }
        }

        public int RightDrive { get { return Model.Drive[1]; } set { Model.Drive[1] = value; } }

        public int CurrentLeft { get; set; } = -1;
        public int CurrentRight { get; set; } = -1;
        public int LastSelected { get; set; } = -1;

        public Manager()
        {
            LeftDrive = -1;
            RightDrive = -1;
            LeftPath = "";
            RightPath = "";
        }
        Logic Model = new Logic();

        private ICommand lDrvChanged = null;
        public ICommand LDrvChanged
        {
            get
            {
                if (lDrvChanged == null)
                    lDrvChanged = new RelayCommand(
                        arg => { Model.Changed_Directory(Drives[LeftDrive], 0); OnPropertyChanged(nameof(LeftFiles), nameof(LeftPath)); },
                        arg => true
            );
                return lDrvChanged;
            }
        }

        private ICommand rDirChanged = null;
        public ICommand RDirChanged
        {
            get
            {
                if (rDirChanged == null)
                    rDirChanged = new RelayCommand(
                        arg => { Model.Changed_Directory(Drives[RightDrive], 1); OnPropertyChanged(nameof(RightFiles), nameof(RightPath)); },
                        arg => true
            );
                return rDirChanged;

            }
        }

        private ICommand lDirChanged = null;
        public ICommand LDirChanged
        {
            get
            {
                if (lDirChanged == null)
                    lDirChanged = new RelayCommand(
                        arg => {
                            string current = LeftFiles[CurrentLeft];
                            if (current == "..")
                            {
                                Model.Changed_Directory(Path.GetDirectoryName(LeftPath), 0);
                                CurrentLeft = -1;
                                OnPropertyChanged(nameof(LeftFiles), nameof(LeftPath), nameof(CurrentLeft));
                                if (LastSelected == 0)
                                    LastSelected = -1;
                            }
                            else if (Directory.Exists(LeftPath + @"\" + current.Substring(3)))
                            {
                                Model.Changed_Directory(LeftPath + @"\" + current.Substring(3), 0);
                                CurrentLeft = -1;
                                OnPropertyChanged(nameof(LeftFiles), nameof(LeftPath), nameof(CurrentLeft));
                                if (LastSelected == 0)
                                    LastSelected = -1;
                            }
                            else
                                LastSelected = 0;
                        },
                        arg => CurrentLeft >= 0
            );
                return lDirChanged;

            }
        }

        private ICommand rightDirChanged = null;
        public ICommand RightDirChanged
        {
            get
            {
                if (rightDirChanged == null)
                    rightDirChanged = new RelayCommand(
                        arg => {
                            string current = RightFiles[CurrentRight];
                            if (current == "..")
                            {
                                Model.Changed_Directory(Path.GetDirectoryName(RightPath), 1);
                                CurrentRight = -1;
                                OnPropertyChanged(nameof(RightFiles), nameof(RightPath), nameof(CurrentRight));
                                if (LastSelected == 1)
                                    LastSelected = -1;
                            }
                            else if (Directory.Exists(RightPath + @"\" + current.Substring(3)))
                            {
                                Model.Changed_Directory(RightPath + @"\" + current.Substring(3), 1);
                                CurrentRight = -1;
                                OnPropertyChanged(nameof(RightFiles), nameof(RightPath), nameof(CurrentRight));
                                if (LastSelected == 1)
                                    LastSelected = -1;
                            }
                            else
                                LastSelected = 1;
                        },
                        arg => CurrentRight >= 0
            );
                return rightDirChanged;

            }
        }

        private ICommand copy = null;
        public ICommand Copy
        {
            get
            {
                if (copy == null)
                    copy = new RelayCommand(
                        arg => {
                            if (LastSelected == 0)
                            {
                                Model.Copy(LeftPath + @"\" + LeftFiles[CurrentRight], RightPath + @"\" + LeftFiles[CurrentRight]);
                                Model.Changed_Directory(RightPath, 1);
                            }
                            else
                            {
                                Model.Copy(RightPath + @"\" + RightFiles[CurrentRight], LeftPath + @"\" + RightFiles[CurrentRight]);
                                Model.Changed_Directory(LeftPath, 0);
                            }

                            OnPropertyChanged(nameof(RightFiles), nameof(LeftFiles));
                        },
                        arg => LastSelected != -1 && LeftPath.Length > 0 && RightPath.Length > 0
            );
                return copy;

            }
        }
    }
}