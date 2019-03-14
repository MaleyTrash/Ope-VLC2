using System.Windows;
using System.Windows.Controls;

namespace VLC2
{
    class PlaylistParser : ICommandParser
    {
        private readonly MainWindow win;
        public PlaylistParser(MainWindow win) { this.win = win; }

        public string Prefix => "playlist";

        public void Parse(string[] split)
        {
            if (split.Length < 2)
            {
                togglePlaylist();
                return;
            }

            string cmd = split[1];
            switch(cmd)
            {
                case "add":
                    if (split.Length >= 3) win.playlist.Add(string.Join(" ", split, 2, split.Length - 2));
                    break;
                case "remove":
                case "pop":
                    if (split.Length >= 3) removeAt(split[2]);
                    else removeAt((win.playlist.Count - 1).ToString());
                    break;
                case "shift":
                    if (win.playlist.Count > 0) removeAt("0");
                    break;
                case "show":
                    Grid.SetColumnSpan(win.Vlc, 1); break;
                case "hide":
                    Grid.SetColumnSpan(win.Vlc, 2); break;
            }
        }

        private void removeAt(string pos)
        {
            try
            {
                int p = int.Parse(pos);
                win.playlist.RemoveAt(p);
            }
            catch { }
        }

        private void togglePlaylist()
        {
            Grid.SetColumnSpan(win.Vlc, Grid.GetColumnSpan(win.Vlc) > 1 ? 1 : 2);
        }
    }
}
