using System;
using System.Windows.Forms;
using PVS.MediaPlayer;

namespace XeviousPlayer2
{
    public partial class Full : Form
    {
        public Player myPlayer;
        private Overlay myOverlay;
        private TrackBar EsseTrack;

        public Full()
        {
            InitializeComponent();                        
        }

        public void SetaTrack(TrackBar Track)
        {
            this.EsseTrack = Track;
        }

        private void MyPlayer_MediaPositionChanged(object sender, PositionEventArgs e)
        {
            // throw new NotImplementedException();
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
                myPlayer.Events.MediaPositionChanged += MyPlayer_MediaPositionChanged; 
                myPlayer.Sliders.Position.TrackBar = EsseTrack;
            }            
            myPlayer.SleepDisabled = true;
            myOverlay = new Overlay(myPlayer);
            myPlayer.Overlay.Window = myOverlay;
            myPlayer.Display.Window = panel1;
            myPlayer.Overlay.Blend = OverlayBlend.Transparent;
            myPlayer.Audio.Volume = volume;
            myPlayer.Play(Arquivo);

            // this.WindowState = FormWindowState.Minimized;
        }

        private void Full_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                Fecha();
        }

        public void MudarPosicao(double novaPosicao)
        {
            if (myPlayer != null)
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

        public void SincronizarTrackBar(double novaPosicao)
        {
            if (trackBarFull != null && !trackBarFull.Capture)
            {
                int novaPosicaoTrackBar = (int)(novaPosicao * trackBarFull.Maximum);
                if (novaPosicaoTrackBar >= trackBarFull.Minimum && novaPosicaoTrackBar <= trackBarFull.Maximum)
                {
                    trackBarFull.Value = novaPosicaoTrackBar;
                }
            }
        }

    }
}
