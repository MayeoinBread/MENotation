namespace MENotation.XmlHandling.Classes
{
    internal class OldDirection
    {
        // TODO flesh out with other bits
        public string Placement { get; set; }
        public NoteType BeatUnit { get; set; }
        
        public int PerMinute {  get; set; }

        public OldDirection()
        {
            Placement = string.Empty;
            BeatUnit = NoteType.unknown;
            PerMinute = 0;
        }
    }
}
