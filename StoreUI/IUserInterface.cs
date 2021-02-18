using System;

namespace StoreUI
{
    public interface IUserInterface
    {
        void PrintText(string text);
        void PrintResult(string text);
        string GetLine();
        string GetLine(string prompt);
    }
}
