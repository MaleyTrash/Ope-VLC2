namespace VLC2
{
    class SeekParser : ICommandParser
    {
        private readonly MainWindow win;
        public SeekParser(MainWindow win) { this.win = win; }

        public string Prefix => "seek";

        public void Parse(string[] split)
        {
            if (split.Length < 2) return;

            string arg = split[1];

            try
            {
                int pos = int.Parse(arg);
                changePosition(pos);
                return;
            }
            catch { }

            switch (arg)
            {
                case "forward":
                case "f":
                case "forw":
                    changePosition(5);
                    break;
                case "back":
                case "b":
                    changePosition(-5);
                    break;
            }
        }

        private void changePosition(int secs)
        {
            secs *= 1000;

            long newPos = win.Vlc.MediaPlayer.Time + secs;

            if (newPos >= win.Vlc.MediaPlayer.Length) newPos = win.Vlc.MediaPlayer.Length - 1;

            win.Vlc.MediaPlayer.Time = newPos;
        }
    }
}
