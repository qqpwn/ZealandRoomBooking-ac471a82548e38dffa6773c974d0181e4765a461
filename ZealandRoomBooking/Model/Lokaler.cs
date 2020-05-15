using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealandRoomBooking.Model
{
    public class Lokaler
    {
        public int LokaleId { get; set; }
        public int Etage { get; set; }
        public string Type { get; set; }
        public string Navn { get; set; }
        public string Bygning { get; set; }

        public Lokaler(int lokaleId, int etage, string type, string navn, string bygning)
        {
            LokaleId = lokaleId;
            Etage = etage;
            Type = type;
            Navn = navn;
            Bygning = bygning;
        }

        public override string ToString()
        {
            return $"Navn: {Navn}, Bygning: {Bygning}, Etage: {Etage}, Type: {Type}";
        }
    }
}
