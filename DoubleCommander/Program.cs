using System;
using DoubleCommander.Views;
using NConsoleGraphics;

namespace DoubleCommander
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(160, 20);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.BackgroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            Console.Clear();

            ListView lv = new ListView(new FileSystemViewer());
            ListView lv2 = new ListView(new FileSystemViewer()) { Enabled = false };
            DoublePanelView dpv = new DoublePanelView(
                new Point(0, 0), 
                new Size(EventsSender.Graphics.ClientHeight, EventsSender.Graphics.ClientWidth));
            dpv.LeftView = lv;
            dpv.RightView = lv2;
            EventsSender.Subscribe(lv);
            EventsSender.Subscribe(lv2);
            EventsSender.Subscribe(dpv);
            EventsSender.Start();
            EventsSender.Join();
        }
    }
}
