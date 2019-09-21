using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace MusicCatalogLib
{
    public class MusicCatalogReader
    {
        public struct SongProps
        {
            public SongProps(string songTitle, string albumTitle, string artistTitle)
            {
                this.SongTitle = songTitle;
                this.AlbumTitle = albumTitle;
                this.ArtistTitle = artistTitle;
            }
            
            public string SongTitle;
            public string AlbumTitle;
            public string ArtistTitle;
        }
        
        public static MusicCatalogReader CreateFromXml(string filepath)
        {
            XDocument doc;
            try
            {
                doc = XDocument.Load(filepath);
            }
            catch (Exception)
            {
                return null;
            }
            
            return MusicCatalogXmlParser.CreateFromXml(doc);
        }

        public string[] GetData(
            string artistTitle,
            string albumTitle,
            string songTitle,
            string genreTitle,
            string subGenreTitle,
            string compilationTitle)
        {
            List<string> result = new List<string>();
            try
            {
                var list = GetPartData(artistTitle, albumTitle, songTitle, genreTitle, subGenreTitle,
                    compilationTitle);
                if (list.Count == 0)
                {
                    result.Add("No data");
                }
                else
                {
                    result.AddRange(list);
                }
            }
            catch (KeyNotFoundException)
            {
                result.Add("No data");
            }

            return result.ToArray();
        }

        public void AddGenres(string[] genresTitles)
        {
            foreach (string title in genresTitles)
            {
                _genres.Add(title, new Genre(title));
            }
        }

        public void AddSubGenre(string subGenreTitle, string[] baseGenreTitles)
        {
            List<Genre> baseGenres = new List<Genre>();
            foreach (string baseGenreTitle in baseGenreTitles)
            {
                baseGenres.Add(_genres[baseGenreTitle]);
            }
            SubGenre newSubGenre = new SubGenre(subGenreTitle, baseGenres.ToArray());
            foreach (Genre baseGenre in baseGenres)
            {
                baseGenre.AddSubGenre(newSubGenre);
            }
            _subGenres.Add(subGenreTitle, newSubGenre);
        }

        public void AddArtist(string artistTitle)
        {
            _artists.Add(artistTitle, new Artist(artistTitle));
        }

        public void AddAlbum(string atristTitle, string albumTitle, int albumYear, string genreTitle, bool subGenreFlag)
        {
            Album album;
            if (subGenreFlag)
            { 
                album = new Album(albumTitle, albumYear, GetSubGenre(genreTitle));
            }
            else
            { 
                album = new Album(albumTitle, albumYear, GetGenre(genreTitle));
            }
            _artists[atristTitle].AddAlbum(album);
        }

        public void AddSong(string artistTitle, string albumTitle, string songTitle)
        {
            _artists[artistTitle].GetAlbum(albumTitle).AddSong(new Song(songTitle));
        }

        public void AddCompilation(string compipationTitle, SongProps[] songData)
        {
            if (compipationTitle == null || compipationTitle.Equals(""))
                return;
            
            List<Song> songs = new List<Song>();
            foreach (SongProps songProps in songData)
            {
                Song tempSong;
                try
                {
                    tempSong = GetSong(songProps.ArtistTitle, songProps.AlbumTitle, songProps.SongTitle);
                }
                catch (Exception)
                {
                    continue;
                }
                songs.Add(tempSong);
            }
            _compilations.Add(compipationTitle, new Compilation(compipationTitle, songs.ToArray()));
        }
        
        internal void AddCompilation(Compilation compilation)
        {
            _compilations.Add(compilation.ToString(), compilation);
        }
        internal Song GetSong(string artistTitle, string albumTitle, string songTitle)
        {
            return _artists[artistTitle].GetAlbum(albumTitle).GetSong(songTitle);
        }

        internal Genre GetGenre(string genreTitle)
        {
            return _genres[genreTitle];
        }

        internal SubGenre GetSubGenre(string subGebreTitle)
        {
            return _subGenres[subGebreTitle];
        }

        // maps of <structure_name, structure>
        private Dictionary<string, Genre> _genres = new Dictionary<string, Genre>();
        private Dictionary<string, SubGenre> _subGenres = new Dictionary<string, SubGenre>();
        private Dictionary<string, Artist> _artists = new Dictionary<string, Artist>();
        private Dictionary<string, Compilation> _compilations = new Dictionary<string, Compilation>();
        
        private List<string> GetPartData(
            string artistTitle,
            string albumTitle,
            string songTitle,
            string genreTitle,
            string subGenreTitle,
            string compilationTitle)
        {
            if (artistTitle != null && !artistTitle.Equals(""))
            {
                Artist artistToLook = _artists[artistTitle];
                return GetPartData(artistTitle, albumTitle, artistToLook.Albums, songTitle, genreTitle, subGenreTitle,
                    compilationTitle);
            }
            
            List<string> result = new List<string>();
            foreach (KeyValuePair<string,Artist> keyValuePair in _artists)
            {
                result.AddRange(GetPartData(keyValuePair.Key, albumTitle, keyValuePair.Value.Albums, songTitle,
                    genreTitle, subGenreTitle, compilationTitle));
            }

            return result;
        }
        private List<string> GetPartData(
            string artistTitle, 
            string albumTitle, 
            Dictionary<string, Album> albums, 
            string songTitle,
            string genreTitle, 
            string subGenreTitle,
            string compilationTitle)
        {
            List<string> result = new List<string>();

            if (albumTitle != null && !albumTitle.Equals(""))
            {
                if (!albums.ContainsKey(albumTitle))
                {
                    return result;
                }
                
                Album albumToLook = albums[albumTitle];
                if (CheckAlbumGenres(albumToLook, genreTitle, subGenreTitle))
                {
                    return GetPartData(artistTitle, albumTitle, albumToLook.GetGenreTitle(), albumToLook.Songs, songTitle,
                        compilationTitle);
                }
                return result;
            }
            
            foreach (var album in albums)
            {
                if (CheckAlbumGenres(album.Value, genreTitle, subGenreTitle))
                {
                    result.AddRange(GetPartData(artistTitle, album.Key, album.Value.GetGenreTitle(), 
                        album.Value.Songs, songTitle, compilationTitle));
                }
            }

            return result;
        }

        private List<string> GetPartData(
            string artistTitle, 
            string albumTitle, 
            string genre,
            Dictionary<string, Song> songs, 
            string songTitle, 
            string compilationTitle)
        {
            List<string> result = new List<string>();
            if (songTitle != null && !songTitle.Equals(""))
            {
                if (!songs.ContainsKey(songTitle))
                {
                    return result;
                }

                if (compilationTitle == null || compilationTitle.Equals("") ||
                    _compilations[compilationTitle].ContainsSong(songs[songTitle]))
                {
                    result.Add(CreateString(artistTitle, albumTitle, genre, songTitle));
                }
                return result;

            } 
            foreach (KeyValuePair<string, Song> songPair in songs)
            {
                if (compilationTitle == null || compilationTitle.Equals("") || _compilations[compilationTitle].ContainsSong(songPair.Value))
                {
                    result.Add(CreateString(artistTitle, albumTitle, genre, songPair.Key));
                }
            }
            
            return result;
        }

        private bool CheckAlbumGenres(Album album, string baseGenreTitle, string subGenreTitle)
        {
            if (subGenreTitle != null && !subGenreTitle.Equals(""))
            {
                if (album.SubGenre != null)
                {
                    return album.SubGenre.Title.Equals(subGenreTitle);
                }

                return false;
            }
            if (baseGenreTitle != null && !baseGenreTitle.Equals(""))
            {
                if (album.SubGenre != null)
                {
                    return album.SubGenre.HasBaseGenre(GetGenre(baseGenreTitle));
                }

                return album.Genre.Title.Equals(baseGenreTitle);
            }

            return true;
        }
        private static string CreateString(params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < args.Length; i++)
            {
                var item = args[i];
                if (item != null && !item.ToString().Equals(""))
                {
                    sb.Append(item);
                }
                else
                {
                    sb.Append("-----");
                }

                if (i != args.Length - 1)
                    sb.Append("  --  ");
            }

            return sb.ToString();
        }
    }
}
