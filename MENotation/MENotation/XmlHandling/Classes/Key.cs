namespace MENotation.XmlHandling.Classes
{
    public class Key
    {
        public int Fifths { get; set; }
        public string Mode { get; set; }

        public Key()
        {
            Fifths = 0;
            Mode = string.Empty;
        }

    }
}
