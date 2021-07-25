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

        public tbConfig() {
            SQLiteCommand command = new SQLiteCommand("Select PathBase, Skin From Config", DalHelper.sqliteConnection);
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    this.PathBase = reader.GetString(0);
                    this.Skin = reader.GetInt32(1);
                }
            }
        }
    }
}
