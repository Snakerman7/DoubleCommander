using DoubleCommander.Resources;
using DoubleCommander.Views;
using NConsoleGraphics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DoubleCommander.Core
{
    public static class EventsSender
    {
        public static event Action<ConsoleGraphics> PaintEventHandler;
        public static event Action<KeyEventArgs> KeyEventHandler;
        public static event Action UpdateEventHandler;
        private static bool _working = true;
        private static readonly Task _painter = new Task(Paint);
        private static readonly Task _keyListener = new Task(ListenKeys);

        public static ConsoleGraphics Graphics { get; } = new ConsoleGraphics();

        public static void Subscribe(View view)
        {
            PaintEventHandler += view.OnPaint;
            KeyEventHandler += view.OnKeyDown;
            UpdateEventHandler += view.OnUpdate;
        }

        public static void Unsubscribe(View view)
        {
            PaintEventHandler -= view.OnPaint;
            KeyEventHandler -= view.OnKeyDown;
            UpdateEventHandler -= view.OnUpdate;
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
            Task.WaitAny(_painter, _keyListener);
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
            bool isControlKeyDown = false;
            bool isFunctionKeyDown = false;
            bool isLetterKeyDown = false;
            while (_working)
            {
                if(!isControlKeyDown && IsAnyKeyDown(NumericConstants.ControlKeys, out var controlKey))
                {
                    KeyEventHandler.Invoke(new KeyEventArgs(controlKey, false));
                }
                if (!isFunctionKeyDown && IsAnyKeyDown(NumericConstants.FunctionKeys, out var funcKey))
                {
                    KeyEventHandler.Invoke(new KeyEventArgs(funcKey, false));
                }
                if(!isLetterKeyDown && IsAnyKeyDown(NumericConstants.LettersKeys, out var letterKey))
                {
                    KeyEventHandler.Invoke(new KeyEventArgs(letterKey, Input.IsKeyDown(Keys.SHIFT)));
                }
                isControlKeyDown = IsAnyKeyDown(NumericConstants.ControlKeys, out _);
                isFunctionKeyDown = IsAnyKeyDown(NumericConstants.FunctionKeys, out _);
                isLetterKeyDown = IsAnyKeyDown(NumericConstants.LettersKeys, out _);
                Thread.Sleep(20);
            }
        }

        private static bool IsAnyKeyDown(Keys[] keys, out Keys downKey)
        {
            downKey = Keys.NONAME;
            foreach(var key in keys)
            {
                if (Input.IsKeyDown(key))
                {
                    downKey = key;
                    return true;
                }
            }
            return false;
        }

        public static char GetLetterKeyChar(Keys key)
        {
            int index = (key - Keys.KEY_A);
            if(index < StringResources.Letters.Length && index >= 0)
            {
                return StringResources.Letters[index];
            }
            return ' ';
        }
    }
}
