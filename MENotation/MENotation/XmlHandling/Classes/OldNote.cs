using MENotation.ScoreRendering;

namespace MENotation.XmlHandling.Classes
{
    public class OldNote
    {
        internal Pitch Pitch { get; set; }
        internal int Duration { get; set; }
        internal NoteType Type { get; set; }
        internal bool IsRest { get; set; }
        internal string Voice { get; set; }
        internal Lyric Lyric { get; set; }
        internal Beam Beam { get; set; }

        // For rendering
        internal double staveLine { get; set; }
        internal bool stemUp { get; set; }
        internal double stemLength { get; set; }
        internal double xPos { get; set; }

        internal AccidentalType Accidental { get; set; }
        internal bool IsChord {  get; set; }

        internal OldNote()
        {
            Duration = 0;
            Type = NoteType.whole;
            Voice = string.Empty;
            Accidental = AccidentalType.None;

            IsRest = false;
            IsChord = false;
            stemUp = true;

        }

    }
}
