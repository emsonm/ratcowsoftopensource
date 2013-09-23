using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sharppunk.graphics;
using System.Drawing;

namespace EngineTestBed
{
    public class Player : sharppunk.Entity
    {
        //[Embed(source = 'assets/swordguy.png')] private const PLAYER2:Class; 

        public static Spritemap playerSprite = new Spritemap(new Bitmap(Bitmap.FromFile("swordguy.png")), 48, 32);

        protected const int PLAYER_HSPEED = 80;
        protected const int GRAVITY = 4;
        protected const int JUMP_HEIGHT = (48 * 5);
        protected Point v; //velocity 
        protected Point a; //acceleration 


        public Player(float x, float y)
            : base(x, y, playerSprite)
        {
            playerSprite.Add("stand", new int[] { 0, 1, 2, 3, 4, 5 }, 0, true);
            playerSprite.Add("run", new int[] { 6, 7, 8, 9, 10, 11 }, 0, true);

            setHitbox(48, 32);

            v = new Point();
            a = new Point();
        }

        public override void Update()
        {
            base.Update();

            //bool falling = true; 

            //var b: Blocks = collide("floor", x, y) as Blocks; 
            //if (b) 
            //{ 
            //    var heightWithOffset: int = y + height; 
            //    if (v.y >= 0 && b.y <= heightWithOffset) 
            //    { 
            //        if (playerSprite.flipped) 
            //        { 
            //          //we need to account for sword 
            //          var realx: int =  x + (width - 10); 

            //          if (realx <= b.x + b.width) 
            //          {                    
            //            //we were falling and we want to stop 
            //            groundlevel = b.y; 
            //            v.y  = 0; 
            //            y = groundlevel - height; 
            //            falling = false; 
            //          } 
            //        } 
            //        else
            //        { 
            //          //we were falling and we want to stop 
            //          groundlevel = b.y; 
            //          v.y  = 0; 
            //          y = groundlevel - height; 
            //          falling = false; 
            //        } 
            //    } 
            //} 


            //if (y + height > FP.screen.height) 
            //{ 
            //  v.y = 0; //stop moving 
            //  y = FP.screen.height - height; 
            //  falling = false; 
            //} 

            ////check input 
            //var hinput : int = 0; 

            //if (Input.check(Key.LEFT)) {  
            //    playerSprite.flipped = true; 
            //    hinput -= 1; 
            //    playerSprite.play("run", false); 
            //} 
            //else if (Input.check(Key.RIGHT)) {  
            //    playerSprite.flipped = false; 
            //    hinput += 1;  
            //    playerSprite.play("run", false); 
            //} 
            //else playerSprite.play("stand", false); 

            //if (Input.pressed(Key.SPACE)) { 
            //  jump(); 
            //} 

            //if (falling) 
            //{ 
            //  //update physics 
            //  a.y = GRAVITY; 
            //  v.y += a.y; 
            //  v.x = PLAYER_HSPEED * hinput ;           
            //} 

            ////apply physics 
            //x += v.x * FP.elapsed; 
            //y += v.y * FP.elapsed; 


        }

        //protected var groundlevel: Number = MP.screen.height; 

        //protected function jump():void 
        //{ 
        //    if (y + height >= groundlevel) 
        //    { 
        //        v.y = -JUMP_HEIGHT; 
        //    }            
        //} 
    }
}
