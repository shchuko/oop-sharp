using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FileCryptoRepository.Core;

namespace FileCryptoRepository
{
    public class FileRepository : IDisposable
    {
        public FileRepository(string inputFilepath, string password)
        {
            _filepath = inputFilepath;
            _password = password;
            ReadData();
        }
        
        public FileRepository(string outputFilepath, int maxSizeBytes, string password, bool overwriteFlag)
        {
            if (!overwriteFlag && File.Exists(outputFilepath))
                throw new InvalidOperationException("File already exists, overwrite flag set to false");
            _filepath = outputFilepath;
            _password = password;
            _maxSizeBytes = maxSizeBytes;
            _usedSizeBytes = 0;
        }

        public void SetFilepath(string outputFilepath)
        {
            _filepath = outputFilepath;
        }
        
        public void AddFile(string filepath)
        {
            var bytes = File.ReadAllBytes(filepath);
            if (bytes.Length > _maxSizeBytes - _usedSizeBytes)
            {
                throw new OutOfMemoryException("File size is too big to save it into repository");
            }

            if (_filenames.Contains(Path.GetFileNameWithoutExtension(filepath)))
            {
                throw new InvalidOperationException("Duplicate files");
            }
            
            _filenames.Add(Path.GetFileNameWithoutExtension(filepath));
            _files.Add(new RepoFileInfo(filepath, bytes));
            _usedSizeBytes += bytes.Length;
        }

        public void AddFiles(IEnumerable<string> filenames)
        {
            foreach (string filename in filenames)
            {
                AddFile(filename);
            }
        }

        public bool RemoveFile(string filepath)
        {
            if (!_filenames.Contains(Path.GetFileNameWithoutExtension(filepath)))
                return false;
            
            var index = _filenames.IndexOf(Path.GetFileNameWithoutExtension(filepath));
            _filenames.RemoveAt(index);
            _files.RemoveAt(index);
            return true;
        }
        
        public void WriteData()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            var temp = new MemoryStream();
            serializer.Serialize(temp, _files);
            int serializedSpaceBytes = (int) temp.Length;
            
            MemoryStream ms = new MemoryStream();
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(_maxSizeBytes);
                writer.Write(_usedSizeBytes); 
                writer.Write(serializedSpaceBytes);
                foreach (byte b in temp.ToArray())
                {
                    writer.Write(b);
                }
                
                Random r = new Random();
                for (int i = _usedSizeBytes; i < _maxSizeBytes; ++i)
                {
                    writer.Write((byte) r.Next(0, 255));
                }
            }

            if (_password != null)
                ms = new MemoryStream(SymmetricCrypt.Aes256Encrypt(ms.ToArray(), _password));

            File.WriteAllBytes(_filepath, ms.ToArray());
        }

        public void ExtractDataToDir(string dirname)
        {
            if (!Directory.Exists(dirname))
                Directory.CreateDirectory(dirname);
            foreach (RepoFileInfo fileInfo in _files)
            {
                File.WriteAllBytes(Path.Combine(dirname, fileInfo.GetShortFilename()), fileInfo.GetBytes());
            }
        }
        public void Dispose()
        {
            WriteData();
        }

        private readonly string _password;
        private string _filepath;
        private int _maxSizeBytes;
        private int _usedSizeBytes;
        private readonly List<string> _filenames = new List<string>();
        private List<RepoFileInfo> _files = new List<RepoFileInfo>();

        private void ReadData()
        {
            MemoryStream stream = new MemoryStream(File.ReadAllBytes(_filepath));
            
            if (_password != null)
            {
                stream = new MemoryStream(SymmetricCrypt.Aes256Decrypt(stream.ToArray(), _password));
            }

            using (BinaryReader br = new BinaryReader(stream))
            {
                _maxSizeBytes = br.ReadInt32();
                _usedSizeBytes = br.ReadInt32();
                int serializedSizeBytes = br.ReadInt32();

                byte[] bytes = br.ReadBytes(serializedSizeBytes);

                
                using (var memoryStream = new MemoryStream(bytes))
                {
                    var deserializer = new BinaryFormatter();
                    _files = (List<RepoFileInfo>) deserializer.Deserialize(memoryStream);
                }

                foreach (RepoFileInfo repoFileInfo in _files)
                {
                    _filenames.Add(repoFileInfo.GetFilename());
                }
            }
        }
    }
}