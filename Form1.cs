using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Data.SQLite; // Schimbăm pe SQLite

namespace proiect_PAW_Dorcea_Alesia_Maria
{
    public partial class Form1 : Form
    {
        private List<Student> listaStudenti = new List<Student>();
        private string stringConexiune = @"Data Source=facultate.db;Version=3;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AnStudiu info1 = new AnStudiu(1, "Informatica", "Prof. Popescu");
            AnStudiu info2 = new AnStudiu(2, "Informatica", "Prof. Ionescu");
            AnStudiu info3 = new AnStudiu(3, "Informatica", "Prof. Vasilescu");
            AnStudiu ciber1 = new AnStudiu(1, "Cibernetica", "Prof. Georgescu");
            AnStudiu ciber2 = new AnStudiu(2, "Cibernetica", "Prof. Marinescu");
            AnStudiu ciber3 = new AnStudiu(3, "Cibernetica", "Prof. Dumitrescu");
            AnStudiu stat1 = new AnStudiu(1, "Statistica", "Prof. Stan");
            AnStudiu stat2 = new AnStudiu(2, "Statistica", "Prof. Stoica");
            AnStudiu stat3 = new AnStudiu(3, "Statistica", "Prof. Avram");
            cbAnStudiu.Items.Add(info1);
            cbAnStudiu.Items.Add(info2);
            cbAnStudiu.Items.Add(info3);
            cbAnStudiu.Items.Add(ciber1);
            cbAnStudiu.Items.Add(ciber2);
            cbAnStudiu.Items.Add(ciber3);
            cbAnStudiu.Items.Add(stat1);
            cbAnStudiu.Items.Add(stat2);
            cbAnStudiu.Items.Add(stat3);
            if (cbAnStudiu.Items.Count > 0)
                cbAnStudiu.SelectedIndex = 0;


            lstDiscipline.Items.Add(new Disciplina("INF101", "Programarea Aplicatiilor Windows", 4));
            lstDiscipline.Items.Add(new Disciplina("INF102", "Baze de Date", 5));
            lstDiscipline.Items.Add(new Disciplina("INF103", "Cercetari Operationale", 4));
            lstDiscipline.Items.Add(new Disciplina("INF104", "Statistica Matematica", 5));
            lstDiscipline.Items.Add(new Disciplina("INF105", "BTI", 4));
            lstDiscipline.Items.Add(new Disciplina("INF106", "Java", 5));
            lstDiscipline.Items.Add(new Disciplina("INF107", "Programare Orientată Obiect", 4));
            lstDiscipline.Items.Add(new Disciplina("INF108", "Python", 5));


            using (SQLiteConnection conexiune = new SQLiteConnection(stringConexiune))
            {
                string creareTabelSql = @"
        CREATE TABLE IF NOT EXISTS Studenti (
            Matricol TEXT PRIMARY KEY,
            Nume TEXT,
            Prenume TEXT,
            DataNasterii TEXT,
            Specializare TEXT
        );";

                using (SQLiteCommand comanda = new SQLiteCommand(creareTabelSql, conexiune))
                {
                    try
                    {
                        conexiune.Open();
                        comanda.ExecuteNonQuery(); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare inițializare DB: {ex.Message}");
                    }
                }
            }
        }
        private void AfiseazaStudenti()
        {
           
            lstStudenti.Items.Clear();

            
            foreach (Student s in listaStudenti)
            {
                lstStudenti.Items.Add(s);
            }

           
            if (listaStudenti.Count > 0)
            {
                
                statusStrip1.Visible = true;
                toolStripStatusLabel1.Text = $"Sistem pregătit. Total studenți înscriși în listă: {listaStudenti.Count}";
            }
            else
            {
                
                statusStrip1.Visible = false;
            }
        }

        private void btnAdauga_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool esteValid = true;
            if (string.IsNullOrWhiteSpace(txtMatricol.Text))
            {
                errorProvider1.SetError(txtMatricol, "Numărul matricol este obligatoriu!");
                esteValid = false;
            }
            if (string.IsNullOrWhiteSpace(txtNume.Text) || txtNume.Text.Length < 2)
            {
                errorProvider1.SetError(txtNume, "Numele trebuie să aibă cel puțin 2 caractere!");
                esteValid = false;
            }
            if (string.IsNullOrWhiteSpace(txtPrenume.Text))
            {
                errorProvider1.SetError(txtPrenume, "Prenumele este obligatoriu!");
                esteValid = false;
            }
            if (dtpDataNasterii.Value >= DateTime.Now.AddYears(-16))
            {
                errorProvider1.SetError(dtpDataNasterii, "Data nașterii invalidă! Studentul trebuie să aibă peste 16 ani.");
                esteValid = false;
            } 
            if (esteValid)
            {
                AnStudiu anSelectat = (AnStudiu)cbAnStudiu.SelectedItem;
                Student studentNou = new Student(
                    txtMatricol.Text.Trim(),
                    txtNume.Text.Trim(),
                    txtPrenume.Text.Trim(),
                    dtpDataNasterii.Value,
                    anSelectat
                );
                listaStudenti.Add(studentNou);
                SalveazaStudentInBazaDeDate(studentNou);
                AfiseazaStudenti();
                MessageBox.Show($"Studentul {studentNou.Nume} a fost adăugat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CurataCampuri();
            }
            else
            {
                MessageBox.Show("Vă rugăm să corectați erorile marcate pe formular!", "Date Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CurataCampuri()
        {
            txtMatricol.Clear();
            txtNume.Clear();
            txtPrenume.Clear();
            dtpDataNasterii.Value = DateTime.Now.AddYears(-19);
            if (cbAnStudiu.Items.Count > 0) cbAnStudiu.SelectedIndex = 0;
        }

        private void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (listaStudenti.Count == 0)
            {
                MessageBox.Show("Nu există studenți în listă pentru a fi salvați!", "Avertizare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Fișiere Date (*.dat)|*.dat";
            sfd.Title = "Alege unde salvezi lista de studenți";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, listaStudenti); 
                    }
                    MessageBox.Show("Datele au fost salvate cu succes în fișier!", "Salvare Reușită", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la salvare: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRestaureaza_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Fișiere Date (*.dat)|*.dat";
            ofd.Title = "Selectează fișierul cu studenți";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
        
                        listaStudenti = (List<Student>)bf.Deserialize(fs);
                    }
                    AfiseazaStudenti();

                    MessageBox.Show($"Restaurare completă! Au fost încărcați {listaStudenti.Count} studenți.", "Restaurare Reușită", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la încărcare: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CurataCampuri();
                errorProvider1.Clear();
                e.Handled = true; 
            }
            if (e.KeyCode == Keys.F1)
            {
                string mesajAjutor = "Ghid rapid de utilizare:\n\n" +
                                     "- Folosiți ALT + litera subliniată pentru navigare rapidă.\n" +
                                     "- Apăsați tasta ESC pentru a reseta câmpurile formularului.\n" +
                                     "- După completare, apăsați pe 'Adăugă Student' sau Enter.";

                MessageBox.Show(mesajAjutor, "Ajutor Aplicație", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Handled = true;
            }
        }

        private void salveazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnSalveaza_Click(sender, e);
        }

        private void restaureazaDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnRestaureaza_Click(sender, e);
        }

        private void ieșireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void ștergeStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstStudenti.SelectedIndex != -1)
            {
                Student studentDeSters = (Student)lstStudenti.SelectedItem;
                DialogResult rezultat = MessageBox.Show(
                    $"Sunteți sigur că doriți să îl ștergeți pe studentul {studentDeSters.Nume} {studentDeSters.Prenume}?",
                    "Confirmare Ștergere",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (rezultat == DialogResult.Yes)
                {
                    listaStudenti.Remove(studentDeSters);
                    AfiseazaStudenti();
                }
            }
            else
            {
                MessageBox.Show("Vă rugăm să selectați un student din listă mai întâi!", "Avertizare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void despreAplicațieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string detaliiProiect = "Sistem de Management Facultate\n" +
                            "Versiunea: 1.0.0 (2026)\n\n" +
                            "Proiect realizat pentru disciplina:\n" +
                            "Programarea Aplicațiilor Windows (PAW)\n\n" +
                            "Autor: Dorcea Alesia Maria\n" +
                            "Grupă: [1050]";

            MessageBox.Show(detaliiProiect, "Despre Aplicație", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void graficSpecializariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGrafic fereastraGrafic = new FormGrafic(listaStudenti);
            fereastraGrafic.ShowDialog();
        }

        private void imprimareListaStudentiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

          
            Font fontTitlu = new Font("Segoe UI", 16, FontStyle.Bold);
            Font fontAntet = new Font("Segoe UI", 11, FontStyle.Bold);
            Font fontText = new Font("Segoe UI", 10, FontStyle.Regular);

            SolidBrush pensula = new SolidBrush(Color.Black);
            Pen penLinie = new Pen(Color.Gray, 1);

            int x = 50;
            int y = 50;

       
            g.DrawString("RAPORT: LISTĂ STUDENȚI ÎNSCRIȘI", fontTitlu, pensula, x, y);
            y += 40; 

            g.DrawString($"Generat la data: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}", fontText, pensula, x, y);
            y += 40;

         
            g.DrawString("Matricol", fontAntet, pensula, x, y);
            g.DrawString("Nume și Prenume", fontAntet, pensula, x + 120, y);
            g.DrawString("Specializare / An", fontAntet, pensula, x + 350, y);
            g.DrawString("Data Nașterii", fontAntet, pensula, x + 550, y);

            y += 25;
            g.DrawLine(penLinie, x, y, x + 700, y); 
            y += 15;

           
            if (listaStudenti.Count == 0)
            {
                g.DrawString("Nu există studenți înregistrați în aplicație.", fontText, pensula, x, y);
            }
            else
            {
                foreach (Student s in listaStudenti)
                {
                 
                    if (y > e.MarginBounds.Bottom) break;

                    string numeComplet = $"{s.Nume} {s.Prenume}";
                    string infoAn = s.AnCurent != null ? $"An {s.AnCurent.ValoareAn} - {s.AnCurent.Specializare}" : "Nespecificat";

              
                    g.DrawString(s.Matricol, fontText, pensula, x, y);
                    g.DrawString(numeComplet, fontText, pensula, x + 120, y);
                    g.DrawString(infoAn, fontText, pensula, x + 350, y);
                    g.DrawString(s.DataNasterii.ToString("dd.MM.yyyy"), fontText, pensula, x + 550, y);

                    y += 30;
                    g.DrawLine(Pens.LightGray, x, y - 5, x + 700, y - 5);
                }
            }
        }

        private void lstDiscipline_MouseDown(object sender, MouseEventArgs e)
        {
            lstDiscipline.DoDragDrop(lstDiscipline.SelectedItem, DragDropEffects.Copy);
        }

        private void lstStudenti_DragEnter(object sender, DragEventArgs e)
        {
           
            if (e.Data.GetDataPresent(typeof(Disciplina)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None; 
            }
        }

        private void lstStudenti_DragDrop(object sender, DragEventArgs e)
        {
            Point punctFereastra = lstStudenti.PointToClient(new Point(e.X, e.Y));
            int indexStudentPeCareSaDatDrumul = lstStudenti.IndexFromPoint(punctFereastra);

           
            if (indexStudentPeCareSaDatDrumul != ListBox.NoMatches)
            {
         
                Disciplina disciplinaTrasa = (Disciplina)e.Data.GetData(typeof(Disciplina));

             
                Student studentSelectat = listaStudenti[indexStudentPeCareSaDatDrumul];

               
                if (!studentSelectat.DisciplineInscrise.Contains(disciplinaTrasa))
                {
                    studentSelectat.DisciplineInscrise.Add(disciplinaTrasa);

                    MessageBox.Show(
                        $"Succes! Studentul {studentSelectat.Nume} a fost înscris prin Drag & Drop la disciplina: {disciplinaTrasa.Denumire}.",
                        "Înscriere Disciplină",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                  
                    AfiseazaStudenti();
                }
                else
                {
                    MessageBox.Show("Acest student este deja înscris la această disciplină!", "Avertizare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void SalveazaStudentInBazaDeDate(Student s)
        {
            using (SQLiteConnection conexiune = new SQLiteConnection(stringConexiune))
            {
                string query = "INSERT INTO Studenti (Matricol, Nume, Prenume, DataNasterii, Specializare) " +
                               "VALUES (@matricol, @nume, @prenume, @data, @spec)";

                using (SQLiteCommand comanda = new SQLiteCommand(query, conexiune))
                {
                    comanda.Parameters.AddWithValue("@matricol", s.Matricol);
                    comanda.Parameters.AddWithValue("@nume", s.Nume);
                    comanda.Parameters.AddWithValue("@prenume", s.Prenume);
                    comanda.Parameters.AddWithValue("@data", s.DataNasterii.ToString("yyyy-MM-dd"));
                    comanda.Parameters.AddWithValue("@spec", s.AnCurent != null ? s.AnCurent.Specializare : "Nespecificat");

                    try
                    {
                        conexiune.Open();
                        comanda.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare la salvarea SQL: {ex.Message}", "Eroare DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void IncarcaStudentiDinBazaDeDate()
        {
            listaStudenti.Clear();

            using (SQLiteConnection conexiune = new SQLiteConnection(stringConexiune))
            {
                string query = "SELECT Matricol, Nume, Prenume, DataNasterii, Specializare FROM Studenti";

                using (SQLiteCommand comanda = new SQLiteCommand(query, conexiune))
                {
                    try
                    {
                        conexiune.Open();
                        using (SQLiteDataReader reader = comanda.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string matricol = reader["Matricol"].ToString();
                                string nume = reader["Nume"].ToString();
                                string prenume = reader["Prenume"].ToString();
                                DateTime dataNas = Convert.ToDateTime(reader["DataNasterii"]);
                                string spec = reader["Specializare"].ToString();

                                AnStudiu anS = new AnStudiu(1, spec, "Profesor Alocat");
                                Student s = new Student(matricol, nume, prenume, dataNas, anS);

                                listaStudenti.Add(s);
                            }
                        }
                        AfiseazaStudenti();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare la încărcarea SQL: {ex.Message}", "Eroare DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void sincronizeazaDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IncarcaStudentiDinBazaDeDate();
            MessageBox.Show("Datele au fost reîncărcate cu succes din baza de date SQL!", "Sincronizare DB",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtCautare_TextChanged(object sender, EventArgs e)
        {
            
            string textCautat = txtCautare.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(textCautat))
            {
               
                AfiseazaStudenti();
            }
            else
            {
               
                var studentiFiltrati = listaStudenti
                                       .Where(s => s.Nume.ToLower().Contains(textCautat) ||
                                                   s.Prenume.ToLower().Contains(textCautat) ||
                                                   s.Matricol.Contains(textCautat))
                                       .ToList();

  
                lstStudenti.Items.Clear();
                foreach (Student s in studentiFiltrati)
                {
                    lstStudenti.Items.Add(s);
                }
            }
        }

        private void sorteazaAlfabeticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listaStudenti.Count > 0)
            {
               
                listaStudenti = listaStudenti
                                .OrderBy(s => s.Nume)
                                .ThenBy(s => s.Prenume)
                                .ToList();

            
                AfiseazaStudenti();

                MessageBox.Show("Lista de studenți a fost sortată alfabetic utilizând interogări LINQ!", "Sortare LINQ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Nu există studenți în listă pentru a-i sorta!", "Listă Goală", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void fisaStudentuluiToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (lstStudenti.SelectedIndex != -1)
            {
                
                Student studentSelectat = (Student)lstStudenti.SelectedItem;

              
                FormFisaStudent fisa = new FormFisaStudent(studentSelectat);

              
                fisa.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vă rugăm să selectați un student din listă!", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }
    }
}
