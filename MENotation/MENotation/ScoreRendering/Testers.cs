using MENotation.XmlHandling;
using MENotation.XmlHandling.Classes;

namespace MENotation.ScoreRendering
{
    internal class Testers
    {
        public static void DrawAllNotesAndRests(RenderHelper _renderHelper)
        {
            _renderHelper.DrawClef(new Clef { Line = 2, Sign = "G" });
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.whole,
                Pitch = new Pitch { Octave = 4, Step = 'D' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.half,
                Pitch = new Pitch { Octave = 4, Step = 'E' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.quarter,
                Pitch = new Pitch { Octave = 4, Step = 'F' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'G' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.sixteenth,
                Pitch = new Pitch { Octave = 4, Step = 'A' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.sixteenth,
                Pitch = new Pitch { Octave = 5, Step = 'C' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 5, Step = 'D' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.quarter,
                Pitch = new Pitch { Octave = 5, Step = 'E' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.half,
                Pitch = new Pitch { Octave = 5, Step = 'F' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.whole,
                Pitch = new Pitch { Octave = 5, Step = 'G' }
            }, "G");

            _renderHelper.DrawRest(new Note
            {
                Type = NoteType.whole,
                IsRest = true,
            });
            _renderHelper.DrawRest(new Note
            {
                Type = NoteType.half,
                IsRest = true,
            });
            _renderHelper.DrawRest(new Note
            {
                Type = NoteType.quarter,
                IsRest = true,
            });
            _renderHelper.DrawRest(new Note
            {
                Type = NoteType.eighth,
                IsRest = true,
            });
            _renderHelper.DrawRest(new Note
            {
                Type = NoteType.sixteenth,
                IsRest = true,
            });
        }
        public static void DrawGClefScale(RenderHelper _renderHelper)
        {
            _renderHelper.DrawClef(new Clef { Line = 2, Sign = "G" });
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'C' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'D' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'E' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'F' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'G' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'A' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'B' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 5, Step = 'C' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 5, Step = 'D' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 5, Step = 'E' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 5, Step = 'F' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 5, Step = 'G' }
            }, "G");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 5, Step = 'A' }
            }, "G");
        }

        public static void DrawFClefScale(RenderHelper _renderHelper)
        {
            _renderHelper.DrawClef(new Clef { Line = 4, Sign = "F" });
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 2, Step = 'E' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 2, Step = 'F' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 2, Step = 'G' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 2, Step = 'A' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 2, Step = 'B' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'C' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'D' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'E' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'F' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'G' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'A' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'B' }
            }, "F");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'C' }
            }, "F");
        }

        public static void DrawCClefScale(RenderHelper _renderHelper)
        {
            _renderHelper.DrawClef(new Clef { Line = 3, Sign = "C" });
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'D' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'E' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'F' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'G' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'A' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 3, Step = 'B' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'C' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'D' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'E' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'F' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'G' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'A' }
            }, "C");
            _renderHelper.DrawConstructedNote(new Note
            {
                Type = NoteType.eighth,
                Pitch = new Pitch { Octave = 4, Step = 'B' }
            }, "C");
        }
    }
}
