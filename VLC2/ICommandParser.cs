using System;
using Vlc.DotNet.Forms;

namespace VLC2
{
    interface ICommandParser
    {
        string Prefix { get; }

        void Parse(string[] split);
    }
}
