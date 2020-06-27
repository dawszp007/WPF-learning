using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniTC.View
{
    /// <summary>
    /// Logika interakcji dla klasy Panel.xaml
    /// </summary>
    public partial class Panel : UserControl
    {
        public string Path
        {
            get { return (string)GetValue(PathDP); }
            set { SetValue(PathDP, value); }
        }

        public static readonly DependencyProperty PathDP = DependencyProperty.Register(
           nameof(Path), typeof(string), typeof(Panel), new FrameworkPropertyMetadata(null));

        public string[] DrivesList
        {
            get { return (string[])GetValue(DrivesListDP); }
            set { SetValue(DrivesListDP, value); }
        }

        public static readonly DependencyProperty DrivesListDP = DependencyProperty.Register(
           nameof(DrivesList), typeof(string[]), typeof(Panel), new FrameworkPropertyMetadata(null));

        public List<string> FilesList
        {
            get { return (List<string>)GetValue(FilesListDP); }
            set { SetValue(FilesListDP, value); }
        }

        public static readonly DependencyProperty FilesListDP = DependencyProperty.Register(
           nameof(FilesList), typeof(List<string>), typeof(Panel), new FrameworkPropertyMetadata(null));

        public int SelectedDrive
        {
            get { return (int)GetValue(SelectedDriveDP); }
            set { SetValue(SelectedDriveDP, value); }
        }

        public static readonly DependencyProperty SelectedDriveDP = DependencyProperty.Register(
           nameof(SelectedDrive), typeof(int), typeof(Panel), new FrameworkPropertyMetadata(null));

        public int CurrItem
        {
            get { return (int)GetValue(CurrItemDP); }
            set { SetValue(CurrItemDP, value); }
        }

        public static readonly DependencyProperty CurrItemDP = DependencyProperty.Register(
           nameof(CurrItem), typeof(int), typeof(Panel), new FrameworkPropertyMetadata(null));


        public static readonly RoutedEvent DriveChangedRegistered =
        EventManager.RegisterRoutedEvent(nameof(DriveChanged),
                     RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                     typeof(Panel));

        public event RoutedEventHandler DriveChanged
        {
            add { AddHandler(DriveChangedRegistered, value); }
            remove { RemoveHandler(DriveChangedRegistered, value); }
        }

        void RaiseDriveChanged()
        {
            RoutedEventArgs newEventArgs =
                    new RoutedEventArgs(Panel.DriveChangedRegistered);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent DirChangedRegistered =
        EventManager.RegisterRoutedEvent(nameof(DirChanged),
                     RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                     typeof(Panel));

        public event RoutedEventHandler DirChanged
        {
            add { AddHandler(DirChangedRegistered, value); }
            remove { RemoveHandler(DirChangedRegistered, value); }
        }

        void RaiseDirChanged()
        {
            RoutedEventArgs newEventArgs =
                    new RoutedEventArgs(Panel.DirChangedRegistered);
            RaiseEvent(newEventArgs);
        }


        public Panel()
        {
            InitializeComponent();
        }



        private void DrivesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseDriveChanged();
        }

        private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseDirChanged();
        }
    }
}