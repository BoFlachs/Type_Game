using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Timers;
using System.Diagnostics;

namespace TypeGame
{
    public class Scherm : Form
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        Label introtekst = new Label();
        Label level = new Label();
        Label goal = new Label();
        Label eindscore = new Label();
        Button start = new Button();
        List<string[]> woordenlijst = new List<string[]>();
        double ms = 2000;
        int woordn;
        int levelc = 1;
        int score;
        string doel = "start";
        string invoer = "";
        bool gelijk = false;

        //Initialisatie van alle teksten en buttons
        public Scherm()
        {
            Vul_Woordenlijst();

            this.Text = "Type-Game";
            this.WindowState = FormWindowState.Maximized;
            this.SizeChanged += this.Form_Size;
            this.BackColor = Color.FloralWhite;

            introtekst.Text = "Druk op Start om te beginnen.\nType vervolgens zo snel mogelijk de letters in beeld.";
            introtekst.Size = new Size(400, 100);
            introtekst.TextAlign = ContentAlignment.MiddleCenter;
            introtekst.ForeColor = Color.Crimson;
            introtekst.Font = new Font(FontFamily.GenericSansSerif, 16.0F, FontStyle.Bold);

            level.Text = "Level " + levelc.ToString() + ":\n " + (ms / 1000).ToString()+ " seconde per woord";
            level.TextAlign = ContentAlignment.MiddleCenter;
            level.Size = new Size(500, 80);
            level.Font = new Font(FontFamily.GenericSansSerif, 24.0F, FontStyle.Bold);
            level.Location = new Point(20, 0);
            level.Hide();

            eindscore.Text = "Je score is: " + score.ToString();
            eindscore.Size = new Size(300, 50);
            eindscore.ForeColor = Color.Crimson;
            eindscore.Font = new Font(FontFamily.GenericSansSerif, 28.0F, FontStyle.Bold);
            eindscore.Hide();

            start.Text = "Start spel";
            start.Click += Start_Click;
            start.BackColor = Color.FloralWhite;
            start.ForeColor = Color.Crimson;
            start.Font = new Font(FontFamily.GenericSansSerif, 12.0F, FontStyle.Italic);
            start.Size = new Size(100, 60);

            this.Controls.Add(start);
            this.Controls.Add(introtekst);
            this.Controls.Add(goal);
            this.Controls.Add(level);
            this.Controls.Add(eindscore);

            this.KeyPreview = true;
            this.KeyPress += Handle_KeyPress;
        }

        //Houd de labels op de juiste locatie als het scherm verkleint wordt
        void Form_Size(object obj, EventArgs e)                             
        {
            start.Left = (this.ClientSize.Width - this.start.Width) / 2;
            start.Top = (this.ClientSize.Height - this.start.Height) / 2;

            introtekst.Left = start.Left - this.introtekst.Width / 3;
            introtekst.Top = start.Top - this.introtekst.Height - 5;

            eindscore.Left = this.ClientSize.Width - this.eindscore.Width;
            eindscore.Top = 10;
        }

        //Initialiseert de start van het spel
        void Start_Click(object o, EventArgs e)
        {
            levelc = 1;     
            introtekst.Hide();
            start.Hide();
            level.Show();
            eindscore.Show();
            eindscore.Left = this.ClientSize.Width - this.eindscore.Width;

            goal.Font = new Font(FontFamily.GenericSansSerif, 28.0F, FontStyle.Bold);
            goal.Size = new Size(250, 100);
            goal.Text = "start";
            goal.Location = new Point(123, 234);

            //Iedere ms miliseconde wordt de input gecheckt, evt een punt toegekend en een nieuw woord geselecteerd
            timer.Interval = ms;
            timer.Elapsed += Check_Input;
            timer.Elapsed += Nieuw_Woord;
            timer.Enabled = true;
            timer.AutoReset = true;
        }

        void Nieuw_Woord(object o, EventArgs e)
        {
            Random rnd = new Random();
            Random rnd2 = new Random();
            int index;

            woordn++;
            invoer = "";
            eindscore.Text = "Je score is: " + score.ToString();
            if(woordn % 5 == 0)
            {
                levelc++;
                ms *= 0.9;
                level.Text = "Level " + levelc.ToString() + ":\n " + (Math.Round(ms / 1000, 1)).ToString() + " seconde per woord";

            }
            if (levelc == 8)
            {
                timer.AutoReset = false;
                level.Hide();
                goal.Hide();
                eindscore.Location = introtekst.Location;
                //start.Text = "Speel opnieuw";
                //start.Show();
                return;
            }

            goal.Location = new Point(rnd.Next(0, this.ClientSize.Width - goal.Width),
                                      rnd.Next(0, this.ClientSize.Height - goal.Height));
            index = rnd2.Next(0, woordenlijst[levelc - 1].Length);

            goal.Text = woordenlijst[levelc - 1][index];
            doel = woordenlijst[levelc - 1][index];
        }

        void Check_Input(object o, EventArgs e)
        {
            gelijk = false;
            for (int i = 0; i <= invoer.Length - doel.Length; i++)
            {
                if (invoer.Substring(i, doel.Length) == doel)
                    gelijk = true;
            }
            if (gelijk)
                score++;
        }

        string Verwerkinput()
        {
            this.KeyPreview = true;
            this.KeyPress += Handle_KeyPress;
            return invoer;
        }

        void Handle_KeyPress(object sender, KeyPressEventArgs e)
        {
            invoer += e.KeyChar;
        }

        void Vul_Woordenlijst()
        {
            string[] level1 = System.IO.File.ReadAllLines("/Users/Bo/documents/ki/Computationele_Linguïstiek/Woorden_Type_game/letter3.txt" );
            woordenlijst.Add(level1);
            string[] level2 = System.IO.File.ReadAllLines("/Users/Bo/documents/ki/Computationele_Linguïstiek/Woorden_Type_game/letter4.txt");
            woordenlijst.Add(level2);
            string[] level3 = System.IO.File.ReadAllLines("/Users/Bo/documents/ki/Computationele_Linguïstiek/Woorden_Type_game/letter5.txt");
            woordenlijst.Add(level3);
            string[] level4 = System.IO.File.ReadAllLines("/Users/Bo/documents/ki/Computationele_Linguïstiek/Woorden_Type_game/letter6.txt");
            woordenlijst.Add(level4);
            string[] level5 = System.IO.File.ReadAllLines("/Users/Bo/documents/ki/Computationele_Linguïstiek/Woorden_Type_game/letter7.txt");
            woordenlijst.Add(level5);
            string[] level6 = System.IO.File.ReadAllLines("/Users/Bo/documents/ki/Computationele_Linguïstiek/Woorden_Type_game/letter8.txt");
            woordenlijst.Add(level6);
            string[] level7 = System.IO.File.ReadAllLines("/Users/Bo/documents/ki/Computationele_Linguïstiek/Woorden_Type_game/letter9.txt");
            woordenlijst.Add(level7);
        }

    }
}
