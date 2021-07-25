using System;
using System.Windows.Forms;
using PVS.MediaPlayer;

// https://www.codeproject.com/Articles/109714/PVS-MediaPlayer-Audio-and-Video-Player-Library

/*
[ x ] Colocar a barra de cima
      [ x ] Falta colocar os icones
[ x ] Colocar os dados da musica
[   ] Colocar a foto
[   ] Colocar a grid
[   ] Ajustar o estilo da Grid
[ x ] Colocar os botões em baixo
[   ] Utilizar os controles como no exemplo
[   ] Preparar o espaço para a visualização
[   ] Colocar a visualização
[   ] Terminar a importação
[   ] Colocar o recurso de tela inteira
[   ] Colocar o recurso de visualização acoplada
 */


namespace XeviousPlayer2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if Media Foundation is installed - you can use this anywhere in your application
            if (!Player.MFPresent)
            {
                // Media Foundation is not installed - show a message and exit the application
                MessageBox.Show ("Microsoft Media Foundation\r\n\r\n" + Player.MFPresent_ResultString + ".",
                    "PVS.MediaPlayer How To ...", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                // Media Foundation is installed - run the application
                Application.Run(new Form1());
            }
        }
    }
}

/*
 List<ApiManagerDoor> doors = new List<ApiManagerDoor>();            

            SQLiteCommand command = new SQLiteCommand("SELECT d.controller_id,z.name AS zone_name,d.id AS door_id,d.name AS door_name FROM doors AS d LEFT JOIN zones AS z ON d.zone_id = z.id LEFT JOIN accounts AS a ON z.account_id = a.id WHERE a.token = '" + token + "' AND a.status = 1", DBSqlite.connection);

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ApiManagerDoor door = new ApiManagerDoor { status = 0 };                                                
                        
                        door.serverId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        door.zoneName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        door.doorId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                        door.doorName = reader.IsDBNull(3) ? "Porta " + door.doorId.ToString() : reader.GetString(3);                        

                        doors.Add(door);
                    }
                }
            }

            return doors;
 */
