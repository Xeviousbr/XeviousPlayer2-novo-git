using System;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Data.Common;

namespace XeviousPlayer2
{
    public partial class Config : Form
    {
        private bool vazio = true;

        public Config()
        {
            InitializeComponent();
            tbs.tbConfig Config = new tbs.tbConfig();
            textBox1.Text = Config.PathBase;
            cbSkin.SelectedIndex = Config.Skin;
            vazio = false;
            ColocaSkin(Config.Skin);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Gravação

            // Atualizar o caminho da pasta base dos Mp3

            // Atualizar o Skin

            ////if (vazio)
            ////{
            ////DalHelper.ExecSql("CREATE TABLE Config(PathBase Text) ");
            ////DalHelper.ExecSql("Insert Into Config (PathBase) Values ('" + textBox1.Text + "')");
            //Gen.PastaMp3 = textBox1.Text;
            //AdicionaMusicas fAdi = new AdicionaMusicas();
            //fAdi.ShowDialog();
            ////} else
            ////{
            ////    DalHelper.ExecSql("Update Config Set PathBase = '" + textBox1.Text + "'");
            ////}
            this.Close();
        }

        private void ColocaSkin(int Skin)
        {
            using (var cmd = new SQLiteCommand(DalHelper.DbConnection()))
            {
                cmd.CommandText = "Select * From Skin Where ID = " + Skin;
                using (SQLiteDataReader regSkin = cmd.ExecuteReader())
                {
                    regSkin.Read();
                    int thiBacA = int.Parse(regSkin["labForA"].ToString());
                    int thiBacB = int.Parse(regSkin["labForB"].ToString());
                    int thiBacC = int.Parse(regSkin["labForC"].ToString());
                    int lvA = int.Parse(regSkin["thiBacA"].ToString());
                    int lvB = int.Parse(regSkin["thiBacB"].ToString());
                    int lvC = int.Parse(regSkin["thiBacC"].ToString());
                    this.BackColor = Color.FromArgb(thiBacA, thiBacB, thiBacC);
                    button1.BackColor = Color.FromArgb(lvA, lvB, lvC);
                    button2.BackColor = button1.BackColor;
                    button3.BackColor = button1.BackColor;
                    button4.BackColor = button1.BackColor;
                    btPrograma.BackColor = button1.BackColor;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Gen.PastaMp3 = textBox1.Text;
            AdicionaMusicas fAdi = new AdicionaMusicas();
            fAdi.ShowDialog();
            fAdi.Dispose();
            this.Close();
        }

        private void btPrograma_Click(object sender, EventArgs e)
        {
            Programacao fProg = new Programacao();
            fProg.ShowDialog();
            fProg.Dispose();
            this.Close();
        }
    }

}
