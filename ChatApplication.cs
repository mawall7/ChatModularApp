using ChatBasicApp;
using SimpleUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatUIManualTest;
using IUI = ChatUIManualTest.IUI;
using SimpleUI;
using IModularChatPeer = ModularChatPeer.IModularChatPeer;

namespace ChatModularApp
{
    using IinputProcessor = SimpleUI.IInputProcessor;
    public class ChatApplication
    {
        public StringBuilder messageBuffer = new StringBuilder();  //TODO internalize ?
        //public StringBuilder receiveBuffer = new StringBuilder();  //not used

        private readonly static CancellationTokenSource cTS = new();
        private readonly static CancellationToken ct = cTS.Token;

        private readonly IinputProcessor _inputProcessor; //change IInputProcessor to use with SimpleUI.InputProcessor

        private readonly ConsoleUIRenderer _consoleRenderer;
        public Action<string> MessageAction {get; set;} //TODO not needed less flexible compared to IUI

        private readonly IModularChatPeer _chatPeer;

        private IUI _ui;

        public ChatApplication(IModularChatPeer chatPeer, IinputProcessor inputProcessor, IUI ui, ConsoleUIRenderer uIRenderer)
        {
            _chatPeer = chatPeer;
            MessageAction = (m) => _ui.Output(m, NetworkServer.MessageType.General);
            _ui = ui;
            _inputProcessor = inputProcessor;
            _inputProcessor.WriteBuffer = messageBuffer;
            ConsoleUIRenderer.WriteBuffer = messageBuffer;
            _consoleRenderer = uIRenderer;
        }

        public async Task Run(string[] args)
        {

            _ui.Output("ChatBasicApplication v.1 is running...", NetworkServer.MessageType.Status);

            if (args[0].ToLower() == "server")
            {
                await _chatPeer.ConnectAsServerAsync();
            }

            else if (args[0].ToLower() == "client")
            {
                await _chatPeer.ConnectAsClientAsync(ct);
            }

            /////////// - Recieve - Listen for message -
            var listenTask = Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    string message;

                    try
                    {
                        message = await _chatPeer.ReceiveAsync(ct);  //await receving a message from remote

                        if (message == null)
                        {
                            cTS.Cancel();
                        }
                        else
                        {
                            if (_ui is ChatUIManualTest.ConsoleUI)
                            {
                                ConsoleRenderer.UpdateConsole(message, messageBuffer); //TODO make property ingen anledning att skicka in den varje gång initialisera i konstruktorn.
                            }
                        }
                    }
                    catch (SocketException ex)
                    {
                        _ui.Output("Receiving from remote failed", NetworkServer.MessageType.Error);
                    }

                }
                    _ui.Output("Server task ended...", NetworkServer.MessageType.Status);
            });

            ////////// - Write and Send - TODO: now both ConsoleRenderer and ProcessInput is Rendering the Console, use one.

            while (!cTS.IsCancellationRequested)
            {
                //if (Console.KeyAvailable)
                //{
                     KeyInfo key = _ui.ReadKey();
                     
                     if(key.Key == AppKey.None)
                    {
                        continue;
                    }


                    //ConsoleInputProcessor visually updates the console window and executes commands and then returns a result including a message , that can be sent by network peer etc. thus ui and network peer message sending is kept separate from input processing.   
                    SimpleUI.ConsoleInputProcessor.CTS = cTS;

                    SimpleUI.InputProcessingResult result = _inputProcessor.ProcessInput(key);  //Proccess commands like quit internally and quits using global cancellationtoken

                    if (result.UICommand == UICommand.Exit)
                    {
                        cTS.Cancel();

                    }
                    if (result.isRenderCommand && _ui is ChatUIManualTest.ConsoleUI) 
                    {
                        _consoleRenderer.Render(result); //Eller override ToString() i InputProcessingResult
                    }

                    if (result.UIAction == UIAction.Send || result.UIAction == UIAction.Write) //false = antingen writing eller command. obs processing sker ovan tex. spara i buffer, och kolla andra kommandon och rendering kommandon som t.ex. space. improve way of keeping ChatPeer separate
                    {
                        try
                        {
                            await _chatPeer.SendMessageAsync(result.MessageToSend); 

                        }
                        catch (SocketException e)
                        {
                            _ui.Output("Message could not be sent.", NetworkServer.MessageType.Error);
                             
                        }
                    
                    }

                //}

            }
                cTS.Dispose();
                Console.WriteLine(Environment.NewLine + "Program Ended.");
                Console.ReadKey();
        }
    
    }

        public static class ConsoleRenderer
        {
            public static readonly object consolelock = new();

            public static StringBuilder WriteBuffer;

            public static void UpdateConsole(string receivedmessage, StringBuilder writeBuffer) //TODO: använd istället ConsoleUIRenderer
            {

                lock (consolelock)  //Handles possible concurrency issues due to race condition , between threads.
                {
                    if (writeBuffer.Length > 0)
                    {
                        Console.Write(Environment.NewLine + receivedmessage + Environment.NewLine + writeBuffer.ToString());
                    }
                    else
                    {
                        Console.WriteLine(receivedmessage);
                    }
                }
            }
        }

    }


