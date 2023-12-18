
CREATE DATABASE PruebaTecnica;
USE PruebaTecnica;

CREATE TABLE [Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NULL,
	[UserNetwork] [varchar](150) NULL ,
	[Password] [varchar](150) NULL 
	PRIMARY KEY (UserId)
	);
	DBCC CHECKIDENT ([Users], RESEED,1); -- obliga a que el contador de llave primaria empiece en 1 

INSERT [Users] VALUES (  N'Jorge Pertuz Egea', N'jpertuz', N'15e2b0d3c33891ebb0f1ef609ec419420c20e320ce94c65fbc8c3312448eb225'); 
	  

CREATE TABLE [Status](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [varchar](150) NULL 
	PRIMARY KEY (StatusId)
	);
	DBCC CHECKIDENT ([Status], RESEED,1); -- obliga a que el contador de llave primaria empiece en 1 
	
CREATE TABLE [Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[StatusId] [int] NOT NULL,
	[Description] [varchar](200) NULL,
	[Price] [decimal],
	PRIMARY KEY (ProductId),
	FOREIGN KEY (StatusId) REFERENCES Status (StatusId)
	);
	DBCC CHECKIDENT ([Products], RESEED,1); -- obliga a que el contador de llave primaria empiece en 1 