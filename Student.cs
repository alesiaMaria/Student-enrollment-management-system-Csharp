using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_PAW_Dorcea_Alesia_Maria
{
    [Serializable]
    public class Student
    {
        public string Matricol { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public DateTime DataNasterii { get; set; }

        public AnStudiu AnCurent { get; set; }

        public List<Disciplina> DisciplineInscrise { get; set; }

        public Student()
        {
            DisciplineInscrise = new List<Disciplina>();
        }

        public Student(string matricol, string nume, string prenume, DateTime dataNasterii, AnStudiu anCurent)
        {
            Matricol = matricol;
            Nume = nume;
            Prenume = prenume;
            DataNasterii = dataNasterii;
            AnCurent = anCurent;
            DisciplineInscrise = new List<Disciplina>();
        }

        public override string ToString()
        {
            string infoAn = AnCurent != null ? AnCurent.ToString() : "Nespecificat";
            return $"{Nume} {Prenume} ({infoAn}) - Matr: {Matricol}";
        }
    }
}
