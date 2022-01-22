using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
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
        private bool Entrou = false;
        private string TextoBtSelecionado = "";
        private string Tague = "";
        private object Cont;
        private BotaoSelec OBotaoSelec = null;
        private int MaxAltura = 400;

        public Programacao()
        {
            InitializeComponent();
            Listas();
            this.OBotaoSelec = new BotaoSelec();
        }

        private void Programacao_Load(object sender, EventArgs e)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT Prog.ID, Prog.HorIn, Prog.Lista, Prog.Periodicidade, Listas.Nome ");
            SQL.Append("FROM Prog ");
            SQL.Append("inner join Listas on Listas.IdLista = Prog.Lista ");
            SQL.Append("ORDER BY Prog.Periodicidade");
            SQLiteCommand command = new SQLiteCommand(SQL.ToString(), DalHelper.DbConnection());
            // DateTime Dt0 = new DateTime(2001, 1, 1);
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Int16 Id = reader.GetInt16(0);
                        DateTime HorIn = reader.GetDateTime(1);
                        Int16 IdLista = reader.GetInt16(2);
                        Int16 Peri = reader.GetInt16(3);
                        string Nome = reader.GetString(4);
                        Single siHora = HorIn.Hour;
                        Single siMinute = HorIn.Minute;
                        Single sMinuto = siMinute / 60;
                        siHora += sMinuto;
                        Single Prop = siHora / 24;
                        Single Top = this.MaxAltura * Prop;
                        string Hora = HorIn.Hour.ToString("00") + ":" + siMinute.ToString("00");
                        string Texto = Nome + Hora;
                        string nmBot = "Bt" + Id.ToString();
                        switch (Peri)
                        {
                            case 1:
                                CarregaBotao(nmBot, Texto, Id, panel2, IdLista, Top);
                                break;
                            case 2:
                                CarregaBotao(nmBot, Texto, Id, panel3, IdLista, Top);
                                break;
                            case 3:
                                CarregaBotao(nmBot, Texto, Id, panel4, IdLista, Top);
                                break;
                            case 4:
                                CarregaBotao(nmBot, Texto, Id, panel5, IdLista, Top);
                                break;
                            default:
                                CarregaBotao(nmBot, Texto, Id, panel1, IdLista, Top);
                                break;
                        }
                    }
                }
            }
        }

        private void Listas()
        {
            string SQL = "Select IdLista, Nome From Listas";
            string ret = DalHelper.Consulta(SQL);
            SQLiteCommand command = new SQLiteCommand(SQL.ToString(), DalHelper.DbConnection());
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    int Cont = 0;
                    while (reader.Read())
                    {
                        Int16 IdLista = reader.GetInt16(0);
                        string Nome = reader.GetString(1);
                        string nmBot = "Bt" + Cont.ToString();
                        int Top = Cont * 20;
                        CarregaBotao(nmBot, Nome, Cont, this.panel1, IdLista, Top);
                        Cont++;
                    }
                }
            }
        }

        private void CarregaBotao(string Nome, string Texto, int I, Panel Painel, Int16 IdLista, Single Top)
        {
            Button bt = new CustomButton();
            bt.AllowDrop = true;
            bt.AutoSize = true;
            bt.Location = new Point(3, 3);
            bt.Name = Nome;
            bt.Size = new Size(194, 23);
            bt.TabIndex = 11;

            bt.Top = (int)Top;
            // bt.Top = I * 20;

            bt.Text = Texto;
            bt.UseVisualStyleBackColor = true;

            int NrBts = panel2.Controls.Count;
            bt.Tag = Painel.Tag + "|" + NrBts.ToString() + "|" + IdLista.ToString();
            // bt.Tag = Painel.Tag + "|" + I.ToString() + "|" + IdLista.ToString();

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
            this.OBotaoSelec.pnSelecionado = Convert.ToInt16(partes[0]);
            this.OBotaoSelec.BtSelecionado = Convert.ToInt16(partes[1]);
            this.OBotaoSelec.IdLista = Convert.ToInt16(partes[2]);
            EsseBt.DoDragDrop(TextoBtSelecionado, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            this.XX = button1.Left + e.X;
            this.YY = button1.Top + e.Y;
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
            // int MaxAltura = 400;
            float PosYBSolt = e.Y - 227;
            float PropBt = PosYBSolt / 418;
            float NvPos = PropBt * this.MaxAltura;
            string sPos = NvPos.ToString();
            string[] arrPos = sPos.Split(',');
            int iPos = int.Parse(arrPos[0]);

            // Dedução do novo horário
            float Momento = 1440 * PropBt;
            int Hora = (int)Momento / 60;
            int Minuto = (int)Momento - (Hora * 60);
            if (TextoBtSelecionado.IndexOf(":") > 0)
            {
                TextoBtSelecionado = TextoBtSelecionado.Substring(0, TextoBtSelecionado.Length - 5);
                TextoBtSelecionado = TextoBtSelecionado.Trim();
            }
            string Texto = TextoBtSelecionado + " " + Hora.ToString() + ":" + Minuto.ToString("00");
            if (this.OBotaoSelec.pnSelecionado == 0)
            {
                int Cont = Painel.Controls.Count;
                string nmBot = "Bt" + Cont.ToString();

                // Proparar o ID da lista como parametro
                CarregaBotao(nmBot, Texto, Cont, Painel, this.OBotaoSelec.IdLista, iPos);
            }
            else
            {
                // Pego e solto no mesmo painel num dos tres
                string[] partes = Tague.Split('|');
                int Item = Convert.ToInt16(partes[1]);
                Painel.Controls[Item].Top = iPos;
                Painel.Controls[Item].Text = Texto;
            }
            button1.Enabled = true;
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
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            List<Progr> Progrs = new List<Progr>();
            this.ContProgs(ref Progrs, ref this.panel2, 1);
            this.ContProgs(ref Progrs, ref this.panel3, 2);
            this.ContProgs(ref Progrs, ref this.panel4, 3);
            this.ContProgs(ref Progrs, ref this.panel5, 4);
            tbProg tbH = new tbProg();
            tbH.Zera();
            foreach (var item in Progrs)
            {
                tbH.HorIn = DateTime.Now;
                tbH.Lista = item.IdProg;
                tbH.Periodicidade = item.Tipo;
                tbH.HorIn = item.Tempo;
                tbH.Adiciona();
            }
            this.DialogResult = DialogResult.OK;
            this.Cursor = Cursors.Default;
            Close();
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
                string Tague = ((Button)Painel.Controls[i]).Tag.ToString();
                string[] sPartesTag = Tague.Split('|');
                EssaProg.IdProg = Convert.ToInt16(sPartesTag[2]);
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

    partial class BotaoSelec
    {
        public int pnSelecionado = 0;
        public int BtSelecionado = -1;
        public Int16 IdLista = 0;
    }

}
