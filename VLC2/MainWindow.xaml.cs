using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using Vlc.DotNet.Core;

namespace VLC2
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ObservableCollection<string> playlist = new ObservableCollection<string>();

        CommandManager cmdManager;
        string lastCmd;

        public MainWindow()
        {
            InitializeComponent();
            cmdManager = new CommandManager(this);

            Vlc.MediaPlayer.BeginInit();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            Vlc.MediaPlayer.VlcLibDirectory = libDirectory;
            Vlc.MediaPlayer.EndInit();

            PlayFile("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_h264.mov");

            Vlc.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            Vlc.MediaPlayer.EndReached += MediaPlayer_EndReached;
            playlist.CollectionChanged += Playlist_CollectionChanged;

            PlaylistView.SelectionChanged += PlaylistView_SelectionChanged;

            cmdManager = new CommandManager(this);

            PlaylistView.ItemsSource = playlist;
        }

        private void Playlist_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Remove) return;

            int h = PlaylistView.SelectedIndex;

            if (e.OldStartingIndex == PlaylistView.SelectedIndex)
            {
                Vlc.MediaPlayer.Stop();
                PlayAt(e.OldStartingIndex);
            }
        }

        private void PlaylistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlayAt(PlaylistView.SelectedIndex);
        }

        private void MediaPlayer_EndReached(object sender, VlcMediaPlayerEndReachedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                int target = PlaylistView.SelectedIndex + 1;

                if (target >= playlist.Count) target = 0;

                PlayAt(target);
            }));
        }

        public void PlayAt(int index)
        {
            if (index < 0 || index >= playlist.Count) return;

            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                Vlc.MediaPlayer.Play(new Uri(playlist[index]));
                PlaylistView.SelectedIndex = index;
            }));
        }

        public void PlayFile(string path)
        {
            playlist.Add(path);
            PlayAt(playlist.Count - 1);
        }

        private void MediaPlayer_PositionChanged(object sender, VlcMediaPlayerPositionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Seeker.Value = e.NewPosition;
            });
        }

        private void Seeker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(Seeker);

            double target = pos.Y / Seeker.ActualHeight;
            Vlc.MediaPlayer.Position = 1f - (float)target;
        }

        private void Command_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                Command.Text = lastCmd;
                return;
            }
            if (e.Key != Key.Enter) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                cmdManager.Parse(Command.Text);
            });
            lastCmd = Command.Text;
            Command.Text = "";
        }
    }
}
