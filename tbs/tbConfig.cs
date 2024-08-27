using System;
using System.Data.SQLite;

namespace XeviousPlayer2.tbs
{
    public class tbConfig
    {
        public string PathBase { get; set; }
        public int Skin { get; set; }

        public bool Progr { get; set; }

        public int UltLista { get; set; }

        public void Carrega()
        {
            try
            {
                string SQL = "SELECT PathBase, Skin, Progr, UltLista FROM Config";
                using (var connection = DalHelper.DbConnection())
                using (var cmd = new SQLiteCommand(SQL, connection))
                {
                    using (SQLiteDataReader reg = cmd.ExecuteReader())
                    {
                        if (reg.HasRows)
                        {
                            reg.Read();
                            this.PathBase = reg["PathBase"].ToString();
                            this.Skin = int.Parse(reg["Skin"].ToString());
                            this.Progr = reg["Progr"].ToString() == "1";
                            this.UltLista = int.Parse(reg["UltLista"].ToString());
                        }
                        else
                        {
                            Gen.Loga("Nenhuma configuração encontrada.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Gen.Loga(ex.Message);
                if (ex.Message.Contains("no such table"))
                {
                    CriarTabelaConfig();
                }
            }
            catch (Exception ex)
            {
                Gen.Loga("Erro genérico: " + ex.Message);
            }
        }

        private void CriarTabelaConfig()
        {
            try
            {
                using (var connection = DalHelper.DbConnection())
                {
                    string createTableSQL = @"
                        CREATE TABLE IF NOT EXISTS Config (
                            PathBase TEXT NOT NULL,
                            Skin INTEGER NOT NULL,
                            Progr BOOLEAN NOT NULL,
                            UltLista INTEGER NOT NULL
                        )";
                    using (var cmd = new SQLiteCommand(createTableSQL, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                Gen.Loga("Tabela 'Config' criada com sucesso.");
            }
            catch (Exception ex)
            {
                Gen.Loga("Erro ao criar a tabela 'Config': " + ex.Message);
            }
        }

        public void Salva()
        {
            string sProgr = "0";
            if (this.Progr == true) sProgr = "1";
            DalHelper.ExecSql("Update Config set Progr = " + sProgr);
        }

        public void SetaUltLista(int iUltLista)
        {
            DalHelper.ExecSql("Update Config set UltLista = " + iUltLista.ToString());
            this.UltLista = iUltLista;
        }

        public void SetaPath(string Path)
        {
            this.PathBase = Path;
            string PATH = "Update Config set PathBase = " + Gen.FA(Path);
            DalHelper.ExecSql(PATH);
        }

    }
}
