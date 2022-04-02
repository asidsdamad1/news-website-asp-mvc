USE [NewsWebsite]
GO
/****** Object:  Table [dbo].[info]    Script Date: 4/2/2022 9:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[info](
	[id] [int] NOT NULL,
	[web_name] [nvarchar](50) NOT NULL,
	[web_des] [nvarchar](200) NULL,
	[web_about] [ntext] NULL,
 CONSTRAINT [PK_info] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Posts]    Script Date: 4/2/2022 9:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[post_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[post_title] [nvarchar](200) NOT NULL,
	[post_slug] [nvarchar](200) NULL,
	[post_teaser] [nvarchar](500) NOT NULL,
	[post_review] [nvarchar](500) NULL,
	[post_content] [ntext] NULL,
	[post_type] [int] NOT NULL,
	[post_tag] [nvarchar](200) NULL,
	[create_date] [datetime] NULL,
	[edit_date] [datetime] NULL,
	[dynasty] [nvarchar](50) NULL,
	[view_count] [int] NOT NULL,
	[rated] [int] NOT NULL,
	[avatar_image] [nvarchar](200) NULL,
	[status] [bit] NOT NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[post_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 4/2/2022 9:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[role_id] [int] NOT NULL,
	[role_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Series]    Script Date: 4/2/2022 9:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Series](
	[series_id] [int] IDENTITY(1,1) NOT NULL,
	[seriesName] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Series] PRIMARY KEY CLUSTERED 
(
	[series_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StickyPosts]    Script Date: 4/2/2022 9:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StickyPosts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[priority] [int] NOT NULL,
	[post_id] [int] NOT NULL,
 CONSTRAINT [PK_StickyPosts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 4/2/2022 9:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[tag_id] [int] IDENTITY(1,1) NOT NULL,
	[tag_name] [nvarchar](50) NULL,
	[language] [nvarchar](2) NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[tag_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_PostTags]    Script Date: 4/2/2022 9:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_PostTags](
	[post_id] [int] NOT NULL,
	[tag_id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_SeriesPost]    Script Date: 4/2/2022 9:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_SeriesPost](
	[post_id] [int] NOT NULL,
	[series_id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/2/2022 9:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](20) NOT NULL,
	[password] [varchar](100) NOT NULL,
	[fullname] [nvarchar](50) NOT NULL,
	[role_id] [int] NOT NULL,
	[status] [bit] NOT NULL,
	[email] [nvarchar](50) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Users]
GO
ALTER TABLE [dbo].[StickyPosts]  WITH CHECK ADD  CONSTRAINT [FK_StickyPosts_Posts] FOREIGN KEY([post_id])
REFERENCES [dbo].[Posts] ([post_id])
GO
ALTER TABLE [dbo].[StickyPosts] CHECK CONSTRAINT [FK_StickyPosts_Posts]
GO
ALTER TABLE [dbo].[Tbl_PostTags]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_PostTags_Posts] FOREIGN KEY([post_id])
REFERENCES [dbo].[Posts] ([post_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tbl_PostTags] CHECK CONSTRAINT [FK_Tbl_PostTags_Posts]
GO
ALTER TABLE [dbo].[Tbl_PostTags]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_PostTags_Tags] FOREIGN KEY([tag_id])
REFERENCES [dbo].[Tags] ([tag_id])
GO
ALTER TABLE [dbo].[Tbl_PostTags] CHECK CONSTRAINT [FK_Tbl_PostTags_Tags]
GO
ALTER TABLE [dbo].[Tbl_SeriesPost]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_SeriesPost_Posts] FOREIGN KEY([post_id])
REFERENCES [dbo].[Posts] ([post_id])
GO
ALTER TABLE [dbo].[Tbl_SeriesPost] CHECK CONSTRAINT [FK_Tbl_SeriesPost_Posts]
GO
ALTER TABLE [dbo].[Tbl_SeriesPost]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_SeriesPost_Series] FOREIGN KEY([series_id])
REFERENCES [dbo].[Series] ([series_id])
GO
ALTER TABLE [dbo].[Tbl_SeriesPost] CHECK CONSTRAINT [FK_Tbl_SeriesPost_Series]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([role_id])
REFERENCES [dbo].[Roles] ([role_id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
