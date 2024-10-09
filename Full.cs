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
        private bool jaChamouProxima = false;
        private Form1 parentForm;

        public Full(Form1 parent)
        {
            InitializeComponent();
            this.parentForm = parent;
        }


        public void SetaTrack(TrackBar Track)
        {
            this.EsseTrack = Track;
        }

        private void MyPlayer_MediaPositionChanged(object sender, PositionEventArgs e)
        {
            if (e.ToStop < 1000000 && !jaChamouProxima)
            {
                jaChamouProxima = true;
                parentForm.ProxMusica();
            }
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
            jaChamouProxima = false;
        }

        private void Full_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                Fecha();
        }

        internal void AjustarVolume(float novoVolume)
        {
            myPlayer.Audio.Volume = novoVolume;
        }

    }
}
