using MENotation.XmlHandling;
using MENotation.XmlHandling.Classes;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Diagnostics;
using Windows.Foundation;

namespace MENotation.ScoreRendering
{
    internal class Calculations
    {
        public static double MeasureText(string unicode)
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = unicode,
                FontFamily = new FontFamily("Bravura"),
                FontSize = 30,
                Foreground = new SolidColorBrush(Colors.Black)
            };

            var container = new Canvas();
            container.Children.Add(textBlock);
            container.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
            container.Arrange(new Rect(new Windows.Foundation.Point(), container.DesiredSize));

            double width = textBlock.ActualWidth;
            double height = textBlock.ActualHeight;

            // Output the measurements for debugging purposes
            Debug.WriteLine($"Text Width: {width}, Text Height: {height}");
            return width;
        }

        // Note size: 18 * 30 (for stemmed notes, with tail), 15 * 10 semi-breve (no spacing)
        // Clef size: 26 based of clef+keysig spacing (22 * staff height (30 for most, save G clef))
        // Rests: 10 * 24 for taller rests, 10 * 8 for half/whole

        // Notes should be drawn with separate head and stem. Stems should be 3.5 spaces for full-length
        // https://w3c.github.io/smufl/latest/tables/noteheads.html

        // Notes with Beams have one of: backward hook, begin, continue, end, forward hook. Number determines the note type (8th = 1)

        public static double GetNoteWidth(Note note)
        {
            if (note.IsRest) return 10;
            else if (note.Type == NoteType.whole) return 15;
            else return 18;
        }

        public static double GetLowestNoteInBeam(Measure measure, int startIdx)
        {
            double maxHeight = double.MinValue;

            while (measure.Elements[startIdx] is Note note && note.Beam != null && !note.Beam.Value.Contains("end"))
            {
                maxHeight = Math.Max(maxHeight, NotePositionCalculator.GetNoteLineOffset(note.Pitch));
                startIdx++;
            }
            return maxHeight;
        }

        public static double CalculateStemLength(Note note, bool stemUp)
        {
            double baseLength = 3.5;
            return baseLength;
        }
    }

    public class NotePositionCalculator
    {
        private const char ReferenceStep = 'B';
        private const int ReferenceOctave = 4;
        private const int stepsPerOctave = 7;

        //public static double GetNoteLineOffset(Pitch pitch)
        //{
        //    int stepValue = GetStepValue(pitch.Step);
        //    int referenceStepValue = GetStepValue(ReferenceStep);
        //    int stepDifference = (pitch.Octave - ReferenceOctave) * 7 + (stepValue - referenceStepValue);
        //    if (pitch.Step == 'C')
        //    {
        //        stepDifference -= 1;
        //    }


        //    return -stepDifference;
        //}

        public static double GetNoteLineOffset(Pitch pitch)
        {
            // Get the step value for the pitch (C = 0, D = 1, ..., B = 6)
            int stepValue = GetStepValue(pitch.Step);

            // Get the step value for the reference step (B4)
            int referenceStepValue = GetStepValue(ReferenceStep);

            // Calculate step difference: how many steps from the reference step to the pitch step
            int stepDifference = stepValue - referenceStepValue;

            // Calculate the octave difference: each octave has 7 steps
            int octaveDifference = pitch.Octave - ReferenceOctave;

            // Correct step difference when the note is in a higher octave (adjust for the 7-note cycle)
            //if (stepValue < referenceStepValue)
            //{
            //    // The step difference will go into the previous octave
            //    octaveDifference -= 1;
            //    stepDifference += 7;  // Ensure the correct offset when the step is in a lower octave
            //}

            // Calculate the total vertical offset in terms of staff lines (each octave has 7 note steps)
            double totalOffset = (octaveDifference * stepsPerOctave) + stepDifference;

            // Return negative offset, as higher notes are drawn above the reference line (B4)
            return -totalOffset / 2.0;
        }

        private static int GetStepValue(char step)
        {
            return step switch
            {
                'C' => 0,
                'D' => 1,
                'E' => 2,
                'F' => 3,
                'G' => 4,
                'A' => 5,
                'B' => 6,
                _ => throw new ArgumentException("Invalid Step value")
            };
        }
    }
}
