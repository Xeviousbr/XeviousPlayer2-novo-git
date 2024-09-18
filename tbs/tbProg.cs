using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;

namespace XeviousPlayer2.tbs
{
    public class tbProg
    {
        public Int16 ID { get; set; }
        public DateTime HorIn { get; set; }
        public int Lista { get; set; }
        public int Periodicidade { get; set; }

        public void Adiciona()
        {
            try
            {
                using (var cmd = DalHelper.DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Prog (HorIn, Lista, Periodicidade) values (@HorIn, @Lista, @Periodicidade)";
                    cmd.Parameters.AddWithValue("@HorIn", HorIn);
                    cmd.Parameters.AddWithValue("@Lista", Lista);
                    cmd.Parameters.AddWithValue("@Periodicidade", Periodicidade);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void Zera()
        {
            try
            {
                using (var cmd = DalHelper.DbConnection().CreateCommand())
                {
                    cmd.CommandText = "Delete From Prog";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void getProg()
        {
            DayOfWeek DiaSem = DateTime.Now.DayOfWeek;
            string SelDias = "1";
            switch (DiaSem)
            {
                case DayOfWeek.Sunday:      // Domingo
                    SelDias = "1,4";
                    break;
                case DayOfWeek.Saturday:    // Sabado
                    SelDias = "1,3";
                    break;
                default:
                    SelDias = "1,2";        // Dias de semana
                    break;
            }
            using (var cmd = new SQLiteCommand(DalHelper.DbConnection()))
            {
                string Hora = DateTime.Now.ToLocalTime().ToString().Substring(11, 8);
                string SQL = "Select ID, HorIn, Lista From Prog Where Periodicidade in (" + SelDias + ") and HorIn < '2001-01-01 "+Hora+"' order by HorIn desc limit 1 ";
                if (Consulta(SQL)==false)
                {
                    SQL = "Select ID, HorIn, Lista From Prog Where Periodicidade in (" + SelDias + ") order by HorIn desc limit 1 ";
                    Consulta(SQL);
                }
            }
        }
        private bool Consulta(string SQL)
        {
            bool ret = false;
            using (var connection = DalHelper.DbConnection())
            using (var command = new SQLiteCommand(SQL, connection))
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    this.ID = reader.GetInt16(0);
                    this.HorIn = reader.GetDateTime(1);
                    this.Lista = reader.GetInt16(2);
                    ret = true;
                }
            }
            return ret;
        }

        public List<string> listas()
        {
            string SQL = "Select Nome From Listas Order By Nome";
            List<string> ret = new List<string>();
            using (var connection = DalHelper.DbConnection())
            using (var command = new SQLiteCommand(SQL, connection))
            {
                try
                {
                    // Executa a consulta para obter os nomes
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Nome = reader.GetString(0);
                            ret.Add(Nome);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Em caso de erro, chama o método para criar a tabela
                    CriaTabela(connection);

                    // Tenta executar novamente a consulta após criar a tabela
                    using (var retryCommand = new SQLiteCommand(SQL, connection))
                    using (DbDataReader reader = retryCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Nome = reader.GetString(0);
                            ret.Add(Nome);
                        }
                    }
                }
            }
            return ret;
        }

        private void CriaTabela(SQLiteConnection connection)
        {
            string createTableSQL = @"CREATE TABLE IF NOT EXISTS Listas (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Nome TEXT NOT NULL)";
            using (var createCommand = new SQLiteCommand(createTableSQL, connection))
            {
                createCommand.ExecuteNonQuery();
            }
        }

        public string getnmLista()
        {
            string SQL = $"SELECT Nome, HorIn, Lista FROM AlgumaTabela WHERE ID = {this.ID}";
            using (var connection = DalHelper.DbConnection())
            using (var command = new SQLiteCommand(SQL, connection))
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    this.HorIn = reader.GetDateTime(1);
                    this.Lista = reader.GetInt16(2);
                    return reader.GetString(0); // Supondo que o nome é o que você quer retornar.
                }
            }
            return ""; // Retorne um valor padrão ou nulo se não encontrar nada.
        }
    }

}
