-- CREATE DATABASE P0_StoreApp;
/*
DROP TABLE Customer;
DROP TABLE StoreInventory;
DROP TABLE CustomerOrder;
DROP TABLE OrderProduct;
*/
-- create tables
CREATE TABLE Customer(
    ID INT NOT NULL IDENTITY(100, 1) PRIMARY KEY,
    FirstName NVARCHAR(16) NOT NULL,
    LastName NVARCHAR(16) NOT NULL
);

CREATE TABLE CustomerOrder(
    OrderNum INT NOT NULL IDENTITY PRIMARY KEY,
    CustomerID INT NOT NULL     --FK
);

CREATE TABLE Location(
    ID INT NOT NULL IDENTITY PRIMARY KEY,
    StoreLocation NVARCHAR(168) NOT NULL 
);

CREATE TABLE OrderProduct(
    OrderNum INT NOT NULL,                 --PK + FK
    ProductName NVARCHAR(30) NOT NULL,     --PK
    Amount INT NOT NULL,        -- need CHECK Constrain will added later
    LocationID INT NOT NULL,    -- FK
    OrderTime DATETIMEOFFSET NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
    PRIMARY KEY (OrderNum, ProductName)
);

--DROP TABLE StoreInventory
CREATE TABLE StoreInventory(
    LocationID INT NOT NULL,   --PK + FK
    ProductName NVARCHAR(30) NOT NULL,      --PK
    Price DECIMAL(9, 2) NOT NULL,
    ProductAmount INT NOT NULL,
    PRIMARY KEY (LocationID, ProductName)
);


-- Add foreign key constrains
ALTER TABLE CustomerOrder ADD CONSTRAINT FK_Customer_ID 
    FOREIGN KEY (CustomerID) REFERENCES Customer(ID);

ALTER TABLE OrderProduct ADD CONSTRAINT FK_Order_Num 
    FOREIGN KEY (OrderNum) REFERENCES CustomerOrder(OrderNum);

ALTER TABLE OrderProduct ADD CONSTRAINT FK_Order_LocationID
    FOREIGN KEY (LocationID) REFERENCES Location(ID);
    
ALTER TABLE StoreInventory ADD CONSTRAINT FK_Inventory_LocationID 
    FOREIGN KEY (LocationID) REFERENCES Location(ID);

SELECT * FROM Location;
SELECT * FROM StoreInventory;
-- Insert Location
INSERT Location VALUES ('1164 44th St, Brooklyn, NY 11219');


-- Insert Inventory
-- delete from StoreInventory where LocationID=1;
INSERT StoreInventory VALUES 
(1, 'Glass Cup', 3.90, 100),
(1, 'Rice Cooker', 149.00, 30),
(1, 'White Porcelain Spoon', 6.90, 100),
(1, 'Double Ringed Plain Notebook', 1.90, 200),
(1, 'Standard File Box', 10.90, 100),
(1, 'Handy Shredder', 14.90, 90);
