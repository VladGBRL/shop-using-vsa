USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'QuestDb')
BEGIN
    ALTER DATABASE QuestDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuestDb;
END
GO
 
CREATE DATABASE QuestDb;
GO
 
USE QuestDb;
GO
CREATE TABLE Users (
    Id           INT            IDENTITY(1,1) PRIMARY KEY,
    Username     NVARCHAR(100)  NOT NULL,
    Email        NVARCHAR(200)  NOT NULL,
    PasswordHash NVARCHAR(500)  NOT NULL,
    CreatedAt    DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
 
    CONSTRAINT UQ_Users_Email UNIQUE (Email)
);
GO
CREATE TABLE Categories (
    Id   INT           IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
 
    CONSTRAINT UQ_Categories_Name UNIQUE (Name)
);
GO
CREATE TABLE Products (
    Id          INT             IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(200)   NOT NULL,
    Description NVARCHAR(1000)  NULL,
    Price       DECIMAL(10, 2)  NOT NULL,
    Stock       INT             NOT NULL DEFAULT 0,
    ImageUrl    NVARCHAR(500)   NULL,
    CategoryId  INT             NOT NULL,
    CreatedAt   DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
 
    CONSTRAINT FK_Products_Category FOREIGN KEY (CategoryId)
        REFERENCES Categories(Id),
    CONSTRAINT CHK_Products_Price CHECK (Price >= 0),
    CONSTRAINT CHK_Products_Stock CHECK (Stock >= 0)
);
GO
CREATE TABLE Orders (
    Id              INT             IDENTITY(1,1) PRIMARY KEY,
    UserId          INT             NOT NULL,
    TotalPrice      DECIMAL(10, 2)  NOT NULL,
    ShippingAddress NVARCHAR(500)   NOT NULL,
    Status          NVARCHAR(50)    NOT NULL DEFAULT 'Pending',
    CreatedAt       DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
 
    CONSTRAINT FK_Orders_User   FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT CHK_Orders_Total CHECK (TotalPrice >= 0),
    CONSTRAINT CHK_Orders_Status CHECK (Status IN ('Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled'))
);
GO
CREATE TABLE OrderItems (
    Id          INT             IDENTITY(1,1) PRIMARY KEY,
    OrderId     INT             NOT NULL,
    ProductId   INT             NOT NULL,
    Quantity    INT             NOT NULL DEFAULT 1,
    Price       DECIMAL(10, 2)  NOT NULL, -- Price at the time of order
 
    CONSTRAINT FK_OrderItems_Order   FOREIGN KEY (OrderId)   REFERENCES Orders(Id),
    CONSTRAINT FK_OrderItems_Product FOREIGN KEY (ProductId) REFERENCES Products(Id),
    CONSTRAINT CHK_OrderItems_Quantity CHECK (Quantity > 0),
    CONSTRAINT CHK_OrderItems_Price  CHECK (Price >= 0)
);
GO
CREATE TABLE CartItems (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    UserId    INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity  INT NOT NULL DEFAULT 1,
    AddedAt   DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
 
    CONSTRAINT FK_CartItems_User    FOREIGN KEY (UserId)    REFERENCES Users(Id),
    CONSTRAINT FK_CartItems_Product FOREIGN KEY (ProductId) REFERENCES Products(Id),
    CONSTRAINT UQ_CartItems_UserProduct UNIQUE (UserId, ProductId),
    CONSTRAINT CHK_CartItems_Qty    CHECK (Quantity > 0)
);
GO
CREATE INDEX IX_Products_CategoryId  ON Products(CategoryId);
CREATE INDEX IX_Orders_UserId        ON Orders(UserId);
CREATE INDEX IX_OrderItems_OrderId   ON OrderItems(OrderId);
CREATE INDEX IX_CartItems_UserId     ON CartItems(UserId);
GO
-- Insert Categories
INSERT INTO Categories (Name) VALUES
('Electronics'),
('Books'),
('Clothing'),
('Home & Kitchen'),
('Sports');
GO

-- Electronics (CategoryId = 1)
INSERT INTO Products (Name, Description, Price, Stock, CategoryId) VALUES
('Smartphone X1', 'Latest smartphone with OLED display', 799.99, 50, 1),
('Laptop Pro 15', 'High-performance laptop', 1299.99, 30, 1),
('Wireless Earbuds', 'Noise cancelling earbuds', 149.99, 100, 1),
('Smartwatch S3', 'Fitness tracking smartwatch', 199.99, 70, 1),
('Bluetooth Speaker', 'Portable speaker', 89.99, 120, 1),
('Gaming Mouse', 'RGB gaming mouse', 49.99, 200, 1),
('Mechanical Keyboard', 'Backlit keyboard', 99.99, 150, 1),
('4K Monitor', 'Ultra HD monitor', 399.99, 40, 1),
('External SSD 1TB', 'Fast storage device', 159.99, 80, 1),
('Webcam HD', '1080p webcam', 69.99, 90, 1),
('Tablet T10', '10-inch tablet', 299.99, 60, 1),
('Router AX3000', 'WiFi 6 router', 129.99, 75, 1),
('Power Bank 20000mAh', 'Portable charger', 39.99, 180, 1),
('Drone Mini', 'Compact drone with camera', 499.99, 25, 1),
('VR Headset', 'Virtual reality headset', 349.99, 35, 1);
GO

-- Books (CategoryId = 2)
INSERT INTO Products (Name, Description, Price, Stock, CategoryId) VALUES
('The Great Novel', 'Fiction bestseller', 19.99, 200, 2),
('Science 101', 'Basic science book', 29.99, 150, 2),
('History of Europe', 'Historical overview', 24.99, 120, 2),
('Cooking Made Easy', 'Recipe book', 18.99, 180, 2),
('Programming C#', 'Learn C# programming', 39.99, 90, 2),
('SQL Mastery', 'Advanced SQL guide', 34.99, 85, 2),
('Fantasy World', 'Epic fantasy novel', 22.99, 140, 2),
('Mystery Case', 'Thriller novel', 17.99, 160, 2),
('Self Development', 'Improve your life', 21.99, 130, 2),
('Business Strategy', 'Business insights', 27.99, 110, 2),
('Children Stories', 'Kids bedtime stories', 14.99, 220, 2),
('Poetry Collection', 'Classic poems', 16.99, 100, 2),
('Art of War', 'Ancient strategy', 12.99, 190, 2),
('Travel Guide', 'Explore the world', 23.99, 95, 2),
('Psychology Basics', 'Intro to psychology', 28.99, 105, 2);
GO

-- Clothing (CategoryId = 3)
INSERT INTO Products (Name, Description, Price, Stock, CategoryId) VALUES
('T-Shirt Basic', 'Cotton t-shirt', 9.99, 300, 3),
('Jeans Slim Fit', 'Stylish jeans', 49.99, 120, 3),
('Hoodie Classic', 'Warm hoodie', 39.99, 150, 3),
('Jacket Winter', 'Insulated jacket', 89.99, 80, 3),
('Sneakers Sport', 'Comfortable sneakers', 59.99, 140, 3),
('Dress Elegant', 'Evening dress', 79.99, 60, 3),
('Shirt Formal', 'Office shirt', 29.99, 170, 3),
('Shorts Casual', 'Summer shorts', 19.99, 200, 3),
('Socks Pack', '5 pairs socks', 14.99, 250, 3),
('Hat Cap', 'Baseball cap', 12.99, 180, 3),
('Sweater Wool', 'Warm sweater', 44.99, 110, 3),
('Skirt Stylish', 'Fashion skirt', 34.99, 90, 3),
('Suit Premium', 'Formal suit', 199.99, 40, 3),
('Pajamas Set', 'Sleepwear', 24.99, 130, 3),
('Scarf Winter', 'Warm scarf', 15.99, 160, 3);
GO

-- Home & Kitchen (CategoryId = 4)
INSERT INTO Products (Name, Description, Price, Stock, CategoryId) VALUES
('Blender Pro', 'Kitchen blender', 59.99, 100, 4),
('Microwave Oven', '800W microwave', 129.99, 60, 4),
('Coffee Maker', 'Automatic coffee machine', 89.99, 90, 4),
('Air Fryer', 'Healthy cooking', 119.99, 70, 4),
('Cookware Set', '10-piece set', 149.99, 50, 4),
('Knife Set', 'Stainless steel knives', 79.99, 85, 4),
('Vacuum Cleaner', 'Powerful vacuum', 199.99, 40, 4),
('Dining Table', 'Wooden table', 299.99, 20, 4),
('Chair Set', '4 chairs', 199.99, 30, 4),
('Bed Frame', 'Queen size frame', 249.99, 25, 4),
('Pillow Memory Foam', 'Comfort pillow', 29.99, 150, 4),
('Curtains Set', 'Window curtains', 39.99, 120, 4),
('Desk Lamp', 'LED lamp', 24.99, 140, 4),
('Storage Box', 'Plastic storage', 19.99, 200, 4),
('Wall Clock', 'Modern clock', 22.99, 110, 4);
GO

-- Sports (CategoryId = 5)
INSERT INTO Products (Name, Description, Price, Stock, CategoryId) VALUES
('Football Ball', 'Standard size ball', 25.99, 150, 5),
('Basketball', 'Indoor/outdoor ball', 29.99, 140, 5),
('Tennis Racket', 'Lightweight racket', 79.99, 80, 5),
('Running Shoes', 'Comfort shoes', 89.99, 120, 5),
('Yoga Mat', 'Non-slip mat', 19.99, 200, 5),
('Dumbbell Set', 'Adjustable weights', 99.99, 60, 5),
('Treadmill', 'Home treadmill', 599.99, 15, 5),
('Exercise Bike', 'Indoor bike', 399.99, 20, 5),
('Skipping Rope', 'Fitness rope', 9.99, 250, 5),
('Boxing Gloves', 'Training gloves', 49.99, 100, 5),
('Swim Goggles', 'Anti-fog goggles', 14.99, 180, 5),
('Camping Tent', '2-person tent', 129.99, 40, 5),
('Backpack Sport', 'Outdoor backpack', 59.99, 90, 5),
('Fishing Rod', 'Carbon rod', 79.99, 70, 5),
('Kayak Inflatable', 'Portable kayak', 299.99, 10, 5);
GO