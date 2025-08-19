using SimpleUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularApp
{
    public class ConsoleUIRenderer
    {
        public static StringBuilder WriteBuffer { get; set; }
        public void Render(InputProcessingResult processingResult)
        {
            var CursorPos = Console.GetCursorPosition();

            if (processingResult.UIAction == UIAction.Write)
            {
                Console.Write(processingResult.MessageToSend);
            }
            if(processingResult.UIAction == UIAction.Send)
            {
                ClearLine();
                Console.WriteLine("Sending message:" + processingResult.MessageToSend);
            }
            if(processingResult.UICommand == UICommand.Space)
            {
                CursorPos = Console.GetCursorPosition();

                //WriteBuffer = WriteBuffer.Insert(CursorPos.Left, " ");
                Console.SetCursorPosition(CursorPos.Left + 1, CursorPos.Top);
                CursorPos = Console.GetCursorPosition();
                ClearLine();
                Console.Write(WriteBuffer.ToString());
                Console.SetCursorPosition(CursorPos.Left, CursorPos.Top);
                
            }
            if(processingResult.UICommand == UICommand.BackSpace)
            {
                if (Console.GetCursorPosition().Left >= 1) //0 is min left position
                {
                    WriteBuffer = WriteBuffer.Remove(CursorPos.Left - 1, 1);
                    ClearLine();
                    Console.Write(WriteBuffer.ToString());
                    Console.SetCursorPosition(CursorPos.Left - 1, CursorPos.Top);
                }
            }
            if(processingResult.UICommand == UICommand.Left)
            {
                if (Console.GetCursorPosition().Left >= 1)
                {

                    Console.SetCursorPosition(CursorPos.Left - 1, CursorPos.Top);
                }
            }
            if (processingResult.UICommand == UICommand.Right)
            {
                if (Console.WindowWidth > CursorPos.Left)
                    {
                        Console.SetCursorPosition(CursorPos.Left + 1, CursorPos.Top); 
                    }
            }


        }

        private static void ClearLine()
        {
            int currentLine = Console.CursorTop;
            Console.SetCursorPosition(0, currentLine);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine); // optional: reset cursor back to start

        }
    }
}
