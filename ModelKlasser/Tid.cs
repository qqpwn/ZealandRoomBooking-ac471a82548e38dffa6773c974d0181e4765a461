using System;
using System.Collections.Generic;
using System.Text;

namespace ModelKlasser
{
    public class Tid
    {
        public int TidId { get; set; }
        public TimeSpan TidFra { get; set; }
        public TimeSpan TidTil { get; set; }

        public Tid()
        {
            
        }
    }
}
