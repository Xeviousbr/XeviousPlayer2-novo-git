using System;
using System.Windows.Forms;

namespace XeviousPlayer2
{
    public partial class Importar : Form
    {
        public Importar()
        {
            InitializeComponent();
            tbs.tbConfig Config = new tbs.tbConfig();
            textBox1.Text = Config.PathBase;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Gen.PastaMp3 = textBox1.Text;
            AdicionaMusicas fAdi = new AdicionaMusicas();
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
    }
}
