DROP TABLE IF EXISTS OrderItem;
DROP TABLE IF EXISTS Inventory;
DROP TABLE IF EXISTS StoreOrder;
DROP TABLE IF EXISTS Product;
DROP TABLE IF EXISTS Location;
DROP TABLE IF EXISTS Customer;

CREATE TABLE Customer (
	customerID int IDENTITY,
	customerName nvarchar(100) NOT NULL,
	CONSTRAINT PK_Customer PRIMARY KEY (customerID)
);
CREATE TABLE Location (
	locationID int IDENTITY,
	locationName nvarchar(100) NOT NULL,
	locationAddress nvarchar(100) NOT NULL,
	CONSTRAINT PK_Location PRIMARY KEY (locationID)
);
CREATE TABLE Product (
	productID int IDENTITY,
	productName nvarchar(100) NOT NULL,
	productPrice decimal(10, 2) NOT NULL,
	CONSTRAINT PK_Product PRIMARY KEY (productID)
)
CREATE TABLE StoreOrder (
	orderID int IDENTITY,
	customerID int references Customer(customerID) NOT NULL,
	locationID int references Location(locationID),
	checkedOut datetime2(3),
	CONSTRAINT PK_Order PRIMARY KEY (orderID)
)
CREATE TABLE OrderItem (
	orderID int NOT NULL REFERENCES StoreOrder(orderID),
	productID int NOT NULL REFERENCES Product(productID),
	quantity int NOT NULL,
	CONSTRAINT PK_OrderItem PRIMARY KEY (orderID, productID)
)
CREATE TABLE Inventory (
	locationID int NOT NULL REFERENCES Location(locationID),
	productID int NOT NULL REFERENCES Product(productID),
	quantity int NOT NULL,
	CONSTRAINT PK_Inventory PRIMARY KEY (locationID, productID)
)