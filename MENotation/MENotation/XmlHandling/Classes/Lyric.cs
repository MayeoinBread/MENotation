using System.Collections.Generic;

namespace MENotation.XmlHandling.Classes
{
    public enum Syllabic
    {
        begin, end, middle, single
    }

    public class Lyric
    {
        internal List<int> Number { get; set; }
        internal List<string> Text { get; set; }
        internal List<string> Elision { get; set; }
        public List<Syllabic> Syllabic { get; set; }

        public Lyric()
        {
            Number = [];
            Text = [];
            Elision = [];
            Syllabic = [];
        }
    }
}
