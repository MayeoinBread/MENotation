namespace MENotation.XmlHandling.Classes
{
    public class Transpose
    {
        public int Diatonic { get; set; }
        public int Chromatic { get; set; }
        public int OctaveChange { get; set; }

        public Transpose()
        {
            Diatonic = 0;
            Chromatic = 0;
            OctaveChange = 0;
        }
    }
}
