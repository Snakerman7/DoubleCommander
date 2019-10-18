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
        public static readonly Keys[] LettersKeys = { Keys.KEY_A, Keys.KEY_B, Keys.KEY_C, Keys.KEY_D, Keys.KEY_E, Keys.KEY_F,
        Keys.KEY_G, Keys.KEY_H, Keys.KEY_I, Keys.KEY_J, Keys.KEY_K, Keys.KEY_L, Keys.KEY_M, Keys.KEY_N, Keys.KEY_O, Keys.KEY_P,
        Keys.KEY_Q, Keys.KEY_R, Keys.KEY_S, Keys.KEY_T, Keys.KEY_U, Keys.KEY_V, Keys.KEY_W, Keys.KEY_X, Keys.KEY_Y, Keys.KEY_Z};
        public static readonly Keys[] FunctionKeys = { Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9 };
        public static readonly Keys[] ControlKeys = { Keys.LEFT, Keys.RIGHT, Keys.UP, Keys.DOWN, Keys.BACK, Keys.RETURN, Keys.TAB, Keys.ESCAPE };
        public static event Action<PaintEventArgs> PaintEvent;
        public static event Action<KeyDownEventArgs> KeyDownEvent;
        public static event Action UpdateEvent;
        private static bool _working = true;
        private static readonly Task _painter = new Task(Paint);
        private static readonly Task _keyListener = new Task(ListenKeys);
        public static ConsoleGraphics Graphics { get; } = new ConsoleGraphics();

        public static void Subscribe(View view)
        {
            PaintEvent += view.OnPaint;
            KeyDownEvent += view.OnKeyDown;
            UpdateEvent += view.OnUpdate;
        }

        public static void Unsubscribe(View view)
        {
            PaintEvent -= view.OnPaint;
            KeyDownEvent -= view.OnKeyDown;
            UpdateEvent -= view.OnUpdate;
        }

        public static void SendUpdateEvent()
        {
            UpdateEvent.Invoke();
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
                PaintEvent.Invoke(new PaintEventArgs(Graphics));
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
                if (!isControlKeyDown && IsAnyKeyDown(ControlKeys, out var controlKey))
                {
                    KeyDownEvent.Invoke(new KeyDownEventArgs(controlKey));
                }
                if (!isFunctionKeyDown && IsAnyKeyDown(FunctionKeys, out var funcKey))
                {
                    KeyDownEvent.Invoke(new KeyDownEventArgs(funcKey));
                }
                if (!isLetterKeyDown && IsAnyKeyDown(LettersKeys, out var letterKey))
                {
                    KeyDownEvent.Invoke(new KeyDownEventArgs(letterKey, Input.IsKeyDown(Keys.SHIFT)));
                }
                isControlKeyDown = IsAnyKeyDown(ControlKeys, out _);
                isFunctionKeyDown = IsAnyKeyDown(FunctionKeys, out _);
                isLetterKeyDown = IsAnyKeyDown(LettersKeys, out _);
                Thread.Sleep(10);
            }
        }

        private static bool IsAnyKeyDown(Keys[] keys, out Keys downKey)
        {
            downKey = Keys.NONAME;
            foreach (var key in keys)
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
            if (index < StringResources.Letters.Length && index >= 0)
            {
                return StringResources.Letters[index];
            }
            return ' ';
        }
    }
}
