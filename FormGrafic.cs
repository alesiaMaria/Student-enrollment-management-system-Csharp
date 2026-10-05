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
    public partial class FormGrafic : Form
    {
        private List<Student> studentiGrafic;

        public FormGrafic(List<Student> lista)
        {
            InitializeComponent();
            studentiGrafic = lista;
            panelGrafic.Paint += new PaintEventHandler(panelGrafic_Paint);
        }

        private void panelGrafic_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int nrInfo = 0, nrCiber = 0, nrStat = 0;


            foreach (var s in studentiGrafic)
            {
                if (s.AnCurent != null)
                {
                    if (s.AnCurent.Specializare == "Informatica") nrInfo++;
                    else if (s.AnCurent.Specializare == "Cibernetica") nrCiber++;
                    else if (s.AnCurent.Specializare == "Statistica") nrStat++;
                }
            }

            int[] valori = { nrInfo, nrCiber, nrStat };
            string[] specializari = { "Informatică", "Cibernetică", "Statistică" };
            Color[] culori = { Color.FromArgb(33, 150, 243), Color.FromArgb(76, 175, 80), Color.FromArgb(255, 152, 0) }; // Albastru, Verde, Portocaliu

          
            int maxValoare = 0;
            foreach (int v in valori) if (v > maxValoare) maxValoare = v;
            if (maxValoare == 0) maxValoare = 1;

            int latimeBara = 60;
            int spatiuIntreBare = 40;
            int pornireX = 80;
            int bazaY = panelGrafic.Height - 60; 
            int inaltimeMaximaGrafic = panelGrafic.Height - 100;

            Pen penAxa = new Pen(Color.DarkGray, 2);
            g.DrawLine(penAxa, pornireX - 20, bazaY, pornireX + (latimeBara + spatiuIntreBare) * 3, bazaY);

            Font fontText = new Font("Segoe UI", 10, FontStyle.Regular);
            Font fontValori = new Font("Segoe UI", 11, FontStyle.Bold);
            SolidBrush pensulaText = new SolidBrush(Color.FromArgb(33, 33, 33));

         
            for (int i = 0; i < valori.Length; i++)
            {
                
                int inaltimeBara = (valori[i] * inaltimeMaximaGrafic) / maxValoare;

                int x = pornireX + i * (latimeBara + spatiuIntreBare);
                int y = bazaY - inaltimeBara;

               
                SolidBrush pensulaBara = new SolidBrush(culori[i]);
                g.FillRectangle(pensulaBara, x, y, latimeBara, inaltimeBara);

                g.DrawRectangle(Pens.DimGray, x, y, latimeBara, inaltimeBara);

                g.DrawString(valori[i].ToString(), fontValori, pensulaText, x + (latimeBara / 4), y - 22);

                g.DrawString(specializari[i], fontText, pensulaText, x - 10, bazaY + 10);
            }
        }
    }
}
