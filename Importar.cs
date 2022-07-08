using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace XeviousPlayer2
{
    public partial class Importar : Form
    {
        public Importar()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Gen.PastaMp3 = textBox1.Text;
            string SQL = "Select IdLista From Listas Where Nome = " + Gen.FA(cbListas.Text);
            string ret = DalHelper.Consulta(SQL);
            AdicionaMusicas fAdi = new AdicionaMusicas();
            fAdi.Lista = int.Parse(ret);
            fAdi.ShowDialog();
            fAdi.Dispose();
            this.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                button4.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button4.Enabled = (textBox1.Text.Length > 4);
        }

        private void Importar_Load(object sender, EventArgs e)
        {
            tbs.tbConfig Config = new tbs.tbConfig();
            textBox1.Text = Config.PathBase;
            tbs.tbProg cProg = new tbs.tbProg();
            List<string> Listas = cProg.listas();
            for (int i = 0; i < Listas.Count; i++)
            {
                cbListas.Items.Add(Listas[i]);
            }
        }

    }
}

