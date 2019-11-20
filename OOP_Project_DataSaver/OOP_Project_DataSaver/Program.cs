using System;
using System.IO;
using System.Security.Cryptography;
using CommandLine;
using FileCryptoRepository;

namespace OOP_Project_DataSaver
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => { options = o; });

            if (options.MaxSize == -1)
            {
                FileRepository fileRepository = null;
                try
                {
                    fileRepository = new FileRepository(options.RepoPath, options.Password);
                }
                catch (IOException)
                {
                    Console.WriteLine("Wrong path to repo");
                    return;
                }
                catch (CryptographicException)
                {
                    Console.WriteLine("Incorrect password");
                    return;
                }
                catch (Exception)
                {
                    return;
                }
                
                BasicRepoOperationsHandler(fileRepository, options);
                fileRepository.Dispose();
            }
            else
            {
                using (var fileRepository = new FileRepository(options.RepoPath, options.MaxSize, options.Password, options.OverWrite))
                {
                    BasicRepoOperationsHandler(fileRepository, options);
                }
            }
            
        }

        static void BasicRepoOperationsHandler(FileRepository fileRepository, Options options)
        {
            foreach (string inputFile in options.InputFiles)
            {
                try
                {
                    fileRepository.AddFile(inputFile);
                    Console.WriteLine("File " + inputFile + " successfully added");
                }
                catch (Exception)
                {
                    Console.WriteLine("File " + inputFile + " adding error. Skipping...");
                }
                        
            }
                    
                    
            foreach (string inputFile in options.DeleteFiles)
            {
                if (fileRepository.RemoveFile(inputFile))
                {
                    Console.WriteLine("Removing file " + inputFile + " successful");
                }
                else
                {
                    Console.WriteLine("Removing file " + inputFile + " error. File not found");
                }
                        
            }
                    
            if (options.ExtractFlag)
            {
                try
                {
                    fileRepository.ExtractDataToDir(options.OutDir);
                    Console.WriteLine("Extracting successful");
                }
                catch (Exception)
                {
                    Console.WriteLine("Extracting error. Operation aborted");
                }
            }
        }
    }
}