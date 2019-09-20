using System.Collections.Generic;

namespace MusicCatalogLib
{
    internal class Compilation
    {
        internal Compilation(string title, Song[] songs)
        {
            _title = title;
            foreach (Song song in songs)
            {
                _songs.Add(song);
            }
        }

        internal bool ContainsSong(Song song)
        {
            return _songs.Contains(song);
        }
        
        internal string Title
        {
            get => _title;
        }
        
        private string _title;
        private HashSet<Song> _songs = new HashSet<Song>();
    }
}