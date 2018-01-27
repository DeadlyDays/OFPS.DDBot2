using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DDBot2
{
    static class BotLibrary
    {

        //
        //--Used to return formatted player chat to be output into discord
        //
        public static HashSet<String> returnGameChat(int gameID)
            //default connection
        {
            MySqlCommand loader = new MySqlCommand(BotSQL.sqlChatLog(gameID), (MySqlConnection)BotSettings.DefConnection);
            HashSet<String> playerChatLog = new HashSet<String>();
            try
            {
                ((MySqlConnection)BotSettings.DefConnection).Open();
                MySqlDataReader o = loader.ExecuteReader();
                if (o == null)
                    ;
                else
                {
                    while (o.Read())
                    {
                        playerChatLog.Add(
                            "[" + o[0] + "]" +
                            "**" + o[1] + "**:" +
                            "" + o[2] + "");
                    }
                }
                ((MySqlConnection)BotSettings.DefConnection).Close();
            }
            catch (MySqlException err)
            {
                Console.WriteLine(err.Message);
            }

            return playerChatLog;
        }
        public static HashSet<String> returnGameChat(int gameID, MySqlConnection conn)
        //specific connection
        {
            MySqlCommand loader = new MySqlCommand(BotSQL.sqlChatLog(gameID), conn);
            HashSet<String> playerChatLog = new HashSet<String>();
            try
            {
                conn.Open();
                MySqlDataReader o = loader.ExecuteReader();
                if (o == null)
                    ;
                else
                {
                    while (o.Read())
                    {
                        playerChatLog.Add(
                            "[" + o[0] + "]" +
                            "**" + o[1] + "**:" +
                            "" + o[2] + "");
                    }
                }
                conn.Close();
            }
            catch (MySqlException err)
            {
                Console.WriteLine(err.Message);
            }

            return playerChatLog;
        }


        //
        //--Used to return stats of a game
        //
        public static String returnCurrentGameStatus()
        {
            String gameID = "";
            String status = "";
            String startTime = "";
            String startDate = "";
            String gameTime = "";

            MySqlCommand loader = new MySqlCommand(BotSQL.sqlLatestGame(), (MySqlConnection)BotSettings.DefConnection);
            try
            {
                ((MySqlConnection)BotSettings.DefConnection).Open();
                object o = loader.ExecuteScalar();
                gameID = Convert.ToString(o);
                loader = new MySqlCommand(BotSQL.sqlStartTime(), (MySqlConnection)BotSettings.DefConnection);
                o = loader.ExecuteScalar();
                startTime = Convert.ToString(o);
                loader = new MySqlCommand(BotSQL.sqlGameTime(Convert.ToInt32(gameID)), (MySqlConnection)BotSettings.DefConnection);
                o = loader.ExecuteScalar();
                o = TimeSpan.FromSeconds(Convert.ToDouble(o)).Duration();
                gameTime = Convert.ToString(o);
                loader = new MySqlCommand(BotSQL.sqlStartDate(), (MySqlConnection)BotSettings.DefConnection);
                o = loader.ExecuteScalar();
                o = Convert.ToDateTime(o).ToShortDateString();
                startDate = Convert.ToString(o);
                ((MySqlConnection)BotSettings.DefConnection).Close();

            }
            catch (MySqlException err)
            {
                Console.WriteLine(err.Message);
            }
            status = "Latest Game: " + gameID + " started at " + startTime + " on " + startDate + " at last check has been running " + gameTime + ".";
            return status;
        }
        public static String returnCurrentGameStatus(int gameNum)
        {
            String gameID = "";
            String status = "";
            String startTime = "";
            String startDate = "";
            String gameTime = "";


            MySqlCommand loader = new MySqlCommand(BotSQL.sqlStartTime(gameNum), (MySqlConnection)BotSettings.DefConnection);
            try
            {
                ((MySqlConnection)BotSettings.DefConnection).Open();
                object o = loader.ExecuteScalar();
                gameID = Convert.ToString(gameNum);
                startTime = Convert.ToString(o);
                loader = new MySqlCommand(BotSQL.sqlGameTime(Convert.ToInt32(gameID)), (MySqlConnection)BotSettings.DefConnection);
                o = loader.ExecuteScalar();
                o = TimeSpan.FromSeconds(Convert.ToDouble(o)).Duration();
                gameTime = Convert.ToString(o);
                loader = new MySqlCommand(BotSQL.sqlStartDate(gameNum), (MySqlConnection)BotSettings.DefConnection);
                o = loader.ExecuteScalar();
                o = Convert.ToDateTime(o).ToShortDateString();
                startDate = Convert.ToString(o);
                ((MySqlConnection)BotSettings.DefConnection).Close();

            }
            catch (MySqlException err)
            {
                Console.WriteLine(err.Message);
            }
            status = "Latest Game: " + gameID + " started at " + startTime + " on " + startDate + " at last check has been running " + gameTime + ".";
            return status;
        }

    }
}
