using MENotation.XmlHandling;
using MENotation.XmlHandling.Classes;
using Microsoft.UI.Xaml;
using MENotation.ScoreRendering;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MENotation
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        //private Score myScore;
        private RenderHelper _renderHelper;
        //private RenderHelperGPT _renderHelper;
        private Score myScore;

        public MainWindow()
        {
            this.InitializeComponent();

            myScore = XmlParser.ParseXmlToScore(XmlImport.ImportXml());

            MainGrid.Loaded += MainGrid_Loaded;
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //_renderHelper = new RenderHelperGPT
            //{
            //    DebugBorders = true,
            //};
            _renderHelper = new RenderHelper();

            //Testers.DrawAllNotesAndRests(_renderHelper);
            //Testers.DrawGClefScale(_renderHelper);
            //_renderHelper.DrawKeySignature(new Key { Fifths = -7 }, "G");
            //_renderHelper.DrawTimeSignature(new Time { Beats = 12, BeatType = 8 });

            //Testers.DrawCClefScale(_renderHelper);
            //_renderHelper.DrawKeySignature(new Key { Fifths = -7 }, "C");

            //Testers.DrawFClefScale(_renderHelper);
            //_renderHelper.DrawKeySignature(new Key { Fifths = -7 }, "F");
            
            //_renderHelper.DrawStaveLines();

            _renderHelper.DrawPart(myScore.PartList[0]);

            _renderHelper.RenderCanvas(MainGrid);
        }
    }
}
