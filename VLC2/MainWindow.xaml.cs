using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Vlc.DotNet.Core;

namespace VLC2
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        CommandManager cmdManager;

        public MainWindow()
        {
            InitializeComponent();

            Vlc.MediaPlayer.BeginInit();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            Vlc.MediaPlayer.VlcLibDirectory = libDirectory;
            Vlc.MediaPlayer.EndInit();

            Vlc.MediaPlayer.Play("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_h264.mov");

            Vlc.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;

            cmdManager = new CommandManager(this);
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
            if (e.Key != Key.Enter) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                cmdManager.Parse(Command.Text);
            });
            Command.Text = "";
        }
    }
}
