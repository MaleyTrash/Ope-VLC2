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
}
