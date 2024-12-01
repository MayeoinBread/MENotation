using System.Collections.Generic;

namespace MENotation.XmlHandling.Classes
{
    public class Identification
    {
        public List<string> Creator {  get; set; }

        public List<string> Rights { get; set; }

        public Encoding Encoding { get; set; }

        public List<string> Source { get; set; }

        public Identification()
        {
            Creator = [];
            Rights = [];
            Source = [];
        }
    }
}
