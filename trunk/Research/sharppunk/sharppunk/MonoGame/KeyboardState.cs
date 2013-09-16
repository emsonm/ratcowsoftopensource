using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sharppunk
{
    public struct KeyboardState
    {
        #region Private Fields

        private bool[] keyStates;

        #endregion Private Fields


        #region Public Properties

        public KeyState this[Keys key]
        {
            get { return keyStates[(int)key] ? KeyState.Down : KeyState.Up; }
            internal set { this.keyStates[(int)key] = (value == KeyState.Down) ? true : false; }
        }

        #endregion


        #region Constructors

        internal KeyboardState(int numKeys)
        {
            keyStates = new bool[numKeys];
        }

        #endregion


        #region Public Methods

        public static bool operator !=(KeyboardState a, KeyboardState b)
        {
            return !(a == b);
        }

        public static bool operator ==(KeyboardState a, KeyboardState b)
        {
            return a.keyStates == b.keyStates;
        }

        public override bool Equals(object obj)
        {
            return (obj is KeyboardState) ? false : (KeyboardState)obj == this;
        }

        public override int GetHashCode()
        {
            return keyStates.GetHashCode();
        }

        public Keys[] GetPressedKeys()
        {
            List<Keys> keysDown = new List<Keys>();
            for (int i = 0; i < keyStates.Length; i++)
                if (keyStates[i])
                    keysDown.Add((Keys)i);

            return keysDown.ToArray();
        }

        public bool IsKeyDown(Keys key)
        {
            return keyStates[(int)key];
        }

        public bool IsKeyUp(Keys key)
        {
            return !keyStates[(int)key];
        }

        #endregion

        public static KeyboardState GetState()
        {
            KeyboardState state = new KeyboardState();
            //int numKeys = 0;
            //byte[] keys = Sdl.SDL_GetKeyState(out numKeys);
            //for (int key = Sdl.SDLK_UNKNOWN; key < Sdl.SDLK_LAST; key++)
            //    if (keys[key] > 0)
            //        state[keyDictionary[key]] = KeyState.Down;

            return state;
        }
    }
}
