using System;
namespace RatCow.Sketch
{
    public interface IGeneral
    {
        long millis();
        void delay(int time);

        void pinMode(int id, byte mode);

        int digitalRead(int id);
        void digitalWrite(int id, int value);
    }
}
