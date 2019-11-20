using System.Collections.Generic;
using FileCryptoRepository;
using CommandLine;
using CommandLine.Text;

namespace OOP_Project_DataSaver
{
    public class Options
    {
        [Option('o', "pathrepo", Required = true, HelpText = "Path to repository")]
        public string RepoPath { get; set; }

        [Option('p', "password", Required = false, HelpText = "Encryption password")]
        public string Password { get; set; } = null;

        [Option('s', "size", Required = false, HelpText = "Maximum size of repository")]
        public int MaxSize { get; set; } = -1;

        [Option('f', "force", Required = false, HelpText = "Force overwrite the repo file if exists")]
        public bool OverWrite { get; set; } = false;
        
        [Option('a', "add", Required = false, Separator = ',', HelpText = "Input files to be added to repo")]
        public IEnumerable<string> InputFiles { get; set; }
        
        [Option('r', "remove", Required = false, Separator = ',', HelpText = "Input files to be removed from repo")]
        public IEnumerable<string> DeleteFiles { get; set; }

        [Option('d', "dir", Required = false, HelpText = "Path to dir to extract files")]
        public string OutDir { get; set; } = "";

        [Option('e', "extract", Required = false, HelpText = "Extract files from repository")]
        public bool ExtractFlag { get; set; } = false;
        
        
    }
}