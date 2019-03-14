using System.Windows;
using Vlc.DotNet.Forms;

namespace VLC2
{
    class DefaultParser : ICommandParser
    {
        private readonly MainWindow win;
        private VlcControl vlc => win.Vlc.MediaPlayer;
        public DefaultParser(MainWindow win) { this.win = win; }

        public string Prefix => "";

        public void Parse(string[] split)
        {
            switch(split[0])
            {
                case "pause":
                    vlc.Pause(); break;
                case "resume":
                    vlc.Play(); break;
                case "exit":
                    Application.Current.Shutdown(); break;
            }
        }
    }
}
