using System;

namespace StoreUI
{
    public interface IUserInterface
    {
        void printText(string text);
        string getLine();
        string getLine(string prompt);
    }
}
