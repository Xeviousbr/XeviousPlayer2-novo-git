using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using XeviousPlayer2.tbs;

namespace XeviousPlayer2
{
    public partial class Importar : Form
    {
        string NovaLista = "NOVA LISTA";

        public Importar()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int NrLista = 0;
            if (txNovo.Visible)
            {
                tbListas cLista = new tbListas();
                cLista.Nome = txNovo.Text;
                NrLista = cLista.Adiciona();
            }
            else
            {
                Gen.PastaMp3 = txPasta.Text;
                string SQL = "Select IdLista From Listas Where Nome = " + Gen.FA(cbListas.Text);
                string ret = DalHelper.Consulta(SQL);
                NrLista = int.Parse(ret);
            }            
            AdicionaMusicas fAdi = new AdicionaMusicas();
            fAdi.Lista = NrLista;
            fAdi.PastaMp3 = txPasta.Text;
            fAdi.ShowDialog();
            fAdi.Dispose();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txPasta.Text = folderBrowserDialog1.SelectedPath;
                button4.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button4.Enabled = (txPasta.Text.Length > 4);
        }

        private void Importar_Load(object sender, EventArgs e)
        {
            tbs.tbConfig Config = new tbs.tbConfig();
            txPasta.Text = Config.PathBase;
            tbs.tbProg cProg = new tbs.tbProg();
            List<string> Listas = cProg.listas();
            for (int i = 0; i < Listas.Count; i++)
                cbListas.Items.Add(Listas[i]);
            cbListas.Items.Add(NovaLista);            
            cbListas.SelectedIndex = 0;
        }

        private void cbListas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbListas.SelectedItem.ToString() == NovaLista)
            {
                txNovo.Visible = true;
                cbListas.Visible = false;
                txNovo.Focus();
            }
        }
    }
}

