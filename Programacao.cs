﻿using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

// https://docs.microsoft.com/pt-br/dotnet/desktop/winforms/advanced/walkthrough-performing-a-drag-and-drop-operation-in-windows-forms?view=netframeworkdesktop-4.8

//Fazer 4 listas
//a primeira tem as listas 
//se carrega elas para as listas 
//quando se solta, define a hora 
//primeira lista é todos os dias
//Segunda dias de semana 
//Terceira, sabados
//Quarta, domingo 

// Maior lista em termos de tamanho de nome
// Musica para motoqueiros

namespace XeviousPlayer2
{
    public partial class Programacao : Form
    {

        private Rectangle dragBoxFromMouseDown;

        private int XX;
        private int YY;
        private bool Entrou = false;
        private string TextoBtSelecionado = "";
        private int BtSelecionado = -1;

        public Programacao()
        {
            InitializeComponent();
            Listas();            
        }

        private void Listas()
        {
            string SQL = "Select Nome From Listas";
            string ret = DalHelper.Consulta(SQL);
            SQLiteCommand command = new SQLiteCommand(SQL.ToString(), DalHelper.DbConnection());
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    int Cont = 0;
                    while (reader.Read())
                    {
                        string Nome = reader.GetString(0);
                        string nmBot = "Bt" + Cont.ToString(); ;
                        CarregaBotao(nmBot, Nome, Cont);
                        Cont++;
                    }
                }
            }
        }

        private void CarregaBotao(string Nome, string Texto, int I)
        {
            Button bt = new Button();
            bt.AllowDrop = true;
            bt.AutoSize = true;
            bt.Location = new Point(3, 3);
            bt.Name = Nome;
            bt.Size = new Size(194, 23);
            bt.TabIndex = 11;
            bt.Top = I * 20;
            bt.Text = Texto;
            bt.UseVisualStyleBackColor = true;
            bt.Tag = I.ToString();
            bt.Cursor = Cursors.Hand;
            bt.MouseDown += new MouseEventHandler(this.bt_MouseDown);
            // this.button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button1_MouseDown);
            // bt.Click += new System.EventHandler(btLista_Click);
            // bt.MouseDown
            this.panel1.Controls.Add(bt);
        }

        private void bt_MouseDown(object sender, MouseEventArgs e)
        {
            Button EsseBt = ((Button)sender);
            TextoBtSelecionado = EsseBt.Text;
            string Tague = EsseBt.Tag.ToString();
            int.TryParse(Tague, out BtSelecionado);

            EsseBt.DoDragDrop(TextoBtSelecionado, DragDropEffects.Copy | DragDropEffects.Move);

            // ((Button)sender).DoDragDrop("button1.Text", DragDropEffects.Move);
            // ((Button)sender).DoDragDrop("button1.Text", DragDropEffects.Copy | DragDropEffects.Move);
            // string Texto = sender.Text;


            //((Button)sender).Top = e.Location.X;
            //((Button)sender).Left = e.Location.Y;

            //Size dragSize = SystemInformation.DragSize;
            //dragBoxFromMouseDown = new Rectangle(
            //    new Point(e.X - (dragSize.Width / 2),
            //              e.Y - (dragSize.Height / 2)),
            //    dragSize);

        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            // button1.DoDragDrop(button1.Text, DragDropEffects.Copy |DragDropEffects.Move);
            // this.DoDragDrop(button1, DragDropEffects.Move);
            this.XX = button1.Left +e.X;
            this.YY = button1.Top+ e.Y;
            this.Entrou = true;
        }

        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
            this.panel1.Controls[BtSelecionado].Text = e.Data.GetData(DataFormats.Text).ToString();
            int PosX = e.X;
            int PosY = e.Y;
            this.Text = "X = " + e.X.ToString() + " Y = " + e.Y.ToString();
        }
    }
}