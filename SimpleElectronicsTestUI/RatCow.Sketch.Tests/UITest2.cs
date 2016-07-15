using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatCow.Sketch.Tests
{
    /// <summary>
    /// Simple little sketch to simulate a cursor on the screen.
    /// </summary>
    public class UITest2 : LCDSketch
    {
        //constants for the pins we are mapping
        const int leftButton = 1;
        const int verticalButton = 2;
        const int rightButton = 3;

        //pin for the first led
        const int ledPin1 = 4;

        //last button we pressed, or -1 when no button is HIGH
        int lastHighButton = -1;

        //to keep tabs on the position
        int x = 0;
        int y = 0;

        char[,] lcdMatrix;

        char cell = '@';

        /// <summary>
        /// init data
        /// </summary>
        public override void setup()
        {
            // set up the LCD's number of columns and rows: 
            lcd.Begin(16, 2);

            lcdMatrix = new char[16, 2];
            for (var x = 0; x < 16; x++)
                for (var y = 0; y < 2; y++)
                    lcdMatrix[x, y] = ' ';

            //Wire the buttons
            api.pinMode(leftButton, INPUT);
            api.pinMode(rightButton, INPUT);
            api.pinMode(verticalButton, INPUT);

            api.pinMode(ledPin1, OUTPUT);
        }


        /// <summary>
        /// processing loop, happens once per cycle.
        /// </summary>
        public override void loop()
        {
            //buttons will stay high, so we need to debounce them.
            //this is done by looking at the last lastHighButton and
            //ignoring the state till it is LOW again.        
            var leftbuttonState = api.digitalRead(leftButton);
            var rightbuttonState = api.digitalRead(rightButton);
            var verticalbuttonState = api.digitalRead(verticalButton);

            //this clears the last position, if you remove the text, this makes the cursor
            //redraw without leaving a trace behind it. We don't really need this, but it's 
            //better to be here to not break things if we forget to do this later on.
            lcd.SetCursor(x, y);
            lcd.Print(" ");

            //this simulates test on the screen
            lcd.SetCursor(0, 0);
            lcd.Print(GetLine(0));
            lcd.SetCursor(0, 1);
            lcd.Print(GetLine(1));

            var cellIsSet = GetChar(x, y) == cell;

            if (cellIsSet)
            {
                api.digitalWrite(ledPin1, HIGH);
            }
            else
            {
                api.digitalWrite(ledPin1, LOW);
            }


            //look at the button states
            if (leftbuttonState == HIGH)
            {
                if (lastHighButton != leftButton)
                {
                    if (x > 0)
                    {
                        x -= 1;
                    }
                    else
                    {
                        FlipY();
                        x = 15;
                    }
                }

                lastHighButton = leftButton;
            }
            else if (rightbuttonState == HIGH)
            {
                if (lastHighButton != rightButton)
                {
                    if (x < 15)
                    {
                        x += 1;
                    }
                    else
                    {
                        FlipY();
                        x = 0;
                    }
                }

                lastHighButton = rightButton;
            }
            else if (verticalbuttonState == HIGH)
            {
                if (lastHighButton != verticalButton)
                {
                    if (cellIsSet)
                        SetChar(x, y, ' ');
                    else
                        SetChar(x, y, cell);
                }

                lastHighButton = verticalButton;
            }
            else
            {
                lastHighButton = -1;
            }

            // set the cursor to column 0, line 1
            // (note: line 1 is the second row, since counting begins with 0):            
            lcd.SetCursor(x, y);
            lcd.Print("*");
        }

        void SetChar(int cx, int cy, char c)
        {
            lcdMatrix[cx, cy] = c;
        }

        char GetChar(int cx, int cy)
        {
            return lcdMatrix[cx, cy];
        }

        string GetLine(int index)
        {
            var result = new StringBuilder();
            for (var l = 0; l < 16; l++)
            {
                result.Append(lcdMatrix[l, index]);
            }

            return result.ToString();
        }

        void FlipY()
        {
            //this basically wraps around...
            if (y == 0)
                y = 1;
            else
                y = 0;
        }
    }
}
