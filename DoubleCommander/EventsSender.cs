using DoubleCommander.Resources;
using DoubleCommander.Views;
using NConsoleGraphics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DoubleCommander
{
    public static class EventsSender
    {
        public static event Action<ConsoleGraphics> PaintEventHandler;
        public static event Action<Keys> KeyEventHandler;
        public static event Action UpdateEventHandler;
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

        public static void SubscribeToUpdateEvent(View view)
        {
            UpdateEventHandler += view.OnUpdate;
        }

        public static void SendUpdateEvent()
        {
            UpdateEventHandler.Invoke();
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
                Graphics.FillRectangle(ColorResources.AppBackground, 0, 0, Graphics.ClientWidth, Graphics.ClientHeight);
                PaintEventHandler.Invoke(Graphics);
                Graphics.FlipPages();
                Thread.Sleep(100);
            }
        }

        private static void ListenKeys()
        {
            bool isDown = false;
            bool isUp = false;
            bool isLeft = false;
            bool isRight = false;
            bool isTab = false;
            bool isReturn = false;
            bool isBack = false;
            bool isF1 = false;
            bool isF2 = false;
            while (_working)
            {
                if (!isDown && Input.IsKeyDown(Keys.DOWN))
                {
                    KeyEventHandler.Invoke(Keys.DOWN);
                }
                else if (!isUp && Input.IsKeyDown(Keys.UP))
                {
                    KeyEventHandler.Invoke(Keys.UP);
                }
                if (!isLeft && Input.IsKeyDown(Keys.LEFT))
                {
                    KeyEventHandler.Invoke(Keys.LEFT);
                }
                else if(!isRight && Input.IsKeyDown(Keys.RIGHT))
                {
                    KeyEventHandler.Invoke(Keys.RIGHT);
                }
                if (!isReturn && Input.IsKeyDown(Keys.RETURN))
                {
                    KeyEventHandler.Invoke(Keys.RETURN);
                }
                if (!isTab && Input.IsKeyDown(Keys.TAB))
                {
                    KeyEventHandler.Invoke(Keys.TAB);
                }
                if (!isBack && Input.IsKeyDown(Keys.BACK))
                {
                    KeyEventHandler.Invoke(Keys.BACK);
                }
                if (!isF1 && Input.IsKeyDown(Keys.F1))
                {
                    KeyEventHandler.Invoke(Keys.F1);
                }
                if (!isF2 && Input.IsKeyDown(Keys.F2))
                {
                    KeyEventHandler.Invoke(Keys.F2);
                }
                isReturn = Input.IsKeyDown(Keys.RETURN);
                isDown = Input.IsKeyDown(Keys.DOWN);
                isLeft = Input.IsKeyDown(Keys.LEFT);
                isRight = Input.IsKeyDown(Keys.RIGHT);
                isUp = Input.IsKeyDown(Keys.UP);
                isTab = Input.IsKeyDown(Keys.TAB);
                isBack = Input.IsKeyDown(Keys.BACK);
                isF1 = Input.IsKeyDown(Keys.F1);
                isF2 = Input.IsKeyDown(Keys.F2);
                Thread.Sleep(40);
            }
        }
    }
}
