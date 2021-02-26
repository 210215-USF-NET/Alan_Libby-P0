--insert into Product(productName, productPrice) values ('Product 1', 1.99), ('Product 2', 2.99), ('Product 3', 3.99), ('Product 4', 4.99), ('Product 5', 5.99);
--select * from Product;

--insert into Location(locationName, locationAddress) values ('TestLocationOdd', '135 Odd Way'), ('TestLocationEven', '246 Even Road'), ('TestLocationAll', '123 Number Street');
--select * from Location;

--insert into Inventory(locationID, productID, quantity) values (1,1,150), (3,1,100), (2,2,250), (3,2,100), (1,3,350), (3,3,100), (2,4,450), (3,4,100), (1,5,550), (3,5,100);
--select productName, locationName, quantity from Inventory join Product on Product.productID = Inventory.productID join Location on Location.locationID = Inventory.locationID ORDER BY productName;

--select * from StoreOrder;