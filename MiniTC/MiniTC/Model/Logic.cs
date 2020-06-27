using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiniTC.Model
{
    class Logic
    {

        public string[] Drives { get; set; }
        public List<string>[] Directories { get; set; }
        public List<string>[] Files { get; set; }
        public int[] Drive { get; set; }
        public string[] CurrentPath { get; set; }

        public Logic()
        {
            Drives = Directory.GetLogicalDrives();
            Update();
            Directories = new List<string>[2];
            Files = new List<string>[2];
            Drive = new int[2];
            CurrentPath = new string[2];
        }

        public void Changed_Directory(string path, int number)
        {
            string lastPath = CurrentPath[number];
            CurrentPath[number] = path;
            Directories[number] = new List<string>();

            if (CurrentPath[number].Substring(Path.GetPathRoot(CurrentPath[number]).Length).Length != 0)
                Directories[number].Add("..");

            try
            {
                foreach (var directory in Directory.GetDirectories(CurrentPath[number]))
                {
                    var dirName = new DirectoryInfo(directory).Name;
                    Directories[number].Add("<D>" + dirName);
                }

                Files[number] = new List<string>();

                foreach (var file in Directory.GetFiles(CurrentPath[number]))
                {
                    var fileName = new FileInfo(file).Name;
                    Directories[number].Add(fileName);
                }
            }
            catch (UnauthorizedAccessException error)
            {
                MessageBox.Show(error.Message);
                Changed_Directory(lastPath, number);
            }
        }
        public void Update()
        {
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            string[] readyDrives = new string[drives.Length];
            int i = 0;
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    readyDrives[i] = drive.ToString();
                    i += 1;
                }
            }
            Drives = readyDrives;
        }

        public void Copy(string source, string destination)
        {
            if (File.Exists(destination))
            {
                MessageBoxResult dialogResult = MessageBox.Show("File existing", "Error", MessageBoxButton.OK);
            }
            else
            {
                try
                {
                    File.Copy(source, destination);
                }
                catch (IOException e)
                {
                    MessageBox.Show(e.Message);
                }
                catch (UnauthorizedAccessException e)
                {
                    MessageBox.Show("Can't open", "Error", MessageBoxButton.OK);
                }
            }
        }

    }
}