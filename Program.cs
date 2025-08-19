using ChatUIManualTest;
using SimpleUI;
using System;
using IinputProcessor = SimpleUI.IInputProcessor;
using ChatBasicApp;
using Iui = ChatUIManualTest.IUI;
using System.Net;
using ModularChatPeer;

namespace ChatModularApp
{
    class Program
    {
        static void Main(string[] args)
        {

            IinputProcessor inputProcessor = new SimpleUI.ConsoleInputProcessor();

            ConsoleUIRenderer consoleUIRenderer = new ConsoleUIRenderer();

            Iui ui = new ChatUIManualTest.ConsoleUI();

            ui.Output("ChatApplication v.3", NetworkServer.MessageType.General);
            
            IModularChatPeer chatPeer = new ModularChatPeer.ModularChatPeer.ModularChatPeer(new(IPAddress.Parse("127.0.0.1"),
                8081), ui,
                new ChatCommunicator());

            ChatApplication application = new(chatPeer, inputProcessor, ui, consoleUIRenderer);


            try
            {
                application.Run(args).GetAwaiter().GetResult();
            }

            catch (Exception e)
            {

                ui.Output("An error occured:" + e.Message, NetworkServer.MessageType.Error);
            }





        }
    }
}
