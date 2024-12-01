using System.Collections.Generic;

namespace MENotation.XmlHandling.Classes
{
    public class Score
    {
        public Work Work { get; set; }
        public Identification Identification { get; set; }
        public List<Part> PartList { get; set; }

        public Score()
        {
            PartList = [];
        }
    }
}
