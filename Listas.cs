using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XeviousPlayer2
{
    public partial class Listas : Form
    {
        public Listas()
        {
            InitializeComponent();
        }

        public string nmLista { get; internal set; }        

        private void Listas_Load(object sender, EventArgs e)
        {
            tbs.tbProg cProg = new tbs.tbProg();
            List<string> Listas = cProg.listas();
            for (int i = 0; i < Listas.Count; i++)
                lsLista.Items.Add(Listas[i]);                    
        }

        private void btAcionar_Click(object sender, EventArgs e)
        {
            nmLista = lsLista.SelectedItems[0].ToString();
        }

        private void lsLista_DoubleClick(object sender, EventArgs e)
        {
            nmLista = lsLista.SelectedItems[0].ToString();
        }
    }
}
