using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testapp
{
    class LCDTest : Sketch
    {
        ILcd lcd = null;

        internal override void InitInterface<T>(T inf)
        {
          if (typeof(T) == typeof(ILcd))
          {
              lcd = (ILcd)inf;
          }
          else
              base.InitInterface(inf);
        }

        //LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

        const int buttonPin = 2;
        const int ledPin = 4;

        int buttonState = 0;         // variable for reading the pushbutton status

        public override void setup()
        {            
            // set up the LCD's number of columns and rows: 
            lcd.Begin(16, 2);
            // Print a message to the LCD.
            lcd.Print("hello, world!");

            api.pinMode(ledPin, OUTPUT);
            api.pinMode(buttonPin, INPUT);
        }

        public override void loop()
        {
            // set the cursor to column 0, line 1
            // (note: line 1 is the second row, since counting begins with 0):
            lcd.SetCursor(0, 1);
            // print the number of seconds since reset:
            lcd.Print(api.millis() / 1000);

            // read the state of the pushbutton value:
            buttonState = api.digitalRead(buttonPin);

            // check if the pushbutton is pressed.
            // if it is, the buttonState is HIGH:
            if (buttonState == HIGH)
            {
                // turn LED on:    
                api.digitalWrite(ledPin, HIGH);
            }
            else
            {
                // turn LED off:
                api.digitalWrite(ledPin, LOW);
            }
        }
    }
}
