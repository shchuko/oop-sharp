namespace MusicCatalogLib
{
    internal class Song
    {
        internal Song(string title)
        {
            _title = title;
        }

        public override string ToString()
        {
            return _title;
        }

        private string _title;
    }
}