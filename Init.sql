USE [Nop4.0]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/29/2017 22:22:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
DROP TABLE [dbo].[Customer]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 11/29/2017 22:22:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Language]') AND type in (N'U'))
DROP TABLE [dbo].[Language]
GO
/****** Object:  Table [dbo].[LanguageOfSite]    Script Date: 11/29/2017 22:22:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LanguageOfSite]') AND type in (N'U'))
DROP TABLE [dbo].[LanguageOfSite]
GO
/****** Object:  Table [dbo].[Site]    Script Date: 11/29/2017 22:22:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Site]') AND type in (N'U'))
DROP TABLE [dbo].[Site]
GO
/****** Object:  Table [dbo].[SiteDomain]    Script Date: 11/29/2017 22:22:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiteDomain]') AND type in (N'U'))
DROP TABLE [dbo].[SiteDomain]
GO
/****** Object:  Table [dbo].[SiteDomain]    Script Date: 11/29/2017 22:22:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiteDomain]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SiteDomain](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NOT NULL,
	[Domain] [nvarchar](200) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_SiteDomain] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[SiteDomain] ON
INSERT [dbo].[SiteDomain] ([Id], [SiteId], [Domain], [DisplayOrder]) VALUES (1, 1, N'nop.jianjialin.com', 1)
SET IDENTITY_INSERT [dbo].[SiteDomain] OFF
/****** Object:  Table [dbo].[Site]    Script Date: 11/29/2017 22:22:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Site]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Site](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SiteName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Site] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[Site] ON
INSERT [dbo].[Site] ([Id], [SiteName]) VALUES (1, N'第一站')
SET IDENTITY_INSERT [dbo].[Site] OFF
/****** Object:  Table [dbo].[LanguageOfSite]    Script Date: 11/29/2017 22:22:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LanguageOfSite]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LanguageOfSite](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NOT NULL,
	[LanguageId] [int] NOT NULL,
	[IsPrimary] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[LanguageName] [nvarchar](50) NOT NULL,
	[LanguageCulture] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_LanguageOfSite] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[LanguageOfSite] ON
INSERT [dbo].[LanguageOfSite] ([Id], [SiteId], [LanguageId], [IsPrimary], [DisplayOrder], [LanguageName], [LanguageCulture]) VALUES (1, 1, 1, 1, 1, N'简体中文', N'zh-CN')
SET IDENTITY_INSERT [dbo].[LanguageOfSite] OFF
/****** Object:  Table [dbo].[Language]    Script Date: 11/29/2017 22:22:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Language]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Language](
	[Id] [int] NOT NULL,
	[LanguageName] [nvarchar](50) NOT NULL,
	[LanguageCultrue] [nvarchar](50) NOT NULL,
	[Published] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [dbo].[Language] ([Id], [LanguageName], [LanguageCultrue], [Published], [DisplayOrder], [IsDeleted]) VALUES (1, N'简体中文', N'zh-CN', 1, 1, 0)
INSERT [dbo].[Language] ([Id], [LanguageName], [LanguageCultrue], [Published], [DisplayOrder], [IsDeleted]) VALUES (2, N'English', N'en-US', 1, 2, 0)
INSERT [dbo].[Language] ([Id], [LanguageName], [LanguageCultrue], [Published], [DisplayOrder], [IsDeleted]) VALUES (3, N'繁体中文', N'zh-TW', 1, 3, 0)
/****** Object:  Table [dbo].[Customer]    Script Date: 11/29/2017 22:22:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customer](
	[Id] [int] NULL,
	[SiteId] [int] NULL,
	[CustomerName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL
) ON [PRIMARY]
END
GO
