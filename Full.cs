using System;
using System.Windows.Forms;
using PVS.MediaPlayer;

namespace XeviousPlayer2
{
    public partial class Full : Form
    {
        public Player myPlayer;
        private Overlay myOverlay;

        public Full()
        {
            InitializeComponent();
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            Fecha();
        }

        private void Fecha()
        {
            // Atualiazar o tempo do player do main
            this.Close();
        }

        public void Para()
        {
            myPlayer.Stop();
        }

        public void Toca(string Arquivo)
        {
            if (myPlayer==null)
            {
                myPlayer = new Player();
            }            
            myPlayer.SleepDisabled = true;
            myOverlay = new Overlay(myPlayer);
            myPlayer.Overlay.Window = myOverlay;   // and attach it to the player
            myPlayer.Display.Window = panel1;
            myPlayer.Overlay.Blend = OverlayBlend.Transparent;
            myPlayer.Play(Arquivo);
        }

        private void Full_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                Fecha();
        }
    }
}
