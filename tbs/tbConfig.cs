using System;
using System.Collections.Generic;
using System.Data;
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

        public void Carrega ()
        {
            string SQL = "Select PathBase, Skin, Progr From Config";
            using (var cmd = new SQLiteCommand(DalHelper.DbConnection()))
            {
                cmd.CommandText = SQL;
                using (SQLiteDataReader reg = cmd.ExecuteReader())
                {
                    reg.Read();
                    this.PathBase = reg["PathBase"].ToString();
                    this.Skin = int.Parse(reg["Skin"].ToString());
                    this.Progr = (reg["Progr"].ToString() == "1");
                }
            }
        }

        public void Salva()
        {
            string sProgr = "0";
            if (this.Progr == true) sProgr = "1";
            DalHelper.ExecSql("Update Config set Progr = " + sProgr);
        }
    }
}
