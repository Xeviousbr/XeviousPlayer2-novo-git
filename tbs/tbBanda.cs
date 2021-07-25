using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace XeviousPlayer2.tbs
{
    public class tbBanda
    {
        public int ID { get; set; }
        public string Nome { get; set; }

        public int Adiciona()
        {
            try
            {
                using (var cmd = DalHelper.DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Bandas(NomeBanda) values (@Nome)";
                    cmd.Parameters.AddWithValue("@Nome", Nome);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string SQL = "Select Max(IDBanda) as ID From Bandas";
            string ret = DalHelper.Consulta(SQL);
            this.ID = int.Parse(ret);
            return this.ID;
        }
    }
}
