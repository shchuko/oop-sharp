using System;
using System.Text;

namespace MusicCatalogLib
{
    public class MusicCatalogReader
    {
        public string GetHelp()
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

        public bool ReadFile(string filename)
        {
            throw new NotImplementedException();
        }
        
        public string[] GetData(
            string artistName, 
            string albumName, 
            string songName, 
            string genreName, 
            string subGenreName,
            string compilationName)
        {
            throw new NotImplementedException();
        }


    }
}
