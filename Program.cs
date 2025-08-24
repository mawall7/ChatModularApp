using ChatUIManualTest;
using SimpleUI;
using System;
using IinputProcessor = SimpleUI.IInputProcessor;
using ChatBasicApp;
using Iui = ChatUIManualTest.IUI;
using System.Net;
using ModularChatPeer;
using MCP = ModularChatPeer.ModularChatPeer.ModularChatPeer; //avoid using same name for class and a namespace, but if it's risky to rename, do it like this.
using System.Text;
using SignalRLibrary;
//using Microsoft.AspNetCore.SignalR.Client;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
//using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatModularApp
{
    class Program
    {
        public static Predicate<string> aliasPredicate; 
        //new Func<string, bool>((name) => name.Length > 1 && name.Length < 25);
        static void Main(string[] args)
        {
            
            aliasPredicate = name => !string.IsNullOrWhiteSpace(name);
            
            args[0] = "srclient";
            
            //var writeBuffer = new StringBuilder();

            //IinputProcessor inputProcessor = new SimpleUI.ConsoleInputProcessor();

            //ConsoleUIRenderer consoleUIRenderer = new ConsoleUIRenderer(writeBuffer);

            //Iui ui = new ChatUIManualTest.ConsoleUI();

            //ui.Output("ChatApplication v.3", NetworkServer.MessageType.General);

            //IModularChatPeer chatPeer = new MCP(new(IPAddress.Parse("127.0.0.1"),
            //    8081), ui,
            //    new ChatCommunicator());

            //ChatApplication application = new(chatPeer, inputProcessor, ui, consoleUIRenderer);


            //try
            //{
            //    application.Run(args).GetAwaiter().GetResult();
            //}

            //catch (Exception e)
            //{

            //    ui.Output("An error occured:" + e.Message, NetworkServer.MessageType.Error);
            //}
            if (args[0] == "server")
            {
                // not used server singnal r sever hub started from SignalRServer solution 
            }
            
            else if (args[0] == "srclient")
            {

                var signalrConnection = new SignalRClient().GetHubConnection();

                signalrConnection.StartAsync().GetAwaiter().GetResult();
                string clientName = null;

                do
                {
                    Console.WriteLine("Enter an alias");
                    clientName = Console.ReadLine();
                } while ( !IsValidAlias(aliasPredicate,clientName));
                        //( IsValidAlias(
                //    (name) =>  !string.IsNullOrEmpty(clientName) 
                //    && clientName.Length > 1 
                //    && clientName.Length < 25
                //    //, clientName));
                
                Console.WriteLine($"Connected to SignalR hub as:{clientName} ");


                while (true)
                {
                    Console.Write("Enter message: ");
                    var msg = Console.ReadLine();
                    signalrConnection.InvokeAsync("SendMessage", "ConsoleClient", msg).GetAwaiter().GetResult(); //invokes the SendMessage Method of the server and that method in turn invokes tells which method in of this SignalRClient.cs shouldbe called in this case ReceiveMessage for all clients (see SignalRServer solution/proj class)
                }
            }
        }
        
        public static bool IsValidAlias(Predicate<string> predicate, string name)
        {
            predicate += (name) => name.Length > 1;
            predicate += (name) => name.Length < 25;

            bool result= false;

            foreach(Predicate<string> p in predicate.GetInvocationList())
            {
                result = p(name);
                if (!result) {
                    Console.WriteLine("Please enter a valid alias! (must be 2-24 characters)");
                    break; }
            }
            return result;
        }
    }
}
