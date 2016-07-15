using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatCow.Sketch.Tests
{
    /// <summary>
    /// Simple little sketch to simulate a cursor on the screen.
    /// 
    /// This allows the user to scroll through a simple list with left and right 
    /// buttons, and select various items with middle button.
    /// </summary>
    public class UIListTest : LCDSketch
    {
        //constants for the pins we are mapping
        const int leftButton = 1;
        const int verticalButton = 2;
        const int rightButton = 3;

        //last button we pressed, or -1 when no button is HIGH
        int lastHighButton = -1;

        //to keep tabs on the position
        int x = 0;
        int y = 0;

        int listPosition = 0;
        string[] list = { "1. Item 1", "2. Item 2", "3. Item 3", "4. Item 4", "5. Item 5" };
        bool[] selectionList = { false, false, false, false, false };

        int direction = 0;
        int cursorPosition = 0;

        int lastListItem = 0;
        int maxiumListItem = 0;

        /// <summary>
        /// init data
        /// </summary>
        public override void setup()
        {
            // set up the LCD's number of columns and rows: 
            lcd.Begin(16, 2);

            //Wire the buttons
            api.pinMode(leftButton, INPUT);
            api.pinMode(rightButton, INPUT);
            api.pinMode(verticalButton, INPUT);

            lastListItem = list.Length - 1;
            maxiumListItem = lastListItem - 1;
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

            //look at the button states
            if (leftbuttonState == HIGH)
            {
                if (lastHighButton != leftButton)
                {
                    direction = -1;

                    if (listPosition > 0)
                    {
                        listPosition -= 1;
                    }
                    else
                    {
                        listPosition = lastListItem;
                    }
                }

                lastHighButton = leftButton;
            }
            else if (rightbuttonState == HIGH)
            {
                if (lastHighButton != rightButton)
                {
                    direction = 1;

                    if (listPosition < lastListItem)
                    {
                        listPosition += 1;
                    }
                    else
                    {
                        listPosition = 0;
                    }
                }

                lastHighButton = rightButton;
            }
            else if (verticalbuttonState == HIGH)
            {
                if (lastHighButton != verticalButton)
                {
                    selectionList[listPosition] = !selectionList[listPosition]; //toggle
                }

                lastHighButton = verticalButton;
            }
            else
            {
                lastHighButton = -1;
            }

            SetCursorPosition();
            SetListCursorPosition();


            //this simulates test on the screen
            lcd.SetCursor(2, 0);
            lcd.Print(list[cursorPosition]);
            lcd.SetCursor(2, 1);
            lcd.Print(list[cursorPosition + 1]);

            lcd.SetCursor(1, 0);
            lcd.Print(selectionList[cursorPosition] ? "X" : " ");
            lcd.SetCursor(1, 1);
            lcd.Print(selectionList[cursorPosition + 1] ? "X" : " ");

            //debug
            //lcd.SetCursor(14, 0);
            //lcd.Print(cursorPosition);
            //lcd.SetCursor(14, 1);
            //lcd.Print(listPosition);

            // set the cursor to column 0, line 1
            // (note: line 1 is the second row, since counting begins with 0):            
            lcd.SetCursor(x, y);
            lcd.Print("*");
        }

        /// <summary>
        /// Updates the Y co-ordinate of the little UI cursor
        /// </summary>
        void SetCursorPosition()
        {
            if (direction == 1)
            {
                y = listPosition == 0 ? 0 : 1;
            }
            else if (direction == -1)
            {
                y = listPosition == lastListItem ? 1 : 0;
            }
        }

        /// <summary>
        /// Updates the cursor position in the list
        /// 
        /// The user can scroll in either direction. When we hit 
        /// the last item, we jump tot he top/bottom of the list.
        /// That's not exactly perfect, but I figured it was 
        /// better than adding in a endless list with no context 
        /// to start/end points.
        /// </summary>
        void SetListCursorPosition()
        {
            if (lastHighButton == -1) return;

            if (y == 1 && listPosition > 0)
                cursorPosition = listPosition - 1;
            else
                cursorPosition = listPosition;
        }
    }
}
