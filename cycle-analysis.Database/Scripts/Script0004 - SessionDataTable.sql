SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SessionData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Row] [int] NOT NULL,
	[HeartRate] [int] NOT NULL,
	[Speed] [int] NOT NULL,
	[Cadence] [int] NOT NULL,
	[Altitude] [int] NOT NULL,
	[Power] [int] NOT NULL,
	[SessionId] [int] NOT NULL,
 CONSTRAINT [PK_SessionData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SessionData]  WITH CHECK ADD  CONSTRAINT [FK_SessionData_Session] FOREIGN KEY([SessionId])
REFERENCES [dbo].[Session] ([Id])
GO
ALTER TABLE [dbo].[SessionData] CHECK CONSTRAINT [FK_SessionData_Session]
GO