using System;
using System.Threading.Tasks;
using System.Threading;
using NConsoleGraphics;
using DoubleCommander.Views;

namespace DoubleCommander
{
    public static class EventsSender
    {
        public static event Action<ConsoleGraphics> PaintEventHandler;
        public static event Action<Keys> KeyEventHandler;

        private static bool _working = true;
        private static readonly Task _painter = new Task(Paint);
        private static readonly Task _keyListener = new Task(ListenKeys);
        public static ConsoleGraphics Graphics { get; } = new ConsoleGraphics();

        public static void Subscribe(View view)
        {
            PaintEventHandler += view.OnPaint;
            KeyEventHandler += view.OnKeyDown;
        }

        public static void Unsubscribe(View view)
        {
            PaintEventHandler -= view.OnPaint;
            KeyEventHandler -= view.OnKeyDown;
        }

        public static void Start()
        {
            _painter.Start();
            _keyListener.Start();
        }

        public static void Stop()
        {
            _working = false;
        }

        public static void Join()
        {
            Task.WaitAll(_painter, _keyListener);
        }

        private static void Paint()
        {
            while (_working)
            {
                Graphics.FillRectangle(0xFFffffff, 0, 0, Graphics.ClientWidth, Graphics.ClientHeight);
                PaintEventHandler.Invoke(Graphics);
                Graphics.FlipPages();
                Thread.Sleep(100);
            }
        }

        private static void ListenKeys()
        {
            bool isDown = false;
            bool isUp = false;
            bool isTab = false;
            bool isReturn = false;
            bool isBack = false;
            while (_working)
            {
                if (!isDown && Input.IsKeyDown(Keys.DOWN))
                {
                    KeyEventHandler.Invoke(Keys.DOWN);
                }
                if(!isUp && Input.IsKeyDown(Keys.UP))
                {
                    KeyEventHandler.Invoke(Keys.UP);
                }
                if(!isReturn && Input.IsKeyDown(Keys.RETURN))
                {
                    KeyEventHandler.Invoke(Keys.RETURN);
                }
                if(!isTab && Input.IsKeyDown(Keys.TAB))
                {
                    KeyEventHandler.Invoke(Keys.TAB);
                }
                if(!isBack && Input.IsKeyDown(Keys.BACK))
                {
                    KeyEventHandler.Invoke(Keys.BACK);
                }
                isReturn = Input.IsKeyDown(Keys.RETURN);
                isDown = Input.IsKeyDown(Keys.DOWN);
                isUp = Input.IsKeyDown(Keys.UP);
                isTab = Input.IsKeyDown(Keys.TAB);
                isBack = Input.IsKeyDown(Keys.BACK);
                Thread.Sleep(40);
            }
        }
    }
}
