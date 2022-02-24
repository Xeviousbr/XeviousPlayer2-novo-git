using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace XeviousPlayer2.tbs
{
    public class tbConfig
    {
        public string PathBase { get; set; }
        public int Skin { get; set; }

        public bool Progr { get; set; }

        public tbConfig() {
            SQLiteCommand command = new SQLiteCommand("Select PathBase, Skin, Progr From Config", DalHelper.sqliteConnection);
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    this.PathBase = reader.GetString(0);
                    this.Skin = reader.GetInt32(1);
                    this.Progr = (reader.GetInt32(2) == 1);
                }
            }
        }

        internal void Salva()
        {
            string sProgr = "0";
            if (this.Progr == true) sProgr = "1";
            DalHelper.ExecSql("Update Config set Progr = " + sProgr);
        }
    }
}
