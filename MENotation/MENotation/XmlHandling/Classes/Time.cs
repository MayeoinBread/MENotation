namespace MENotation.XmlHandling.Classes
{
    public class Time
    {
        public int Beats { get; set; }
        public int BeatType { get; set; }

        public Time()
        {
            Beats = 0;  // upper
            BeatType = 0;  // lower
        }
    }
}
