using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using MySql.Data;
using System.IO;


namespace DDBot2
{
    class Program
    {
        static DiscordClient discord;//discord instance
        static CommandsNextModule commands;
        static InteractivityModule interactivity;

        static void Main(string[] args)
        {
            BotSettings.init();//grab all the settings

            DirectoryInfo dir =
                    new DirectoryInfo
                    (
                        Environment.ExpandEnvironmentVariables("%AppData%\\DDBot2\\")
                    );

            if (BotSettings.Token == null || BotSettings.Token == "")
            {
                Console.WriteLine("Failed to load Settings at '" + dir.ToString() + "'");
                Console.ReadLine();
                return;
            }
            if(BotSettings.DBServerList != null)
                if (BotSettings.DBServerList.Count > 0)
                    BotSettings.DefConnection = BotSettings.DBServerList[0];//default to first listed DB
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
            
        }
        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = BotSettings.Token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug

            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                    await e.Message.RespondAsync("Dropping Database.....(type 'pretty pony' to cancel within specified timeframe)");
            };
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "."
            });
            interactivity = discord.UseInteractivity(new InteractivityConfiguration
            {

            });
            commands.RegisterCommands<MyCommands>();

            await discord.ConnectAsync();
            await Task.Delay(-1);

        }

        

    }
}
