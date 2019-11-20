using System;
using System.IO;

namespace FileCryptoRepository.Core
{
    [Serializable]
    internal class RepoFileInfo
    {
        internal RepoFileInfo(string filename, byte[] byteArray)
        {
            _filename = filename;
            _bytes = byteArray;
        }

        internal RepoFileInfo(string filename)
        {
            _filename = filename;
            _bytes = File.ReadAllBytes(filename);
        }

        internal byte[] GetBytes()
        {
            return _bytes;
        }

        internal int GetBytesLength()
        {
            return _bytes.Length;
        }

        internal string GetShortFilename()
        {
            return Path.GetFileNameWithoutExtension(_filename);
        }
        
        internal string GetFilename()
        {
            return _filename;
        }

        private readonly string _filename;
        
        private readonly byte[] _bytes;
    }
}