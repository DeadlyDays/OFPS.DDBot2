using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace DDBot2
{
    static class BotSettings
    {
        
        //ServerAddress, Username, SchemaName, Port, Password, SSLMode(None)
        private static List<MySqlConnection> dbServerList;
        public static List<MySqlConnection> DBServerList
        {
            get
            {
                if (dbServerList == null)
                    return new List<MySqlConnection> { };
                else return dbServerList;
            }
            set
            {
                if (value == null)
                    dbServerList = null;
                else if (dbServerList == null)
                    dbServerList = new List<MySqlConnection> { value[0] };
                else
                    dbServerList = value;
            }
        }

        //reference, point at default MySqlConnection
        private static object defConnection;
        public static object DefConnection
        {
            get
            {
                if (defConnection != null)
                    return defConnection;
                else
                    return new MySqlConnection("");
            }
            set
            {
                defConnection = value;
            }
        }

        //Discord Token for Bot Connection
        private static String token;
        public static String Token
        {
            get;
            set;
        }

        //String of settings file
        private static String file;

        public static void init()
        {
            loadFile();
            loadToken();
            loadDBList();
        }

        //
        //-- Load File to String
        //
        private static void loadFile()
        {
            //grab base directory, set to object
            DirectoryInfo dir = 
                    new DirectoryInfo
                    (
                       Environment.ExpandEnvironmentVariables("%AppData%\\DDBot2\\")
                    );
            //grab settings file if it exists, otherwise create empty file
            if (File.Exists(Path.Combine(dir.ToString(), "settings.ini")))
                //if file exists
            {
                file = File.ReadAllText(Path.Combine(new Uri(dir.ToString()).LocalPath, "settings.ini"));
            }
            else
            //file doesn't exist
            {
                
            }

        }

        //
        //-- Parse File for Settings
        //
        private static void loadDBList()
        {
            //regex to gather all db details
            //ServerAddress, Username, SchemaName, Port, Password, SSLMode(None)
            //[DBProdServer]
            //ServerAddress=this.com
            //User=username
            //SchemaName=Schema
            //Port=3306
            //Password=password1
            //SSLMode=None
            String regex =
                "\n*([(?<Name>.+)].*\n*.*" +
                "ServerAddress=(?<ServerAddress>.+).*\n*.*" +
                "User=(?<User>.+).*\n*.*" +
                "SchemaName=(?<SchemaName>.+).*\n*.*" +
                "Port=(?<Port>.+).*\n*.*" +
                "Password=(?<Password>.+).*\n*.*" +
                "SSLMode=(?<SSLMode>.+).*\n*.*" +
                ").*\n*"
                ;
            if (file == null)
                return;
            //return a collection of results
            MatchCollection results = Regex.Matches(file, regex);

            if(results.Count == 0)
            //no results
            {
                return;
            }
            else
            //Continue to parse
            {
                String conn = "";
                foreach(Match result in results)
                    //iterate through results
                {
                    conn = "";
                    //server=xenophobiaxrt.com;user=ofpstestdb;database=ofpstestdb;port=3306;password=fX9K!YfZzUXc;SslMode=None
                    conn +=
                        "server=" + Regex.Replace(result.Groups[2].ToString(), "\r", "") + ";" +
                        "user=" + Regex.Replace(result.Groups[3].ToString(), "\r", "") + ";" +
                        "database=" + Regex.Replace(result.Groups[4].ToString(), "\r", "") + ";" +
                        "port=" + Regex.Replace(result.Groups[5].ToString(), "\r", "") + ";" +
                        "password=" + Regex.Replace(result.Groups[6].ToString(), "\r", "") + ";" +
                        "SslMode=" + Regex.Replace(result.Groups[7].ToString(), "\r", "") + ";"
                        ;
                    MySqlConnection connector = new MySqlConnection(conn);
                    if (dbServerList == null)
                        dbServerList = new List<MySqlConnection> { connector };
                    else
                        dbServerList.Add(connector);
                }
            }
        }
        private static void loadToken()
            //Load the Bot Token to connect to discord
        {
            String regex =
                "Token=(?<Token>.+)\n+.*" +
                ".*\n*"
                ;
            if (file == null)
                return;
            //return a collection of results
            Match result = Regex.Match(file, regex);

            if (result == null)
            //no results
            {
                return;
            }
            else
            //Continue to parse
            {
                Token = result.Groups[1].Value;
            }


        }




        //
        //-- Write Settings to File
        //




    }
}
