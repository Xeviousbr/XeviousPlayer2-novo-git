using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using XeviousPlayer2.tbs;

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
        private int pnSelecionado = 0;
        private int BtSelecionado = -1;
        private bool Entrou = false;
        private string TextoBtSelecionado = "";
        private string Tague = "";
        private object Cont;
        
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
                        CarregaBotao(nmBot, Nome, Cont, this.panel1);
                        Cont++;
                    }
                }
            }
        }

        private void CarregaBotao(string Nome, string Texto, int I, Panel Painel)
        {
            Button bt = new CustomButton();
            bt.AllowDrop = true;
            bt.AutoSize = true;
            bt.Location = new Point(3, 3);
            bt.Name = Nome;
            bt.Size = new Size(194, 23);
            bt.TabIndex = 11;
            bt.Top = I * 20;
            bt.Text = Texto;
            bt.UseVisualStyleBackColor = true;
            bt.Tag = Painel.Tag +"|" + I.ToString();
            bt.Cursor = Cursors.Hand;
            bt.ForeColor = Color.Aqua;
            bt.MouseDown += new MouseEventHandler(this.bt_MouseDown);
            Painel.Controls.Add(bt);
        }

        private void bt_MouseDown(object sender, MouseEventArgs e)
        {
            Button EsseBt = ((Button)sender);
            TextoBtSelecionado = EsseBt.Text;
            this.Tague = EsseBt.Tag.ToString();
            string[] partes = Tague.Split('|');
            // int.TryParse(Tague, out this.BtSelecionado);
            this.pnSelecionado = Convert.ToInt16(partes[0]);
            this.BtSelecionado = Convert.ToInt16(partes[1]);
            EsseBt.DoDragDrop(TextoBtSelecionado, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            this.XX = button1.Left +e.X;
            this.YY = button1.Top+ e.Y;
            this.Entrou = true;
        }

        private void Paineis_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Paineis_DragDrop(ref DragEventArgs e, ref Panel Painel)
        {
            // Ocorre quando se solta o botão
            int MaxAltura = 400;
            float PosYBSolt = e.Y + 673;
            float PropBt = PosYBSolt / 418;
            float NvPos = PropBt * MaxAltura;
            string sPos = NvPos.ToString();
            string[] arrPos = sPos.Split(',');
            int iPos = int.Parse(arrPos[0]);

            // Dedução do novo horário
            float Momento = 1440 * PropBt;
            int Hora = (int)Momento / 60;
            int Minuto = (int)Momento - (Hora * 60);
            string Texto = this.panel1.Controls[BtSelecionado].Text + " " + Hora.ToString() + ":" + Minuto.ToString("00");

            if (this.pnSelecionado == 0)
            {
                // Soltura do painel inicial, deveria ser para todos os tres outros paineis
                // mas por enquanto é só um
                int Cont = Painel.Controls.Count;
                string nmBot = "Bt" + Cont.ToString();
                CarregaBotao(nmBot, Texto, Cont, Painel);
                Painel.Controls[Cont].Top = iPos;
            }
            else
            {
                // Pego e solto no mesmo painel num dos tres
                string[] partes = Tague.Split('|');
                int Item = Convert.ToInt16(partes[1]);
                Painel.Controls[Item].Top = iPos;
                Painel.Controls[Item].Text = Texto;
            }
        }

        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
            this.Paineis_DragDrop(ref e, ref this.panel2);
        }
        private void panel3_DragDrop(object sender, DragEventArgs e)
        {
            this.Paineis_DragDrop(ref e, ref this.panel3);
        }

        private void panel4_DragDrop(object sender, DragEventArgs e)
        {
            this.Paineis_DragDrop(ref e, ref this.panel4);
        }

        private void panel5_DragDrop(object sender, DragEventArgs e)
        {
            this.Paineis_DragDrop(ref e, ref this.panel5);
        }

        private void Programacao_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Progr> Progrs = new List<Progr>();
            this.ContProgs(ref Progrs, ref this.panel2, 1);
            this.ContProgs(ref Progrs, ref this.panel3, 2);
            this.ContProgs(ref Progrs, ref this.panel4, 3);
            this.ContProgs(ref Progrs, ref this.panel5, 4);
            // Colocar o [ ] Loop de inserção
            tbHorarios tbH = new tbHorarios();
            tbH.HorIn = DateTime.Now;
            tbH.Lista = 1;
            tbH.Periodicidade = 1;
            tbH.Adiciona();
        }

        private void ContProgs(ref List<Progr> progrs, ref Panel Painel, int Tipo)
        {
            for (int i = 0; i < Painel.Controls.Count; i++)
            {
                Progr EssaProg = new Progr();
                EssaProg.IdProg = 0;
                string sTempo = Painel.Controls[i].Text;
                string sHora = sTempo.Substring(sTempo.Length - 5, 5);
                string[] sPartes = sHora.Split(':');
                int Hora = Convert.ToInt16(sPartes[0]);
                int Minu = Convert.ToInt16(sPartes[1]);
                EssaProg.Tempo = new DateTime(2001, 1, 1, Hora, Minu, 0);
                EssaProg.Tipo = Tipo;
                progrs.Add(EssaProg);
            }
        }
    }

    partial class Progr
    {
        public int IdProg = 0;
        public DateTime Tempo;
        public int Tipo = 0;
    }

}
