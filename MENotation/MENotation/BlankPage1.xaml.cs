using MENotation.XmlHandling.Classes;
using MENotation.XmlHandling;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MENotation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {

        private Score myScore;

        public BlankPage1()
        {
            this.InitializeComponent();

            //XmlImport.ImportXml();
            //System.Diagnostics.Debug.WriteLine(XmlParser.ParseXmlToScore(XmlImport.ImportXml()));
            myScore = XmlParser.ParseXmlToScore(XmlImport.ImportXml());

            //this.Content.XamlRoot.Changed += XamlRoot_Changed;
        }

        private void PlotAllSymbolsOnSameLine()
        {
            Canvas canvas = new()
            {
                Width = 1440,
                Height = 600,
                Background = new SolidColorBrush(Colors.White)
            };

            double dpiScale = GetDpiScaleFactor();
            double fontSize = 30 * dpiScale; // Adjust the font size as needed

            // List of musical symbols to plot
            List<string> symbols =
    [
        "\uE050", // Treble Clef
        "\uE062", // Bass Clef
        "\uE1D2", // Whole Note
        "\uE1D3", // Half Note
        "\uE1D5", // Quarter Note
        "\uE1D7", // Eighth Note
        "\uE1D9", // Sixteenth Note
        "\uE4E3", // Whole Rest
        "\uE4E4", // Half Rest
        "\uE4E5", // Quarter Rest
        "\uE4E7", // Eighth Rest
        "\uE4E9"  // Sixteenth Rest
    ];

            double originX = 50 * dpiScale; // Starting X position
            double originY = 150 * dpiScale; // Same Y position for alignment
            double xOffset = 30 * dpiScale; // Horizontal spacing between symbols

            foreach (var unicode in symbols)
            {
                // Create the Glyph for the symbol
                Glyphs glyph = new()
                {
                    FontUri = new Uri("ms-appx:///Assets/Bravura.otf"), // Replace with your font path
                    FontRenderingEmSize = fontSize,
                    UnicodeString = unicode,
                    Fill = new SolidColorBrush(Colors.Black),
                    OriginX = originX,
                    OriginY = originY
                };

                // Add the glyph to the canvas
                canvas.Children.Add(glyph);

                Border glyphBorder = new()
                {
                    BorderBrush = new SolidColorBrush(Colors.Blue),
                    BorderThickness = new Thickness(1),
                    Width = fontSize, // Approximate glyph width
                    Height = fontSize // Approximate glyph height
                };

                Canvas.SetLeft(glyphBorder, originX);
                Canvas.SetTop(glyphBorder, originY - fontSize);

                canvas.Children.Add(glyphBorder);

                // Move the X position for the next symbol
                originX += xOffset;
            }

            // Add the canvas to the UI
            StaveGrid.Children.Add(canvas);

            Line baseline = new()
            {
                X1 = 20,
                Y1 = originY,
                X2 = originX + xOffset * symbols.Count,
                Y2 = originY,
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 1
            };
            canvas.Children.Add(baseline);
        }

        private void DefaultUICheck(double dpiScale)
        {
            Canvas canvas = new()
            {
                Width = 600,
                Height = 700,
                Background = new SolidColorBrush(Colors.White)
            };

            // Shared origin for comparison
            double originX = 100 * dpiScale;
            double originY = 100 * dpiScale;

            // Font size for all glyphs
            double fontSize = 30 * dpiScale;

            // List of glyphs to compare
            List<(string unicode, string name)> symbols =
    [
        ("\uE1D5", "Quarter Note"), // Quarter Note
        ("\uE1D3", "Half Note"),    // Half Note
        ("\uE1D2", "Whole Note"),   // Whole Note
        ("\uE050", "Treble Clef"),  // Treble Clef
        ("\uE062", "Bass Clef")     // Bass Clef
    ];

            double yOffset = 0;

            foreach (var (unicode, name) in symbols)
            {
                // Create the Glyph
                Glyphs glyph = new()
                {
                    FontUri = new Uri("ms-appx:///Assets/Bravura.otf"), // Replace with your font path
                    FontRenderingEmSize = fontSize,
                    UnicodeString = unicode,
                    Fill = new SolidColorBrush(Colors.Black),
                    OriginX = originX,
                    OriginY = originY + yOffset
                };

                // Add the glyph to the canvas
                canvas.Children.Add(glyph);

                // Add a border around the symbol
                Border glyphBorder = new()
                {
                    BorderBrush = new SolidColorBrush(Colors.Red),
                    BorderThickness = new Thickness(1),
                    Width = fontSize, // Approximate width
                    Height = fontSize, // Approximate height
                };

                Canvas.SetLeft(glyphBorder, originX);
                Canvas.SetTop(glyphBorder, originY + yOffset - fontSize); // Adjust for baseline

                canvas.Children.Add(glyphBorder);

                // Add a label to identify the glyph
                TextBlock label = new()
                {
                    Text = name,
                    FontSize = 12 * dpiScale,
                    Foreground = new SolidColorBrush(Colors.Blue),
                    Margin = new Thickness(0, 0, 0, 0)
                };

                Canvas.SetLeft(label, originX + 50 * dpiScale); // Position label next to glyph
                Canvas.SetTop(label, originY + yOffset - fontSize / 2);

                canvas.Children.Add(label);

                Line baseline = new()
                {
                    X1 = 50,
                    Y1 = originY + yOffset,
                    X2 = 500,
                    Y2 = originY + yOffset,
                    Stroke = new SolidColorBrush(Colors.Blue),
                    StrokeThickness = 1
                };

                canvas.Children.Add(baseline);

                // Increment Y offset for the next symbol
                yOffset += 50 * dpiScale;
            }

            // Add canvas to the UI
            StaveGrid.Children.Add(canvas);
        }

        private void SetupUI(double dpiScale)
        {
            System.Diagnostics.Debug.WriteLine($"DPI Scale Factor: {dpiScale}");

            Canvas canvas = new()
            {
                Width = 600,
                Height = 300,
                Background = new SolidColorBrush(Colors.White)
            };

            DrawStaff(canvas, dpiScale);

            DrawClef(canvas, dpiScale);

            //TextBlock quarterNote = new()
            //{
            //    Text = "\uE1D2",
            //    FontFamily = new FontFamily("Bravura"),
            //    FontSize = 30 * dpiScale,
            //    Margin = new Thickness(100 * dpiScale, 60 * dpiScale, 0, 0),
            //    Foreground = new SolidColorBrush(Colors.Black),
            //    Padding = new Thickness(0)
            //};
            Glyphs quarterNoteGlyphs = new()
            {
                FontUri = new Uri("ms-appx:///Assets/Bravura.otf"), // Replace with your font file location
                FontRenderingEmSize = 30 * dpiScale,               // Set the font size (scaled)
                UnicodeString = "\uE1D5",                          // Quarter note Unicode in Bravura
                Fill = new SolidColorBrush(Colors.Black),          // Set color
                OriginX = 100 * dpiScale,                          // Horizontal position
                OriginY = 60 * dpiScale                            // Vertical position

            };

            canvas.Children.Add(quarterNoteGlyphs);

            double glyphWidth = 30 * dpiScale * 0.6; // Approximation for width
            double glyphHeight = 30 * dpiScale;     // Approximation for height

            // Create a Border to wrap around the Glyphs
            Border glyphBorder = new()
            {
                Width = glyphWidth,
                Height = glyphHeight,
                BorderBrush = new SolidColorBrush(Colors.Red),
                BorderThickness = new Thickness(2)
            };

            // Position the Border to align with the Glyphs
            Canvas.SetLeft(glyphBorder, 100 * dpiScale);
            Canvas.SetTop(glyphBorder, 60 * dpiScale - glyphHeight); // Adjust to account for glyph baseline

            //canvas.Children.Add(quarterNote);

            canvas.Children.Add(glyphBorder);

            //glyphBorder.Loaded += (s, e) =>
            //{
            //    double textWidth = glyphBorder.ActualWidth;
            //    double textHeight = glyphBorder.ActualHeight;

            //    System.Diagnostics.Debug.WriteLine($"Glyph Actual Width: {textWidth}");
            //    System.Diagnostics.Debug.WriteLine($"Glyph Actual Height: {textHeight}");
            //};
            canvas.Loaded += (s, e) =>
            {
                double actualWidth = quarterNoteGlyphs.ActualWidth;
                double actualHeight = quarterNoteGlyphs.ActualHeight;

                System.Diagnostics.Debug.WriteLine($"Glyphs Actual Width: {actualWidth}");
                System.Diagnostics.Debug.WriteLine($"Glyphs Actual Height: {actualHeight}");
            };

            StaveGrid.Children.Add(canvas);
        }

        private void XamlRoot_Changed(XamlRoot sender, XamlRootChangedEventArgs args)
        {
            StaveGrid.Children.Clear();
            double dpiScale = this.Content.XamlRoot.RasterizationScale;
            System.Diagnostics.Debug.WriteLine($"DPI Scale Updated: {dpiScale}");
            SetupUI(dpiScale);
        }

        double GetDpiScaleFactor()
        {
            if (this.Content.XamlRoot != null)
            {
                return this.Content.XamlRoot.RasterizationScale;
            }
            return 1.0;
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";

            string json = JsonSerializer.Serialize(myScore, new JsonSerializerOptions { WriteIndented = true });
            System.Diagnostics.Debug.WriteLine(json);
        }

        private void DrawStaff(Canvas canvas, double dpiScale)
        {
            double staffLineThickness = 2 * dpiScale;
            double staffSpacing = 20 * dpiScale;
            double staffWidth = 500 * dpiScale;
            double topMargin = 50 * dpiScale;
            double leftMargin = 50 * dpiScale;

            for (int i = 0; i < 5; i++)
            {
                Line line = new()
                {
                    X1 = leftMargin,
                    Y1 = topMargin + i * staffSpacing,
                    X2 = leftMargin + staffWidth,
                    Y2 = topMargin + i * staffSpacing,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = staffLineThickness
                };
                canvas.Children.Add(line);
            }
        }

        private void DrawClef(Canvas canvas, double dpiScale)
        {
            double clefFontSize = 40 * dpiScale;

            TextBlock trebleClef = new()
            {
                Text = "\uE050",
                FontFamily = new FontFamily("Bravura"),
                FontSize = clefFontSize,
                Margin = new Thickness(30 * dpiScale, 20 * dpiScale, 0, 0),
                Foreground = new SolidColorBrush(Colors.Black),
                Padding = new Thickness(0)
            };

            Border border = new()
            {
                BorderBrush = new SolidColorBrush(Colors.Red),
                BorderThickness = new Thickness(2),
                Child = trebleClef
            };
            //canvas.Children.Add(trebleClef);

            border.Loaded += (s, e) =>
            {
                double textWidth = border.ActualWidth;
                double textHeight = border.ActualHeight;

                System.Diagnostics.Debug.WriteLine($"Text Block Actual Width: {textWidth}");
                System.Diagnostics.Debug.WriteLine($"Text Block Actual Height: {textHeight}");
            };

            canvas.Children.Add(border);
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            //DefaultUICheck(GetDpiScaleFactor());
            //SetupUI(GetDpiScaleFactor());
            PlotAllSymbolsOnSameLine();
        }
    }
}
