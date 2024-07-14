using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace XeviousPlayer2
{
    public static class DalHelper
    {
        private static string LocalBDlc = "";
        public static string LocalBD { get => getLocalBD(); }

        private static string getLocalBD()
        {
            if (LocalBDlc.Length == 0)
            {
                LocalBDlc = Application.StartupPath + "\\XeviousPlayer.sqlite";
            }
            return LocalBDlc;
        }

        public static SQLiteConnection DbConnection()
        {
            var connection = new SQLiteConnection("Data Source=" + LocalBD + "; Version=3;");
            connection.Open();
            return connection;
        }

        public static void CriarBancoSQLite()
        {
            try
            {
                SQLiteConnection.CreateFile(LocalBD);
            }
            catch (Exception ex)
            {
                Gen.Loga(ex.Message);
                throw;
            }
        }

        public static DataTable GetRecords(string SQL)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var connection = DbConnection())
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = SQL;
                    da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                Gen.Loga(ex.Message);
                throw;
            }
        }

        public static void ExecSql(string SQL)
        {
            try
            {
                using (var connection = DbConnection())
                using (var cmd = new SQLiteCommand(SQL, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Gen.Loga(ex.Message);
                throw;
            }
        }

        public static SQLiteDataReader TrazDados(string SQL)
        {
            try
            {
                using (var connection = DbConnection())
                using (var cmd = new SQLiteCommand(SQL, connection))
                {
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                Gen.Loga(ex.Message);
                return null;
            }
        }

        public static string Consulta(string SQL)
        {
            try
            {
                using (var connection = DbConnection())
                using (var cmd = new SQLiteCommand(SQL, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader[0].ToString();
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Gen.Loga(ex.Message);
                return null;
            }
        }
    }
}
