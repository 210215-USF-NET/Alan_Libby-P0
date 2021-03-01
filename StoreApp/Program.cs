using System;
using System.Collections.Generic;
using StoreModels;
using StoreUI;
using StoreData;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using StoreData.Entities;
using System.Transactions;

namespace StoreApp
{
    class Program
    {
        static IUserInterface userInterface;
        static IDataStore dataStore;
        static StoreModels.Order cart = null;
        static StoreModels.Customer currentUser = null;
        static bool managerMenu = false;
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            string connectionString = configuration.GetConnectionString("store");

            DbContextOptions<storeContext> options = new DbContextOptionsBuilder<storeContext>()
            .UseSqlServer(connectionString)
            .Options;

            using var ctx = new storeContext(options);
            var transaction = ctx.Database.BeginTransaction();

            userInterface = new ConsoleUI();
            //dataStore = new MemoryDataStore();
            dataStore = new DatabaseDataStore(ctx, ref transaction);
            bool exit = false;
            while (!exit) {
                Login();
                exit = MainMenu();
            }
            transaction.Dispose();
        }
        static void Login() {
            string name = userInterface.GetLine("Enter your name: ");
            currentUser = dataStore.GetCustomer(name);
            if (currentUser == null) {
                currentUser = new StoreModels.Customer();
                currentUser.Name = name;
                currentUser.CustomerID = dataStore.AddCustomer(name, currentUser);
            }
            cart = new StoreModels.Order();
            cart.Customer = currentUser;
            managerMenu = false;
            if (currentUser.IsManager) {
                managerMenu = userInterface.GetLine("Would you like to log in as a manager? [ y / N ]: ").ToLower().Equals("y");
            }
        }

        static bool MainMenu() {
            //Console.Clear();
            while (currentUser != null) {
                userInterface.PrintText("Welcome, " + currentUser.Name);
                userInterface.PrintText(
                    "Please choose an option to continue...\n" +
                    (managerMenu ? "[0] View inventory changes\n" : "[0] View cart\n") +
                    (managerMenu ? "[1] Restock item\n" : (cart.CheckedOut ? "[1] Check Cart Inventory\n" : "[1] Add item to cart\n")) +
                    "[2] Search for items\n" +
                    (managerMenu ?  "[3] Save inventory changes\n" : (cart.CheckedOut ? "[3] Place Order Again\n" : "[3] Check Out\n")) +
                    (managerMenu ? "[4] View order history\n" : (cart.CheckedOut ? "[4] Close previous order\n" : "[4] View Previous Order\n")) +
                    "[5] Log Out\n" +
                    "[6] Exit"
                );
                switch(userInterface.GetLine()) {
                    case "0":   // View cart
                        Console.Clear();
                        ViewCartMenu();
                        break;
                    case "1":   // Search available items & add to cart | Check inventory of items in cart
                        Console.Clear();
                        if (cart.CheckedOut) {
                            string inventoryLog = "Checking whether all products from this order are still in stock\n";
                            bool canReorder = true;
                            foreach(Item item in cart.Items) {
                                int curInventory = dataStore.GetLocationInventory(cart.Location, item.Product);
                                inventoryLog += "\n" + item.Product.ProductName + " in stock: " + curInventory + " (ordered " + item.Quantity + ')';
                                if (curInventory < item.Quantity) canReorder = false;
                            }
                            inventoryLog += "\n\nYour order is" + (canReorder ? "" : " not") + " in stock.";
                            userInterface.PrintResult(inventoryLog);
                            break;
                        }
                        AddToCartMenu();
                        break;
                    case "2":   // Search full item list
                        Console.Clear();
                        ListProductsMenu();
                        break;
                    case "3":   // Check out | Copy Cart
                        Console.Clear();
                        if (cart.CheckedOut) {
                            bool canReorder = true;
                            foreach(Item item in cart.Items) {
                                int curInventory = dataStore.GetLocationInventory(cart.Location, item.Product);
                                if (curInventory < item.Quantity) canReorder = false;
                            }
                            if (canReorder) {
                                Order newCart = cart.copy();
                                foreach(Item item in cart.Items) {
                                    dataStore.UpdateLocationInventory(newCart.Location, item.Product, -item.Quantity);
                                }
                                cart = newCart;
                                userInterface.PrintResult("Copied all items into your cart");
                            } else {
                                userInterface.PrintResult("Not enough inventory to copy this order");
                            }
                            break;
                        }
                        if (cart.Items.Count == 0) {
                            userInterface.PrintResult("Please add at least one item to your cart first");
                            break;
                        }
                        if (userInterface.GetLine("Your total is " + cart.Total.ToString("C") + "\nCheck out? [ y / N ]: ").ToLower().Equals("y")) {
                            userInterface.PrintResult("Order successful, thanks for shopping with us!");
                            cart.CheckedOut = true;
                            cart.CheckoutTimestamp = DateTime.Now;
                            dataStore.PlaceOrder(cart);
                            cart = new Order();
                            cart.Customer = currentUser;
                        }
                        Console.Clear();
                        break;
                    case "4":   // View previous order | return to current order
                        Console.Clear();
                        if (cart.CheckedOut) {
                            cart = new Order();
                            cart.Customer = currentUser;
                            break;
                        }
                        bool sortByPrice = userInterface.GetLine("Sort by price? (default: date) [ y / N ]: ").ToLower().Equals("y");
                        bool asc = userInterface.GetLine("Use ascending order? [ y / N ]: ").ToLower().Equals("y");
                        List<Order> previousOrders =  managerMenu ? dataStore.GetAllOrders() : dataStore.GetCustomerOrders(currentUser);
                        if (sortByPrice) {
                            previousOrders.Sort((o1, o2) => {
                                decimal diff = o1.Total - o2.Total;
                                if (diff == 0) return 0;
                                if (diff < 0) return -1;
                                return 1;
                            });
                        }
                        if (asc) {
                            previousOrders.Reverse();
                        }
                        int index = CartSelectMenu(previousOrders);
                        if (index < 0) {
                            break;
                        }
                        if (userInterface.GetLine("This will overwrite your current cart\nProceed? [ y / N ]: ").ToLower().Equals("y")) {
                            cart = previousOrders[index];
                        }
                        Console.Clear();
                        break;
                    case "5":   // Log out
                        Console.Clear();
                        currentUser = null;
                        break;
                    case "6":   // Exit
                        Console.Clear();
                        return true;
                    default:
                        Console.Clear();
                        userInterface.PrintText("Invalid input, please enter one of the given options");
                        break;
                }
            }
            return false;
        }

        static void ListProductsMenu() {
            userInterface.PrintText("Enter the number associated with any product to view more information");
            userInterface.PrintText("Or press enter to continue");
            List<StoreModels.Product> products = dataStore.GetAllProducts();
            for (int i = 0; i < products.Count; i++) {
                userInterface.PrintText("[" + i + "] " + products[i].ProductName);
            }
            if (products.Count == 0) {
                userInterface.PrintText("There are no products in the data store");
            }
            string input = userInterface.GetLine();
            int index;
            Console.Clear();
            if (!int.TryParse(input, out index))
                return;
            if (index < 0 || index >= products.Count)
                return;
            Console.Clear();
            userInterface.PrintText(products[index].ToString());
            userInterface.PrintText("\nAvailable At:");
            foreach(StoreModels.Location loc in dataStore.GetAvailableLocations(products[index])) {
                userInterface.PrintText("\t" + loc.LocationName + "\t(" + dataStore.GetLocationInventory(loc, products[index]) + " in stock)");
            }
            userInterface.PrintResult("");
        }

        static void AddToCartMenu() {
            userInterface.PrintText("Enter the number associated with any product to add it to your cart");
            userInterface.PrintText("Or press enter to continue without adding anything");
            List<StoreModels.Product> products;
            if (cart.Location != null) {
                products = dataStore.GetAvailableProducts(cart.Location);
            } else {
                products = dataStore.GetAllProducts();
            }
            for (int i = 0; i < products.Count; i++) {
                userInterface.PrintText("[" + i + "] " + products[i].ProductName);
            }
            if (products.Count == 0) {
                userInterface.PrintText("There are no products to show");
            }
            string input = userInterface.GetLine();
            int index;
            Console.Clear();
            if (!int.TryParse(input, out index))
                return;
            if (index < 0 || index >= products.Count)
                return;
            Console.Clear();
            if (cart.Location == null) {
                LocationSelectMenu(products[index]);
            }
            Console.Clear();
            if (cart.Location == null) {
                return;
            }
            int inventory = dataStore.GetLocationInventory(cart.Location, products[index]);
            int nProduct;
            do {
                userInterface.PrintText("How many of this product would you like to buy?");
                userInterface.PrintText("Please enter a number in the range [ 1 , " + inventory + " ]");
                input = userInterface.GetLine();
                if (!int.TryParse(input, out nProduct)) {
                    Console.Clear();
                    userInterface.PrintResult("Canceled adding to cart");
                    return;
                }
            } while (nProduct < 1 || nProduct > inventory);
            Item item = cart.Items.Find((item) => item.Product == products[index]);
            if (item == null) {
                item = new Item();
                item.Product = products[index];
                item.Quantity = nProduct;
                cart.Items.Add(item);
            } else {
                item.Quantity += nProduct;
            }
            dataStore.UpdateLocationInventory(cart.Location, products[index], -nProduct);
            Console.Clear();
            userInterface.PrintResult("Successfully added " + nProduct + " of " + products[index].ProductName + " to cart");
        }

        static void LocationSelectMenu(StoreModels.Product product) {
            userInterface.PrintText("Enter the number corresponding to the location you want to order from");
            userInterface.PrintText("Or press enter to return to the main menu");
            List<StoreModels.Location> locations = dataStore.GetAvailableLocations(product);
            for (int i = 0; i < locations.Count; i++) {
                userInterface.PrintText("[" + i + "] " + locations[i].LocationName + "\t(" + dataStore.GetLocationInventory(locations[i], product) + " in stock)");
            }
            if (locations.Count == 0) {
                userInterface.PrintText("There are no locations to show");
            }
            string input = userInterface.GetLine();
            Console.Clear();
            int index;
            if (!int.TryParse(input, out index))
                return;
            if (index < 0 || index >= locations.Count)
                return;
            cart.Location = locations[index];
        }

        static void ViewCartMenu() {
            if (cart.CheckedOut) {
                userInterface.PrintText("Press enter to return to the main menu");
            } else {
                userInterface.PrintText(managerMenu ? "Enter the number of an item to cancel restocking it" : "Enter the number of an Item to remove it from your cart");
                userInterface.PrintText("Or press enter to return to the main menu");
            }
            if (cart.Location != null) {
                userInterface.PrintText((managerMenu ? "Updating inventory for " : "Ordering from ") + cart.Location.LocationName);
            }
            for (int i = 0; i < cart.Items.Count; i++) {
                userInterface.PrintText("[" + i + "] " + cart.Items[i].Product.ProductName + "\t(" + cart.Items[i].Quantity + " @ " + cart.Items[i].Product.Price.ToString("C") + " each)\t" + cart.Items[i].Total.ToString("C"));
            }
            if (cart.Items.Count == 0) {
                userInterface.PrintText("There are no items in your cart");
            }
            string input = userInterface.GetLine();
            int index;
            Console.Clear();
            if (!int.TryParse(input, out index) || cart.CheckedOut)
                return;
            if (index < 0 || index >= cart.Items.Count)
                return;
            Item item = cart.Items[index];
            dataStore.UpdateLocationInventory(cart.Location, item.Product, item.Quantity);
            cart.Items.RemoveAt(index);
            if (cart.Items.Count == 0) {
                cart.Location = null;
            }
            userInterface.PrintResult("Removed " + item.Quantity + " of " + item.Product.ProductName + " from cart");
        }

        static int CartSelectMenu(List<Order> previousOrders) {
            for (int i = 0; i < previousOrders.Count; i++) {
                userInterface.PrintText("[" + i + "] " + (managerMenu ? previousOrders[i].Customer.Name + "\t" : "") + previousOrders[i].CheckoutTimestamp.ToString() + "\t" + previousOrders[i].Total.ToString("C"));
            }
            if (previousOrders.Count == 0) {
                userInterface.PrintText(managerMenu ? "There don't seem to be any previous orders" : "You don't seem to have placed any previous orders");
            }
            string input = userInterface.GetLine();
            int index;
            Console.Clear();
            if (!int.TryParse(input, out index))
                return -1;
            if (index < 0 || index >= previousOrders.Count)
                return -1;
            return index;
        }
    }
}
