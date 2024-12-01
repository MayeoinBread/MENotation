using System.Collections.Generic;

namespace MENotation.XmlHandling.Classes
{
    public class Beam
    {
        internal List<int> Number { get; set; }

        internal List<string> Value { get; set; }

        internal Beam()
        {
            // Beam Number non-zero if beam is active
            Number = [];
            Value = [];
        }
    }
}
