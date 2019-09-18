using System;
using System.Collections.Generic;
using System.Text;

namespace MusicCatalog
{
    class Album
    {
        private static int albumIDCounter = 0;
        internal readonly int albumID = 0;
        internal readonly string albumName;
        internal readonly int albumYear;
        internal readonly string albumCountry;

        public Album(int year, string name, string country)
        {
            albumID = ++albumIDCounter;
            albumYear = year;
            albumName = name;
            albumCountry = country;
        }

    }
}
