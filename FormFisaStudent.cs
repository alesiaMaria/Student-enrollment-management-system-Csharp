using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proiect_PAW_Dorcea_Alesia_Maria
{
    public partial class FormFisaStudent : Form
    {
        private Student studentCurent;

        public FormFisaStudent(Student student)
        {
            InitializeComponent();
            this.studentCurent = student;

            SeteazaModVizualizare();
            AfiseazaDetaliile();
        }

        private void SeteazaModVizualizare()
        {
            
            txtMatricol.ReadOnly = true;
            txtNume.ReadOnly = true;
            txtPrenume.ReadOnly = true;
            dtpDataNasterii.Enabled = false;
            txtAnStudiu.ReadOnly = true;
        }

        private void AfiseazaDetaliile()
        {
            if (studentCurent != null)
            {
              
                txtMatricol.Text = studentCurent.Matricol;
                txtNume.Text = studentCurent.Nume;
                txtPrenume.Text = studentCurent.Prenume;
                dtpDataNasterii.Value = studentCurent.DataNasterii;


                string numeCurat = studentCurent.Nume.Replace(" ", "").Replace("-", "").ToLower();
                string prenumeCurat = studentCurent.Prenume.Replace(" ", "").Replace("-", "").ToLower();
                string emailGenerat = $"{prenumeCurat}.{numeCurat}@stud.ase.ro";
                txtEmail.Text = emailGenerat;
               
                if (studentCurent.AnCurent != null)
                {
                    txtAnStudiu.Text = $"An {studentCurent.AnCurent.ValoareAn} - {studentCurent.AnCurent.Specializare}";
                }
                else
                {
                    txtAnStudiu.Text = "Nespecificat";
                }

                lsCursuri.Items.Clear();
                if (studentCurent.DisciplineInscrise != null)
                {
                    foreach (Disciplina d in studentCurent.DisciplineInscrise)
                    {
                        lsCursuri.Items.Add(d);
                    }
                }
            }
        }
    }
}
