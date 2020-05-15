namespace PilkarzeMVVM.ViewModel
{
    using System.ComponentModel;
    using System.IO;
    using System.Text.Json;
    using System.Windows.Input;
    using PilkarzeMVVM.Model;
    using PilkarzeMVVM.ViewModel.BaseClass;

    internal class PilkarzeVM : ViewModelBase
    {
        #region declaration

        private double? age = 18;
        private string firstName = null;
        private string lastName = null;
        private double? weight = 75;
        private Pilkarz selected = null;
        private BindingList<Pilkarz> players = new BindingList<Pilkarz>();
        private string path = "..//db.json";
        #endregion

        #region propMethods
        public double? Age
        {
            get => age; set
            {
                age = value;
                OnPropertyChanged(nameof(Age));
            }
        }
        public string FirstName
        {
            get => firstName; set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public Pilkarz Selected
        {
            get => selected; set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
                if (copy_Command.CanExecute(null)) copy_Command.Execute(null);
            }
        }
        public BindingList<Pilkarz> PlayersList
        {
            get => players; set
            {
                players = value;
                OnPropertyChanged(nameof(PlayersList));
            }
        }
        public string LastName
        {
            get => lastName; set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public double? Weight
        {
            get => weight; set
            {
                weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }
        #endregion

        #region ICommands

        private ICommand add_Command;
        private ICommand del_Command;
        private ICommand edit_Command;
        private ICommand clear_Command;
        private ICommand copy_Command;
        private ICommand load_Command;
        private ICommand save_Command;


        public ICommand AddCommand
        {
            get
            {
                if (add_Command is null)
                {
                    add_Command = new RelayCommand(
                        execute =>
                        {
                            var footballer = new Pilkarz(FirstName, LastName, (double)Age, (double)Weight);
                            if (!PlayersList.Contains(footballer))
                            {
                                PlayersList.Add(footballer);
                                OnPropertyChanged(nameof(PlayersList));
                                ClearCommand.Execute(null);
                            }
                        }, canExecute => IsEmpty
                    );
                }
                return add_Command;
            }
        }

        public ICommand DelCommand
        {
            get
            {
                if (del_Command is null)
                {
                    del_Command = new RelayCommand(execute =>
                    {
                        var footballer = new Pilkarz(FirstName, LastName, (double)Age, (double)Weight);
                        if (PlayersList.Contains(footballer))
                        {
                            PlayersList.Remove(footballer);
                            OnPropertyChanged(nameof(PlayersList));
                        }
                    }, canExecute => IsEmpty && Selected != null);
                }
                return del_Command;
            }
        }
        public ICommand ClearCommand
        {
            get
            {
                if (clear_Command is null)
                {
                    clear_Command = new RelayCommand(
                        execute =>
                        {
                            FirstName = LastName = null;
                            Weight = Age = null;
                        }, canExecute => true
                    );
                }
                return clear_Command;
            }
        }

        public ICommand CopyCommand
        {
            get
            {
                if (copy_Command is null)
                {
                    copy_Command = new RelayCommand(
                        execute =>
                        {
                            FirstName = Selected.FirstName;
                            LastName = Selected.LastName;
                            Age = Selected.Age;
                            Weight = Selected.Weight;
                        }, canExecute => Selected != null
                    );
                }
                return copy_Command;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (edit_Command is null)
                {
                    edit_Command = new RelayCommand(execute =>
                    {
                        var newFootballer = new Pilkarz(FirstName, LastName, (double)Age, (double)Weight);
                        if (PlayersList.Contains(Selected))
                        {
                            var index = PlayersList.IndexOf(selected);
                            PlayersList[index].Copy(newFootballer);
                            PlayersList.ResetItem(index);
                            ClearCommand.Execute(null);

                        }
                    }, canExecute => IsEmpty && Selected != null);
                }
                return edit_Command;
            }
        }
        public ICommand LoadCommand
        {
            get
            {
                if (load_Command is null)
                {
                    load_Command = new RelayCommand(execute =>
                    {
                        var jsonFootballers = File.ReadAllText(path);
                        PlayersList = JsonSerializer.Deserialize<BindingList<Pilkarz>>(jsonFootballers);
                        OnPropertyChanged(nameof(LoadCommand));
                        PlayersList.ResetBindings();
                    }, canExecute => File.Exists(path) && (new FileInfo(path).Length > 0));
                }
                return load_Command;
            }
        }
        private bool IsEmpty { get { return (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && Age > 0 && Weight > 0); } }

        public ICommand SaveCommand
        {
            get
            {
                if (save_Command is null)
                {
                    save_Command = new RelayCommand(execute =>
                    {
                        var jsonFootballers = JsonSerializer.Serialize(PlayersList);
                        File.WriteAllText(path, jsonFootballers);
                        OnPropertyChanged(nameof(SaveCommand));
                    }, canExecute => true);
                }
                return save_Command;
            }
        }

        #endregion
    }
}