using System;
namespace Testapp
{
    internal interface ILcd
    {
        void Clear();
        void SetCursor(int x, int y);
        void Write(char c);
        void Print(string s);
        void Print(long l);
        void Begin(int x, int h);
    }
}
