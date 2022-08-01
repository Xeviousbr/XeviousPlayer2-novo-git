using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

// http://www.macoratti.net/17/04/cshp_sqlite1.htm

namespace XeviousPlayer2
{
    public static class DalHelper
    {
        public static SQLiteConnection sqliteConnection;
        private static string LocalBDlc="";
        public static string LocalBD { get => getLocalBD(); }

        private static string getLocalBD()
        {
            if (LocalBDlc.Length==0)
            {
                LocalBDlc = Application.StartupPath + "\\XeviousPlayer.sqlite";
            }
            return LocalBDlc;
        }

        public static SQLiteConnection DbConnection()
        {            
            sqliteConnection = new SQLiteConnection("Data Source="+ LocalBD+"; Version=3;");
            sqliteConnection.Open();
            return sqliteConnection;
        }

        public static void CriarBancoSQLite()
        {
            try
            {
                SQLiteConnection.CreateFile(LocalBD);
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                throw;
            }

        }

        /* public static void CriarTabelaSQlite()
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Config (PathBase Text) ";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } */

        //public static DataTable GetClientes()
        //{
        //    SQLiteDataAdapter da = null;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        using (var cmd = DbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "SELECT * FROM Clientes";
        //            da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
        //            da.Fill(dt);
        //            return dt;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static DataTable getRecords(string SQL)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = SQL;
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static void Add(Cliente cliente)
        //{
        //    try
        //    {
        //        using (var cmd = DbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "INSERT INTO Clientes(id, Nome, email ) values (@id, @nome, @email)";
        //            cmd.Parameters.AddWithValue("@Id", cliente.Id);
        //            cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
        //            cmd.Parameters.AddWithValue("@Email", cliente.Email);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static void ExecSql(string SQL)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    cmd.CommandText = SQL;
                    cmd.ExecuteNonQuery();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SQLiteDataReader TrazDados(string SQL)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    cmd.CommandText = SQL;
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        return reader;
                    }
                    //using (var reader = cmd.ExecuteReader())
                    //{
                    //    reader.Read();
                    //    return reader;
                    //}
                };
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
        }

        public static string Consulta(string SQL)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    cmd.CommandText = SQL;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {                            
                            return reader[0].ToString();
                            // return reader["PathBase"].ToString();
                        }
                        return null;
                    }
                };
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
        }

    }
}

