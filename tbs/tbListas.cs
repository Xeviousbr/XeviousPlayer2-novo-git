using System;

namespace XeviousPlayer2.tbs
{

    // CLASSE NÃO USADA

    class tbListas
    {
        public int ID { get; set; }
        public string Nome { get; set; }

        public int Adiciona()
        {
            try
            {
                using (var cmd = DalHelper.DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Listas(Nome) values (@Nome) ";
                    cmd.Parameters.AddWithValue("@Nome", Nome);
                    cmd.ExecuteNonQuery();
                    string SQL = "Select Max(IdLista) as ID From Listas";
                    string ret = DalHelper.Consulta(SQL);
                    int ID = int.Parse(ret);
                    return ID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        /* public int IdPeloNome(string Nome)
        {
            using (var cmd = DalHelper.DbConnection().CreateCommand())
            {
                string sql = "Select IdLista From Listas Where Nome = '" + Nome + "'";
                string ret = DalHelper.Consulta(SQL);
                int ID = int.Parse(ret);
                return ID;
            }

            return 0;
        } */
    }
}
