using System;
using System.Xml;

namespace MusicCatalogLib
{
    public class MusicCatalogXmlParser
    {
        internal static MusicCatalogReader CreateFromXml(XmlDocument dataDoc)
        {
            if (dataDoc == null)
            {
                return null;
            }
            MusicCatalogXmlParser parser = new MusicCatalogXmlParser(dataDoc);
            return parser._musicCatalogReader;
        }
        
        private MusicCatalogXmlParser(XmlDocument dataDoc)
        {
            _dataDoc = dataDoc;
            _musicCatalogReader = new MusicCatalogReader();
        }

        private void ParseBegin()
        {
            ParseGenres();
            ParseSubgenres();
            ParseSongs();
            ParseCompilations();
        }

        private void ParseGenres()
        {
            throw new NotImplementedException();
        }

        private void ParseSubgenres()
        {
            throw new NotImplementedException();
        }

        private void ParseSongs()
        {
            throw new NotImplementedException();
        }

        private void ParseCompilations()
        {
            throw new NotImplementedException();
        }

        private MusicCatalogReader _musicCatalogReader;
        private XmlDocument _dataDoc;
    }
}