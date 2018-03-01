using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;


namespace DDBot2
{
    public class MyCommands
    {
        //Stuff to keep thread safety
        private static HashSet<int> chatlogging = new HashSet<int>();
        private readonly SemaphoreSlim readlock = new SemaphoreSlim(1, 2);
        
        private List<DataSet> myData = new List<DataSet>();

        [Command("currentgame"), Description("Ex '.currentgame' returns the ID and start time/date of the latest game")]
        public async Task CurrentGame(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            String result = BotLibrary.returnCurrentGameStatus();
            
            if(result != null)
            {
                await ctx.RespondAsync($"Hi, {ctx.User.Mention}! " + result);
            }
            else
            {
                await ctx.RespondAsync($"Hi, {ctx.User.Mention}! Game Status is currently: " + "Unavailable");
            }
            /*var interactivity = ctx.Client.GetInteractivityModule();
            var msg = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id && xm.Content.ToLower() == "how are you?", TimeSpan.FromMinutes(1));
            if (msg != null)
                await ctx.RespondAsync($"I'm fine, thank you!");*/
        }
        [Command("gamestatus"), Description("Ex '.gamestatus *gameid*' requires the ID of the game you want to check and returns the start time/date and info from the last checkin with that game.")]
        public async Task GameStatus(CommandContext ctx, int gameID)
        {
            await ctx.TriggerTypingAsync();

            String result = BotLibrary.returnCurrentGameStatus(gameID);

            if(result != null)
            {
                await ctx.RespondAsync($"Hi, {ctx.User.Mention}! " + result);
            }
            else
            {
                await ctx.RespondAsync($"Hi, {ctx.User.Mention}! Game Status is currently: " + "Unavailable");
            }
        }
        [Command("addgamechat"), Description("Ex '.addgamechat *gameid*' requires the ID of the game you want to monitor chat on, then starts an indefinite loop where it will return all chat in that game.")]
        public async Task addgamechat(CommandContext ctx, int gameID)
        {
            HashSet<String> chatLog = new HashSet<String>();
            Random rand = new Random();
            Boolean boo = false;
            int ID = 0 ;//ID of thread
            
            while (!boo)
            {
                ID = rand.Next(99999);
                try
                {
                    await readlock.WaitAsync();
                    
                    boo = chatlogging.Add(ID);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    readlock.Release();
                }
            }//will continue only if true(unused ID)
            await ctx.RespondAsync($"Logging Game:" + gameID + ", Handle is " + ID + " ; Use this handle with command *.exitchat handle* to end logging");
            

            while (boo)
            {
                HashSet<String> temp = BotLibrary.returnGameChat(gameID);
                foreach (String x in temp)
                {
                    //await ctx.TriggerTypingAsync();
                    if (chatLog.Add(x))
                        await ctx.RespondAsync(x);
                    
                }
                //await ctx.RespondAsync($"Looping beboop!");
                await Task.Delay(10000);
                
                try
                {
                    await readlock.WaitAsync();
                    if (chatlogging.Overlaps(new int[1] { ID }))
                    {
                        //exists
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    readlock.Release();
                }
                   
            }
        }
        [Command("stopchat"), Description("Ex '.stopchat *handle*' Requires the handle number of the thread that is logging chat, stops that thread from continuing to output text into any channel")]
        public async Task Stopchat(CommandContext ctx, int ID)
        {
            
            try
            {
                await readlock.WaitAsync();
                chatlogging.RemoveWhere(a => a == ID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                readlock.Release();
            }
            
            await ctx.RespondAsync($"Stopping Chat (" + ID + ")!");
            
        }
        [Command("stopall"), Description("Ex '.stopall' Stops all the threads that are outputting text into all channels")]
        public async Task Stopall(CommandContext ctx)
        {
            HashSet<int> temp = new HashSet<int>();
            try
            {
                
                await readlock.WaitAsync();
                temp = chatlogging;
                chatlogging = new HashSet<int>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                readlock.Release();
            }
            String esc = "";
            foreach (int x in temp)
            {
                esc += x + ";";
            }
            await ctx.RespondAsync($"Stopping Chat Theads (" + esc + ")!");
        }
        [Command("chatthreads"), Description("Ex '.chatthreads' returns all the thread handle numbers that are active")]
        public async Task Chatthreads(CommandContext ctx)
        {
            HashSet<int> temp = new HashSet<int>();
            try
            {

                await readlock.WaitAsync();
                temp = chatlogging;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                readlock.Release();
            }
            String esc = "";
            foreach (int x in temp)
            {
                esc += x + ";";
            }
            await ctx.RespondAsync($"Thread Handles (" + esc + ")!");
        }
        [Command("joinedGame"), Description("Ex '.joinedGame 5' returns the number of players that joined game 5")]
        public async Task JoinedGame(CommandContext ctx)
        {
            int ans = 0;

            //Find all the players that have joined

            await ctx.RespondAsync($"Total Players that have Joined Game " + ctx + " is " + ans);
        }
        [Command("listPlayers"), Description("Ex '.listPlayers 5' returns a list of player names that are in game 5")]
        public async Task ListPlayers(CommandContext ctx)
        {
            String ans = "";

            //Find all the players that currently ingame

            await ctx.RespondAsync($"Player in Game " + ctx + " are " + ans);
        }
        [Command("kickPlayer"), Description("Ex '.kickPlayer Jed 5' kicks a player named Jed from game 5")]
        public async Task ListPlayers(CommandContext ctx, CommandContext ctx2)
        {

            //Find all the players that currently ingame

            await ctx.RespondAsync($"Player " + ctx + " kicked from game " + ctx2);
        }
        [Command("message"), Description("Ex '.message \"Jed is a fag\" 5' messages game 5")]
        public async Task Message(CommandContext ctx, CommandContext ctx2)
        {

            //Find all the players that currently ingame

            await ctx.RespondAsync($"Message Sent");
        }
        

    }
}
