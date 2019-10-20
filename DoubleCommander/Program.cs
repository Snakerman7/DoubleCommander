using DoubleCommander.Common;
using DoubleCommander.Core;
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
            _ = new DoublePanelView(
                new Point(0, 0), 
                new Size(EventsSender.Graphics.ClientHeight, EventsSender.Graphics.ClientWidth));
            EventsSender.Start();
        }
    }
}
