using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_PAW_Dorcea_Alesia_Maria
{
    [Serializable]
    public class AnStudiu
    {
        public int ValoareAn { get; set; }
        public string Specializare { get; set; }
        public string TutoreAn { get; set; }

        public AnStudiu() { }

        public AnStudiu(int valoareAn, string specializare, string tutoreAn)
        {
            ValoareAn = valoareAn;
            Specializare = specializare;
            TutoreAn = tutoreAn;
        }
        public override string ToString()
        {
            return $"Anul {ValoareAn} - {Specializare}";
        }
    }
}
