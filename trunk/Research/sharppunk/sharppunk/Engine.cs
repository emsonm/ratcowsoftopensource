using System;
using System.Drawing;
using sharppunk.Utils;

namespace sharppunk
{
    public class Engine
    {
        private System.Drawing.Graphics graphics;

        public static Engine currentEngine;

        public Engine(int width, int height, string assetsDirectory = "./")
            : base()
        {
            MP.Width = width;
            MP.Height = height;
            MP.currentWorld = new World();

            MP.AllowUserResizing = true;

            MP.Buffer = new Bitmap(
                Convert.ToInt32(Math.Ceiling((double)MP.Width)),
                Convert.ToInt32(Math.Ceiling((double)MP.Height)));

            //graphics.SynchronizeWithVerticalRetrace = false;
            //graphics.PreferredBackBufferWidth = MP.Width;
            //graphics.PreferredBackBufferHeight = MP.Height;
            //graphics.ApplyChanges ();

            //this.IsFixedTimeStep = false;
            //graphics.IsFullScreen = true;

            //graphics.ApplyChanges ();
            //Content.RootDirectory = assetsDirectory;
            Engine.currentEngine = this;
        }

        public static Bitmap EmbeddFile(String filename)
        {
            return currentEngine.LoadBitmap(filename);
        }

        private Bitmap LoadBitmap(string filename)
        {
            throw new NotImplementedException();
        }

        protected void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures
            //MP.Buffer = graphics;
            // TODO: use this.Content to load your game content here
            //eater.LoadGraphic(Content,"smiley.png");
            //MP.CurrentWorld.Add (eater);
            //myWorld.addRender(eater);
            //myWorld.addUpdate(eater);
        }

        private object renderLocker = new object();

        protected void Draw(uint time)
        {
            //graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            ClearScreen();

            lock (renderLocker)
            {
                Render();
            }
        }

        private void ClearScreen()
        {
            using (var graphics = System.Drawing.Graphics.FromImage(MP.Buffer))
            {
                graphics.Clear(Color.CornflowerBlue); //FillRectangle(new SolidBrush(Color.CornflowerBlue), 0.0f, 0.0f, MP.Width, MP.Height);
            }
        }

        protected void Update(uint time)
        {
            Input.UpdateKeyboardInput();
            MP.Elapsed = time / 1000; //time is in miliseconds

            // Write frame rate to console
            frameRateSum += 1 / MP.Elapsed;
            frameRateCount += 1;
            if (frameRateCount == 100)
            {
                Console.WriteLine(frameRateSum / frameRateCount);
                frameRateSum = 0;
                frameRateCount = 0;
            }

            //if (FP.tweener.active && FP.tweener._tween) FP.tweener.updateTweens();
            if (MP.CurrentWorld.Active)
            {
                //if (FP._world._tween) FP._world.updateTweens();
                MP.CurrentWorld.Update();
            }
            MP.CurrentWorld.UpdateLists();
            if (MP.nextWorld != null) checkWorld();
            Input.SaveOldKeyboardInput();
            //base.Update (gameTime);
        }

        /**
         * Renders the game, rendering the World and Entities.
         */

        public void Render()
        {
            ClearScreen();

            MP.CurrentWorld.Render();
        }

        /** @private Switch Worlds if they've changed. */

        private void checkWorld()
        {
            if (MP.nextWorld == null) return;
            MP.currentWorld.End();
            MP.currentWorld.UpdateLists();
            //if (FP._world && FP._world.autoClear && FP._world._tween) FP._world.clearTweens();
            MP.currentWorld = MP.nextWorld;
            MP.nextWorld = null;
            MP.Camera = MP.CurrentWorld.Camera;
            MP.currentWorld.UpdateLists();
            MP.currentWorld.Begin();
            MP.currentWorld.UpdateLists();
        }

        private double frameRateSum = 0;
        private int frameRateCount = 0;
    }
}