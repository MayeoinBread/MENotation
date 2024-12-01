using System.Collections.Generic;

namespace MENotation.XmlHandling.Classes
{
    public class Part
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<Measure> Measures { get; set; }
        public Part ()
        {
            Id = string.Empty;
            Name = string.Empty;
            Measures = [];
        }
    }
}
