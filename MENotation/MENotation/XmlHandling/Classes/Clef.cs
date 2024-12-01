namespace MENotation.XmlHandling.Classes
{
    public class Clef
    {
        // G, F, C, percussion, TAB, jianpu
        public string Sign { get; set; }

        // G=2, F=4, C=3
        public int Line { get; set; }

        public Clef()
        {
            Line = 0;
            Sign = string.Empty;
        }
    }
}
