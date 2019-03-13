namespace VLC2
{
    class VolumeParser : ICommandParser
    {
        private readonly MainWindow win;
        public VolumeParser(MainWindow win) { this.win = win; }
        
        public string Prefix => "volume";

        public void Parse(string[] split)
        {
            if (split.Length < 2) return;

            string arg = split[1];

            try
            {
                int vol = int.Parse(arg);
                setVolume(vol);
                return;
            }
            catch { }

            switch(arg) {
                case "up": setVolume(win.Vlc.MediaPlayer.Audio.Volume + 5); break;
                case "down": setVolume(win.Vlc.MediaPlayer.Audio.Volume - 5); break;
                case "doprava": setVolume(100); break;
            }
        }

        private void setVolume(int vol)
        {
            if (vol < 0) vol = 0;
            if (vol > 100) vol = 100;

            win.Vlc.MediaPlayer.Audio.Volume = vol; 
        }
    }

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
