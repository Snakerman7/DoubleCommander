using DoubleCommander.Common;
using DoubleCommander.Views;
using System;

namespace DoubleCommander
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 120;
            Console.WindowHeight = 30;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.BackgroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            Console.Clear();

            DoublePanelView dpv = new DoublePanelView(
                new Point(0, 0), 
                new Size(EventsSender.Graphics.ClientHeight, EventsSender.Graphics.ClientWidth));
            EventsSender.Subscribe(dpv);
            EventsSender.Start();
            EventsSender.Join();
        }
    }
}
