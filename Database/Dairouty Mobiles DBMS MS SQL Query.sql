--=================Dairouty Mobiles DBMS=================--
--1) DB Creation :
Create DataBase Dairouty_Mobiles
Use Dairouty_Mobiles
-----------------------------------------------------------
--2) Tables : 
Create Table Inventory.Products 
(	
	Product_ID nvarchar(30) Primary Key,
	Category nvarchar(20),
	Brand nvarchar(20),
	Model nvarchar(20),
	Color nvarchar(20),
	RAM_GB tinyint,
	ROM_GB int,
	Price_EGP decimal(10,2) Not Null,
	Total_Stock int Not Null
)
Create Table Branches 
(	
	Branch_ID Int Primary Key,
	Branch_Name nvarchar(20) DEFAULT 'Dairouty', 
	Location nvarchar(30),
	Landline varchar(9),
	Manager_ID Nvarchar(20)
)
Create Table System_Accounts 
(	
	Acc_ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Username nvarchar(15) Not Null,
    Password nvarchar(15) NOT NULL,
    ProfilePicture Varbinary(max)

)
Create Table Employees.Sellers 
(	
	Seller_ID AS ('S' + CAST(Seller_Num AS NVARCHAR(19))) PERSISTED PRIMARY KEY,
	Seller_Num INT IDENTITY(1,1), 
	Name nvarchar(20) NOT NULL,
	Phone_Num varchar(11) Default 'Unknown',
	Email nvarchar(50) NOT NULL,
	Birth_Date Date,
	Hiring_Date Date Default GetDate(),
	Salary_EGP Decimal(10,2),
	Vacation_Day nvarchar(20) Default 'None' ,
	Branch_ID Int References Branches(Branch_ID),
	Acc_ID UNIQUEIDENTIFIER References System_Accounts(Acc_ID),
    ProfilePicture Varbinary(max)

)
Create Table Employees.Admins 
(	
	Admin_ID AS ('A' + CAST(Admin_Num AS NVARCHAR(19))) PERSISTED PRIMARY KEY,
	Admin_Num INT IDENTITY(1,1), 
	Fname nvarchar(15),
	Lname nvarchar(15),
	Phone_Num varchar(11) Default 'Unknown',
	Email nvarchar(50) NOT NULL,
	Acc_ID UNIQUEIDENTIFIER References System_Accounts(Acc_ID)
)
Create Table Customers 
(	
	Cust_ID AS ('C' + CAST(Cust_Num AS NVARCHAR(19))) PERSISTED PRIMARY KEY,
	Cust_Num INT IDENTITY(1,1), 
	Name nvarchar(15) Not Null,
	Phone_Num varchar(11)
)
Create Table Customer_Addresses
(	
	Cust_ID nvarchar(20) References Customers(Cust_ID),
	Address nvarchar(30),
	Primary Key (Cust_ID,Address)
)
Create Table Sales.Invoices
(	
	Invoice_ID int Primary Key,
	Date Date Default GetDate(),
	Payment_Method nvarchar(20),
	Cust_ID nvarchar(20) References Customers(Cust_ID),
	Branch_ID int References Branches(Branch_ID),
	Seller_ID nvarchar(20) References Employees.Sellers(Seller_ID)
)
Create Table Sales.Inv_Line_Items
(	
	Invoice_ID int References Sales.Invoices(Invoice_ID),
	Line_Item_ID int IDENTITY(1,1),
	Product_ID nvarchar(30) References Inventory.Products(Product_ID),
	Unit_Price decimal(10,2),
	Quantity int 
	Primary Key (Invoice_ID,Line_Item_ID)
)
Create Table Inventory.Branches_Stock
(	
	Branch_ID int References Branches(Branch_ID),
	Product_ID nvarchar(30) References Inventory.Products(Product_ID),
	Quantity int
	Primary Key (Branch_ID,Product_ID)
)
-----------------------------------------------------------
--3) Altering :
Alter Table Branches
Add Foreign Key (Manager_ID) References Employees.Sellers(Seller_ID)
-----------------------------------------------------------
--4) Constraints :
ALTER TABLE Inventory.Products
ADD CONSTRAINT CHK_Products_Price CHECK (Price_EGP > 0)

ALTER TABLE Sales.Inv_Line_Items
ADD CONSTRAINT CHK_Line_Item_Quantity CHECK (Quantity > 0)

ALTER TABLE Inventory.Branches_Stock
ADD CONSTRAINT CHK_Branch_Stock CHECK (Quantity >= 0)

ALTER TABLE Employees.Sellers
ADD CONSTRAINT CHK_Salary CHECK (Salary_EGP > 0)
-----------------------------------------------------------
--5) Indexes :
CREATE INDEX idx_invoices_cust_id ON Sales.Invoices (Cust_ID)
CREATE INDEX idx_invoices_branch_id ON Sales.Invoices (Branch_ID)
CREATE INDEX idx_invoices_seller_id ON Sales.Invoices (Seller_ID)

CREATE INDEX idx_inv_line_items_invoice_id ON Sales.Inv_Line_Items (Invoice_ID)
CREATE INDEX idx_inv_line_items_product_id ON Sales.Inv_Line_Items (Product_ID)

CREATE INDEX idx_branches_stock_branch_id ON Inventory.Branches_Stock (Branch_ID)
CREATE INDEX idx_branches_stock_product_id ON Inventory.Branches_Stock (Product_ID)

CREATE INDEX idx_customer_addresses_cust_id ON Customer_Addresses (Cust_ID)
-----------------------------------------------------------
--6) Triggers :
CREATE OR ALTER TRIGGER Insert_Branch_Name
ON Branches
AFTER INSERT
AS
BEGIN
    UPDATE Branches
    SET Branch_Name = 'Dairouty ' + CAST(Branch_ID AS NVARCHAR)
    WHERE Branch_ID IN (SELECT Branch_ID FROM INSERTED) AND Branch_Name = 'Dairouty'
END


CREATE OR ALTER TRIGGER ManageBranchStock
ON Sales.Inv_Line_Items
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	IF NOT EXISTS
	(
		SELECT 1
		FROM Inventory.Branches_Stock b INNER JOIN Inserted i ON b.Branch_ID = (SELECT Branch_ID FROM  Sales.Invoices WHERE Invoice_ID = i.Invoice_ID) AND b.Product_ID = i.Product_ID
	)
	BEGIN
	    RAISERROR ('Product not available in the branch stock.', 16, 1) 
		ROLLBACK TRANSACTION
   	    RETURN
	END
    --INSERT
    UPDATE Inventory.Branches_Stock
    SET Quantity -= i.Quantity
    FROM  Inventory.Branches_Stock b INNER JOIN Inserted i ON b.Branch_ID = (SELECT Branch_ID FROM  Sales.Invoices WHERE Invoice_ID = i.Invoice_ID) AND b.Product_ID = i.Product_ID
    --DELETE
    UPDATE  Inventory.Branches_Stock
    SET Quantity += d.Quantity
    FROM  Inventory.Branches_Stock b INNER JOIN Deleted d ON b.Branch_ID = (SELECT Branch_ID FROM  Sales.Invoices WHERE Invoice_ID = d.Invoice_ID) AND b.Product_ID = d.Product_ID
    -- UPDATE
    UPDATE  Inventory.Branches_Stock
    SET Quantity = b.Quantity + d.Quantity - i.Quantity
    FROM  Inventory.Branches_Stock b 
	INNER JOIN Inserted i ON b.Branch_ID = (SELECT Branch_ID FROM  Sales.Invoices WHERE Invoice_ID = i.Invoice_ID) AND b.Product_ID = i.Product_ID
    INNER JOIN Deleted d ON b.Branch_ID = (SELECT Branch_ID FROM  Sales.Invoices WHERE Invoice_ID = d.Invoice_ID) AND b.Product_ID = d.Product_ID
    -- Validate Stock 
    IF EXISTS
	(
        SELECT 1
        FROM  Inventory.Branches_Stock
        WHERE Quantity < 0
    )
    BEGIN
        RAISERROR ('Out of Stock', 16, 1)
        ROLLBACK TRANSACTION
        RETURN
    END
END


CREATE OR ALTER TRIGGER UpdateProductTotalStock
ON  Inventory.Branches_Stock
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    UPDATE Inventory.Products
    SET Total_Stock = (
        SELECT SUM(Quantity)
        FROM  Inventory.Branches_Stock
        WHERE Product_ID = Inventory.Products.Product_ID
    )
    WHERE Product_ID IN (
        SELECT DISTINCT Product_ID FROM Inserted
        UNION
        SELECT DISTINCT Product_ID FROM Deleted
    )
END
-----------------------------------------------------------
--7) Procedure :
CREATE OR ALTER PROCEDURE Create_Account
    @Username NVARCHAR(15), 
    @Password NVARCHAR(15)      
AS
BEGIN
    IF EXISTS (SELECT 1 FROM System_Accounts WHERE Username = @Username)
    BEGIN
        PRINT 'Error: Username already exists.'
        RETURN
    END

    INSERT INTO System_Accounts (Username, Password)
    VALUES (@Username, @Password)
    PRINT 'Account created successfully!'
END


CREATE TYPE dbo.LineItemType AS TABLE
(
    Product_ID NVARCHAR(30),
    Unit_Price DECIMAL(10, 2),
    Quantity INT
)
CREATE OR ALTER PROCEDURE Create_New_Inv
    @CustomerName NVARCHAR(15),
    @CustomerPhone VARCHAR(11),
    @PaymentMethod NVARCHAR(20),
    @SellerID NVARCHAR(20),
    @BranchID INT,
    @LineItems dbo.LineItemType READONLY
AS
BEGIN
    BEGIN TRANSACTION
    DECLARE @CustomerID NVARCHAR(20)
    DECLARE @InvoiceID INT

    -- Customer Insertion or Retrieval
    IF NOT EXISTS (SELECT 1 FROM Customers WHERE Phone_Num = @CustomerPhone AND Name = @CustomerName)
    BEGIN
        INSERT INTO Customers (Name, Phone_Num) VALUES (@CustomerName, @CustomerPhone)
		SET @CustomerID = 'C' + CAST(SCOPE_IDENTITY() AS NVARCHAR(19));
    END
    ELSE
    BEGIN
        SET @CustomerID = (SELECT Cust_ID FROM Customers WHERE Phone_Num = @CustomerPhone AND Name = @CustomerName)
    END

    -- Invoice Insertion
    SET @InvoiceID = (SELECT ISNULL(MAX(Invoice_ID),0) + 1 FROM Sales.Invoices)  -- Calculate the next Invoice_ID
    INSERT INTO Sales.Invoices (Invoice_ID, Date, Payment_Method, Cust_ID, Branch_ID, Seller_ID)
    VALUES (@InvoiceID, GETDATE(), @PaymentMethod, @CustomerID, @BranchID, @SellerID)

    -- Line Items Insertion
    INSERT INTO Sales.Inv_Line_Items (Invoice_ID, Product_ID, Unit_Price, Quantity)
    SELECT @InvoiceID, Product_ID, Unit_Price, Quantity
    FROM @LineItems

    COMMIT TRANSACTION
END
-----------------------------------------------------------
--8) Functions : 
--1. Scalar 
CREATE OR ALTER FUNCTION dbo.CalculateLineTotalPrice (@Line_Item_ID int)
RETURNS DECIMAL(10, 2)
Begin
		DECLARE @Line_Total_Price DECIMAL(10, 2)
        SELECT @Line_Total_Price = Quantity * Unit_Price
        FROM Sales.Inv_Line_Items
        WHERE Line_Item_ID = @Line_Item_ID
		Return @Line_Total_Price
End
CREATE OR ALTER FUNCTION dbo.CalculateInvTotalAmmount (@Invoice_ID int)
RETURNS DECIMAL(10, 2)
Begin
		DECLARE @Inv_Total_Ammount DECIMAL(10, 2)
        SELECT @Inv_Total_Ammount = SUM(DBO.CalculateLineTotalPrice(Line_Item_ID))
        FROM  Sales.Inv_Line_Items
        WHERE Invoice_ID = @Invoice_ID
		Return @Inv_Total_Ammount
End
--2. MultiStatment
Create or Alter Function GetAdminNamePassedFormat (@Format Varchar(20))
Returns @T Table (AdminName varchar(20)) 
as
	Begin
		if (@Format = 'First') 
			Insert into @T
			SELECT ISNULL(Fname, 'Unknown')
			From Employees.Admins
		ELSE if (@Format = 'Lname') 
			Insert into @T
			SELECT ISNULL(Lname, 'Unknown')
			From Employees.Admins
		ELSE if (@Format = 'Full')
			Insert into @T
			SELECT ISNULL(Fname, '') + ' ' + ISNULL(Lname, '')
			From Employees.Admins
		Return 
	End
----------------------------------------------------------
--9) Inserting :
--1.Branches 
Insert into Branches(Branch_ID,Location,Landline) Values (1,N'شارع مصطفي كامل غبريال','035744855'),(2,N'شارع مصطفي كامل غبريال','035744855'),(3,N'شارع مصطفي كامل غبريال','035744855'),(6,N'شارع مصطفي كامل غبريال','035744855')
Insert into Branches(Branch_ID,Branch_Name,Location,Landline) Values (4,'Dairouty Smouha',N'سموحة امام مول زهران','034292384'),(5,'Dairouty 45',N'العاصفرة شارع 45','035350569')

--2.Products
Insert into Inventory.Products Values
(1,'Mobile','Samsung','Galaxy A05s','Black',4,128,5500.00,20),(2,'Mobile','Samsung','Galaxy A05s','Silver',4,128,5500.00,10),(3,'Mobile','Samsung','Galaxy A05s','Black',6,128,6000.00,15),(4,'Mobile','Samsung','Galaxy A15','Blue Black',4,128,6350.00,5),
(5,'Mobile','Samsung','Galaxy A15','Light Blue',6,128,7000.00,10),(6,'Mobile','Samsung','Galaxy A15','Black',8,256,8500.00,25),(7,'Mobile','Samsung','Galaxy A25','Black',8,256,10600.00,17),(8,'Mobile','Samsung','Galaxy A35','Lemon',8,128,12800.00,10),
(9,'Mobile','Samsung','Galaxy A35','Lilac',8,128,12800.00,5),(10,'Mobile','Samsung','Galaxy A35','Navy',8,256,14300.00,30),(11,'Mobile','Samsung','Galaxy A55','Ice Blue',8,128,17500.00,0),(12,'Mobile','Samsung','Galaxy A55','Lemon',8,256,18900.00,12),
(13,'Mobile','Samsung','Galaxy S 24 Ultra','Black',12,256,52000.00,20),(14,'Mobile','Samsung','Galaxy S 24 Ultra','Gray',12,256,52000.00,14),(15,'Mobile','Xiaomi','A2+','Black',3,64,4700.00,10),(16,'Mobile','Xiaomi','A3','Green',3,64,4300.00,5),
(17,'Mobile','Xiaomi','A3','Black',4,128,4850.00,15),(18,'Mobile','Xiaomi','Redmi 13c','White',4,128,5350.00,3),(19,'Mobile','Xiaomi','Redmi 13c','Blue',6,128,5800.00,10),(20,'Mobile','Xiaomi','Redmi 13c','Black',8,256,6600.00,13),
(21,'Mobile','Xiaomi','Redmi 12','Black',8,128,7100.00,8),(22,'Mobile','Xiaomi','Redmi 12','Sky Blue',8,256,7700.00,18),(23,'Mobile','Xiaomi','Redmi Note 12','Black',6,128,7600.00,10),(24,'Mobile','Xiaomi','Redmi Note 12','Black',8,128,8250.00,30),
(25,'Mobile','Xiaomi','Redmi Note 13','Green',6,128,8450.00,4),(26,'Mobile','Xiaomi','Redmi	Note 13','Blue',8,128,9000.00,18),(27,'Mobile','Xiaomi','Redmi Note 12','Black',8,256,9700.00,35),(28,'Mobile','Realme','Note 50','Black',4,128,4500.00,10),
(29,'Mobile','Realme','C65','Blue',8,256,7000.00,14),(30,'Mobile','Realme','R12 Pro 5G','Black',12,512,16200.00,0),(31,'Mobile','Realme','R12 Pro Plus 5G','Black',12,512,19200.00,11),(32,'Mobile','Oppo','Reno 11 F','Green',8,256,12750.00,14),
(33,'Mobile','Oppo','Reno 12 F','Orange',8,256,12900.00,18),(34,'Mobile','Oppo','Reno 11 5G','Black',12,256,16100.00,10),(35,'Mobile','Vivo','Y02T','Black',4,128,5500.00,6),(36,'Mobile','Infinix','Smart 8','Black',4,128,4350.00,1),
(37,'Mobile','Nokia','C10','Blue',2,32,2500.00,3),(38,'Mobile','Itel','A70','Black',8,256,4400.00,6),(39,'Mobile','Honor','X5 Plus','Green',4,64,5000.00,16),(40,'Accessories','Realme','Buds Air 3',NULL,NULL,NULL,2000.00,5),
(41,'Accessories','Realme','Watch 2',NULL,NULL,NULL,1200.00,5),(42,'Accessories','Realme','Watch R100',NULL,NULL,NULL,1600.00,4),(43,'Mobile','Iphone','Iphone 16 Pro Max','Desert Titanium',8,256,77000.00,0),(44,'Mobile','Iphone','Iphone 16 Pro Max','Desert Titanium',8,1000,100000.00,0)

--3.Branches_Stock
INSERT INTO Inventory.Branches_Stock VALUES
(1, '1', 5), (2, '1', 8), (3, '1', 7),(1, '2', 4), (4, '2', 6),(2, '3', 5), (3, '3', 5), (5, '3', 5),(6, '4', 5),(1, '5', 3), (2, '5', 2), (4, '5', 5),(1, '6', 10), (3, '6', 10), (5, '6', 5),(2, '7', 7), (6, '7', 10),(1, '8', 4), (5, '8', 6),(3, '9', 5),(4, '10', 10), (5, '10', 10), 
(6, '10', 10),(2, '12', 6), (4, '12', 6),(1, '13', 10), (6, '13', 10),(3, '14', 7), (4, '14', 7),(5, '15', 10),(6, '16', 5),(1, '17', 5), (3, '17', 5), (6, '17', 5),(2, '18', 3),(4, '19', 6), (5, '19', 4),(1, '20', 5), (3, '20', 8),(2, '21', 4), (4, '21', 4),(1, '22', 8), (6, '22', 10),
(3, '23', 10),(2, '24', 10), (5, '24', 10), (6, '24', 10),(1, '25', 4),(4, '26', 9), (6, '26', 9),(2, '27', 15), (3, '27', 10), (5, '27', 10),(4, '28', 10),(3, '29', 7), (6, '29', 7),(2, '31', 5), (4, '31', 6),(1, '32', 7), (5, '32', 7),(3, '33', 9), (6, '33', 9),(2, '34', 10),(1, '35', 6),
(4, '36', 1),(3, '37', 3),(5, '38', 6),(1, '39', 8), (6, '39', 8),(2, '40', 5),(3, '41', 5),(4, '42', 4)

--4.Customers
INSERT INTO Customers VALUES ('Ziad Magdi','01274351083'),('Omar Yakout','01009964699'),('Mohamed Ehab','01064788006'),('Youssef Waleed','01060538911'),('Saged Khaled','01206711122'),('Mohamed Osama','01200206538')

--5.Customer_Addresses
INSERT INTO Customer_Addresses VALUES ('C1','Janaklees'),('C2','North Coast'),('C2','Saba Basha'),('C3','North Coast'),('C3','Sidi Bishr'),('C4','Miami'),('C4','North Coast'),('C7','Roushdy'),('C8','Fawzy Moaaz')

--6.System_Accounts
EXEC Create_Account @Username = 'abdallah' ,@Password = '18102012'
EXEC Create_Account @Username = 'Dairo' ,@Password = '2828'
EXEC Create_Account @Username = 'ali' ,@Password = '2008'

--7.Admins
INSERT INTO Employees.Admins VALUES
('Abdallah','Eldairouty','0122138937','eldairoutysat@yahoo.com','e3d5484b-f506-4bd0-be38-5658fcd863aa')

--8.Sellers
INSERT INTO Employees.Sellers VALUES
('Mohamed Eldairouty','01277669139','mohamedeldairouty554@gmail.com','07-21-2005',getdate(),12000.00,'Saturday',2,'a2ac43e8-1791-4be3-a18f-8cc7534874d7') ,
('Ali','01211361000','alieldairouty23@gmail.com','02-12-2008',getdate(),9000.00,'Friday',4,'5dbc23c0-150d-40f3-a7ff-6ee5f9281c3b')

--9.Invoices
INSERT INTO Sales.Invoices VALUES
(1,Getdate(),'Instapay','C1',4,'S2'),(2,'03-10-2024','Credit Card','C2',2,'S1'),(3,'10-18-2024','Cash','C3',2,'S1'),(4,Getdate(),'Cash','C4',4,'S2')

--10.Inv_Line_Items
INSERT INTO Sales.Inv_Line_Items VALUES
(1,14,52250.00,1),(2,12,18900.00,1),(2,40,2000.00,1),(3,5,7000.00,1),(4,5,7100,2)

--Testing Proc 
DECLARE @LineItems dbo.LineItemType
INSERT INTO @LineItems VALUES (1, 13000.00, 1)
EXEC Create_New_Inv 
    @CustomerName = 'Malak Amr',
    @CustomerPhone = '01066222726',
    @PaymentMethod = 'Cash',
    @SellerID = 'S2',
    @BranchID = 2,
    @LineItems = @LineItems
-----------------------------------------------------------
--10) Updating :
UPDATE Branches
SET Manager_ID = 'S1'
WHERE Branch_ID = 2
UPDATE Branches
SET Manager_ID = 'S2'
WHERE Branch_ID = 4
-----------------------------------------------------------
--11) DQL :
--1. Display Invoices Total Ammount DESC
SELECT Invoice_ID, dbo.CalculateInvTotalAmmount(Invoice_ID) as Total_Ammount
FROM Sales.Invoices i
ORDER BY Total_Ammount DESC

--2. Total Revenue for Each Branch 
SELECT B.Branch_Name,SUM(L.Unit_Price * L.Quantity) as Total_Revenue
FROM Branches B, Sales.Inv_Line_Items L, Sales.Invoices I
WHERE B.Branch_ID=I.Branch_ID AND I.Invoice_ID = L.Invoice_ID
GROUP BY B.Branch_Name
ORDER BY Total_Revenue DESC

--3. Rank Top Selling Products 
SELECT P.Product_ID,P.Brand,P.Model,SUM(L.Quantity) as Total_Quantity_Sold, 
RANK () OVER (ORDER BY SUM(L.Quantity) DESC) as [Rank]
FROM Inventory.Products P , Sales.Inv_Line_Items L
WHERE P.Product_ID = L.Product_ID
GROUP BY P.Product_ID,p.Brand,P.Model
ORDER BY [Rank]

--4. Low Stock Products in Each Products
SELECT B.Branch_Name,P.Product_ID,P.Brand,P.Model, S.Quantity as Stock
FROM Branches B , Inventory.Products P , Inventory.Branches_Stock S
WHERE B.Branch_ID = S.Branch_ID AND P.Product_ID = S.Product_ID AND S.Quantity < 5
ORDER BY Stock ASC

--5. TOP Seller
SELECT TOP 1 S.Name, COUNT(DISTINCT I.Invoice_ID) AS Total_Invoices , SUM(L.Unit_Price * L.Quantity) AS Total_Amount
FROM Employees.Sellers s , Sales.Invoices I , Sales.Inv_Line_Items L
WHERE I.Seller_ID = S.Seller_ID AND L.Invoice_ID = I.Invoice_ID
GROUP BY S.Name
-----------------------------------------------------------
--12) Schemas :
CREATE SCHEMA Sales
ALTER SCHEMA Sales TRANSFER dbo.Invoices
ALTER SCHEMA Sales TRANSFER dbo.Inv_Line_Items

CREATE SCHEMA Inventory
ALTER SCHEMA Inventory TRANSFER dbo.Products
ALTER SCHEMA Inventory TRANSFER dbo.Branches_Stock

CREATE SCHEMA Employees
ALTER SCHEMA Employees TRANSFER dbo.Sellers
ALTER SCHEMA Employees TRANSFER dbo.Admins
-----------------------------------------------------------
--13) Views :
CREATE VIEW Customer_Purchase_Insights
WITH ENCRYPTION
AS
	SELECT c.Name AS Customer_Name, c.Phone_Num AS Customer_Phone, COUNT(i.Invoice_ID) AS Total_Purchases, SUM(ili.Quantity * ili.Unit_Price) AS Total_Spent
	FROM Customers c JOIN Sales.Invoices i ON c.Cust_ID = i.Cust_ID JOIN Sales.Inv_Line_Items ili ON i.Invoice_ID = ili.Invoice_ID
	GROUP BY c.Name, c.Phone_Num

CREATE VIEW Monthly_Revenue_By_Branch
WITH ENCRYPTION
AS
	SELECT FORMAT(i.Date, 'yyyy-MM') AS Revenue_Month,b.Branch_Name,SUM(ili.Quantity * ili.Unit_Price) AS Total_Revenue
	FROM Sales.Invoices i JOIN Sales.Inv_Line_Items ili ON i.Invoice_ID = ili.Invoice_ID JOIN Branches b ON i.Branch_ID = b.Branch_ID
	GROUP BY FORMAT(i.Date, 'yyyy-MM'), b.Branch_Name
-----------------------------------------------------------
--14) Testing Views :
SELECT *
FROM Customer_Purchase_Insights
ORDER BY Total_Spent DESC

SELECT *
FROM Monthly_Revenue_By_Branch
ORDER BY Revenue_Month,Total_Revenue DESC

