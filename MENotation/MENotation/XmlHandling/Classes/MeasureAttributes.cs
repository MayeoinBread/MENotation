namespace MENotation.XmlHandling.Classes
{
    internal class MeasureAttributes
    {
        public Key Key {  get; set; }
        public Time Time { get; set; }
        public Clef Clef { get; set; }
        
        // Number of divisions per quarter note (ie 2 = 8th note)
        public int Divisions { get; set; }

        // Number of staves in the part (eg. 2 for Piano)
        public int Staves { get; set; }

        public StaffDetails StaffDetails { get; set; }

        public Transpose Transpose { get; set; }

        public MeasureAttributes()
        {
        }
    }
}
