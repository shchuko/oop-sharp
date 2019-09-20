using System.Collections.Generic;

namespace MusicCatalogLib
{
    internal class Artist
    {
        internal Artist(string title)
        {
            _title = title;
        }

        internal string Title
        {
            get => _title;
        }

        public Dictionary<string, Album> Albums
        {
            get => _albums;
        }
        
        internal Album GetAlbum(string albumTitle)
        {
            return _albums[albumTitle];
        }

        internal void AddAlbum(Album album)
        {
            _albums.Add(album.ToString(), album);
        }

        public override string ToString()
        {
            return _title;
        }

        private string _title;
        private readonly Dictionary<string, Album> _albums = new Dictionary<string, Album>();
    }
}