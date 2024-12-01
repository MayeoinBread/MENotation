using MENotation.XmlHandling;
using System.Collections.Generic;

namespace MENotation.ScoreRendering
{
    public static class GlyphDictionaries
    {
        public static readonly Dictionary<string, (string unicode, int width, int height)> ClefUnicodes = new()
        {
            { "G", ("\ue050", 22, 55) }, { "F", ("\ue062", 24, 30) }, { "C", ("\ue05c", 24, 32) },
            { "percussion", ("\ue069", 22, 55) }, { "TAB", ("\ue06E", 24, 30) }
        };

        public static readonly Dictionary<NoteType, (string unicode, int width, int height, double bvOffset)> NoteUnicodes = new()
        {
            { NoteType.whole, ("\ue1d2", 15, 10, 5) }, { NoteType.half, ("\ue1d3", 11, 32, 27.5) },
            { NoteType.quarter, ("\ue1d5", 11, 32, 27.5) }, { NoteType.eighth, ("\ue1d7", 18, 32, 27.5) },
            { NoteType.sixteenth, ("\ue1d9", 18, 32, 27.5) }
        };

        public static readonly Dictionary<NoteType, (string unicode, int width, int height)> RestUnicodes = new()
        {
            { NoteType.whole, ("\ue4e3", 10, 8) }, { NoteType.half, ("\ue4e4", 10, 8) },
            { NoteType.quarter, ("\ue4e5", 10, 24) }, { NoteType.eighth, ("\ue4e6", 10, 14) },
            { NoteType.sixteenth, ("\ue4e7", 10, 23) }
        };

        public static readonly Dictionary<AccidentalType, (string unicode, int width, int height)> AccidentalUnicodes = new()
        {
            { AccidentalType.Sharp, ("\ue262", 9, 22) }, { AccidentalType.Flat, ("\ue260", 8, 20) },
            { AccidentalType.Natural, ("\ue261", 6, 22) }, { AccidentalType.DoubleSharp, ("\ue263", 9, 9) },
            { AccidentalType.DoubleFlat, ("\ue264", 13, 19) },
            { AccidentalType.TripleSharp, ("\ue265", 13, 19) }, {AccidentalType.TripleFlat, ("\ue266", 13, 19)},
            { AccidentalType.NaturalFlat, ("\ue267", 13, 19) }, {AccidentalType.NaturalSharp, ("\ue268", 13, 19) },
            { AccidentalType.SharpSharp, ("\ue269", 13, 19) }, {AccidentalType.ParensLeft, ("\ue26a", 13, 19)},
            { AccidentalType.ParensRight, ("\ue26b", 13, 19) }, {AccidentalType.BracketLeft, ("\ue26c", 13, 19)},
            { AccidentalType.BracketRight, ("\ue26d", 13, 19) }
        };

        public static readonly Dictionary<int, (string unicode, int width, int height)> NumberUnicodes = new()
        {
            { 0, ("\ue080", 14, 17) }, { 1, ("\ue081", 11, 17) }, { 2, ("\ue082", 14, 17) },
            { 3, ("\ue083", 13, 17) }, { 4, ("\ue084", 14, 17) }, { 5, ("\ue085", 13, 17) },
            { 6, ("\ue086", 14, 17) }, { 7, ("\ue087", 14, 17) }, { 8, ("\ue088", 13, 17) }, { 9, ("\ue089", 13, 17) }
        };

        public static readonly Dictionary<NoteType, (string unicode, int width, int height)> NoteHeadUnicodes = new()
        {
            { NoteType.whole, ("\uE0A2", 13, 8) }, { NoteType.half, ("\uE0A3", 9, 8) }, { NoteType.quarter, ("\uE0A4", 9, 8) },
            { NoteType.eighth, ("\uE0A4", 9, 8) }, { NoteType.sixteenth, ("\uE0A4", 9, 8) }
        };

        public static readonly Dictionary<NoteType, (string unicodeUp, string unicodeDown, int widthUp, int widthDown, int height)> FlagsUnicodes = new()
        {
            { NoteType.eighth, ("\uE240", "\uE241", 9, 10, 25) }, { NoteType.sixteenth, ("\uE242", "\uE243", 9, 10, 25) }
        };
    }
}
