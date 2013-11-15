using System.Windows.Forms;

namespace sharppunk.Utils
{
    public static class Input
    {
        public static bool Check(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static bool Pressed(Keys key)
        {
            bool isPressedNow = keyboardState.IsKeyDown(key);
            bool wasPressedLastFrame = oldKeyboardState.IsKeyDown(key);
            return (isPressedNow && (!wasPressedLastFrame));
        }

        public static bool Released(Keys key)
        {
            bool isPressedNow = keyboardState.IsKeyDown(key);
            bool wasPressedLastFrame = oldKeyboardState.IsKeyDown(key);
            return ((!isPressedNow) && wasPressedLastFrame);
        }

        internal static void UpdateKeyboardInput()
        {
            keyboardState = KeyboardState.GetState();
        }

        internal static void SaveOldKeyboardInput()
        {
            oldKeyboardState = KeyboardState.GetState();
        }

        private static KeyboardState keyboardState = KeyboardState.GetState();
        private static KeyboardState oldKeyboardState = KeyboardState.GetState();
    }
}