USE [master]
GO
/****** Object:  Database [QLCHGD]    Script Date: 07/05/2021 21:39:37 ******/
CREATE DATABASE [QLCHGD]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QLCHGD', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\QLCHGD.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'QLCHGD_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\QLCHGD_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [QLCHGD] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QLCHGD].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QLCHGD] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QLCHGD] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QLCHGD] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QLCHGD] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QLCHGD] SET ARITHABORT OFF 
GO
ALTER DATABASE [QLCHGD] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [QLCHGD] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QLCHGD] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QLCHGD] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QLCHGD] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QLCHGD] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QLCHGD] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QLCHGD] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QLCHGD] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QLCHGD] SET  ENABLE_BROKER 
GO
ALTER DATABASE [QLCHGD] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QLCHGD] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QLCHGD] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QLCHGD] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QLCHGD] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QLCHGD] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QLCHGD] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QLCHGD] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [QLCHGD] SET  MULTI_USER 
GO
ALTER DATABASE [QLCHGD] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QLCHGD] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QLCHGD] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QLCHGD] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [QLCHGD] SET DELAYED_DURABILITY = DISABLED 
GO
USE [QLCHGD]
GO
/****** Object:  Table [dbo].[account]    Script Date: 07/05/2021 21:39:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[account](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [varchar](100) NOT NULL,
	[password] [varchar](100) NOT NULL,
	[auth] [nvarchar](20) NOT NULL,
	[state] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[bill]    Script Date: 07/05/2021 21:39:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bill](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date_create] [date] NOT NULL,
	[total_price] [float] NOT NULL,
	[address] [nvarchar](500) NOT NULL,
	[note] [nvarchar](500) NULL,
	[state] [int] NULL,
	[id_customer] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[bill_detail]    Script Date: 07/05/2021 21:39:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bill_detail](
	[id_bill] [int] NOT NULL,
	[id_product] [int] NOT NULL,
	[quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_bill] ASC,
	[id_product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[category]    Script Date: 07/05/2021 21:39:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[customer]    Script Date: 07/05/2021 21:39:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[customer](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[gender] [nvarchar](10) NOT NULL,
	[phone] [int] NOT NULL,
	[address] [nvarchar](200) NOT NULL,
	[id_account] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[employee]    Script Date: 07/05/2021 21:39:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[employee](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[gender] [nvarchar](10) NOT NULL,
	[phone] [int] NOT NULL,
	[address] [nvarchar](200) NOT NULL,
	[id_account] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[manufacturer]    Script Date: 07/05/2021 21:39:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[manufacturer](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[phone] [int] NOT NULL,
	[address] [nvarchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[product]    Script Date: 07/05/2021 21:39:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[product](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[price] [float] NOT NULL,
	[quantity] [int] NOT NULL,
	[width] [float] NOT NULL,
	[height] [float] NOT NULL,
	[detail] [nvarchar](500) NULL,
	[image_main] [varchar](200) NOT NULL,
	[image1] [varchar](200) NULL,
	[image2] [varchar](200) NULL,
	[image3] [varchar](200) NULL,
	[id_category] [int] NOT NULL,
	[id_manufacturer] [int] NOT NULL,
	[id_unit] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[unit]    Script Date: 07/05/2021 21:39:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[unit](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[account] ON 

INSERT [dbo].[account] ([id], [user_name], [password], [auth], [state]) VALUES (1, N'admin', N'21232f297a57a5a743894a0e4a801fc3', N'Admin', 0)
INSERT [dbo].[account] ([id], [user_name], [password], [auth], [state]) VALUES (2, N'baoha', N'b1aa9da54463bb88e067d3f305b23772', N'Nhân viên', 0)
INSERT [dbo].[account] ([id], [user_name], [password], [auth], [state]) VALUES (3, N'Thanhnga', N'c3784911e7a3dd4a2e7b1bdf2f8a2bb4', N'Nhân viên', 0)
INSERT [dbo].[account] ([id], [user_name], [password], [auth], [state]) VALUES (4, N'diemkieu', N'94a76f3d06289f3e9cb69a4901a3baa0', N'Nhân viên', 0)
INSERT [dbo].[account] ([id], [user_name], [password], [auth], [state]) VALUES (5, N'haha', N'4e4d6c332b6fe62a63afe56171fd3725', N'Khách hàng', 0)
INSERT [dbo].[account] ([id], [user_name], [password], [auth], [state]) VALUES (6, N'nganga', N'e8b2aab709c076d1b7f2934ed9794cc7', N'Khách hàng', 0)
INSERT [dbo].[account] ([id], [user_name], [password], [auth], [state]) VALUES (7, N'kieukieu', N'b42be8588e5958c4f42b50de19ba12a8', N'Khách hàng', 0)
SET IDENTITY_INSERT [dbo].[account] OFF
SET IDENTITY_INSERT [dbo].[category] ON 

INSERT [dbo].[category] ([id], [name]) VALUES (1, N'đồ gia dụng nhà bếp')
INSERT [dbo].[category] ([id], [name]) VALUES (2, N'đồ dùng phòng tắm')
INSERT [dbo].[category] ([id], [name]) VALUES (3, N'thể thao và du lịch')
INSERT [dbo].[category] ([id], [name]) VALUES (4, N'giặt giũ và chăm sóc nhà cửa')
SET IDENTITY_INSERT [dbo].[category] OFF
SET IDENTITY_INSERT [dbo].[customer] ON 

INSERT [dbo].[customer] ([id], [name], [gender], [phone], [address], [id_account]) VALUES (1, N'Hà Lê', N'Nữ', 75432987, N'Sóc Trăng', 5)
INSERT [dbo].[customer] ([id], [name], [gender], [phone], [address], [id_account]) VALUES (2, N'Thanh Nga', N'Nữ', 945326234, N'Tây Ninh', 6)
INSERT [dbo].[customer] ([id], [name], [gender], [phone], [address], [id_account]) VALUES (3, N'Kem kem', N'Nam', 564325783, N'Hải Phòng', 7)
SET IDENTITY_INSERT [dbo].[customer] OFF
SET IDENTITY_INSERT [dbo].[employee] ON 

INSERT [dbo].[employee] ([id], [name], [gender], [phone], [address], [id_account]) VALUES (1, N'Lê Thị Bảo Hà', N'Nữ', 1234567, N'Châu Đốc', 2)
INSERT [dbo].[employee] ([id], [name], [gender], [phone], [address], [id_account]) VALUES (2, N'Huỳnh Thị Thanh Nga', N'Nữ', 76543298, N'Mỹ Thới', 3)
INSERT [dbo].[employee] ([id], [name], [gender], [phone], [address], [id_account]) VALUES (3, N'Huỳnh Thị Diễm Kiều', N'Nữ', 789546328, N'Cà Mau', 4)
SET IDENTITY_INSERT [dbo].[employee] OFF
SET IDENTITY_INSERT [dbo].[manufacturer] ON 

INSERT [dbo].[manufacturer] ([id], [name], [phone], [address]) VALUES (1, N'Bảo Hà', 76544224, N'Chợ Mới')
INSERT [dbo].[manufacturer] ([id], [name], [phone], [address]) VALUES (2, N'Thanh Nga', 98756326, N'Châu Thành')
INSERT [dbo].[manufacturer] ([id], [name], [phone], [address]) VALUES (3, N'Diễm Kiều', 987452456, N'Long xuyên')
SET IDENTITY_INSERT [dbo].[manufacturer] OFF
SET IDENTITY_INSERT [dbo].[product] ON 

INSERT [dbo].[product] ([id], [name], [price], [quantity], [width], [height], [detail], [image_main], [image1], [image2], [image3], [id_category], [id_manufacturer], [id_unit]) VALUES (1, N'Bông tắm', 10000, 100, 5, 12, N'Mịn màn', N'/Images/o-dung-phong-tam/bong-tam-cai-main.jpg', N'/Images/o-dung-phong-tam/bong-tam-cai-1.jpg', NULL, NULL, 2, 1, 1)
INSERT [dbo].[product] ([id], [name], [price], [quantity], [width], [height], [detail], [image_main], [image1], [image2], [image3], [id_category], [id_manufacturer], [id_unit]) VALUES (2, N'Khăn tắm', 15000, 30, 13, 15, N'Cotton', N'/Images/o-dung-phong-tam/khan-tam-cai-main.jpg', N'/Images/o-dung-phong-tam/khan-tam-cai-1.jpg', NULL, NULL, 2, 1, 1)
INSERT [dbo].[product] ([id], [name], [price], [quantity], [width], [height], [detail], [image_main], [image1], [image2], [image3], [id_category], [id_manufacturer], [id_unit]) VALUES (3, N'Nồi inox', 1600000, 50, 16, 16, N'Rất bền', N'/Images/o-gia-dung-nha-bep/noi-inox-cai-main.jpg', N'/Images/o-gia-dung-nha-bep/noi-inox-cai-1.png', NULL, NULL, 1, 2, 1)
INSERT [dbo].[product] ([id], [name], [price], [quantity], [width], [height], [detail], [image_main], [image1], [image2], [image3], [id_category], [id_manufacturer], [id_unit]) VALUES (4, N'Chảo chống dính', 350000, 600, 15, 17, N'Rất bền', N'/Images/o-gia-dung-nha-bep/chao-chong-dinh-cai-main.jpg', N'/Images/o-gia-dung-nha-bep/chao-chong-dinh-cai-1.jpg', NULL, NULL, 1, 2, 1)
INSERT [dbo].[product] ([id], [name], [price], [quantity], [width], [height], [detail], [image_main], [image1], [image2], [image3], [id_category], [id_manufacturer], [id_unit]) VALUES (5, N'Túi thể thao', 500000, 123, 50, 30, N'Nhẹ nhàng', N'/Images/the-thao-va-du-lich/tui-the-thao-chiec-main.jpg', NULL, N'/Images/the-thao-va-du-lich/tui-the-thao-chiec-2.jpg', N'/Images/the-thao-va-du-lich/tui-the-thao-chiec-3.jpg', 3, 3, 3)
INSERT [dbo].[product] ([id], [name], [price], [quantity], [width], [height], [detail], [image_main], [image1], [image2], [image3], [id_category], [id_manufacturer], [id_unit]) VALUES (6, N'Nón thể thao', 100000, 560, 15, 15, NULL, N'/Images/the-thao-va-du-lich/non-the-thao-cai-main.jpg', NULL, NULL, NULL, 3, 3, 1)
INSERT [dbo].[product] ([id], [name], [price], [quantity], [width], [height], [detail], [image_main], [image1], [image2], [image3], [id_category], [id_manufacturer], [id_unit]) VALUES (7, N'Móc dán tường', 15000, 70, 10, 30, N'Chắc chắn', N'/Images/giat-giu-va-cham-soc-nha-cua/moc-dan-tuong-cai-main.jpg', N'/Images/giat-giu-va-cham-soc-nha-cua/moc-dan-tuong-cai-1.png', NULL, NULL, 4, 2, 1)
INSERT [dbo].[product] ([id], [name], [price], [quantity], [width], [height], [detail], [image_main], [image1], [image2], [image3], [id_category], [id_manufacturer], [id_unit]) VALUES (8, N'Cây lau nhà', 150000, 30, 30, 15, N'Chắc chắn', N'/Images/giat-giu-va-cham-soc-nha-cua/cay-lau-nha-cai-main.jpg', N'/Images/giat-giu-va-cham-soc-nha-cua/cay-lau-nha-cai-1.jpg', NULL, NULL, 4, 3, 1)
SET IDENTITY_INSERT [dbo].[product] OFF
SET IDENTITY_INSERT [dbo].[unit] ON 

INSERT [dbo].[unit] ([id], [name]) VALUES (1, N'cái')
INSERT [dbo].[unit] ([id], [name]) VALUES (2, N'thùng')
INSERT [dbo].[unit] ([id], [name]) VALUES (3, N'chiếc')
SET IDENTITY_INSERT [dbo].[unit] OFF
ALTER TABLE [dbo].[bill]  WITH CHECK ADD FOREIGN KEY([id_customer])
REFERENCES [dbo].[customer] ([id])
GO
ALTER TABLE [dbo].[bill_detail]  WITH CHECK ADD FOREIGN KEY([id_bill])
REFERENCES [dbo].[bill] ([id])
GO
ALTER TABLE [dbo].[bill_detail]  WITH CHECK ADD FOREIGN KEY([id_product])
REFERENCES [dbo].[product] ([id])
GO
ALTER TABLE [dbo].[customer]  WITH CHECK ADD FOREIGN KEY([id_account])
REFERENCES [dbo].[account] ([id])
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD FOREIGN KEY([id_account])
REFERENCES [dbo].[account] ([id])
GO
ALTER TABLE [dbo].[product]  WITH CHECK ADD FOREIGN KEY([id_category])
REFERENCES [dbo].[category] ([id])
GO
ALTER TABLE [dbo].[product]  WITH CHECK ADD FOREIGN KEY([id_manufacturer])
REFERENCES [dbo].[manufacturer] ([id])
GO
ALTER TABLE [dbo].[product]  WITH CHECK ADD FOREIGN KEY([id_unit])
REFERENCES [dbo].[unit] ([id])
GO
USE [master]
GO
ALTER DATABASE [QLCHGD] SET  READ_WRITE 
GO
