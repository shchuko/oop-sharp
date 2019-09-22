namespace OOP_task3
{
    class Program
    {
        static void Main(string[] args)
        {
            ParserTester.Test("Wrong file format test", "filename");
            ParserTester.Test("File open error test", "filename.ini");
            ParserTester.Test("No section test", "/home/shchuko/ini/noSection.ini");
            ParserTester.Test("No key test", "/home/shchuko/ini/noKey.ini");
            ParserTester.Test("Cast error test", "/home/shchuko/ini/castError.ini");
            ParserTester.Test("Good ini test", "/home/shchuko/ini/goodIni.ini");
        }
    }
}