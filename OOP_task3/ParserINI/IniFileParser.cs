using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using IniParser;
using IniParser.Model;
using ParserINI.Exceptions;


namespace ParserINI
{
    public class IniFileParser
    {
        public IniFileParser(string filepath)
        {
            if (filepath == null)
            {
                throw new NullReferenceException();
            }
            
            if (!new Regex(@".*\.ini").IsMatch(filepath))
            {
                throw new WrongFileFormatException();
            }
            
            string fileText;
            try
            {
                // reading file and deleting comments
                fileText = Regex.Replace(File.ReadAllText(filepath), @";.*$", "", RegexOptions.Multiline);
            }
            catch (Exception)
            {
                throw new FileReadingException();
            }

            try
            {
                _data = new StringIniParser().ParseString(fileText);
            }
            catch (Exception)
            {
                throw new FileReadingException();
            }
        }

        public string GetStringValue(string section, string key)
        {
            return GetValue(section, key);
        }

        public int GetIntValue(string section, string key)
        {
            int result;
            try
            {
                result = Convert.ToInt32(GetValue(section, key));
            }
            catch (FormatException)
            {
                throw new ValueCastException();
            }
            return result;
        }

        public long GetLongValue(string section, string key)
        {
            long result;
            try
            {
                result = Convert.ToInt64(GetValue(section, key));
            }
            catch (FormatException)
            {
                throw new ValueCastException();
            }
            return result;
        }

        public double GetDoubleValue(string section, string key)
        {
            double result;
            try
            {
                result = Convert.ToDouble(GetValue(section, key));
            }
            catch (FormatException)
            {
                throw new ValueCastException();
            }
            return result;
        }
        
        private IniData _data;

        private string GetValue(string section, string key)
        {
            if (!_data.Sections.ContainsSection(section))
            {
                throw new SectionNotFoundException(section);
            }

            KeyDataCollection keyMap = _data[section];;
            if (!keyMap.ContainsKey(key))
            {
                throw new KeyNotFoundException(key);
            }
            
            return keyMap[key];
        }
    }
}