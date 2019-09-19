using System;

namespace MusicCatalogLib
{
    public class ArgumentsParser
    {
        public ArgumentsParser(MusicCatalog mc)
        {
            // TODO
        }

        public string[] ParseAndExec(string[] args)
        {
            // TODO
            return null;
        }

        public string[] ParseAndExec(string args)
        {
            return ParseAndExec(args.Split("\\s+"));
        }
    }
}