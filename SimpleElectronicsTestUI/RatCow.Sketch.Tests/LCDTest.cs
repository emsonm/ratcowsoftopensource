using System;
using RatCow.Sketch;

namespace RatCow.Sketch.Tests
{
    public class LCDTest : Sketch
    {
        ILcd lcd = null;

        public override void InitInterface<T>(T inf)
        {
          if (typeof(T) == typeof(ILcd))
          {
              lcd = (ILcd)inf;
          }
          else
              base.InitInterface(inf);
        }

        //LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

        const int buttonPin1 = 1;
        const int ledPin1 = 4;

        const int buttonPin2 = 2;
        const int ledPin2 = 5;

        const int buttonPin3 = 3;

        public override void setup()
        {            
            // set up the LCD's number of columns and rows: 
            lcd.Begin(16, 2);
            // Print a message to the LCD.
            lcd.Print("hello, world!");

            api.pinMode(ledPin1, OUTPUT);
            api.pinMode(buttonPin1, INPUT);

            api.pinMode(ledPin2, OUTPUT);
            api.pinMode(buttonPin2, INPUT);

            api.pinMode(buttonPin3, INPUT);
        }

        public override void loop()
        {
            // set the cursor to column 0, line 1
            // (note: line 1 is the second row, since counting begins with 0):
            lcd.SetCursor(0, 1);
            // print the number of seconds since reset:
            lcd.Print(api.millis() / 1000);

            updateButton(buttonPin1, ledPin1);
            updateButton(buttonPin2, ledPin2);
            updateScreen(buttonPin3);
        }

        void updateButton(int buttonPin, int ledPin)
        {
            // read the state of the pushbutton value:
            var buttonState = api.digitalRead(buttonPin); 

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

        void updateScreen(int buttonPin)
        {
            // read the state of the pushbutton value:
            var buttonState = api.digitalRead(buttonPin);

            // check if the pushbutton is pressed.
            // if it is, the buttonState is HIGH:
            if (buttonState == HIGH)
            {
                lcd.SetCursor(0, 0);
                lcd.Print("Goodbye, moon!");
            }
            else
            {
                lcd.SetCursor(0, 0);
                lcd.Print("hello, world! ");
            }
        }
    }
}
