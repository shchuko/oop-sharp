using System.Collections.Generic;

namespace MusicCatalogLib
{
    internal class Album
    {
        internal Album(string title, int year, Genre genre)
        {
            _title = title;
            _year = year;
            _genre = genre;
            _subGenre = null;
        }

        internal Album(string title, int year, SubGenre subGenre)
        {
            _title = title;
            _year = year;
            _genre = null;
            _subGenre = subGenre;
        }

        internal string GetGenreTitle()
        {
            if (_genre == null)
            {
                return _subGenre.ToString();
            }

            return _genre.ToString();
        }
        internal string Title
        {
            get => _title;
        }
        
        internal int Year
        {
            get => _year;
        }
        
        internal Genre Genre
        {
            get => _genre;
        }
        
        internal SubGenre SubGenre
        {
            get => _subGenre;
        }

        internal Dictionary<string, Song> Songs
        {
            get => _songs;
        }
        
        internal Song GetSong(string songTitle)
        {
            return _songs[songTitle];
        }

        internal void AddSong(Song song)
        {
            _songs.Add(song.ToString(), song);
        }

        public override string ToString()
        {
            return _title;
        }

        private string _title;
        private int _year;
        private Genre _genre;
        private SubGenre _subGenre;
        private readonly Dictionary<string, Song> _songs = new Dictionary<string, Song>();
    }
}