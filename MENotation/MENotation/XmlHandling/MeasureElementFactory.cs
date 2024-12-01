using MENotation.ScoreRendering;
using MENotation.XmlHandling.Classes;
using System;
using System.Xml;

namespace MENotation.XmlHandling
{
    public enum NoteType
    {
        sixteenth, eighth, quarter, half, whole, unknown
    }

    public enum DirectionType
    {
        rehearsal, segno, coda, words, symbol, wedge, dynamics, dashes, bracket, pedal, metronome, octaveShift, harpPedals, damp, dampAll, eyeglasses, stringMute, scordatura,
        image, principalVoice, percussion, accordionRegistration, staffDivide, otherDirection
    }

    public enum BarlineType
    {
        dashed, dotted, heavy, heavyHeavy, heavyLight, lightHeavy, lightLight, none, regular, shrt, tick
    }

    public enum FermataType
    {
        normal, angled, square, doubleAngled, doubleSquare, doubleDot, halfCurve, curlew, none
    }

    public abstract class MeasureElement
    {

    }

    public class Note : MeasureElement
    {
        public Pitch Pitch { get; set; }
        public int Duration { get; set; }
        public NoteType Type { get; set; }
        public bool IsRest { get; set; }
        public string Voice { get; set; }
        public Lyric Lyric { get; set; }
        public Beam Beam { get; set; }

        public bool StemUp { get; set; }

        public AccidentalType Accidental { get; set; }
        public bool IsChord { get; set; }
        public bool IsDot {  get; set; }

        public double staveLine { get; set; }
    }

    public class Backup : MeasureElement
    {
        public int Duration { get; set; }
    }

    public class Forward : MeasureElement
    {
        public int Duration { get; set; }
    }

    public class Ending
    {
        public string Number { get; set; }
        public string Type { get; set; }  // start, stop, discontinue
        public string Text { get; set; }
    }

    public class Repeat
    {
        public string Direction { get; set; }  // backward, forward
        public int Times { get; set; }
    }

    public class Barline : MeasureElement
    {
        public string Location { get; set; }
        public BarlineType BarStyle { get; set; }
        public bool IsWavy {  get; set; }
        public bool IsSegno {  get; set; }
        public bool IsCoda {  get; set; }
        public FermataType Fermata { get; set; }
        public Ending Ending { get; set; }
        public Repeat Repeat { get; set; }
    }

    public class Attributes : MeasureElement
    {
        public Key Key { get; set; }
        public Time Time { get; set; }
        public Clef Clef { get; set; }
        public int Divisions { get; set; }
        public int Staves { get; set; }
        public StaffDetails StaffDetails { get; set; }
        public Transpose Transpose { get; set; }
    }

    public class Direction : MeasureElement
    {
        public DirectionType DirectionType { get; set; }
        public int Offset { get; set; }
        public string Voice { set; get; }
        public int Staff { get; set; }
        public string Placement {  set; get; }

        // TODO direction-type have different properties/variables. Below for metronome
        public NoteType BeatUnit { get; set; }
        public int PerMinute {  get; set; }
    }

    public static class MeasureElementFactory
    {
        public static MeasureElement CreateElement(XmlNode node)
        {
            switch (node.Name)
            {
                case "note":
                    return XmlParser.GetNoteFromNode(node);
                case "backup":
                    return XmlParser.GetBackupForNode(node);
                case "forward":
                    return XmlParser.GetForwardForNode(node);
                case "attributes":
                    return XmlParser.GetMeasureAttributesForMeasure(node);
                case "direction":
                    return XmlParser.GetDirectionAttributesForMeasure(node);
                case "barline":
                    return new Barline
                    {

                    };
                default:
                    throw new InvalidOperationException($"Unknown node type: {node.Name}");
            }
        }
    }
}
