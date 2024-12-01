using System.Collections.Generic;

namespace MENotation.XmlHandling.Classes
{
    public class Measure
    {
        public string Number { get; set; }
        public float Width { get; set; }
        //public Attributes Attributes { get; set; }
        //public Direction Direction { get; set; }

        //public List<Note> Notes { get; set; }
        public List<MeasureElement> Elements { get; set; }

        public Measure()
        {
            Number = string.Empty;
            Width = -1;
            Elements = [];
        }
    }
}
