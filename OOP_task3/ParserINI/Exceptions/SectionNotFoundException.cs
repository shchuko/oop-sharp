namespace ParserINI.Exceptions
{
    public class SectionNotFoundException : System.Exception
    {
        public SectionNotFoundException()
        {
            _sectionName = "";
        }

        public SectionNotFoundException(string sectionName) : base(sectionName)
        {
            _sectionName = sectionName;
        }

        public string GetSectionName()
        {
            return _sectionName;
        }

        private string _sectionName;
    }
}