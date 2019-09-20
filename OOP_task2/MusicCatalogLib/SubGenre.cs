using System.Collections.Generic;

namespace MusicCatalogLib
{
    internal class SubGenre
    {
        internal SubGenre(string title, Genre[] baseGenres)
        {
            _title = title;
            foreach (Genre baseGenre in baseGenres)
            {
                baseGenre.AddSubGenre(this);
                _baseGenres.Add(baseGenre);
            }
        }
        
        internal string Title
        {
            get => _title;
        }
        
        internal bool HasBaseGenre(Genre genre)
        {
            return _baseGenres.Contains(genre);
        }
        
        public override string ToString()
        {
            return _title;
        }
        
        private string _title;
        private HashSet<Genre> _baseGenres = new HashSet<Genre>();
    }
}