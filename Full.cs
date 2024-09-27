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

        public void Toca(string Arquivo, float volume)
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
            myPlayer.Audio.Volume = volume;
            myPlayer.Play(Arquivo);
        }

        private void Full_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                Fecha();
        }

        public void MudarPosicao(double novaPosicao)
        {
            if (myPlayer != null && myPlayer.Has.Video)
            {
                // Obter o tempo total do vídeo no Full
                TimeSpan totalTempo = myPlayer.Position.ToStop;

                // Calcular a nova posição
                long novaPosicaoTicks = (long)(totalTempo.Ticks * novaPosicao);

                // Definir a nova posição no player do Full
                myPlayer.Position.FromStart = TimeSpan.FromTicks(novaPosicaoTicks);
            }
        }

        internal void AjustarVolume(float novoVolume)
        {
            myPlayer.Audio.Volume = novoVolume;
        }

        public void SincronizarTrackBar(TrackBar trackBar)
        {
            if (myPlayer != null)
            {
                // Sincroniza o trackBar do Full com o myPlayer do Form1
                myPlayer.Sliders.Position.TrackBar = trackBar;
            }
        }


    }
}
