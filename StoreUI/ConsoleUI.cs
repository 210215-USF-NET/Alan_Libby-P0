using System;

namespace StoreUI
{
    /// <summary>
    /// View for the Store application that reads to and writes from the console
    /// </summary>
    public class ConsoleUI : IUserInterface
    {
        public void printText(string text) {
            Console.WriteLine(text);
        }

        public string getLine(string prompt) {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public string getLine() {
            return getLine("");
        }
    }
}