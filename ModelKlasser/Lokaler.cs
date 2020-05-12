using System;
using System.Collections.Generic;
using System.Text;

namespace ModelKlasser
{
    public class Lokaler
    {
        public Lokaler()
        {
            
        }

        public int LokaleId { get; set; }
        public int Etage { get; set; }
        public string Type { get; set; }
        public string Navn { get; set; }
        public string Bygning { get; set; }
    }
}
