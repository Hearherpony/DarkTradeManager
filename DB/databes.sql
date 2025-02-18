USE master;
GO

EXEC sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO

EXEC sp_configure 'Ad Hoc Distributed Queries', 1;
GO
RECONFIGURE;
GO

IF DB_ID('DarkTradeDB') IS NULL
    CREATE DATABASE [DarkTradeDB];
GO

USE [DarkTradeDB];
GO

IF OBJECT_ID('[UserRole]') IS NULL
BEGIN
    CREATE TABLE [UserRole]
    (
        RoleID INT PRIMARY KEY IDENTITY,
        RoleName NVARCHAR(100) NOT NULL
    );
END
GO

IF OBJECT_ID('[UserEntity]') IS NULL
BEGIN
    CREATE TABLE [UserEntity]
    (
        UserID INT PRIMARY KEY IDENTITY,
        UserSurname NVARCHAR(100) NOT NULL,
        UserName NVARCHAR(100) NOT NULL,
        UserMiddle NVARCHAR(100) NOT NULL,
        UserLogin NVARCHAR(MAX) NOT NULL,
        UserPassword NVARCHAR(MAX) NOT NULL,
        UserRole INT NOT NULL FOREIGN KEY REFERENCES [UserRole](RoleID)
    );
END
GO

IF OBJECT_ID('[Order]') IS NULL
BEGIN
    CREATE TABLE [Order]
    (
        OrderID INT PRIMARY KEY IDENTITY,
        OrderStatus NVARCHAR(MAX) NOT NULL,
        OrderDeliveryDate DATETIME NOT NULL,
        OrderPickupPoint NVARCHAR(MAX) NOT NULL
    );
END
GO

IF OBJECT_ID('[ItemEntity]') IS NULL
BEGIN
    CREATE TABLE [ItemEntity]
    (
        ItemArticle NVARCHAR(100) PRIMARY KEY,
        ItemName NVARCHAR(MAX) NOT NULL,
        ItemDescription NVARCHAR(MAX) NOT NULL,
        ItemCategory NVARCHAR(MAX) NOT NULL,
        ItemImage IMAGE NOT NULL,
        ItemManufacturer NVARCHAR(MAX) NOT NULL,
        ItemCost DECIMAL(19,4) NOT NULL,
        ItemDiscount TINYINT NULL,
        ItemStock INT NOT NULL,
        ItemStatus NVARCHAR(MAX) NOT NULL
    );
END
GO

IF OBJECT_ID('[OrderItem]') IS NULL
BEGIN
    CREATE TABLE [OrderItem]
    (
        OrderID INT NOT NULL FOREIGN KEY REFERENCES [Order](OrderID),
        ItemArticle NVARCHAR(100) NOT NULL FOREIGN KEY REFERENCES [ItemEntity](ItemArticle),
        PRIMARY KEY (OrderID, ItemArticle)
    );
END
GO

IF OBJECT_ID('[Pickups]') IS NULL
BEGIN
    CREATE TABLE [Pickups]
    (
        PickupID INT PRIMARY KEY IDENTITY,
        OrderID INT NOT NULL FOREIGN KEY REFERENCES [Order](OrderID),
        PickupDate DATETIME NOT NULL,
        Address NVARCHAR(255) NOT NULL
    );
END
GO

INSERT INTO [UserRole] (RoleName)
VALUES ('Administrator'), ('Manager'), ('Client');
GO

INSERT INTO [UserEntity] (
    UserSurname,
    UserName,
    UserMiddle,
    UserLogin,
    UserPassword,
    UserRole
)
SELECT
    [UserSurname],
    [UserName],
    [UserMiddle],
    [UserLogin],
    [UserPassword],
    [UserRole]
FROM OPENROWSET(
    'Microsoft.ACE.OLEDB.12.0',
    'Excel 12.0 Xml;HDR=YES;Database=D:/Teh/задание/user_import.xlsx',
    'SELECT [UserSurname],[UserName],[UserMiddle],[UserLogin],[UserPassword],[UserRole] FROM [Sheet1$]'
) AS t;
GO

ALTER TABLE [ItemEntity] ALTER COLUMN ItemImage IMAGE NULL;
GO
INSERT INTO [ItemEntity] (
    ItemArticle,
    ItemName,
    ItemDescription,
    ItemCategory,
    ItemImage,
    ItemManufacturer,
    ItemCost,
    ItemDiscount,
    ItemStock,
    ItemStatus
)
SELECT
    [ItemArticle],
    [ItemName],
    [ItemDescription],
    [ItemCategory],
    NULL, 
    [ItemManufacturer],
    [ItemCost],
    [ItemDiscount],
    [ItemStock],
    [ItemStatus]
FROM OPENROWSET(
    'Microsoft.ACE.OLEDB.12.0',
    'Excel 12.0 Xml;HDR=YES;Database=D:/Teh/задание/products_import.xlsx',
    'SELECT [ItemArticle],[ItemName],[ItemDescription],[ItemCategory],[ItemManufacturer],[ItemCost],[ItemDiscount],[ItemStock],[ItemStatus] FROM [Sheet1$]'
) AS t;
GO

INSERT INTO [Order] (
    OrderStatus,
    OrderDeliveryDate,
    OrderPickupPoint
)
SELECT
    [OrderStatus],
    [OrderDeliveryDate],
    [OrderPickupPoint]
FROM OPENROWSET(
    'Microsoft.ACE.OLEDB.12.0',
    'Excel 12.0 Xml;HDR=YES;Database=D:/Teh/задание/order_import.xlsx',
    'SELECT [OrderStatus],[OrderDeliveryDate],[OrderPickupPoint] FROM [Sheet1$]'
) AS t;
GO

INSERT INTO [Pickups] (
    OrderID,
    PickupDate,
    Address
)
SELECT
    [OrderID],
    [PickupDate],
    [Address]
FROM OPENROWSET(
    'Microsoft.ACE.OLEDB.12.0',
    'Excel 12.0 Xml;HDR=YES;Database=D:/Teh/задание/pickups_import.xlsx',
    'SELECT [OrderID],[PickupDate],[Address] FROM [Sheet1$]'
) AS t;
GO

INSERT INTO [OrderItem] (
    OrderID,
    ItemArticle
)
SELECT
    [OrderID],
    [ItemArticle]
FROM OPENROWSET(
    'Microsoft.ACE.OLEDB.12.0',
    'Excel 12.0 Xml;HDR=YES;Database=D:/Teh/задание/order_items_import.xlsx',
    'SELECT [OrderID],[ItemArticle] FROM [Sheet1$]'
) AS t;
GO
