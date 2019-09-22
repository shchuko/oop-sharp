using System;
using  ParserINI;

namespace OOP_task3
{
    public static class ParserTester
    {
        public static void Test(string testName, string filepath)
        {
            Console.WriteLine("===== " + testName + " =====");
            IniFileParser parserIni = null;
            try
            {
                parserIni = new IniFileParser(filepath);
            }
            catch (ParserINI.Exceptions.WrongFileFormatException)
            {
                Console.Error.WriteLine("Wrong file format, filepath: " + filepath);
                return;
            }
            catch (ParserINI.Exceptions.FileReadingException)
            {
                Console.Error.WriteLine("Couldn't read file, filepath: " + filepath);
                return;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return;
            }

            try
            {
                Console.WriteLine("Int val: " + parserIni.GetIntValue("SOME_SECTION", "SomeInt"));
                Console.WriteLine("Long val: " + parserIni.GetLongValue("SOME_SECTION", "SomeLong"));
                Console.WriteLine("Double val: " + parserIni.GetDoubleValue("SOME_SECTION", "SomeDouble"));
                Console.WriteLine("String val: " + parserIni.GetStringValue("SOME_SECTION", "SomeString"));
            }
            catch (ParserINI.Exceptions.SectionNotFoundException e)
            {
                Console.Error.WriteLine("Section not found: " + e.GetSectionName());
            }
            catch (ParserINI.Exceptions.KeyNotFoundException e)
            {
                Console.Error.WriteLine("Key not found: " + e.GetKeyName());
            }
            catch (ParserINI.Exceptions.ValueCastException)
            {
                Console.Error.WriteLine("Cast exception handled");
            }
        }
    }
}