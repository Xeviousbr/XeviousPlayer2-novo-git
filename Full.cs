using System;
using System.Windows.Forms;
using PVS.MediaPlayer;

namespace XeviousPlayer2
{
    public partial class Full : Form
    {
        private Player myPlayer;
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

        public void Toca(string Arquivo)
        {
            myPlayer = new Player();
            myPlayer.SleepDisabled = true;
            myOverlay = new Overlay(myPlayer);     // create (an instance of) the overlay
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
