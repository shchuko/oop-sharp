using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;


namespace MusicCatalogLib
{
    public class MusicCatalogXmlParser
    {
        internal static MusicCatalogReader CreateFromXml(XDocument dataDoc)
        {
            if (dataDoc == null)
            {
                return null;
            }
            MusicCatalogXmlParser parser = new MusicCatalogXmlParser(dataDoc);
            return parser._musicCatalogReader;
        }
        
        private MusicCatalogXmlParser(XDocument dataDoc)
        {
            _dataDoc = dataDoc;
            _musicCatalogReader = new MusicCatalogReader();
            ParseStart();
        }

        private void ParseStart()
        {
            try
            {
                ParseGenres();
                ParseSubgenres();
                ParseSongs();
                ParseCompilations();
            }
            catch (NullReferenceException)
            {
                _musicCatalogReader = null;
            }
        }

        private void ParseGenres()
        {
            IEnumerable<string> genres =
                from element in _dataDoc.Root.Element("basegenres").Elements("title")
                select element.Value;
            _musicCatalogReader.AddGenres(genres.ToArray());
        }
        
        private void ParseSubgenres()
        {
            IEnumerable<XElement> subgenres =
                from element in _dataDoc.Root.Element("subgenres").Elements("subgenre")
                select element;

            foreach (XElement subgenre in subgenres)
            {
                string title = subgenre.Element("title").Value;
                IEnumerable<string> baseGenres =
                    from element in subgenre.Elements("base")
                    select element.Value;
                _musicCatalogReader.AddSubGenre(title, baseGenres.ToArray());
            }
        }

        private void ParseSongs()
        {
            IEnumerable<XElement> artists =
                from element in _dataDoc.Root.Elements("artist")
                select element;
            foreach (XElement artist in artists)
            {
                string artistTitle = artist.Element("title").Value;
                _musicCatalogReader.AddArtist(artistTitle);
                
                IEnumerable<XElement> albums = artist.Elements("album");
                foreach (XElement album in albums)
                {
                    string albumTitle = album.Element("title").Value;
                    int albumYear = Convert.ToInt32(album.Element("year").Value);
                    bool subGenreFlag = album.Element("subgenre") != null;
                    if (subGenreFlag)
                    {
                        string subGenre = album.Element("subgenre").Value;
                        _musicCatalogReader.AddAlbum(artistTitle, albumTitle, albumYear, subGenre, true);
                    }
                    else
                    {
                        string baseGenre = album.Element("genre").Value;
                        _musicCatalogReader.AddAlbum(artistTitle, albumTitle, albumYear, baseGenre, false);
                    }

                    IEnumerable<string> songs =
                        from element in album.Elements("song")
                        select element.Element("title").Value;
                    
                    foreach (var songTitle in songs)
                    {
                        _musicCatalogReader.AddSong(artistTitle, albumTitle, songTitle);
                    }
                }
            }
        }

        private void ParseCompilations()
        {
            IEnumerable<XElement> compilations =
                from element in _dataDoc.Root.Elements("compilation")
                select element;
            foreach (XElement compilation in compilations)
            {
                string compilationTitle = compilation.Element("title").Value;
                IEnumerable<XElement> songsNodes =
                    from element in compilations.Elements("song")
                    select element;
                
                List<MusicCatalogReader.SongProps> songs = new List<MusicCatalogReader.SongProps>();
                foreach (XElement songNode in songsNodes)
                {
                    songs.Add(new MusicCatalogReader.SongProps(
                        songNode.Element("title").Value,
                        songNode.Element("album").Value,
                        songNode.Element("artist").Value));
                }
                
                _musicCatalogReader.AddCompilation(compilationTitle, songs.ToArray());
            }
        }
        
        private MusicCatalogReader _musicCatalogReader;
        private XDocument _dataDoc;
    }
}