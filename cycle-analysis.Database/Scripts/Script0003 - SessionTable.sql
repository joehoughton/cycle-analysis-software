SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Session](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[SoftwareVersion] [float] NOT NULL,
	[MonitorVersion] [int] NOT NULL,
	[SMode] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[Length] [datetime] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Interval] [int] NOT NULL,
	[Upper1] [int] NOT NULL,
	[Lower1] [int] NOT NULL,
	[AthleteId] [int] NOT NULL,
 CONSTRAINT [PK_Session_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Athlete] FOREIGN KEY([AthleteId])
REFERENCES [dbo].[Athlete] ([Id])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_Athlete]
GO
