namespace VLC2
{
    class PlayParser : ICommandParser
    {
        private readonly MainWindow win;
        public PlayParser(MainWindow win) { this.win = win; }

        public string Prefix => "play";

        public void Parse(string[] split)
        {
            if (split.Length < 2)
            {
                win.Vlc.MediaPlayer.Play();
                return;
            }

            win.PlayFile(string.Join(" ", split, 1, split.Length - 1));
        }
    }
}
