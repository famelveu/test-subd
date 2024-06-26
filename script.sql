USE [master]
GO
/****** Object:  Database [sc_x99]    Script Date: 19.05.2024 15:43:52 ******/
CREATE DATABASE [sc_x99]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'sc_x99', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\sc_x99.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'sc_x99_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\sc_x99_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [sc_x99] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [sc_x99].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [sc_x99] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [sc_x99] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [sc_x99] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [sc_x99] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [sc_x99] SET ARITHABORT OFF 
GO
ALTER DATABASE [sc_x99] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [sc_x99] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [sc_x99] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [sc_x99] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [sc_x99] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [sc_x99] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [sc_x99] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [sc_x99] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [sc_x99] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [sc_x99] SET  ENABLE_BROKER 
GO
ALTER DATABASE [sc_x99] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [sc_x99] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [sc_x99] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [sc_x99] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [sc_x99] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [sc_x99] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [sc_x99] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [sc_x99] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [sc_x99] SET  MULTI_USER 
GO
ALTER DATABASE [sc_x99] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [sc_x99] SET DB_CHAINING OFF 
GO
ALTER DATABASE [sc_x99] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [sc_x99] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [sc_x99] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [sc_x99] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [sc_x99] SET QUERY_STORE = OFF
GO
USE [sc_x99]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name_c] [varchar](15) NOT NULL,
	[tel_num_c] [varchar](10) NOT NULL,
	[addres_c] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_master] [int] NULL,
	[id_client] [int] NOT NULL,
	[price] [int] NULL,
	[date_order] [date] NOT NULL,
	[date_complete] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[role_u] [int] NOT NULL,
	[name_u] [varchar](15) NOT NULL,
	[surname_u] [varchar](15) NOT NULL,
	[tel_num_u] [varchar](10) NOT NULL,
	[pw] [varchar](8) NOT NULL,
	[listed] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Заказы]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View [dbo].[Заказы] AS
SELECT Orders.id, Users.name_u AS 'Мастер', Clients.name_c AS 'Клиент', Orders.price AS 'Стоимость', Orders.date_order AS 'Дата заказа', Orders.date_complete AS 'Дата выполнения' FROM Orders  
JOIN Users ON Users.id = Orders.id_master JOIN Clients ON Clients.id = Orders.id_client
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name_r] [varchar](5) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Пользователи]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[Пользователи] AS
SELECT Users.id, Roles.name_r AS Роль, Users.name_u AS 'Имя', Users.surname_u AS 'Фамилия', Users.tel_num_u AS 'Телефон', Users.pw AS 'Пароль', Users.listed AS 'Состояние' FROM Users 
JOIN Roles ON Roles.id = Users.role_u
GO
/****** Object:  Table [dbo].[Services_to_orders]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services_to_orders](
	[id_o] [int] NOT NULL,
	[id_s] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Services]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name_s] [varchar](40) NOT NULL,
	[cost_s] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Услугикзаказам]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[Услугикзаказам] AS
SELECT Services_to_orders.id_o AS 'id заказа', Services.name_s AS 'id услуги' FROM Services_to_orders JOIN Services ON Services.id = Services_to_orders.id_s
GO
/****** Object:  View [dbo].[Роли]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[Роли] AS
SELECT id, name_r AS 'Название' FROM Roles
GO
/****** Object:  View [dbo].[Услуги]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[Услуги] AS
Select id, name_s AS 'Название', cost_s AS 'Цена' from Services
GO
/****** Object:  View [dbo].[Клиенты]    Script Date: 19.05.2024 15:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[Клиенты] AS
Select id, name_c AS 'Имя', tel_num_c AS 'Телефон', addres_c AS 'Адрес' from Clients
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [Orders_fk1] FOREIGN KEY([id_master])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [Orders_fk1]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [Orders_fk2] FOREIGN KEY([id_client])
REFERENCES [dbo].[Clients] ([id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [Orders_fk2]
GO
ALTER TABLE [dbo].[Services_to_orders]  WITH CHECK ADD  CONSTRAINT [Services_to_orders_fk0] FOREIGN KEY([id_o])
REFERENCES [dbo].[Orders] ([id])
GO
ALTER TABLE [dbo].[Services_to_orders] CHECK CONSTRAINT [Services_to_orders_fk0]
GO
ALTER TABLE [dbo].[Services_to_orders]  WITH CHECK ADD  CONSTRAINT [Services_to_orders_fk1] FOREIGN KEY([id_s])
REFERENCES [dbo].[Services] ([id])
GO
ALTER TABLE [dbo].[Services_to_orders] CHECK CONSTRAINT [Services_to_orders_fk1]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [Users_fk1] FOREIGN KEY([role_u])
REFERENCES [dbo].[Roles] ([id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [Users_fk1]
GO
USE [master]
GO
ALTER DATABASE [sc_x99] SET  READ_WRITE 
GO
