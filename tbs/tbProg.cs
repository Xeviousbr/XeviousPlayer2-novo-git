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
            SQLiteCommand command = new SQLiteCommand(SQL, DalHelper.sqliteConnection);
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
    }
}
