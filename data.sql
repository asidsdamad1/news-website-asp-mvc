USE [NewsWebsite]
GO
/****** Object:  Table [dbo].[info]    Script Date: 3/30/2022 10:55:16 PM ******/
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
/****** Object:  Table [dbo].[Posts]    Script Date: 3/30/2022 10:55:16 PM ******/
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
/****** Object:  Table [dbo].[Roles]    Script Date: 3/30/2022 10:55:16 PM ******/
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
/****** Object:  Table [dbo].[Series]    Script Date: 3/30/2022 10:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Series](
	[series_id] [int] NOT NULL,
	[seriesName] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Series] PRIMARY KEY CLUSTERED 
(
	[series_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StickyPosts]    Script Date: 3/30/2022 10:55:16 PM ******/
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
/****** Object:  Table [dbo].[Tags]    Script Date: 3/30/2022 10:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[tag_id] [int] IDENTITY(1,1) NOT NULL,
	[tag_name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[tag_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_PostTags]    Script Date: 3/30/2022 10:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_PostTags](
	[post_id] [int] NOT NULL,
	[tag_id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_SeriesPost]    Script Date: 3/30/2022 10:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_SeriesPost](
	[post_id] [int] NOT NULL,
	[series_id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/30/2022 10:55:16 PM ******/
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
INSERT [dbo].[info] ([id], [web_name], [web_des], [web_about]) VALUES (1, N'nhom 4+', N'nhom 4+', N'<p>nhom 4+</p>
')
GO
SET IDENTITY_INSERT [dbo].[Posts] ON 

INSERT [dbo].[Posts] ([post_id], [user_id], [post_title], [post_slug], [post_teaser], [post_review], [post_content], [post_type], [post_tag], [create_date], [edit_date], [dynasty], [view_count], [rated], [avatar_image], [status]) VALUES (16, 1, N'eeeeeeeeewwww', N'eeeeeeeeewwww', N'ss
', N's', N'<p>ss</p>
', 1, NULL, CAST(N'2022-03-25T16:47:41.437' AS DateTime), NULL, N'TrongNuoc', 4, 3, N'eeeeeeeeewwww-47.png', 1)
INSERT [dbo].[Posts] ([post_id], [user_id], [post_title], [post_slug], [post_teaser], [post_review], [post_content], [post_type], [post_tag], [create_date], [edit_date], [dynasty], [view_count], [rated], [avatar_image], [status]) VALUES (17, 1, N'Bóng đá Việt Namassa', N'bong-da-viet-nam', N'sss
', N'555', N'<p>sss<img alt="" src="/Upload/images/images/download.jpg" /></p>
', 1, NULL, CAST(N'2022-03-25T17:33:11.633' AS DateTime), CAST(N'2022-03-25T22:31:35.957' AS DateTime), N'TrongNuoc', 7, 3, N'bong-da-viet-namassa-17.jpg', 1)
INSERT [dbo].[Posts] ([post_id], [user_id], [post_title], [post_slug], [post_teaser], [post_review], [post_content], [post_type], [post_tag], [create_date], [edit_date], [dynasty], [view_count], [rated], [avatar_image], [status]) VALUES (1016, 1, N'MCU đối đầu AM', N'mcu-doi-dau-am', N'sadad
', N'123', N'<p>sadad</p>
', 1, NULL, CAST(N'2022-03-28T15:13:36.880' AS DateTime), NULL, N'TrongNuoc', 5, 3, N'mcu-doi-dau-am-45.png', 1)
SET IDENTITY_INSERT [dbo].[Posts] OFF
GO
INSERT [dbo].[Roles] ([role_id], [role_name]) VALUES (1, N'admin')
INSERT [dbo].[Roles] ([role_id], [role_name]) VALUES (2, N'editor')
GO
SET IDENTITY_INSERT [dbo].[StickyPosts] ON 

INSERT [dbo].[StickyPosts] ([id], [priority], [post_id]) VALUES (7, 1, 16)
INSERT [dbo].[StickyPosts] ([id], [priority], [post_id]) VALUES (8, 100, 17)
INSERT [dbo].[StickyPosts] ([id], [priority], [post_id]) VALUES (9, 3, 17)
SET IDENTITY_INSERT [dbo].[StickyPosts] OFF
GO
SET IDENTITY_INSERT [dbo].[Tags] ON 

INSERT [dbo].[Tags] ([tag_id], [tag_name]) VALUES (2, N'Bóng đá')
INSERT [dbo].[Tags] ([tag_id], [tag_name]) VALUES (1003, N'Chính Trị')
INSERT [dbo].[Tags] ([tag_id], [tag_name]) VALUES (1004, N'Công nghệ')
SET IDENTITY_INSERT [dbo].[Tags] OFF
GO
INSERT [dbo].[Tbl_PostTags] ([post_id], [tag_id]) VALUES (16, 2)
INSERT [dbo].[Tbl_PostTags] ([post_id], [tag_id]) VALUES (17, 2)
INSERT [dbo].[Tbl_PostTags] ([post_id], [tag_id]) VALUES (17, 1003)
INSERT [dbo].[Tbl_PostTags] ([post_id], [tag_id]) VALUES (1016, 2)
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([user_id], [username], [password], [fullname], [role_id], [status], [email]) VALUES (1, N'admin', N'0192023A7BBD73250516F069DF18B500', N'hieu1', 1, 1, NULL)
INSERT [dbo].[Users] ([user_id], [username], [password], [fullname], [role_id], [status], [email]) VALUES (2, N'123', N'202CB962AC59075B964B07152D234B70', N'1232', 1, 1, NULL)
INSERT [dbo].[Users] ([user_id], [username], [password], [fullname], [role_id], [status], [email]) VALUES (3, N'111', N'698D51A19D8A121CE581499D7B701668', N'111', 1, 1, NULL)
INSERT [dbo].[Users] ([user_id], [username], [password], [fullname], [role_id], [status], [email]) VALUES (4, N'hieu', N'202CB962AC59075B964B07152D234B70', N'123', 2, 1, NULL)
INSERT [dbo].[Users] ([user_id], [username], [password], [fullname], [role_id], [status], [email]) VALUES (5, N'1234', N'C4CA4238A0B923820DCC509A6F75849B', N'123', 2, 1, NULL)
INSERT [dbo].[Users] ([user_id], [username], [password], [fullname], [role_id], [status], [email]) VALUES (6, N'12', N'ECCBC87E4B5CE2FE28308FD9F2A7BAF3', N'3', 2, 1, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
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
