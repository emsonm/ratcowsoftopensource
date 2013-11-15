using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.Reflection;

namespace sharppunk
{
    public class KeyboardState
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

        #endregion Public Properties

        #region Constructors

        internal KeyboardState(int numKeys)
        {
            keyStates = new bool[numKeys];
        }

        #endregion Constructors

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

        #endregion Public Methods

        public static KeyboardState GetState()
        {
            KeyboardState state = new KeyboardState(keyCount);
            //int numKeys = 0;
            //byte[] keys = Sdl.SDL_GetKeyState(out numKeys);
            //for (int key = Sdl.SDLK_UNKNOWN; key < Sdl.SDLK_LAST; key++)
            //    if (keys[key] > 0)
            //        state[keyDictionary[key]] = KeyState.Down;

            foreach ( var keystate in Utils.KeyMessageFilter.Filter.KeyTable )
            {
                state[keystate.Key] = (keystate.Value ? KeyState.Down : KeyState.Up);
            }


            return state;
        }

        private static Keys minKey = EnumUtils<Keys>.Min( new Keys() );
        private static Keys maxKey = EnumUtils<Keys>.Max( new Keys() );
        private static int keyCount = EnumUtils<Keys>.Count( new Keys() );


    }


    public sealed class EnumUtils<T>
    {
        /// <summary>
        /// Gets the max value of the given enum
        /// </summary>
        public static T Max( T enumInstance )
        {
            try
            {
                Type enuType = enumInstance.GetType();

                FieldInfo[] fi = enuType.GetFields();

                Type filedType = fi[ 0 ].GetValue( enumInstance ).GetType();
                object o =
                  Convert.ChangeType( fi[ fi.Length - 1 ].GetValue( enumInstance ), filedType );
                return (T) o;
            }
            catch
            {
                return default( T );
            }
        }

        /// <summary>
        /// Gets the max value of the given enum
        /// </summary>
        public static int Count( T enumInstance )
        {
            try
            {
                Type enuType = enumInstance.GetType();

                FieldInfo[] fi = enuType.GetFields();

                Type filedType = fi[ 0 ].GetValue( enumInstance ).GetType();
                return fi.Length;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets the min value of the given enum
        /// </summary>
        public static T Min( T enumInstance )
        {
            try
            {
                Type enuType = enumInstance.GetType();

                FieldInfo[] fi = enuType.GetFields();

                Type filedType = fi[ 0 ].GetValue( enumInstance ).GetType();
                object o =
                  Convert.ChangeType( fi[ 1 ].GetValue( enumInstance ), filedType );
                return ( T )o;
            }
            catch
            {
                return default( T );
            }
        }
    }
}