namespace MENotation.XmlHandling.Classes
{
    public class Pitch
    {
        internal char Step { get; set; }

        // 0-9, 4=middleC
        internal int Octave { get; set; }
        
        // Sharp/flat (1/-1)
        // Not used for display, only playback (MIDI)
        internal int Alter { get; set; }

        internal Pitch()
        {
            Step = new char();
            Octave = 0;
            Alter = 0;
        }
    }
}
