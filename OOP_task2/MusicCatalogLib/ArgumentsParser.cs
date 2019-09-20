using System;
using System.Text.RegularExpressions;


namespace MusicCatalogLib
{
    public class ArgumentsParser
    {
        public string[] ParseAndExec(string[] args)
        {
            string filepath = null;
            string artistName = null;
            string albumName = null;
            string songName = null;
            string compilationName = null;
            string genreTitle = null;
            string subGenreName = null;
            
            foreach (string arg in args)
            {
                if (HelpRegex.IsMatch(arg))
                {
                    return new[] { GetHelp() };
                }

                if (filepath == null && FilepathRegex.IsMatch(arg))
                {
                    filepath = FilepathRegex.Match(arg).Groups[1].Value;
                    _musicCatalogReader = MusicCatalogReader.CreateFromXml(filepath);
                    if (_musicCatalogReader == null)
                    {
                        return new[] {"Couldn't open file: \"" + filepath + "\"", "Exiting..."};
                    }
                }

                if (artistName == null && ArtistRegex.IsMatch(arg))
                {
                    artistName = ArtistRegex.Match(arg).Groups[1].Value;
                }
                
                if (albumName == null && AlbumRegex.IsMatch(arg))
                {
                    albumName = AlbumRegex.Match(arg).Groups[1].Value;
                }

                if (songName == null && SongRegex.IsMatch(arg))
                {
                    songName = SongRegex.Match(arg).Groups[1].Value;
                }

                if (compilationName == null && CompilationRegex.IsMatch(arg))
                {
                    compilationName = CompilationRegex.Match(arg).Groups[1].Value;
                }

                if (genreTitle == null && GenreRegex.IsMatch(arg))
                {
                    genreTitle = GenreRegex.Match(arg).Groups[1].Value;
                }

                if (subGenreName == null && SubGenreRegex.IsMatch(arg))
                {
                    subGenreName = SubGenreRegex.Match(arg).Groups[1].Value;
                }
            }

            if (filepath == null)
            {
                return new[] {"--filepath= parameter required", "Exiting..."};
            }

            string[] result = _musicCatalogReader.GetData(
                artistName, 
                albumName, 
                songName, 
                genreTitle, 
                subGenreName, 
                compilationName);
            if (result.Length == 0)
            {
                return new[] {"No data found"};
            }

            return result;
        }

        private MusicCatalogReader _musicCatalogReader;
        
        private static readonly Regex HelpRegex = new Regex(@"(--help)|(-h)");
        private static readonly Regex FilepathRegex = new Regex(@"--filepath=(.+)");
        private static readonly Regex ArtistRegex = new Regex(@"--artist=(.+)");
        private static readonly Regex AlbumRegex = new Regex(@"--album=(.+)");
        private static readonly Regex SongRegex = new Regex(@"--song=(.+)");
        private static readonly Regex GenreRegex = new Regex(@"--genre=(.+)");
        private static readonly Regex SubGenreRegex = new Regex(@"--subgenre=(.+)");
        private static readonly Regex CompilationRegex = new Regex(@"--compilation=(.+)");
        
        private string GetHelp()
        {
            return "Supported MusicCatalogReader flags:\n" +
                   "\t-h, --help\t\t\tprint help\n" +
                   "\t--filepath=FILEPATH\t\tload data from xml file on FILEPATH\n" +
                   "\t--artist=ARTIST\t\t\tget music of ARTIST\n" +
                   "\t--album=ALBUM\t\t\tget music of ALBUM\n" +
                   "\t--song=SONG\t\t\tget music with song name equals SONG\n" +
                   "\t--genre=GENRE\t\t\tget music with genre of GENRE (will be ignored if --subgenre=\"\" used)\n" +
                   "\t--subgenre=SUBGENRE\t\tget music with subgenre of SUBGENRE\n" +
                   "\t--compilation=COMPILATION\tget music of COMPILATION";
        }
    }
}
