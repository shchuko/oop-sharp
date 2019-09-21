using System;
using System.Text.RegularExpressions;


namespace MusicCatalogLib
{
    public class ArgumentsParser
    {
        public string[] ParseAndExec(string[] args)
        {
            string filepath = null;
            string artistTitle = null;
            string albumTitle = null;
            string songTitle = null;
            string compilationTitle = null;
            string genreTitle = null;
            string subGenreTitle = null;
            
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
                        return new[] {"File reading error: \"" + filepath + "\""};
                    }
                }

                if (artistTitle == null && ArtistRegex.IsMatch(arg))
                {
                    artistTitle = ArtistRegex.Match(arg).Groups[1].Value;
                }
                
                if (albumTitle == null && AlbumRegex.IsMatch(arg))
                {
                    albumTitle = AlbumRegex.Match(arg).Groups[1].Value;
                }

                if (songTitle == null && SongRegex.IsMatch(arg))
                {
                    songTitle = SongRegex.Match(arg).Groups[1].Value;
                }

                if (compilationTitle == null && CompilationRegex.IsMatch(arg))
                {
                    compilationTitle = CompilationRegex.Match(arg).Groups[1].Value;
                }

                if (genreTitle == null && GenreRegex.IsMatch(arg))
                {
                    genreTitle = GenreRegex.Match(arg).Groups[1].Value;
                }

                if (subGenreTitle == null && SubGenreRegex.IsMatch(arg))
                {
                    subGenreTitle = SubGenreRegex.Match(arg).Groups[1].Value;
                }
            }

            if (filepath == null)
            {
                return new[] {"--filepath= parameter required"};
            }

            string[] result = _musicCatalogReader.GetData(
                artistTitle, 
                albumTitle, 
                songTitle, 
                genreTitle, 
                subGenreTitle, 
                compilationTitle);
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
