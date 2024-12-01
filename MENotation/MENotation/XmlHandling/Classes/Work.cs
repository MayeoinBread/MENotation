using System.Collections.Generic;

namespace MENotation.XmlHandling.Classes
{
    public class Work
    {
        public string Number { get; set; }
        public string Title { get; set; }

        public Work() {
            Number = string.Empty;
            Title = string.Empty;
        }
    }
}
