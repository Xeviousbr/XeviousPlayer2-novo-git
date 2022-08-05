using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeviousPlayer2.tbs;

namespace XeviousPlayer2
{
    public class tbMusicas
    {
        public int ID { get; set; }
        
        public string Lugar { get; set; }        
        public long Tamanho { get; set; }
        public int Tempo { get; set; }      // Quantidade de segundos
        public int MaxVol { get; set; }     // Não sei se tem como guardar os volumas acho qe sim
        public string TocadoEm { get; set; }
        public int Pular { get; set; }
        public int Pulado { get; set; }
        public int CutIni { get; set; }
        public int CutFim { get; set; }
        public int TemImagem { get; set; }
        public string Nome
        {
            get { return getNome(); }
            set { SetaNome(value); }
        }

        public int Banda { get; set; }
        public string NomeBanda { get; set; }

        public int Ano
        {
            get { return lcAno; }
            set { SetaAno(value); }
        }

        private int lcAno = 0;
        private int lcBanda = 0;
        private string lcNome = "";
        private string lcNmBanda = "";

        private int SetaAno(int value)
        {
            if (value<1900)
            {
                return 0;
            } else
            {
                // Incluir a musica numa lista de ano, ex. 2000-2009
                // Um grupo de ano, deve apontar para o próximo grupo de ano
                return value;
            }            
        }

        public int SetaBanda(string bandaTemp)
        {
            bool MusValida = true;
            if (bandaTemp.Length < 3) MusValida = false;
            if (bandaTemp == "Mp3") MusValida = false;
            if (MusValida == false)
            {
                int PosHifen = Nome.IndexOf('-');
                if (PosHifen > -1) 
                {
                    int PosUltHifen = Nome.LastIndexOf('-');
                    if (PosUltHifen!= PosHifen)
                        this.NomeBanda = Nome.Substring(PosUltHifen+2);
                    else
                        this.NomeBanda = Nome.Substring(PosHifen+2);
                    
                } 
            } else
            {
                this.NomeBanda = bandaTemp;
            }

            // Procurar pela banda
            string SQL = "Select IDBanda From Bandas Where NomeBanda = '" + this.NomeBanda + "'";
            string ret = DalHelper.Consulta(SQL);
            if (ret != null)
                this.ID = int.Parse(ret);
            else
            {
                if (this.NomeBanda == null)
                    this.ID = 0;
                else
                {
                    tbBanda tbB = new tbBanda();
                    tbB.Nome = this.NomeBanda;
                    this.ID = tbB.Adiciona();
                }                    
            }
            return this.ID; 
        }

        public void SetaGenero(string nome)
        {
            // Se não é vazio, deve procurar o estilo em Listas
            int x = 0;
            // Se não tem, cria a lista
            // Incluir a musica na lista
        }

        public string SetaNome(string value)
        {
            // Aqui deve ter críticas relacionadas ao nome
            // "001 - Thom Brennan - Pulse"
            lcNome = value;
            return value;
        }

        private string getNome()
        {
            return lcNome;
        }

        public void Adiciona()
        {
            try
            {
                using (var cmd = DalHelper.DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Musicas(Nome, Lugar, Banda, Tempo, Tamanho, TocadoEm) values (@Nome, @Lugar, @Banda, @Tempo, @Tamanho, datetime('now'))";
                    cmd.Parameters.AddWithValue("@Nome", Nome);
                    cmd.Parameters.AddWithValue("@Lugar", Lugar);
                    cmd.Parameters.AddWithValue("@Banda", Banda);
                    cmd.Parameters.AddWithValue("@Tempo", Tempo);
                    cmd.Parameters.AddWithValue("@Tamanho", Tamanho);
                    // cmd.Parameters.AddWithValue("@TocadoEm", AgoraSQL());
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* private string AgoraSQL()
        {
            DateTime dAgora = new DateTime();
            string Agora = string.Format("{0:yy-MM-dd}", dAgora);
            return Agora;
        } */

    }
}
