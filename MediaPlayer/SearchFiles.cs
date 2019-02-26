using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Media
{
    class SearchFiles
    {
        public List<string> AudioList;
        public List<string> VideoList;
        private Thread[] TObj;

        public SearchFiles()
        {
            AudioList = new List<string>();
            VideoList = new List<string>();
        }
        private void SearchMediaFiles(string path, string pattern)
        {
            string[] dirs = null;
            string[] files = null;

            try
            {

                dirs = Directory.GetDirectories(path);
                foreach (string d in dirs)
                {
                    SearchMediaFiles(d, pattern);
                }

                files = Directory.GetFiles(path, pattern).Where(s => s.EndsWith(".mp3") || s.EndsWith(".mp4") || s.EndsWith(".mkv") ).ToArray();

                foreach (string file in files)
                {
                    if (file.EndsWith(".mp3"))
                    {
                        AudioList.Add(file);
                    }
                    else if (file.EndsWith(".mp4") || file.EndsWith(".mkv"))
                    {
                        VideoList.Add(file);
                    }
                }

            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (IOException)
            {

            }
            catch (Exception)
            {
                //throw;
            }

        }
        public void LogAudioAndVideoInformation(string File1, string File2)
        {
            string Folder = FilePathInfo.Path + @"\Media Information";
            try
            {
                foreach (Thread thread in TObj)
                {
                    thread.Join();
                }
            }
            catch (NullReferenceException)
            {

            }
            catch (Exception)
            {

            }

            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }

            using (StreamWriter sw = File.CreateText(File1))
            {
                foreach (string audio in AudioList)
                {
                    sw.WriteLine(audio);
                }
            }

            using (StreamWriter sw = File.CreateText(File2))
            {
                foreach (string video in VideoList)
                {
                    sw.WriteLine(video);
                }
            }
        }
        public void SearchInAllDrives(string pattern)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            TObj = new Thread[allDrives.Length];

            int i = 0;

            foreach (DriveInfo drive in allDrives)
            {
                if (!drive.RootDirectory.ToString().Equals(@"C:\"))
                {
                    if (drive.DriveType.ToString().Equals("Fixed"))
                    {
                        TObj[i] = new Thread(() => { SearchMediaFiles(drive.RootDirectory.ToString(), pattern); });
                        TObj[i].Start();
                        i++;
                    }
                }

            }
        }
        ~SearchFiles()
        {
            AudioList.Clear();
            AudioList = null;
            VideoList.Clear();
            VideoList = null;
            TObj = null;
        }
    }
}
