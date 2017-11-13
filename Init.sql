USE [Nop4.0]
GO
/****** Object:  Table [dbo].[LanguageOfSite]    Script Date: 11/14/2017 00:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LanguageOfSite](
	[Id] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[LanguageId] [int] NOT NULL,
	[IsPrimary] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_LanguageOfSite] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 11/14/2017 00:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language](
	[Id] [int] NOT NULL,
	[LanguageName] [nvarchar](50) NOT NULL,
	[LanguageCultrueName] [nvarchar](50) NOT NULL,
	[Published] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[Language] ([Id], [LanguageName], [LanguageCultrueName], [Published], [DisplayOrder], [IsDeleted]) VALUES (1, N'简体中文', N'zh-CN', 1, 1, 0)
INSERT [dbo].[Language] ([Id], [LanguageName], [LanguageCultrueName], [Published], [DisplayOrder], [IsDeleted]) VALUES (2, N'English', N'en-US', 1, 2, 0)
INSERT [dbo].[Language] ([Id], [LanguageName], [LanguageCultrueName], [Published], [DisplayOrder], [IsDeleted]) VALUES (3, N'繁体中文', N'zh-TW', 1, 3, 0)
/****** Object:  Table [dbo].[Customer]    Script Date: 11/14/2017 00:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] NULL,
	[SiteId] [int] NULL,
	[CustomerName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL
) ON [PRIMARY]
GO
