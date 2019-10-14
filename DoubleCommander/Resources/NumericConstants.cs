using NConsoleGraphics;

namespace DoubleCommander.Resources
{
    public static class NumericConstants
    {
        public const int ListViewItemHeight = 15;
        public const int MarginUpLeft = 5;
        public const int MarginRightDown = 10;
        public const int MaxFileNameLength = 46;
        public const int FontSize = 10;


        public static readonly Keys[] LettersKeys = { Keys.KEY_A, Keys.KEY_B, Keys.KEY_C, Keys.KEY_D, Keys.KEY_E, Keys.KEY_F,
        Keys.KEY_G, Keys.KEY_H, Keys.KEY_I, Keys.KEY_J, Keys.KEY_K, Keys.KEY_L, Keys.KEY_M, Keys.KEY_N, Keys.KEY_O, Keys.KEY_P,
        Keys.KEY_Q, Keys.KEY_R, Keys.KEY_S, Keys.KEY_T, Keys.KEY_U, Keys.KEY_V, Keys.KEY_W, Keys.KEY_X, Keys.KEY_Y, Keys.KEY_Z};

        public static readonly Keys[] FunctionKeys = { Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9 };

        public static readonly Keys[] ControlKeys = { Keys.LEFT, Keys.RIGHT, Keys.UP, Keys.DOWN, Keys.BACK, Keys.RETURN, Keys.TAB, Keys.ESCAPE };
    }
}
