using System;
using System.Collections.Generic;
using System.Text;

namespace MusicCatalog
{
    class Artist
    {
        private int artistIDCounter = 0;
        internal readonly int artistID = 0;
        internal readonly string artistName;
        internal readonly int artistYear;
        internal readonly string artistCountry;

        public Artist(string name, int year, string country)
        {
            artistID = ++artistIDCounter;
            artistName = name;
            artistYear = year;
            artistCountry = country;
        }

    }
}
