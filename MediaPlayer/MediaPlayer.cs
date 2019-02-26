using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Threading;
using System.IO;
using System.Speech.Recognition;


namespace Media
{
    class MediaPlayer
    {
        private SearchFiles SearchFilesObj;
        public int AudPos;
        public int VidPos;
        public string[] Audios;
        public string[] Videos;
        public int Indicator;

        public MediaPlayer()
        {
            SearchFilesObj = new SearchFiles();
            AudPos = 0;
            VidPos = 0;
            Indicator = 1;
            LoadAllSongs();
        }

        public void LoadFile()
        {
            SearchFilesObj.SearchInAllDrives("*.*");
            SearchFilesObj.LogAudioAndVideoInformation(FilePathInfo.AudioFile, FilePathInfo.VideoFile);
        }

        public void LoadAllSongs()
        {

            if ((!File.Exists(FilePathInfo.AudioFile)) && (!File.Exists(FilePathInfo.VideoFile)))
            {
                LoadFile();
            }

            Audios = File.ReadAllLines(FilePathInfo.AudioFile);
            Videos = File.ReadAllLines(FilePathInfo.VideoFile);
        }

        
    }

}
