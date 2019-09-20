using System.Collections.Generic;

namespace MusicCatalogLib
{
    internal class Genre
    {
        internal Genre(string title)
        {
            _title = title;
        }
        
        internal string Title
        {
            get => _title;
        }

        internal void AddSubGenre(SubGenre subGenre)
        {
            _subGenres.Add(subGenre);
        }

        internal bool HasSubGenre(SubGenre subGenre)
        {
            return _subGenres.Contains(subGenre);
        }

        public override string ToString()
        {
            return _title;
        }

        private string _title;
        private HashSet<SubGenre> _subGenres = new HashSet<SubGenre>();
    }

}