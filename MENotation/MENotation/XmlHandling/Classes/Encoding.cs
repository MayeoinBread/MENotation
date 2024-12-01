using System.Collections.Generic;

namespace MENotation.XmlHandling.Classes
{
    public class Encoding
    {
        public List<string> Date {  get; set; }
        public List<string> Encoder { get; set; }
        public List<string> Software { get; set; }
        public List<string> Description { get; set; }

        public Encoding()
        {
            Date = [];
            Encoder = [];
            Software = [];
            Description = [];
        }
    }
}
