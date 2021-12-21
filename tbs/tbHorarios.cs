﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeviousPlayer2.tbs
{
    public class tbHorarios
    {
        public int ID { get; set; }
        public DateTime HorIn { get; set; }
        public int Lista { get; set; }
        public int Periodicidade { get; set; }

        public void Adiciona()
        {
            try
            {
                using (var cmd = DalHelper.DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Horario (HorIn, Lista, Periodicidade) values (@HorIn, @Lista, @Periodicidade)";
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

    }
}