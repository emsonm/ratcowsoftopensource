using System;
using System.Drawing;

namespace sharppunk
{
    public static class MP
    {
        /**
        * The current screen buffer, drawn to in the render loop.
        */
        public static Bitmap Buffer;
        private static Graphics _bufferObject = null;
        private static int bufferRequests = 0;
        private static object bufferLock = new object();

        internal static Graphics BeginRender()
        {
            if (_bufferObject == null)
            {
                _bufferObject = Graphics.FromImage(Buffer);           
            }

            lock (bufferLock)
            {
                bufferRequests++;
                System.Diagnostics.Debug.WriteLine("++" + bufferRequests.ToString());
            }

            return _bufferObject;
        }

        internal static void EndRender()
        {
            lock (bufferLock)
            {
                bufferRequests--;
                System.Diagnostics.Debug.WriteLine("--" + bufferRequests.ToString());
                if (bufferRequests < 0) bufferRequests = 0;
                System.Diagnostics.Debug.WriteLine("(R)--" + bufferRequests.ToString());
            }

            if (_bufferObject != null)
            {
                _bufferObject.Dispose();
                _bufferObject = null;
            }
        }

        public static double Elapsed;

        public static int Width;
        public static int Height;

        public static Vector2 Camera;

        public static float FElapsed
        {
            get { return (float)Elapsed; }
        }

        public static World CurrentWorld
        {
            get { return currentWorld; }
            set
            {
                if (currentWorld != value)
                {
                    nextWorld = value;
                }
            }
        }

        public static float Degs2Rad(float degrees)
        {
            return (degrees / 180 * ((float)Math.PI));
        }

        public static int Rand(int amount)
        {
            return random.Next(0, amount);
        }

        /*internal*/
        public static World currentWorld;
        internal static World nextWorld;

        private static Random random = new Random();

        public static bool AllowUserResizing { get; set; }
    }
}