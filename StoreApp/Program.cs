using System;
using StoreModels;
using StoreUI;
using StoreData;

namespace StoreApp
{
    class Program
    {
        static IUserInterface userInterface;
        static IDataStore dataStore;
        static Order cart = null;
        static Customer currentUser = null;
        static void Main(string[] args)
        {
            userInterface = new ConsoleUI();
            dataStore = new MemoryDataStore();
            bool exit = false;
            while (!exit) {
                Login();
                exit = MainMenu();
            }
        }
        static void Login() {
            string name = userInterface.getLine("Enter your name: ");
            currentUser = dataStore.GetCustomer(name);
            if (currentUser == null) {
                currentUser = new Customer();
                currentUser.Name = name;
                dataStore.AddCustomer(name, currentUser);
            }
            cart = new Order();
        }

        static bool MainMenu() {
            //Console.Clear();
            while (currentUser != null) {
                userInterface.printText("Welcome, " + currentUser.Name);
                userInterface.printText(
                    "Please choose an option to continue...\n" +
                    "[0] View cart\n" +
                    "[1] Add item to cart\n" +
                    "[2] Search for items\n" +
                    "[3] Check Out\n" +
                    "[4] Log Out\n" +
                    "[5] Exit"
                );
                switch(userInterface.getLine()) {
                    case "0":
                        Console.Clear();
                        userInterface.printText("TODO");
                        break;
                    case "1":
                        Console.Clear();
                        userInterface.printText("TODO");
                        break;
                    case "2":
                        Console.Clear();
                        userInterface.printText("TODO");
                        break;
                    case "3":
                        Console.Clear();
                        userInterface.printText("TODO");
                        break;
                    case "4":
                        Console.Clear();
                        currentUser = null;
                        break;
                    case "5":
                        Console.Clear();
                        return true;
                    default:
                        Console.Clear();
                        userInterface.printText("Invalid input, please enter one of the given options");
                        break;
                }
            }
            return false;
        }
    }
}
