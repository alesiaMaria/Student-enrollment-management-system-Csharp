using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Clasa Disciplina
namespace proiect_PAW_Dorcea_Alesia_Maria
{
    [Serializable]
    public class Disciplina
    {
        public string CodDisciplina { get; set; }
        public string Denumire { get; set; }
        public int NrCredite { get; set; }

        public Disciplina() { }

        public Disciplina(string cod, string denumire, int credite)
        {
            CodDisciplina = cod;
            Denumire = denumire;
            NrCredite = credite;
        }

        
        public override string ToString()
        {
            return $"{Denumire} ({NrCredite} Credite)";
        }

    }
}
