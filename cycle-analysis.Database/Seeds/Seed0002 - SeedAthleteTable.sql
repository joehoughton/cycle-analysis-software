SET IDENTITY_INSERT [dbo].[Athlete] ON 
INSERT [dbo].[Athlete] ([Id], [Username], [FirstName], [LastName], [Email], [RegistrationDate], [Image], [LactateThreshold], [Weight], [UniqueKey]) VALUES (1, N'duncanmullier', N'Duncan', N'Mullier', N'duncanmullier@email.com', CAST(N'2016-02-15 03:34:40.067' AS DateTime), N'duncan.jpg', 345, 69, N'cb4db54c-4e65-43c7-90d4-8b687523cff7')
INSERT [dbo].[Athlete] ([Id], [Username], [FirstName], [LastName], [Email], [RegistrationDate], [Image], [LactateThreshold], [Weight], [UniqueKey]) VALUES (2, N'joehoughton', N'Joe', N'Houghton', N'joehoughton@email.com', CAST(N'2016-02-15 05:34:10.063' AS DateTime), N'hamish.jpg', 320, 85.73, N'1364676a-bab0-4c75-a585-ebef61f72ac5')
SET IDENTITY_INSERT [dbo].[Athlete] OFF
