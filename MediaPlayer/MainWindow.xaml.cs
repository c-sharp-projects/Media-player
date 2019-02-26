using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.Windows.Threading;

namespace Media
{

    public partial class MediaPlayerUI : Window
    {
        DispatcherTimer timer;
        bool IsSeeked;
        bool IsPlay;
        bool IsAList;
        bool IsVList;
        MediaPlayer MObj;
        MediaCodes Indicator;
        public MediaPlayerUI()
        {
            InitializeComponent();
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Seek_Timer;
            timer.Start();

            LoadIcon();

            IsSeeked = false;
            IsPlay = false;
            IsAList = false;
            IsVList = false;
            

            MObj = new MediaPlayer();
            Indicator = MediaCodes.PlayVideo;
            MyMediaElement.Source = new Uri(MObj.Videos[MObj.VidPos]);
            
        }       
        private void Seek_Timer(object sender, EventArgs e)
        {
            if ((MyMediaElement.Source != null) && (MyMediaElement.NaturalDuration.HasTimeSpan) && (!IsSeeked))
            {
                Seeker.Minimum = 0;
                Seeker.Maximum = MyMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                Seeker.Value = MyMediaElement.Position.TotalSeconds;
            }
        }
        private void Seek_Drag_Started(object sender, DragStartedEventArgs e)
        {
            IsSeeked = true;
        }
        private void Seek_Drag_Completed(object sender, DragCompletedEventArgs e)
        {
            IsSeeked = false;
            MyMediaElement.Position = TimeSpan.FromSeconds(Seeker.Value);
        }
        private void Seek_Value_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PlayTime.Text = TimeSpan.FromSeconds(Seeker.Value).ToString(@"hh\:mm\:ss");
        }
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MyMediaElement.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }
        private void LoadIcon()
        {
            try
            {
                Next.Content = new Image { Source = new BitmapImage(new Uri(FilePathInfo.Next)) };
                Previous.Content = new Image { Source = new BitmapImage(new Uri(FilePathInfo.Prev)) };
                PlayOrPause.Content = new Image { Source = new BitmapImage(new Uri(FilePathInfo.Play)) };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void ListDoubleClick(object sender, EventArgs e)
        {
            ListBox list = (ListBox)sender;

            if (IsAList)
            {
                Indicator = MediaCodes.PlayAudio;
                MObj.AudPos = list.SelectedIndex;                
            }
            else
            {
                Indicator = MediaCodes.PlayVideo;
                MObj.VidPos = list.SelectedIndex;
            }
            
            PlaySong(Indicator);
        }
        private void ActionListener(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.Uid)
            {
                case "Play":
                    if (IsPlay)
                    {
                        PauseSong();
                    }
                    else
                    {                        
                        PlaySong(MediaCodes.Play);
                    }
                    break;

                case "Prev":
                    PreviousSong();
                    break;

                case "Next":
                    NextSong();
                    break;

                case "AudioList":
                    if (!IsAList)
                    {
                        IsAList = true;
                        IsVList = false;
                        ShowList(MediaCodes.AudioList);
                        return;
                    }                    
                    PlayList.Items.Clear();
                    PlayList.Visibility = Visibility.Hidden;
                    IsAList = false;
                    break;

                case "VideoList":
                    if (!IsVList)
                    {
                        IsVList = true;
                        IsAList = false;
                        ShowList(MediaCodes.VideoList);
                        return;
                    }                    
                    PlayList.Items.Clear();
                    PlayList.Visibility = Visibility.Hidden;
                    IsVList = false;
                    break;
            }
            
        }
        public void PlaySong(MediaCodes Indicator)
        {

            if (!File.Exists(FilePathInfo.AudioFile) && !File.Exists(FilePathInfo.VideoFile))
            {
                MObj.LoadAllSongs();
            }

            switch (Indicator)
            {
                case MediaCodes.PlayAudio:

                    if (!File.Exists(MObj.Audios[MObj.AudPos]))
                    {
                        NextSong();
                    }
                    MyMediaElement.Source = new Uri(MObj.Audios[MObj.AudPos]);
                    
                    break;
                case MediaCodes.PlayVideo:

                    if (!File.Exists(MObj.Videos[MObj.VidPos]))
                    {
                        NextSong();
                    }

                    MyMediaElement.Source = new Uri(MObj.Videos[MObj.VidPos]);
                    
                    break;
                case MediaCodes.Play:

                    if (!File.Exists(MObj.Audios[MObj.AudPos]))
                    {
                        NextSong();
                    }
                    if (!File.Exists(MObj.Videos[MObj.VidPos]))
                    {
                        NextSong();
                    }
                    
                    
                    break;
            }

            IsPlay = true;
            PlayOrPause.Content = new Image { Source = new BitmapImage(new Uri(FilePathInfo.Pause)) };
            PlayOrPause.ToolTip = "Play";
            MyMediaElement.Play();
        }
        public void PauseSong()
        {
            IsPlay = false;
            PlayOrPause.Content = new Image { Source = new BitmapImage(new Uri(FilePathInfo.Play)) };
            PlayOrPause.ToolTip = "Pause";
            MyMediaElement.Pause();
        }
        public void StopSong()
        {
            MyMediaElement.Stop();
        }
        public void NextSong()
        {
            if (!File.Exists(FilePathInfo.AudioFile) && !File.Exists(FilePathInfo.VideoFile))
            {
                MObj.LoadAllSongs();
            }

            switch (Indicator)
            {
                case MediaCodes.PlayAudio:

                    ++MObj.AudPos;

                    if (MObj.AudPos > MObj.Audios.Length - 1)
                    {
                        MObj.AudPos = 0;
                    }
                    if (!File.Exists(MObj.Audios[MObj.AudPos]))
                    {
                        NextSong();
                    }

                    MyMediaElement.Source = new Uri(MObj.Audios[MObj.AudPos]);
                    MyMediaElement.Play();
                    break;
                case MediaCodes.PlayVideo:

                    ++MObj.VidPos;

                    if (MObj.VidPos > MObj.Videos.Length - 1)
                    {
                        MObj.VidPos = 0;
                    }
                    if (!File.Exists(MObj.Videos[MObj.VidPos]))
                    {
                        NextSong();
                    }

                    MyMediaElement.Source = new Uri(MObj.Videos[MObj.VidPos]);
                    MyMediaElement.Play();
                    break;

            }
            IsPlay = true;
            PlayOrPause.Content = new Image { Source = new BitmapImage(new Uri(FilePathInfo.Pause)) };
            PlayOrPause.ToolTip = "Play";
        }
        public void PreviousSong()
        {
            if ((!File.Exists(FilePathInfo.AudioFile)) && (!File.Exists(FilePathInfo.VideoFile)))
            {
                MObj.LoadAllSongs();
            }

            switch (Indicator)
            {
                case MediaCodes.PlayAudio:
                    --MObj.AudPos;

                    if (MObj.AudPos < 0)
                    {
                        MObj.AudPos = MObj.Audios.Length - 1;
                    }
                    if (!File.Exists(MObj.Audios[MObj.AudPos]))
                    {
                        PreviousSong();
                    }
                    MyMediaElement.Source = new Uri(MObj.Audios[MObj.AudPos]);
                    MyMediaElement.Play();
                    break;
                case MediaCodes.PlayVideo:
                    --MObj.VidPos;

                    if (MObj.VidPos < 0)
                    {
                        MObj.VidPos = MObj.Videos.Length - 1;
                    }
                    if (!File.Exists(MObj.Videos[MObj.VidPos]))
                    {
                        PreviousSong();
                    }
                    MyMediaElement.Source = new Uri(MObj.Videos[MObj.VidPos]);
                    MyMediaElement.Play();
                    break;
            }
            IsPlay = true;
            PlayOrPause.Content = new Image { Source = new BitmapImage(new Uri(FilePathInfo.Pause)) };
            PlayOrPause.ToolTip = "Play";
        }
        public void ShowList(MediaCodes Indicator)
        {
            PlayList.Items.Clear();
            PlayList.Visibility = Visibility.Visible;

            switch (Indicator)
            {
                case MediaCodes.AudioList:

                    foreach (string s in MObj.Audios)
                    {
                        PlayList.Items.Add(s.Substring(s.LastIndexOf('\\')+1));
                    }
                    
                    break;
                case MediaCodes.VideoList:

                    foreach (string s in MObj.Videos)
                    {
                        PlayList.Items.Add(s.Substring(s.LastIndexOf('\\') + 1));
                    }

                    break;
            }
        }
    }
}
