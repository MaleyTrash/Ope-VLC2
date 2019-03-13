using System;
using System.Collections.Generic;
using System.Reflection;

namespace VLC2
{
    class CommandManager
    {
        private readonly MainWindow window;

        List<ICommandParser> parsers = new List<ICommandParser>();
        ICommandParser defaultParser = null;

        public CommandManager(MainWindow window)
        {
            this.window = window;

            Type ti = typeof(ICommandParser);
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (!t.IsInterface && !t.IsAbstract && ti.IsAssignableFrom(t))
                    {
                        var parser = (ICommandParser)Activator.CreateInstance(t, window);

                        if (parser.Prefix == string.Empty) defaultParser = parser;
                        else parsers.Add(parser);
                    }
                }
            }
        }

        public void Parse(string cmd)
        {
            string[] split = cmd.Split(' ');

            if (split.Length < 1) return;

            foreach(var p in parsers)
            {
                if (p.Prefix == split[0])
                {
                    p.Parse(split);
                    return;
                }
            }

            if (defaultParser != null) defaultParser.Parse(split);
        }
    }
}
