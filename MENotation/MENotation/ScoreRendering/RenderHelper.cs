using MENotation.XmlHandling;
using MENotation.XmlHandling.Classes;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI;

namespace MENotation.ScoreRendering
{
    //public enum BarlineType { Single, Double, Final, RepeatOpen, RepeatClose, RepeatDouble }
    //public enum AccidentalType { None, Flat, Natural, Sharp, DoubleFlat, DoubleSharp }
    public enum AccidentalType
    {
        None, Flat, Natural, Sharp, DoubleSharp, DoubleFlat,
        TripleSharp, TripleFlat, NaturalFlat, NaturalSharp, SharpSharp,
        ParensLeft, ParensRight, BracketLeft, BracketRight
    }

    internal class RenderHelper
    {
        // General settings
        private const double StaffMiddleLineOffset = 2.0;
        private const double StepToPixelMultiplier = 7.5; // Distance in pixels per step (1 staff step = 4 pixels)
        private readonly double leftPadding = 5;
        private readonly double noteHorizontalSpacing = 20;

        // Staff settings
        private readonly double staffLineThickness = 0.5;
        private readonly double stemLineThickness = 0.75;
        private readonly double staffTopMargin = 50;
        private const double staffLeftMargin = 10;

        private double horizontalOffset = staffLeftMargin;
        private double verticalOffset = 0;

        // Note stuff
        private double lyricLowestNoteStep;
        private readonly List<Point> openBeamPoint = [new(-1, -1), new(-1, -1)];
        private Point emptyPoint = new(-1, -1);
        private bool lastStartBeamUp = true;

        // Colors
        //private readonly Color background = Colors.White;
        private readonly Color foreground = Colors.Black;

        // Font and canvas
        private readonly Uri BravuraFontUri = new("ms-appx:///Assets/Bravura.otf");
        private readonly int lyricFontSize = 8;
        private readonly Canvas canvas = new()
        {
            Width = 1440,
            Height = 600,
            Background = new SolidColorBrush(Colors.White)
        };

        private Clef activeClef;
        private Key activeKey;
        private Time activeTime;
        private int divisions;

        private double noteOffsetToAdd = 0;

        // TODO Forward/Backward (or whatever it's called), Staff (numbers)
        //  Accidentals

        public void DrawPart(Part part)
        {
            Attributes measureAttributes = null;

            // Y-down is positive, so step 4 is the bottom line. This is the "minimum" point that keeps lyrics below the stave
            lyricLowestNoteStep = 4;

            foreach (var measure in part.Measures)
            {
                //if (measure.Attributes != null)
                //{
                //    measureAttributes = measure.Attributes;
                //    if (measureAttributes.Clef != null)
                //        activeClef = measureAttributes.Clef;
                //    if (measureAttributes.Key != null)
                //        activeKey = measureAttributes.Key;
                //    if (measureAttributes.Time != null)
                //        activeTime = measureAttributes.Time;
                //    if (measureAttributes.Divisions != 0)
                //        divisions = measureAttributes.Divisions;
                //}
                //foreach (var note in measure.Notes)
                //{
                //    if (!note.IsRest)
                //    {
                //        var noteLineOffset = NotePositionCalculator.GetNoteLineOffset(note.Pitch) - GetClefOffset(activeClef.Sign);
                //        lyricLowestNoteStep = Math.Max(lyricLowestNoteStep, noteLineOffset);
                //    }
                //}
                foreach (var item in measure.Elements)
                {
                    switch (item)
                    {
                        case Attributes attributes:
                            measureAttributes = attributes;
                            if (measureAttributes.Clef != null)
                                activeClef = measureAttributes.Clef;
                            if (measureAttributes.Key != null)
                                activeKey = measureAttributes.Key;
                            if (measureAttributes.Time != null)
                                activeTime = measureAttributes.Time;
                            if (measureAttributes.Divisions > 0)
                                divisions = measureAttributes.Divisions;
                            break;
                        case Note note:
                            if (!note.IsRest)
                            {
                                var noteLineOffset = NotePositionCalculator.GetNoteLineOffset(note.Pitch) - GetClefOffset(activeClef.Sign);
                                lyricLowestNoteStep = Math.Max(lyricLowestNoteStep, noteLineOffset);
                            }
                            break;
                    }
                }
            }

            // Add buffer for stem pointing down on lowest note
            // TODO not always needed, only if beams on lower notes with stem pointing down
            lyricLowestNoteStep += 2.5;

            //foreach (Measure measure in part.Measures)
            //{
            for (int i=0; i<part.Measures.Count; i++)
            //for (int i = 0; i < 1; i++)
            {
                //if (i < 27)
                //{
                //    continue;
                //}
                var measure = part.Measures[i];

                if (horizontalOffset == staffLeftMargin)
                {
                    DrawBarline(BarlineType.regular);
                    horizontalOffset += staffLeftMargin;
                }

                //if (measure.Attributes != null)
                //{
                //    // TODO better way of handling this. Strip it out to a function or something, as it might crop up a bit
                //    measureAttributes = measure.Attributes;
                //    if (measureAttributes.Clef != null)
                //    {
                //        activeClef = measureAttributes.Clef;
                //        DrawClef(measureAttributes.Clef);
                //    }
                //    if (measureAttributes.Key != null)
                //    {
                //        activeKey = measureAttributes.Key;
                //        DrawKeySignature(activeKey, activeClef.Sign);
                //    }
                //    if (measureAttributes.Time != null)
                //    {
                //        activeTime = measureAttributes.Time;
                //        DrawTimeSignature(activeTime);
                //    }
                //    if (measureAttributes.Divisions != 0)
                //    {
                //        divisions = measureAttributes.Divisions;
                //    }
                //    horizontalOffset += noteHorizontalSpacing;
                //}
                //else
                //{
                //    measure.Attributes = measureAttributes;

                //}
                //// TODO try to catch for 0 Divisions (and dividing by 0 later)
                //if (measure.Attributes.Divisions == 0)
                //{
                //    measure.Attributes.Divisions = divisions;
                //}

                //// TODO figure out Directions possibilities, and if it can be at different parts of a measure, etc.
                //if (measure.Direction != null)
                //{
                //    DrawDirection(measure.Direction);
                //}

                DrawMeasure(measure);

                // Barline adjustments
                horizontalOffset += 10;
                bool lastBar = part.Measures.IndexOf(measure) == part.Measures.Count - 1;
                DrawBarline(lastBar ? BarlineType.heavyLight : BarlineType.regular);
                

                // TODO new line when we reach the end of the screen
                if (horizontalOffset >= 1200)
                {
                    DrawStaveLines();
                    verticalOffset += lyricLowestNoteStep + 7;
                    
                    horizontalOffset = staffLeftMargin;
                    DrawBarline(BarlineType.regular);
                    horizontalOffset += staffLeftMargin;
                    
                }
                else if (!lastBar)
                {
                    horizontalOffset += 10;
                }
            }

            // This might draw double on the last line?
            DrawStaveLines();
        }

        public void DrawMeasure(Measure measure)
        {
            // TODO sort out forward/backups properly. May need to look at the whole spacing of things again...
            //  Keep backups/forwards in memory to undo the changes to horizontalOffset so the staff goes properly
            //  At the end of each bar (maybe keep a barMaxWidth?

            Attributes measureAttributes = null;
            string clefSign = activeClef.Sign;
            double m_divisions = divisions;
            double barMaxWidth = horizontalOffset;
            double barStart = horizontalOffset;
            double barTotalDur = 0;

            Debug.WriteLine($"Measure: {measure.Number}");

            for (int i = 0; i < measure.Elements.Count; i++)
            {
                switch (measure.Elements[i])
                {
                    case Attributes attributes:
                        measureAttributes = attributes;

                        if (measureAttributes.Clef != null)
                        {
                            activeClef = measureAttributes.Clef;
                            DrawClef(activeClef);
                        }
                        if (measureAttributes.Key != null)
                        {
                            activeKey = measureAttributes.Key;
                            DrawKeySignature(measureAttributes.Key, activeClef.Sign);
                        }
                        if (measureAttributes.Time != null)
                        {
                            activeTime = measureAttributes.Time;
                            DrawTimeSignature(activeTime);
                        }
                        if (measureAttributes.Divisions != 0)
                        {
                            m_divisions = measureAttributes.Divisions;
                        }
                        horizontalOffset += noteHorizontalSpacing;
                        // Update this in case we have the above in random bars
                        barStart = horizontalOffset;
                        break;
                    case Direction direction:
                        DrawDirection(direction);
                        break;
                    case Note currentNote:
                        //Note nextNote = (i + 1) < measure.Elements.Count ? measure.Elements[i + 1] : null;

                        if (currentNote.Pitch != null)
                            Debug.WriteLine($"Note: {currentNote.Pitch.Step}, hOffset: {horizontalOffset}");
                        else
                            Debug.WriteLine($"Note: {currentNote.Type}, hOffset: {horizontalOffset}");
                        noteOffsetToAdd = DrawConstructedNote(currentNote, clefSign);

                        // TODO This isn't adding in for chord notes properly
                        if ((i + 1) < measure.Elements.Count && measure.Elements[i + 1] is Note nextNote)
                        {
                            // Don't offset if the note is a chord
                            if (!nextNote.IsChord)
                            {
                                // Add the note width
                                horizontalOffset += noteOffsetToAdd;
                                // Add the note spacing
                                horizontalOffset += noteHorizontalSpacing * (currentNote.Duration / m_divisions);
                                barTotalDur += currentNote.Duration;
                            }
                        }

                        //if (currentNote.IsRest)
                        //{
                        //    //horizontalOffset += noteOffsetToAdd;
                        //    horizontalOffset += noteHorizontalSpacing * (currentNote.Duration / m_divisions);
                        //}
                        //else if (currentNote.IsRest)
                        //{
                        //    // Add the note width
                        //    horizontalOffset += noteOffsetToAdd;
                        //    // Add the note spacing
                        //    horizontalOffset += noteHorizontalSpacing * (currentNote.Duration / m_divisions);
                        //}
                        break;
                    case Backup backup:
                        Debug.WriteLine($"backupDur: {backup.Duration}, totalDur: {barTotalDur}");
                        if (backup.Duration > barTotalDur)
                        {
                            horizontalOffset = barStart;
                            barTotalDur = 0;
                        }
                        //else
                        //{
                        //horizontalOffset -= noteHorizontalSpacing * (backup.Duration / m_divisions);
                        //}
                        break;
                    case Forward forward:
                        horizontalOffset += noteHorizontalSpacing * (forward.Duration / m_divisions);
                        break;
                }
                //Note currentNote = measure.Notes[i];
                //Note nextNote = (i + 1) < measure.Notes.Count ? measure.Notes[i + 1] : null;
                //DrawConstructedNote(currentNote, clefSign);

                // Don't offset if the note is a chord
                //if (nextNote != null && !nextNote.IsChord)
                //{
                //    // TODO this is bugging out sometimes, BATB bar 28
                //    // Divisions is 0 in some instances, potentially handled above?
                //    horizontalOffset += noteHorizontalSpacing * (currentNote.Duration / divisions);
                //}

                barMaxWidth = Math.Max(barMaxWidth, horizontalOffset);
            }

            //foreach (Note note in measure.Notes)
            //{
            //    DrawConstructedNote(note, clefSign);
            //    // TODO this is bugging out sometimes, BATB bar 28
            //    horizontalOffset += noteHorizontalSpacing * (note.Duration / divisions);
            //}
            horizontalOffset = barMaxWidth;
        }

        public void DrawDirection(Direction direction)
        {
            //if (direction.Placement == "above")
            //{

            //}
            if (GlyphDictionaries.NoteUnicodes.TryGetValue(direction.BeatUnit, out var noteTup))
            {
                double glyphFontSize = 12;
                AddGlyph(noteTup.unicode, horizontalOffset - 2 * noteTup.width, -5, glyphFontSize);
                horizontalOffset += noteTup.width / 30 * glyphFontSize;
                // TODO fix the vertical placement compared to the note glyph
                AddText($" = {direction.PerMinute}", 0, -7, foreground, 12);
            }
            // TODO use Number glyphs instead. Should be smaller ones in the pool
        }

        public void DrawStaveLines(double staveWidth = 0, int numLines = 5)
        {
            staveWidth = staveWidth == 0 ? horizontalOffset - staffLeftMargin : staveWidth;

            for (int i = 0; i < numLines; i++)
            {
                double stepPosition = i - StaffMiddleLineOffset + verticalOffset;
                double pixelY = StepToPixels(stepPosition);

                canvas.Children.Add(new Line
                {
                    X1 = staffLeftMargin,
                    Y1 = pixelY,
                    X2 = staffLeftMargin + staveWidth,
                    Y2 = pixelY,
                    Stroke = new SolidColorBrush(foreground),
                    StrokeThickness = staffLineThickness
                });
            }
        }

        public void DrawLedgerLine(double stepPosition, double ledgerWidth)
        {
            canvas.Children.Add(new Line
            {
                X1 = horizontalOffset - (ledgerWidth / 2),
                Y1 = StepToPixels(stepPosition + verticalOffset),
                X2 = horizontalOffset + ledgerWidth + (ledgerWidth / 2),
                Y2 = StepToPixels(stepPosition + verticalOffset),
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = staffLineThickness
            });
        }

        public void DrawBarline(BarlineType barline = BarlineType.regular)
        {
            AddVerticalLine(horizontalOffset, -2, 2, foreground);

            if (barline == BarlineType.lightHeavy)
            {
                horizontalOffset += 5;
                AddVerticalLine(horizontalOffset, -2, 2, foreground, staffLineThickness * 4);
                horizontalOffset += staffLineThickness * 2;
            }
            else if (barline == BarlineType.lightLight)
            {
                horizontalOffset += 5;
                AddVerticalLine(horizontalOffset, -2, 2, foreground);
            }
            // TODO add remaining barlines
        }

        private void AddVerticalLine(double offset, double topStep, double bottomStep, Color color, double thickness = 1)
        {
            Line line = new Line();
            line.X1 = offset;
            line.X2 = offset;
            line.Y1 = StepToPixels(topStep + verticalOffset);
            line.Y2 = StepToPixels(bottomStep + verticalOffset);
            line.Stroke = new SolidColorBrush(color);
            line.StrokeThickness = thickness;
            //{
            //    X1 = offset,
            //    Y1 = StepToPixels(topStep + verticalOffset),
            //    X2 = offset,
            //    Y2 = StepToPixels(bottomStep + verticalOffset),
            //    Stroke = new SolidColorBrush(color),
            //    StrokeThickness = thickness
            //};
            canvas.Children.Add(line);
        }

        private void AddStemLine(double hOffset, double startLine, double endLine)
        {
            AddVerticalLine(hOffset, startLine, endLine, foreground, stemLineThickness);
        }

        private void DrawBeam(Point startPoint, Point endPoint, double thickness = 1)
        {

            DrawPolygon([
                new(startPoint.X, StepToPixels(startPoint.Y + verticalOffset)),
                new(endPoint.X, StepToPixels(endPoint.Y + verticalOffset)),
                new(endPoint.X, StepToPixels(endPoint.Y + verticalOffset) + thickness),
                new(startPoint.X, StepToPixels(startPoint.Y + verticalOffset) + thickness)
                ]);
        }

        private void DrawPolygon(List<Point> pointList)
        {
            PointCollection points = [.. pointList];
            Polygon polygon = new()
            {
                Points = points,
                Fill = new SolidColorBrush(foreground)
            };
            canvas.Children.Add(polygon);
        }

        public void DrawClef(Clef clef)
        {
            if (GlyphDictionaries.ClefUnicodes.TryGetValue(clef.Sign, out var value))
            {
                // Reference positions based on clef types and their lines
                double clefLineOffset = 0;

                // Calculate the vertical position based on the clef's line
                // Adjust the middle line offset so that it accounts for the line relative to the middle of the staff
                if (clef.Sign == "G") // G Clef (Treble Clef)
                {
                    clefLineOffset = clef.Line - 1; // 2 is the middle line for G Clef (G4)
                }
                else if (clef.Sign == "F") // F Clef (Bass Clef)
                {
                    clefLineOffset = clef.Line - 5; // 4 is the middle line for F Clef (F3)
                }
                else if (clef.Sign == "C") // C Clef (Alto/Tenor Clef)
                {
                    clefLineOffset = clef.Line - 3; // 3 is the middle line for C Clef (C4)
                }

                AddGlyph(value.unicode, horizontalOffset, clefLineOffset);
                horizontalOffset += value.width + leftPadding;
            }
        }

        public void DrawKeySignature(Key key, string clefSign = "G")
        {
            if (clefSign is not ("G" or "C" or "F")) return;

            var sharpOffsets = new[] { -2, -0.5, -2.5, -1, 0.5, -1.5, 0 };
            var flatOffsets = new[] { 0, -1.5, 0.5, -1, 1, -0.5, 1.5 };

            double lineOffset = clefSign switch { "C" => 0.5, "F" => 1, _ => 0 };

            var accOffsets = key.Fifths > 0 ? sharpOffsets : flatOffsets;
            var ksAccType = key.Fifths > 0 ? AccidentalType.Sharp : AccidentalType.Flat;

            for (int i = 0; i < Math.Abs(key.Fifths); i++)
            {
                DrawAccidental(ksAccType, accOffsets[i] + lineOffset);
            }
            horizontalOffset += leftPadding;
        }

        public void DrawTimeSignature(Time time)
        {
            var upperWidth = CalculateTotalWidth(time.Beats.ToString());
            var lowerWidth = CalculateTotalWidth(time.BeatType.ToString());

            var upperPad = Math.Max(0, (lowerWidth - upperWidth) / 2);
            var lowerPad = Math.Max(0, (upperWidth - lowerWidth) / 2);

            DrawNumbers(time.Beats.ToString(), horizontalOffset + upperPad, -1);
            DrawNumbers(time.BeatType.ToString(), horizontalOffset + lowerPad, 1);
            horizontalOffset += Math.Max(upperWidth, lowerWidth) + leftPadding;
        }

        public double DrawConstructedNote(Note note, string clefSign)
        {
            if (note.IsRest)
            {
                // TODO rests have a 'measure="yes"' attribute, anything special with it?
                return DrawRest(note);
            }

            if (GlyphDictionaries.NoteHeadUnicodes.TryGetValue(note.Type, out var noteHeadUni))
            {
                double noteLinePosition = NotePositionCalculator.GetNoteLineOffset(note.Pitch) - GetClefOffset(clefSign);

                // Draw the lyric first (if any), as this can affect the positioning of other parts of the note
                var lyricWidth = DrawNoteLyric(note);

                var addOffsetWidth = Math.Max(noteHeadUni.width, lyricWidth);

                
                if (note.Type != NoteType.whole)
                {
                    bool stemUp = noteLinePosition > 0;

                    if (GlyphDictionaries.FlagsUnicodes.TryGetValue(note.Type, out var flagsTup))
                    {
                        if (note.Beam != null)
                        {
                            foreach (int beamIndex in note.Beam.Number)
                            {
                                double beamYOffset = (beamIndex - 1) * 0.75;

                                if (note.Beam.Value[beamIndex - 1] == "begin" && openBeamPoint[beamIndex - 1] == emptyPoint)
                                {
                                    openBeamPoint[beamIndex - 1] = new Point(
                                        stemUp ? horizontalOffset + noteHeadUni.width : horizontalOffset,
                                        stemUp ? noteLinePosition - 3.5 + beamYOffset : noteLinePosition + 3.5 - beamYOffset
                                    );
                                    lastStartBeamUp = stemUp;
                                }
                                else if (note.Beam.Value[beamIndex - 1] == "continue" && openBeamPoint[beamIndex - 1] != emptyPoint)
                                {
                                    stemUp = lastStartBeamUp;
                                }
                                else if (note.Beam.Value[beamIndex - 1] == "end" && openBeamPoint[beamIndex - 1] != emptyPoint)
                                {
                                    stemUp = lastStartBeamUp;
                                    var closeBeamPoint = new Point(
                                        stemUp ? horizontalOffset + noteHeadUni.width : horizontalOffset,
                                        stemUp ? noteLinePosition - 3.5 + beamYOffset : noteLinePosition + 3.5 - beamYOffset
                                    );
                                    DrawBeam(openBeamPoint[beamIndex - 1], closeBeamPoint, (stemUp ? 7 : - 7) * staffLineThickness);
                                    openBeamPoint[beamIndex - 1] = emptyPoint;
                                }
                            }

                            var stemEnd = stemUp ? noteLinePosition - 3.5 : noteLinePosition + 3.5;
                            var stemHPos = stemUp ? horizontalOffset + noteHeadUni.width : horizontalOffset;

                            AddStemLine(stemHPos, noteLinePosition, stemEnd);
                        }
                        else
                        {
                            var stemEnd = stemUp ? noteLinePosition - 3.5 : noteLinePosition + 3.5;
                            var stemHPos = stemUp ? horizontalOffset + noteHeadUni.width : horizontalOffset;

                            AddStemLine(stemHPos, noteLinePosition, stemEnd);

                            AddGlyph(
                                stemUp ? flagsTup.unicodeUp : flagsTup.unicodeDown,
                                stemHPos, stemEnd);
                            addOffsetWidth = stemUp
                                ? Math.Max(lyricWidth, noteHeadUni.width + flagsTup.widthUp)
                                : Math.Max(addOffsetWidth, flagsTup.widthDown);
                        }
                    }
                    else
                    {
                        var stemEnd = stemUp ? noteLinePosition - 4 : noteLinePosition + 4;
                        var stemHPos = stemUp ? horizontalOffset + noteHeadUni.width : horizontalOffset;

                        AddStemLine(stemHPos, noteLinePosition, stemEnd);
                    }
                }

                // Draw the head
                AddGlyph(noteHeadUni.unicode, horizontalOffset, noteLinePosition);
                // Draw dot if needed
                if (note.IsDot)
                {
                    DrawDot(horizontalOffset, noteLinePosition);
                }

                if (noteLinePosition < -3)
                {
                    for (int j = (int)noteLinePosition; j <= -3; j++)
                    {
                        DrawLedgerLine(j, noteHeadUni.width);
                    }
                }
                else if (noteLinePosition > 3)
                {
                    for (int j = 3; j <= noteLinePosition; j++)
                    {
                        DrawLedgerLine(j, noteHeadUni.width);
                    }
                }

                //if (!currentNote.IsRest)
                //{
                //    double noteLinePosition = NotePositionCalculator.GetNoteLineOffset(currentNote.Pitch) - GetClefOffset(clefSign);
                //    if (noteLinePosition < -3)
                //    {
                //        if (GlyphDictionaries.NoteHeadUnicodes.TryGetValue(currentNote.Type, out var noteUni))
                //        {
                //            for (int j = (int)noteLinePosition; j < -3; j++)
                //            {
                //                DrawLegerLine(j, noteUni.width);
                //            }
                //        }
                //    }
                //    else if (noteLinePosition > 3)
                //    {
                //        if (GlyphDictionaries.NoteHeadUnicodes.TryGetValue(currentNote.Type, out var noteUni))
                //        {
                //            for (int j = 3; j < noteLinePosition; j++)
                //            {
                //                DrawLegerLine(j, noteUni.width);
                //            }
                //        }
                //    }
                //}

                if (GlyphDictionaries.AccidentalUnicodes.TryGetValue(note.Accidental, out var accUni))
                {
                    AddGlyph(accUni.unicode, horizontalOffset - accUni.width, noteLinePosition);
                }

                // TODO addOffsetWidth might need refinement if multiple sequential notes have long lyrics?
                //  Might be fine, words are just right beside each other on shorter notes due to the spacing
                //  Only need to add this on the last note of a chord, which we can sort out outside this function
                //   But we need to pass this to know what to add on
                if (note.IsChord)
                {
                    // TODO need to check nextNote here too to know that we don't need to do this
                    //  Also need to know if we should offset the notes if they are just one step apart
                    return addOffsetWidth;
                }
            }
            return 0;
        }

        public double DrawRest(Note note)
        {
            if (GlyphDictionaries.RestUnicodes.TryGetValue(note.Type, out var restTup))
            {
                AddGlyph(restTup.unicode, horizontalOffset, note.Type == NoteType.whole ? -1 : 0);
                horizontalOffset += restTup.width;
                DrawDot(horizontalOffset, -0.5, 30);
                return restTup.width;
            }
            return 0;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0042:Deconstruct variable declaration", Justification = "Pointless in this situation")]
        public double DrawNoteLyric(Note note)
        {
            if (note.Lyric == null || note.Lyric.Text.Count == 0)
                return 0.0;

            string longest_str = note.Lyric.Text.OrderByDescending(lyric => lyric.Length).FirstOrDefault();
            //string longest_str = note.Lyric.Text.Aggregate(string.Empty,
            //    (longest, current) => current.Length > longest.Length ? current : longest);

            TextBlock tmpLyric = new()
            {
                Text = longest_str,
                FontSize = lyricFontSize
            };
            tmpLyric.Measure(new Size(0, 0));
            double maxLyricWidth = tmpLyric.ActualWidth;

            var noteProps = GlyphDictionaries.NoteHeadUnicodes[note.Type];

            for (int i=0; i<note.Lyric.Number.Count; i++)
            {
                AddText(note.Lyric.Text[i], noteProps.width, lyricLowestNoteStep + note.Lyric.Number[i] - 1, foreground, lyricFontSize);
            }
            return maxLyricWidth;
        }

        private void AddText(string text, double noteWidth, double textLine, Color color, double fontSize)
        {
            TextBlock lyricText = new()
            {
                Text = text,
                Foreground = new SolidColorBrush(color),
                FontSize = fontSize
            };
            lyricText.Measure(new Size(0, 0));

            Canvas.SetLeft(lyricText, horizontalOffset - (lyricText.ActualWidth / 2) + (noteWidth / 2));
            Canvas.SetTop(lyricText, StepToPixels(textLine + verticalOffset));

            canvas.Children.Add(lyricText);
        }

        private void DrawAccidental(AccidentalType accidental, double stepOffset)
        {
            if (GlyphDictionaries.AccidentalUnicodes.TryGetValue(accidental, out var accTup))
            {
                AddGlyph(accTup.unicode, horizontalOffset, stepOffset);
                horizontalOffset += accTup.width;
            }
        }

        private void DrawNumbers(string numbers, double xOffset, double stepOffset)
        {
            foreach (char c in numbers)
            {
                int number = int.Parse(c.ToString());
                if (GlyphDictionaries.NumberUnicodes.TryGetValue(number, out var numTup))
                {
                    AddGlyph(numTup.unicode, xOffset, stepOffset);
                    xOffset += numTup.width;
                }
            }
        }

        private static double CalculateTotalWidth(string numbers)
        {
            double total = 0;
            foreach (char c in numbers)
            {
                int number = int.Parse(c.ToString());
                if (GlyphDictionaries.NumberUnicodes.TryGetValue(number, out var numTup))
                    total += numTup.width;
            }
            return total;
        }

        private void AddGlyph(string glyphUnicode, double xOffset, double stepOffset = 0, double fontSize = 30)
        {
            double pixelOffset = StepToPixels(stepOffset + verticalOffset);
            var glyph = new Glyphs
            {
                FontUri = BravuraFontUri,
                FontRenderingEmSize = fontSize,
                UnicodeString = glyphUnicode,
                Fill = new SolidColorBrush(foreground),
                OriginX = xOffset,
                OriginY = pixelOffset
            };
            canvas.Children.Add(glyph);
        }

        private void DrawDot(double xOffset, double stepOffset = 0, double fontSize = 30)
        {
            AddGlyph("\ue1e7", xOffset, stepOffset, fontSize);
        }

        private double StepToPixels(double stepOffset)
        {
            return staffTopMargin + stepOffset * StepToPixelMultiplier;
        }

        // This adjusts the B4 reference point (middle line of stave) across the different clefs
        //  so regardless of clef, the middle line is always used for drawing etc.
        private static int GetClefOffset(string clefSign) =>
            clefSign == "F" ? 6 : clefSign == "C" ? 3 : 0;

        public void RenderCanvas(Grid grid)
        {
            grid.Children.Clear();
            grid.Children.Add(canvas);
        }
    }
}
