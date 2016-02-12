SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([Id], [Name]) VALUES (1, N'Admin')
SET IDENTITY_INSERT [dbo].[Role] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [Username], [Email], [HashedPassword], [Salt], [IsLocked], [DateCreated]) VALUES (1019, N'joehoughton', N'joehoughton@email.com', N'C/QQLW7Awf7sJGTr36rxkxq62SbSoIpzm8jaVpfIgu8=', N'fs4OFBGmFF9y2Urq2iUk0Q==', 0, CAST(N'2015-12-28 17:22:02.217' AS DateTime))
SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[UserRole] ON 

INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (1008, 1019, 1)
SET IDENTITY_INSERT [dbo].[UserRole] OFF
