using System;
using System.IO;
using System.Text;
using System.Drawing;
using PVS.MediaPlayer;
using System.Data.SQLite;
using System.Data.Common;
using XeviousPlayer2.tbs;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

// Provavelmente não de pra pegar informações de visualização das musicas
// Então pelo FFMpeg deve dar
// https://www.codeproject.com/Articles/5337264/NET-Wrapper-of-FFmpeg-libraries

namespace XeviousPlayer2
{
    public partial class Form1 : Form
    {
        /*
            https://sourceforge.net/projects/mfnet
            http://mfnet.sourceforge.net
            https://www.codeproject.com/Articles/109714/PVS-MediaPlayer-Audio-and-Video-Player-Library
        */

        // **** Class Fields **************************************************************************

        #region Class Fields

        private Player myPlayer;
        private Overlay myOverlay;          // in file 'Overlay.cs'
        private System.Windows.Forms.Timer delayTimer;

        // private int shapeStatus;        // shapes - 0:none, 1:oval, 2:none, 3:rounded, 4:none, 5:star

        private InfoLabel myInfoLabel;
        private StringBuilder myInfoLabelText = new StringBuilder(64);

        // used with drawing audio output levels
        // levelUnit is the size of 1 unit (of 32767) and 140 is the width of panel4 and panel5
        private double levelUnit = 140; // size of panel
        private int leftLevel;
        private int rightLevel;
        private Brush levelBrush = new HatchBrush(HatchStyle.LightVertical, Color.FromArgb(179, 173, 146));

        private Metadata metaData;           // media metadata properties
        private OpenFileDialog myOpenFileDlg;      // used with selection of media to play
        /* private const string    OPENMEDIA_DIALOG_FILTER =
            " Media Files (*.*)|*.3g2; *.3gp; *.3gp2; *.3gpp; *.aac; *.adts; *.asf; *.avi; *.m4a; *.m4v; *.mkv; *.mov; *.mp3; *.mp4; *.mpeg; *.mpg; *.sami; *.smi; *.wav; *.webm; *.wma; *.wmv|" +
            " All Files|*.*"; */

        private bool isDisposed;         // used with cleaning up

        // private string NomeLista = "";

        #endregion

        // **** Main **********************************************************************************

        //private bool Tocando = false;
        private long eToEnd = -1;
        private int IndiceNaLista;
        private bool TratarFinalDaMusica = true;
        private bool ProgLigada = false;
        private string TocandoAgora = "";
        private bool TemVideo = false;
        private int _ListaAtu = -1;
        //private string Lista = "";

        private bool Fechando = false;
        private string nmLista;

        private Full cFull;
        private bool usuarioInteragindoComTrackBar = false;
        private TimeSpan novaPosicaoTempo;
        private DateTime ultimoAjusteManual;

        public int ListaAtu
        {
            get { return _ListaAtu; }
            set { 
                if (value!= _ListaAtu)
                {
                    _ListaAtu = value;
                    tbConfig Config = new tbConfig();
                    Config.SetaUltLista(value);
                }                
            }
        }

        #region Main

        public Form1()
        {
            Gen.Loga("InitializeComponent");
            InitializeComponent();                      // this call is required by the designer            
            Gen.Loga("new Player()");
            myPlayer                = new Player();     // create a player
            myPlayer.Display.Window = panel1;           // and set its display to panel1
            myPlayer.Repeat         = false;             // repeat media playback when finished
            myPlayer.SleepDisabled  = true;             // prevent the computer from entering sleep mode
            Gen.Loga("new OpenFileDialog()");
            myOpenFileDlg = new OpenFileDialog()        // create a file selector
            {
                Title       = "Play Media",
                Filter      = " Media Files (*.*)|" + Gen.OPENMEDIA_DIALOG_FILTER +" All Files|*.*",
                Multiselect = true,
                FilterIndex = 1 
            };
            myPlayer.Display.DragEnabled = true;
            myPlayer.Display.DragCursor = Cursors.SizeAll; // 'SizeAll' is also the default drag cursor
            myPlayer.CursorHide.Add(this);
            myPlayer.CursorHide.Delay = 3; // 3 seconds is also the default waiting time
            Gen.Loga("new Overlay(myPlayer)");
            myOverlay = new Overlay(myPlayer);     // create (an instance of) the overlay
            myPlayer.Overlay.Window = myOverlay;   // and attach it to the player
            myPlayer.Overlay.Blend = OverlayBlend.Transparent;
            myPlayer.Events.MediaEnded += MyPlayer_MediaEnded;  // see eventhandler below
            myPlayer.Events.MediaEndedNotice += MyPlayer_MediaEndedNotice;  // see eventhandler below
            myPlayer.TaskbarProgress.Add(this);
            myPlayer.TaskbarProgress.Mode = TaskbarProgressMode.Track;
            myPlayer.Events.MediaPositionChanged += MyPlayer_MediaPositionChanged; // see handler below
            myPlayer.Sliders.Position.TrackBar = trackBar1;
            myPlayer.Events.MediaAudioVolumeChanged += MyPlayer_MediaAudioVolumeChanged; // see handler below
            myPlayer.Events.MediaAudioBalanceChanged += MyPlayer_MediaAudioBalanceChanged; // see below
            myPlayer.Sliders.AudioVolume  = trackBar2;    // audio volume slider
            myPlayer.Sliders.AudioBalance = trackBar3;    // audio balance slider
            myPlayer.Events.MediaPeakLevelChanged += MyPlayer_MediaPeakLevelChanged; // see handler below
            myPlayer.Events.MediaSubtitleChanged += MyPlayer_MediaSubtitleChanged; // see eventhandler below
           myPlayer.Subtitles.DirectoryDepth = 1; // search base folder and containing folders 1 level deep
            myInfoLabel = new InfoLabel
            {
                RoundedCorners  = true,                     // use rounded corners
                FontSize        = 9.75f,                    // set font size (same as main window)
                TextMargin      = new Padding(2, 0, 2, 0),  // fine tuning inner spacing
                AlignOffset     = new Point(0, 7)           // move closer to slider thumb
            };
            LinearGradientBrush myBackBrush = new LinearGradientBrush(new Rectangle(new Point(0, 0), myInfoLabel.Size), SystemColors.ButtonHighlight, SystemColors.ButtonShadow, LinearGradientMode.Vertical);
            myInfoLabel.BackBrush = myBackBrush;

            // trackBar1.Scroll += TrackBar1_Scroll;   // see eventhandler below

            // Same for the audio volume and balance sliders:
            trackBar2.Scroll += TrackBar2_Scroll;   // see eventhandler below
            trackBar3.Scroll += TrackBar3_Scroll;   // see eventhandler below

            //ColocaSkin();


            //Toca(@"H:\Temp\Mp3Novos\Ave Maria  Aria Vol 2  Cafe del Mar.mp3");
            string[] arguments = Environment.GetCommandLineArgs();
            if (arguments.Length > 1) { 
                Gen.Loga("Musica arguments[1].ToString()");
                Toca(arguments[1].ToString());
            }
            else
            {
                tbConfig Config = new tbConfig();
                Config.Carrega();
                this.ProgLigada = Config.Progr;
                if (this.ProgLigada)
                {
                    Gen.Loga("ProgLigada");
                    tsProg.Image = ligadoToolStripMenuItem.Image;
                    VePrograma();
                } else
                    Gen.Loga("Prog Não Ligada");
                if (Config.UltLista > 0)
                {
                    this._ListaAtu = Config.UltLista;
                    setaLista(this.ListaAtu);
                }
            }
        }

        private void VePrograma()
        {
            tbProg tbH = new tbProg();
            tbH.getProg();

            // ATC
            // SEMPRE 1 PQ NA IMPORTAÇÃO TO ZERANDO AS LISTAS
            // this.ListaAtu = 1;

            if (tbH.Lista > 0)
            {
                this.ListaAtu = tbH.Lista;
                this.nmLista = tbH.getnmLista();
                setaLista(this.ListaAtu);
            } 
            
        }

        // Show display overlay at start up
        private void Form1_Shown(object sender, System.EventArgs e)
        {
        // show the display overlay at the start of the application (even if no movie is playing):
        // this instruction is put here because the player's display has to be 'created and visible'
        // to show the overlay:
        //myPlayer.Overlay.Hold = true;

        // http://portugalvbnet.blogspot.com/2011/10/continuando-o-artigo-anterior-sobre.html

            //DalHelper.CriarBancoSQLite();
        }

        // Cleaning up - this is moved here from the 'Form1.Designer.cs' file and appended:
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                if (disposing)
                {
                    // disposing a player also stops its overlay, display clones, eventhandlers etc.
                    if (myPlayer != null)       myPlayer.Dispose();         // stop and dispose the player
                    if (myOverlay != null)      myOverlay.Dispose();        // dispose the display overlay
                    if (myOpenFileDlg != null)  myOpenFileDlg.Dispose();    // dispose the file selector
                    if (myInfoLabel != null)    myInfoLabel.Dispose();      // dispose the info label
                    if (levelBrush != null)     levelBrush.Dispose();       // dispose the paint brush
                    if (metaData != null)       metaData.Dispose();         // dispose media metadata properties

                    // used by the designer - clean up:
                    if (components != null) components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // Dispose metadata properties
        private void DisposeMetadata()
        {
            if (metaData != null)
            {
                myOverlay.subtitlesLabel.Text = string.Empty;
                panel1.BackgroundImage = null;
                metaData.Dispose(); // this also disposes the image (used with panel1.BackgroundImage)
                metaData = null;
            }
        }

        #endregion

        // **** Media Play / Pause - Resume / Stop ****************************************************

        #region Media Play / Pause - Resume / Stop

        private void PlayMedia()
        {
            if (myOpenFileDlg.ShowDialog() == DialogResult.OK)
            {
                Toca(myOpenFileDlg.FileName);
            }
        }

        private void Toca(string Musica)
        {
            if (myPlayer.Playing)
                try
                {
                    myPlayer.Stop();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            this.TratarFinalDaMusica = false;
            myPlayer.Play(Musica);
            this.TocandoAgora = Musica;
#if DEBUG
            // myPlayer.Audio.Volume = (float)0.01;
#endif
            if (myPlayer.LastError)
                MessageBox.Show(myPlayer.LastErrorString);
            else
            {
                metaData = myPlayer.Media.GetMetadata();
                string Nome = "";
                if (metaData.Title == null)
                    Nome = Gen.RetNomePeloCaminho(Musica);
                else
                {
                    Nome = metaData.Title;
                    if (Nome.Length < 2)
                        Nome = Gen.RetNomePeloCaminho(Musica);
                }
                lbMusica.Text = Gen.TrataNome(Nome, metaData.Artist);
                lbArtista.Text = metaData.Artist;
                lbAlbum.Text = metaData.Album;
                lbGenero.Text = metaData.Genre;
                lbAno.Text = metaData.Year;
                lbLocal.Text = Musica;
                lbBandaNome.Text = lbArtista.Text + " " + lbMusica.Text;                
                myOverlay.subtitlesLabel.Text = metaData.Artist + "\r\n" + Nome;
                panel1.Visible = myPlayer.Has.Video;
                this.TemVideo = myPlayer.Has.Video;
                if (!this.TemVideo)
                {
                    panel1.Visible = false;
                    if (metaData.Image != null)
                    {
                        // NÃO TA CARREGANDO A IMAGEM
                        string ArqImg = Application.StartupPath + @"\img.bmp";
                        metaData.Image.Save(ArqImg, ImageFormat.Bmp);
                        picImg.Load(ArqImg);
                        picImg.Visible = true;
                        picImg.Refresh();
                    }                        
                }
                string sStatus = "Tocando " + Nome + " de " + metaData.Artist;
                if (this.nmLista.Length >0)
                    sStatus += " Lista: "+this.nmLista;
                Status.Text = sStatus;
            }
            this.eToEnd = -1;
            this.TratarFinalDaMusica = true;
        }

        //private void PauseMedia()
        //{
        //    myPlayer.Paused = !myPlayer.Paused;
        //    if (myPlayer.Paused) button2.Text = "Resume";
        //    else button2.Text = "Pause";
        //}

        private void StopMedia()
        {
            myPlayer.Stop();
        }

        #endregion

        // **** Player Eventhandlers ******************************************************************

        #region Player Eventhandlers

        // Show changed audio volume value
        private void MyPlayer_MediaAudioVolumeChanged(object sender, System.EventArgs e)
        {
            //label3.Text = (myPlayer.Audio.Volume).ToString("0.00");
        }

        // Show changed audio balance value
        private void MyPlayer_MediaAudioBalanceChanged(object sender, System.EventArgs e)
        {
            //label4.Text = (myPlayer.Audio.Balance).ToString("0.00");
        }

        // Display the elapsed and remaining playback time
        private void MyPlayer_MediaPositionChanged(object sender, PositionEventArgs e)
        {
            if (Fechando==false)
            {
                label1.Text = TimeSpan.FromTicks(e.FromStart).ToString().Substring(0, 8); // "hh:mm:ss"
                lbTempo.Text = label1.Text;
                if (this.TratarFinalDaMusica == true)
                {
                    if (this.TemVideo)
                    {
                        if (e.ToStop < 1000000)
                        {
                            this.ProxMusica();
                        }
                    }
                    else
                    {
                        if (this.eToEnd > -1)
                        {
                            if (this.eToEnd < e.ToEnd)
                            {
                                this.ProxMusica();
                            }
                            else
                            {
                                if (e.ToEnd == 0)
                                {
                                    this.ProxMusica();
                                }
                            }
                        }
                    }
                    this.eToEnd = e.ToEnd;
                }

            }
        }

        // Handle media audio output levels - calculate the values and paint the level displays
        private void MyPlayer_MediaPeakLevelChanged(object sender, PeakLevelEventArgs e)
        {
            // you could add some 'logic' here to make the movements of the indicators less 'jumpy'

            if (e.MasterPeakValue == -1) // media playback has stopped, paused or ended
            {
                // graphical presentation:
                leftLevel = rightLevel = 0;

                // value display:
                //label5.Text = "0.00"; // same format as below
                //label6.Text = "0.00";
            }
            else
            {
                // check e.ChannelCount for more than 2 (= stereo) channels
                // if you want to display the peak levels of all audio channels

                // graphical presentation:
                leftLevel = (int)(e.ChannelsValues[0] * levelUnit); // levelUnit is just the width of panel 4 and 5
                rightLevel = (int)(e.ChannelsValues[1] * levelUnit);

                // value display (use string format because of float rounding errors - zero can become a small value):
                //label5.Text = e.ChannelsValues[0].ToString("0.00");
                //label6.Text = e.ChannelsValues[1].ToString("0.00");
            }
            panel4.Invalidate();
            panel5.Invalidate();
        }

        // Paint the left channel audio output level display
        private void Panel4_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(levelBrush, 0, 0, leftLevel, panel4.ClientRectangle.Height);
        }

        // Paint the right channel audio output level display
        private void Panel5_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(levelBrush, 0, 0, rightLevel, panel5.ClientRectangle.Height);
        }

        // Mouse clicked on player display
        private void MyPlayer_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(e.Button.ToString() + " mouse button clicked on player display.", "PVS.MediaPlayer How To...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (myPlayer.Display.Mode == DisplayMode.Stretch) myPlayer.Display.Mode = DisplayMode.ZoomCenter;
            else myPlayer.Display.Mode = DisplayMode.Stretch;
        }

        // Mouse clicked on player display clone
        private void Clone_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(e.Button.ToString() + " mouse button clicked on player display clone.", "PVS.MediaPlayer How To...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CloneProperties props = myPlayer.DisplayClones.GetProperties((Control)sender);
            if (props.Layout == CloneLayout.Stretch) props.Layout = CloneLayout.Zoom;
            else props.Layout = CloneLayout.Stretch;
            myPlayer.DisplayClones.SetProperties((Control)sender, props);
        }

        // Media has finished or stopped playing (1)
        private void MyPlayer_MediaEndedNotice(object sender, EndedEventArgs e)
        {
            // you can just stop any processes (and not starting new media) from the
            // MediaEndedNotice eventhandler that is fired just before the MediaEnded event.

            int x = 0;
            switch (e.StopReason)
            {
                case StopReason.Finished:
                    x = 1;
                    break;

                case StopReason.AutoStop:
                    x = 2;
                    break;

                case StopReason.UserStop:
                    x = 3;
                    break;

                case StopReason.Error:
                    x = 4;
                    // this can be treated the same as StopReason.Finished
                    break;
            }

        }

        // Media has finished or stopped playing (2)
        private void MyPlayer_MediaEnded(object sender, EndedEventArgs e)
        {
            DisposeMetadata();
            int x = 0;
            switch (e.StopReason)
            {
                case StopReason.Finished:
                    x = 1;
                    // play next media ...
                    break;

                case StopReason.AutoStop:
                    x = 2;
                    break;

                case StopReason.UserStop:
                    x = 3;
                    break;

                case StopReason.Error:
                    x = 4;
                    // this can be treated the same as StopReason.Finished if
                    // you don't want to handle these errors (may not occur at all)
                    break;
            }
        }

        // Get media subtitles / media subtitle has changed
        private void MyPlayer_MediaSubtitleChanged(object sender, SubtitleEventArgs e)
        {
            // In this example the subtitle's text is shown in a label on a display overlay
            myOverlay.subtitlesLabel.Text = e.Subtitle;
        }

        private void TrackBar2_Scroll(object sender, System.EventArgs e)
        {
            if (cFull!=null)
            {
                float novoVolume = trackBar2.Value / 100f;
                cFull.AjustarVolume(novoVolume);
            }
            Point myInfoLabelLocation = myPlayer.Sliders.ValueToPoint(trackBar2, trackBar2.Value);
            myInfoLabel.Show("Volume: " + (myPlayer.Audio.Volume).ToString("0.00"), trackBar2, myInfoLabelLocation);
        }

        // Show an info label on the audio balance slider of the player when scrolled
        private void TrackBar3_Scroll(object sender, System.EventArgs e)
        {
            // Get the audio balance slider's x-coordinate of the current balance (= thumb location)
            // (myInfoLabel.AlignOffset has been set to 0, 7)
            Point myInfoLabelLocation = myPlayer.Sliders.ValueToPoint(trackBar3, trackBar3.Value);

            // Calculate balance display value
            float val = myPlayer.Audio.Balance;

            // Set the text for the info label (using StringBuilder)
            myInfoLabelText.Length = 0;
            if (val == 0) myInfoLabelText.Append("Balance: Center");
            else
            {
                if (val < 0) myInfoLabelText.Append("Balance: Left ").Append((-val).ToString("0.00"));
                else myInfoLabelText.Append("Balance: Right ").Append((val).ToString("0.00"));
            }

            // Show the info label
            myInfoLabel.Show(myInfoLabelText.ToString(), trackBar3, myInfoLabelLocation);
        }

        #endregion

        // **** Controls Handling *********************************************************************

        #region Toolbar de cima

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            PlayMedia();
            //Tocando = true;
        }

        #endregion

        #region Toolbar de baixo

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            float VolAnt = 0;

            // Verifica se existe uma lista carregada (nmLista não é nula nem vazia)
            if (!string.IsNullOrEmpty(nmLista))
            {
                if (myOpenFileDlg.ShowDialog() == DialogResult.OK)
                {
                    string[] arquivosSelecionados = myOpenFileDlg.FileNames; // Obter todos os arquivos selecionados

                    foreach (string novaMidia in arquivosSelecionados)
                    {
                        string nomeBanda = ExtrairNomeBanda(novaMidia);
                        AdicionarMidiaNaLista(novaMidia, nomeBanda);
                    }

                    // Tocar a primeira mídia imediatamente, se desejar
                    if (arquivosSelecionados.Length > 0)
                    {
                        Toca(arquivosSelecionados[0]);
                    }
                }
            }
            else
            {
                // Se não houver lista carregada, continuar com o comportamento padrão
                if (myPlayer.Paused)
                {
                    VolAnt = myPlayer.Audio.Volume;
                    myPlayer.Audio.Volume = 0;
                    myPlayer.Paused = false;
                }

                PlayMedia(); // Continua abrindo o diálogo normalmente

                if (VolAnt > 0)
                    myPlayer.Audio.Volume = VolAnt;
            }
        }

        private void AdicionarMidiaNaLista(string novaMidia, string nomeBanda)
        {
            string nome = Path.GetFileNameWithoutExtension(novaMidia); // Obtém o nome da música sem a extensão
            string lugar = novaMidia; // Caminho completo da mídia no HD

            FileInfo fileInfo = new FileInfo(novaMidia);
            long tamanhoBytes = fileInfo.Length;
            long lTamanho = tamanhoBytes / 1024;
            string tamanhoKB = lTamanho.ToString(); 

            // Abre a conexão com o banco de dados
            using (var connection = DalHelper.DbConnection())
            {
                // Obtém ou insere a banda no banco de dados
                int idBanda = InserirOuObterIdBanda(connection, nomeBanda);

                // Obtém ou insere a música no banco de dados, passando o ID da banda
                int idMusica = InserirOuObterIdMusica(connection, nome, lugar, tamanhoKB, idBanda);

                // Adiciona a música à lista no banco de dados
                AdicionarMusicaNaLista(connection, idMusica, this.ListaAtu);

                // Adiciona a música ao ListView na interface
                ListViewItem novoItem = new ListViewItem(new string[] {
                    idMusica.ToString(), // ID da música
                    nome,
                    lugar, // Caminho da mídia
                    "0", // Número de vezes tocada inicial
                    DateTime.Now.ToString("dd/MM/yyyy"), // Data atual como última vez tocada
                    tamanhoKB
                }, -1);

                // Adiciona o item no início da lista (ListView)
                listView.Items.Insert(0, novoItem);
            }
        }

        private string ExtrairNomeBanda(string caminhoArquivo)
        {
            // Obtém o nome do arquivo sem o caminho completo
            string nomeArquivo = Path.GetFileNameWithoutExtension(caminhoArquivo);

            // Divide o nome do arquivo por " - " e pega a primeira parte como o nome da banda
            string[] partes = nomeArquivo.Split(new string[] { " - " }, StringSplitOptions.None);

            if (partes.Length > 1)
            {
                return partes[0]; // A primeira parte antes do " - " é o nome da banda
            }
            else
            {
                return "Banda Desconhecida"; // Retorna um valor padrão se não for possível extrair o nome da banda
            }
        }

        private void AdicionarMusicaNaLista(SQLiteConnection connection, int idMusica, int idLista)
        {
            // Verifica se a música já está associada à lista
            string sqlVerifica = "SELECT COUNT(*) FROM LisMus WHERE IdMusica = @IdMusica AND Lista = @Lista";
            using (var commandVerifica = new SQLiteCommand(sqlVerifica, connection))
            {
                commandVerifica.Parameters.AddWithValue("@IdMusica", idMusica);
                commandVerifica.Parameters.AddWithValue("@Lista", idLista);

                var result = commandVerifica.ExecuteScalar();
                if (result != null && Convert.ToInt32(result) == 0)
                {
                    // Se não estiver na lista, insere
                    string sqlInserir = "INSERT INTO LisMus (IdMusica, Lista) VALUES (@IdMusica, @Lista)";
                    using (var commandInserir = new SQLiteCommand(sqlInserir, connection))
                    {
                        commandInserir.Parameters.AddWithValue("@IdMusica", idMusica);
                        commandInserir.Parameters.AddWithValue("@Lista", idLista);
                        commandInserir.ExecuteNonQuery();
                    }
                }
            }
        }

        private int InserirOuObterIdBanda(SQLiteConnection connection, string nomeBanda)
        {
            int idBanda = 0;

            // Verifica se a banda já existe no banco de dados
            string sqlVerifica = "SELECT IDBanda FROM Bandas WHERE Nome = @Nome";
            using (var commandVerifica = new SQLiteCommand(sqlVerifica, connection))
            {
                commandVerifica.Parameters.AddWithValue("@Nome", nomeBanda);

                var result = commandVerifica.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    idBanda = Convert.ToInt32(result); // A banda já existe, retorna o ID
                }
            }

            // Se a banda não existir, insere-a
            if (idBanda == 0)
            {
                string sqlInserir = @"INSERT INTO Bandas (Nome) VALUES (@Nome); 
                              SELECT last_insert_rowid();";
                using (var commandInserir = new SQLiteCommand(sqlInserir, connection))
                {
                    commandInserir.Parameters.AddWithValue("@Nome", nomeBanda);

                    var result = commandInserir.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idBanda = Convert.ToInt32(result); // Banda inserida, retorna o novo ID
                    }
                }
            }

            return idBanda;
        }

        private int InserirOuObterIdMusica(SQLiteConnection connection, string nome, string lugar, string tamanho, int idBanda)
        {
            int idMusica = 0;

            // Verifica se a música já existe no banco de dados
            string sqlVerifica = "SELECT IDMusica FROM Musicas WHERE Nome = @Nome AND Lugar = @Lugar";
            using (var commandVerifica = new SQLiteCommand(sqlVerifica, connection))
            {
                commandVerifica.Parameters.AddWithValue("@Nome", nome);
                commandVerifica.Parameters.AddWithValue("@Lugar", lugar);

                var result = commandVerifica.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    idMusica = Convert.ToInt32(result); // A música já existe, retorna o ID
                }
            }

            // Se a música não existe, inseri-la no banco de dados
            if (idMusica == 0)
            {
                string sqlInserir = @"INSERT INTO Musicas (Nome, Lugar, Vezes, TocadoEm, Tamanho, Banda) 
                              VALUES (@Nome, @Lugar, 0, @TocadoEm, @Tamanho, @Banda);
                              SELECT last_insert_rowid();";
                using (var commandInserir = new SQLiteCommand(sqlInserir, connection))
                {
                    commandInserir.Parameters.AddWithValue("@Nome", nome);
                    commandInserir.Parameters.AddWithValue("@Lugar", lugar);
                    commandInserir.Parameters.AddWithValue("@TocadoEm", DateTime.Now.ToString("yyyy-MM-dd"));
                    commandInserir.Parameters.AddWithValue("@Tamanho", tamanho);
                    commandInserir.Parameters.AddWithValue("@Banda", idBanda); // Adiciona o ID da banda

                    var result = commandInserir.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idMusica = Convert.ToInt32(result); // A música foi inserida, retorna o novo ID
                    }
                }
            }
            return idMusica;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // Aciona o Microfone
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            // Musica Anterior
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // Recarrega
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            if (IndiceNaLista>0)
            {
                this.listView.Items[this.IndiceNaLista].Focused = false;
                this.listView.Items[this.IndiceNaLista].Selected = false;
                IndiceNaLista = IndiceNaLista - 2;
                this.ProxMusica();
            }
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            myPlayer.Paused = true;
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            if (cFull == null)
            {
                myPlayer.Paused = false;
            }
            else
            {
                cFull.myPlayer.Paused = false;
            }            
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            if (cFull==null)
            {
                myPlayer.Paused = true;
            } else
            {
                cFull.myPlayer.Paused = true;                
            }            
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            // Gravar
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            if (IndiceNaLista < this.listView.Items.Count)
            {
                this.ProxMusica();
            }
        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            // ir a frente
        }

        private void toolStripButton20_Click(object sender, EventArgs e)
        {
            // mostrar ou esconder o controle de volume
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            // Compartilhar
        }

        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            Config cConfig = new Config();
            int ListaAtu = Gen.Lista;
            cConfig.ShowDialog();
            if (ListaAtu!= Gen.Lista)
            {
                setaLista(Gen.Lista);
                tbConfig tConfig = new tbConfig();
                tConfig.SetaUltLista(Gen.Lista);
            }
        }

        #endregion

        #region Inicializacao

        //private void ColocaSkin()
        //{
        //    // Por enquanto tem só duas telas que usam o Skin
        //    // Mas se tiver mais o ideal é colocar numa classe específica
        //    string SQL = "Select Skin From Config";
        //    string ret = DalHelper.Consulta(SQL);
        //    int Skin = int.Parse(ret);
        //    using (var cmd = new SQLiteCommand(DalHelper.DbConnection()))
        //    {
        //        cmd.CommandText = "Select * From Skin Where ID = " + Skin;
        //        using (SQLiteDataReader regSkin = cmd.ExecuteReader())
        //        {
        //            regSkin.Read();
        //            int thiBacA = int.Parse(regSkin["labForA"].ToString());
        //            int thiBacB = int.Parse(regSkin["labForB"].ToString());
        //            int thiBacC = int.Parse(regSkin["labForC"].ToString());                    
        //            int panA = int.Parse(regSkin["panBacA"].ToString());
        //            int panB = int.Parse(regSkin["panBacB"].ToString());
        //            int panC = int.Parse(regSkin["panBacC"].ToString());                    
        //            int lvA = int.Parse(regSkin["thiBacA"].ToString());
        //            int lvB = int.Parse(regSkin["thiBacB"].ToString());
        //            int lvC = int.Parse(regSkin["thiBacC"].ToString());
        //            this.BackColor = Color.FromArgb(thiBacA, thiBacB, thiBacC);
        //            this.panel1.BackColor = Color.FromArgb(panA, panB, panC);
        //            this.listView.BackColor = Color.FromArgb(lvA, lvB, lvC);
        //        }
        //    }
        //}

        #endregion

        public void setaLista(int ilista)
        {

            StringBuilder SQL = new StringBuilder();
            SQL.AppendLine("Select Musicas.IDMusica, Musicas.Nome, Musicas.Lugar, Musicas.Vezes, Musicas.TocadoEm, ");
            SQL.AppendLine("Musicas.Tamanho, Musicas.Tempo, Listas.Nome as NomeLista");
            SQL.AppendLine("From LisMus");
            SQL.AppendLine("Inner Join Musicas on Musicas.IDMusica = LisMus.IdMusica");
            SQL.AppendLine("Inner Join Bandas on Bandas.IDBanda = Musicas.Banda");
            SQL.AppendLine("Inner Join Listas on Listas.IdLista = LisMus.Lista");
            SQL.AppendLine("Where LisMus.Lista = "+ ilista.ToString());

            SQLiteCommand command = new SQLiteCommand(SQL.ToString(), DalHelper.DbConnection());

            // listView.Columns.Clear();
            listView.Items.Clear();
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string Nome = reader.GetString(1);
                        string Lugar = reader.GetString(2);

                        // É NECESSÁRIO UM AJUSTE DE SQL, pra trocar de para H:
                        // Lugar = Lugar.Replace("F:", "E:");

                        string Vezes = reader.GetInt16(3).ToString();
                        string TocadoEm = reader.GetDateTime(4).ToString();
                        Int32 iTam32 = reader.GetInt32(5);
                        Single sTam = iTam32 / 100000;
                        sTam = sTam / 10;
                        string Tamanho = sTam.ToString() + " Kb";
                        ListViewItem listViewItem1 = new ListViewItem(new string[] { Nome, Lugar, Vezes, TocadoEm, Tamanho }, -1);
                        listView.Items.Add(listViewItem1);
                        this.nmLista = reader.GetString(7);
                    }
                }
            }
            listView.View = View.Details;
            this.listView.Refresh();

            this.IndiceNaLista = -1;
            ProxMusica(false);


        }

        public void ProxMusica(bool PreverProgramacao=true)
        {
            Gen.Loga("Chamando ProxMusica. Índice atual: " + IndiceNaLista);
            if (PreverProgramacao)
                if (this.ProgLigada == true)
                {
                    tbProg tbH = new tbProg();
                    tbH.getProg();
                    if (tbH.Lista>0)
                        if (tbH.Lista != this.ListaAtu)
                        {
                            this.ListaAtu = tbH.Lista;
                            setaLista(this.ListaAtu);
                            return;
                        }
                }
            bool Sair = false;
            int ic = this.listView.Items.Count;
            while (Sair==false)
            {
                int il = this.IndiceNaLista;                
                if (il > -1)
                {                    
                    if (ic > 0)
                    {
                        if (il < ic)
                        {
                            this.listView.Items[il].Focused = false;
                            this.listView.Items[il].Selected = false;
                        } else
                        {
                            Sair = true;
                        }
                    } else
                    {
                        Sair = true;
                    }
                }
                this.IndiceNaLista++;                
                if (ic > 0)
                {
                    if (this.IndiceNaLista< ic)
                    {
                        string Tocar = this.listView.Items[this.IndiceNaLista].SubItems[1].Text;
                        float VolAnt = 0;
                        if (File.Exists(Tocar))
                        {
#if DEBUG
                            VolAnt = (float)0.01;
#else
                VolAnt = myPlayer.Audio.Volume;
#endif
                            this.ColocaDadosMusica();
                            if (cFull == null)
                            {
                                this.Toca(Tocar);
                                myPlayer.Audio.Volume = VolAnt;
                            }
                            else
                            {
                                cFull.Toca(Tocar, VolAnt);
                            }                            
                            Sair = true;
                        }

                    }
                }
            }
        }

        private void ColocaDadosMusica()
        {
            this.listView.Items[this.IndiceNaLista].Focused = true;
            this.listView.Items[this.IndiceNaLista].Selected = true;
            lbVezes.Text = this.listView.Items[this.IndiceNaLista].SubItems[2].Text;
            lbTocou.Text = this.listView.Items[this.IndiceNaLista].SubItems[3].Text;
            lbTam.Text = this.listView.Items[this.IndiceNaLista].SubItems[4].Text;

        }

        #region Mostrar e ocultar Detalhes

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            OcultarDetalhes();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            OcultarDetalhes();
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            MostrarDetalhes();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MostrarDetalhes();
        }

        private void OcultarDetalhes()
        {
            pnlDetalhes.Visible = false;
            listView.Top = 65;
            listView.Height = 500;
            label2.Visible = true;
            label3.Visible = false;
            label3.Visible = false;
            pictureBox3.Visible = true;
            lbTempo.Visible = true;
            lbBandaNome.Visible = true;
            lbBandaNome.Visible = true;
        }

        private void MostrarDetalhes()
        {
            pnlDetalhes.Visible = true;
            listView.Top = 219;
            listView.Height = 305;
            label2.Visible = false;
            label3.Visible = true;
            pictureBox3.Visible = false;
            lbTempo.Visible = false;
            lbBandaNome.Visible = false;
            lbBandaNome.Visible = false;
        }

        #endregion

        private void AjustaIndice(string tocar)
        {
            for (int i = 0; i < this.listView.Items.Count; i++)
                if (listView.Items[i].SubItems[1].Text == tocar)
                {
                    this.IndiceNaLista = i;
                    break;
                }
        }

        private void ligadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbs.tbConfig Config = new tbs.tbConfig();
            Config.Progr = true;
            Config.Salva();
            tsProg.Image = ligadoToolStripMenuItem.Image;
            this.ProgLigada = Config.Progr;
        }

        private void desligadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbs.tbConfig Config = new tbs.tbConfig();
            Config.Progr = false;
            Config.Salva();
            tsProg.Image = desligadoToolStripMenuItem.Image;
            this.ProgLigada = Config.Progr;
        }

        private void tsProg_ButtonClick(object sender, EventArgs e)
        {

        }

        private void listView_Click(object sender, EventArgs e)
        {
            string Tocar = listView.SelectedItems[0].SubItems[1].Text;
            if (panel1.Visible) // Verifica se o player principal está visível
            {
                this.AjustaIndice(Tocar);
                this.Toca(Tocar);
            }
            else
            {
                // Abrir a tela Full novamente e tocar a nova música
                if (cFull == null)
                {
                    AbrirFull();
                } else
                {
                    cFull.Para();
                    cFull.Show();
                }
                if (Screen.AllScreens.Length > 1)
                {
                    Screen segundaTela = Screen.AllScreens[1];
                    cFull.StartPosition = FormStartPosition.Manual;
                    cFull.Location = segundaTela.Bounds.Location;
                    cFull.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    cFull.WindowState = FormWindowState.Maximized;
                }                
                cFull.Toca(Tocar, myPlayer.Audio.Volume); 
                panel1.Visible = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Fechando = true;
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            using (Listas cListas = new Listas())
            {
                if (cListas.ShowDialog() == DialogResult.OK)
                {
                    this.nmLista = cListas.nmLista;
                    int ID = 0;
                    using (SQLiteConnection conn = DalHelper.DbConnection()) 
                    {
                        string sql = "SELECT IdLista FROM Listas WHERE Nome = @Nome";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@Nome", this.nmLista);
                            object result = cmd.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                ID = Convert.ToInt32(result);                                
                            }
                            else
                            {
                                // Insere nova lista e obtém o ID
                                sql = "INSERT INTO Listas (Nome) VALUES (@Nome); SELECT last_insert_rowid();";
                                cmd.CommandText = sql;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@Nome", this.nmLista);

                                result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    ID = Convert.ToInt32(result);
                                }
                                else
                                {
                                    throw new Exception("Falha ao inserir nova lista.");
                                }
                            }
                            this.ListaAtu = ID;
                        }
                    }
                    setaLista(ID);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // panel1.c
            int x = 0;
        }

        #region Full
        private void AbrirFull()
        {
            if (cFull == null)
            {
                cFull = new Full(this);
                cFull.SetaTrack(this.trackBar1);
            }
            if (Screen.AllScreens.Length > 1)
            {

                Screen segundaTela = Screen.AllScreens[1];
                cFull.StartPosition = FormStartPosition.Manual;
                cFull.Location = segundaTela.Bounds.Location;
                cFull.WindowState = FormWindowState.Maximized; // Tela cheia
            }
            else
            {
                cFull.WindowState = FormWindowState.Maximized;
            }
            cFull.Show();
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            if (cFull == null)
            {
                myPlayer.Sliders.Position.TrackBar = null;
                AbrirFull();
            }
            else
            {
                cFull.Para();
            }
            cFull.Toca(this.TocandoAgora, myPlayer.Audio.Volume);
            panel1.Visible = false;
            myPlayer.Paused = true;
        }

        #endregion


    }
}
