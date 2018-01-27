using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDBot2
{
    static class BotSQL
    {

        public static String sqlChatLog(int gameID)
        {
            return 
                "SELECT PCL1.Time, PN1.Name, PCL1.Message " +
                "FROM PlayerChatLogTB PCL1 " +
                "LEFT JOIN PlayerGameTB PG1 " +
                "ON PCL1.PlayerGameTB_UID=PG1.UID " +
                "LEFT JOIN PlayerNameTB PN1 " +
                "ON PG1.PlayerTB_SteamID=PN1.PlayerTB_SteamID " +
                "" +
                "" +
                "" +
                "" +
                "" +
                "" +
                "WHERE ((TIME_TO_SEC(TIMEDIFF(CURRENT_TIME, PCL1.Time)) <= " + 30 + ") " +
                "" +
                "AND (PG1.GameTB_UID=" + gameID + ") ) " +
                "GROUP BY PCL1.UID " +
                "ORDER BY PCL1.UID ASC;";
        }
        public static String sqlLatestGame()
        {
            return "SELECT UID FROM GameTB ORDER BY UID DESC LIMIT 1";//Latest Game
        }
        public static String sqlStartTime()
        {
            return "SELECT StartTime FROM GameTB ORDER BY UID DESC LIMIT 1";//Start Time
        }
        public static String sqlStartTime(int gameID)
        {
            return "SELECT StartTime FROM GameTB WHERE UID=" + gameID + " ORDER BY UID DESC LIMIT 1";//Start Time
        }
        public static String sqlStartDate()
        {
            return "SELECT StartDate FROM GameTB ORDER BY UID DESC LIMIT 1";//Start Date
        }
        public static String sqlStartDate(int gameID)
        {
            return "SELECT StartTime FROM GameTB WHERE UID=" + gameID + " ORDER BY UID DESC LIMIT 1";//Start Time
        }
        public static String sqlGameTime(int gameID)
        {
            return "SELECT GameTime FROM GameInstanceTB WHERE (GameTB_UID=" + gameID + ") ORDER BY UID DESC";
        }

    }
}
