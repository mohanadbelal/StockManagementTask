IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'StockManagementDb')
BEGIN
    CREATE DATABASE [StockManagementDb];
END
GO

USE [StockManagementDb];
GO

-- Material Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Material')
BEGIN
    CREATE TABLE [dbo].[Material](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [Color] [nvarchar](50) NOT NULL,
        [CurrentStock] [int] NOT NULL,
        [MinimumRequiredStock] [int] NOT NULL,
        [CreatedAt] [datetime] NOT NULL DEFAULT (getdate()),
        [UpdatedAt] [datetime] NULL DEFAULT (getdate()),
        CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- User Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'User')
BEGIN
    CREATE TABLE [dbo].[User](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Username] [nvarchar](50) NOT NULL,
        [PasswordHash] [varbinary](max) NOT NULL,
        [Role] [varchar](50) NOT NULL,
        [CreatedAt] [datetime] NULL DEFAULT (getdate()),
        CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- StockTransaction Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StockTransaction')
BEGIN
    CREATE TABLE [dbo].[StockTransaction](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [MaterialId] [int] NOT NULL,
        [Quantity] [int] NOT NULL,
        [TransactionType] [bit] NOT NULL,
        [TransactionDate] [datetime] NOT NULL DEFAULT (getdate()),
        CONSTRAINT [PK_StockTransaction] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_StockTransaction_Material] FOREIGN KEY([MaterialId]) REFERENCES [dbo].[Material] ([Id])
    );
END
GO

-- Stored Procedures

-- Delete Material
CREATE OR ALTER PROCEDURE [dbo].[sp_Material_Delete]
	@Id int 
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM dbo.StockTransaction WHERE MaterialId = @Id;
	DELETE FROM dbo.Material WHERE Id = @Id;
END
GO

-- Upsert Material
CREATE OR ALTER PROCEDURE [dbo].[sp_Material_Upsert]
    @Name NVARCHAR(100),
    @Color NVARCHAR(50),
    @CurrentStock INT,
    @MinimumRequiredStock INT,
    @inMaterialId INT = null
AS
BEGIN
    IF (@inMaterialId IS NULL OR @inMaterialId = 0)
    BEGIN 
        INSERT INTO [dbo].[Material]
        ([Name], [Color], [CurrentStock], [MinimumRequiredStock])
        VALUES
        (@Name, @Color, @CurrentStock, @MinimumRequiredStock);
    END
    ELSE
    BEGIN
        UPDATE dbo.Material 
        SET Name = @Name, Color = @Color, CurrentStock = @CurrentStock, MinimumRequiredStock = @MinimumRequiredStock, UpdatedAt = getdate()
        WHERE Id = @inMaterialId;
    END
END
GO

-- Delete StockTransaction
CREATE OR ALTER PROCEDURE [dbo].[sp_StockTransaction_Delete]
	@Id int
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE @TransactionType AS bit;
	DECLARE @Quantity AS int;
	DECLARE @MaterialId AS int;
	SELECT @TransactionType = TransactionType, @Quantity = Quantity, @MaterialId = MaterialId FROM dbo.StockTransaction WHERE Id = @Id;

	IF (@TransactionType = 1)
	BEGIN 
		UPDATE dbo.Material SET CurrentStock = CurrentStock - @Quantity WHERE Id = @MaterialId;
	END
	ELSE
	BEGIN 
		UPDATE dbo.Material SET CurrentStock = CurrentStock + @Quantity WHERE Id = @MaterialId;
	END

	DELETE FROM dbo.StockTransaction WHERE Id = @Id;
END
GO

-- Insert StockTransaction
CREATE OR ALTER PROCEDURE [dbo].[sp_StockTransaction_Insert]
	@MaterialId int,
	@Quantity int,
	@TransactionType bit
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[StockTransaction]
    ([MaterialId], [Quantity], [TransactionType])
    VALUES
    (@MaterialId, @Quantity, @TransactionType);

    IF (@TransactionType = 1)
    BEGIN
        UPDATE dbo.Material SET CurrentStock = CurrentStock + @Quantity WHERE Id = @MaterialId;
    END
    ELSE
    BEGIN
        UPDATE dbo.Material SET CurrentStock = CurrentStock - @Quantity WHERE Id = @MaterialId;
    END
END
GO

-- Create User
CREATE OR ALTER PROCEDURE [dbo].[sp_User_Create]
	@Username AS nvarchar(50),
	@PasswordHash AS varbinary(MAX),
	@Role AS nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT * FROM [dbo].[User] WHERE Username = @Username)
	BEGIN
        INSERT INTO [dbo].[User](
            Username,
            [PasswordHash],
            Role
        ) VALUES (
            @Username,
            @PasswordHash,
            @Role
        );
    END
END
GO
