CREATE DATABASE [ScmDb]
GO

USE [ScmDb]
GO

CREATE TABLE [dbo].[User] (
	 IdUser						int				NOT NULL	IDENTITY(1, 1)
	,UserName					nvarchar(50)	NOT NULL	UNIQUE
	,[Password]					nvarchar(50)	NOT NULL
	,FirstName					nvarchar(30)	NOT NULL
	,LastName					nvarchar(30)	NOT NULL
	,Email						nvarchar(30)	NOT NULL	UNIQUE
	,Deleted					bit

	,CONSTRAINT PK_User PRIMARY KEY(IdUser)
);
GO

ALTER TABLE [dbo].[User] ADD CONSTRAINT DF_User_Deleted DEFAULT 0 FOR [Deleted];
GO


CREATE TABLE [dbo].[OperatingSystem] (
	 IdOperatingSystem			int				NOT NULL	IDENTITY(1, 1)
	,OperatingSystemName		nvarchar(10)	NOT NULL	UNIQUE

	,CONSTRAINT PK_OperatingSystem PRIMARY KEY(IdOperatingSystem)
);
GO

INSERT INTO [dbo].[OperatingSystem] (OperatingSystemName) VALUES ('Windows');
INSERT INTO [dbo].[OperatingSystem] (OperatingSystemName) VALUES ('Linux');
GO


CREATE TABLE [dbo].[Configuration] (
	 IdConfiguration			int				NOT NULL	IDENTITY(1, 1)
	,TimeOfCreation				datetime		NOT NULL
	,TimeOfLastUpdate			datetime		NULL
	,[Hash]						nvarchar(1000)	NULL
	,Name						nvarchar(50)	NOT NULL	UNIQUE
	,ShortDescription			nvarchar(150)	NOT NULL
	,FullDescription			nvarchar(2000)	NULL
	,ConfigurationScript		nvarchar(max)	NOT NULL
	,IsPublic					bit				NOT NULL
	,OperatingSystemId			int				NOT NULL	-- FOR WHICH OPERATING SYSTEM CONFIGURATION IS CREATED
	,UserId						int				NOT NULL	-- CREATED BY USER
	,Deleted					bit

	,CONSTRAINT PK_Configuration PRIMARY KEY(IdConfiguration)
);
GO

ALTER TABLE [dbo].[Configuration] ADD CONSTRAINT DF_Configuration_TimeOfCreation DEFAULT GETDATE() FOR [TimeOfCreation];
GO

ALTER TABLE [dbo].[Configuration] ADD CONSTRAINT DF_Configuration_Deleted DEFAULT 0 FOR [Deleted];
GO

ALTER TABLE [dbo].[Configuration]  WITH CHECK ADD  CONSTRAINT [FK_Configuration_OperatingSystem] FOREIGN KEY([OperatingSystemId])
REFERENCES [dbo].[OperatingSystem] ([IdOperatingSystem]);
GO

ALTER TABLE [dbo].[Configuration]  WITH CHECK ADD  CONSTRAINT [FK_Configuration_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([IdUser]);
GO


CREATE TABLE [dbo].[ConfigurationReview] (
	 IdConfigurationReview		int				NOT NULL	IDENTITY(1, 1)
	,TimeOfCreation				datetime		NOT NULL
	,Rating						int				NOT NULL
	,Comment					nvarchar(1000)	NULL
	,UserId						int				NOT NULL
	,ConfigurationId			int				NOT NULL
	,Deleted					bit

	,CONSTRAINT PK_ConfigurationReview PRIMARY KEY(IdConfigurationReview)
);
GO

ALTER TABLE [dbo].[ConfigurationReview] ADD CONSTRAINT DF_ConfigurationReview_TimeOfCreation DEFAULT GETDATE() FOR [TimeOfCreation];
GO

ALTER TABLE [dbo].[ConfigurationReview] ADD CONSTRAINT CHK_Rating CHECK (Rating > 0 AND Rating < 6);
GO

ALTER TABLE [dbo].[ConfigurationReview] ADD CONSTRAINT DF_ConfigurationReview_Deleted DEFAULT 0 FOR [Deleted];
GO

ALTER TABLE [dbo].[ConfigurationReview]  WITH CHECK ADD  CONSTRAINT [FK_ConfigurationReview_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([IdUser]);
GO

ALTER TABLE [dbo].[ConfigurationReview]  WITH CHECK ADD  CONSTRAINT [FK_ConfigurationReview_Configuration] FOREIGN KEY([ConfigurationId])
REFERENCES [dbo].[Configuration] ([IdConfiguration]);
GO


CREATE TABLE [dbo].[Server] (
	 IdServer					int				NOT NULL	IDENTITY(1, 1)
	,ServerName					nvarchar(50)	NOT NULL	UNIQUE
	,IpAddress					nvarchar(45)	NOT NULL	UNIQUE	-- IPv4 ADDRESS MAPPED TO IPv6 CAN CONTAIN UP TO 45 CHARACTERS, e.g. 0000:0000:0000:0000:0000:ffff:192.168.100.228
	,IsOnline					bit				NOT NULL
	,OperatingSystemId	int				NOT NULL
	,UserId						int				NOT NULL	-- USER THAT IS OWNER OF THE SERVER
	,Deleted					bit

	,CONSTRAINT PK_Server PRIMARY KEY(IdServer)
);
GO

ALTER TABLE [dbo].[Server] ADD CONSTRAINT DF_Server_Deleted DEFAULT 0 FOR [Deleted];
GO

ALTER TABLE [dbo].[Server]  WITH CHECK ADD  CONSTRAINT [FK_Server_OperatingSystem] FOREIGN KEY([OperatingSystemId])
REFERENCES [dbo].[OperatingSystem] ([IdOperatingSystem]);
GO

ALTER TABLE [dbo].[Server]  WITH CHECK ADD  CONSTRAINT [FK_Server_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([IdUser]);
GO


CREATE TABLE [dbo].[ServerConfiguration] (
	 ServerId					int				NOT NULL
	,ConfigurationId			int				NOT NULL
	,Deleted					bit

	,CONSTRAINT PK_UserGroup PRIMARY KEY (ServerId, ConfigurationId)
);
GO

ALTER TABLE [dbo].[ServerConfiguration] ADD CONSTRAINT DF_ServerConfiguration_Deleted DEFAULT 0 FOR [Deleted];
GO

ALTER TABLE [dbo].[ServerConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_ServerConfiguration_Server] FOREIGN KEY([ServerId])
REFERENCES [dbo].[Server] ([IdServer]);
GO

ALTER TABLE [dbo].[ServerConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_ServerConfiguration_Configuration] FOREIGN KEY([ConfigurationId])
REFERENCES [dbo].[Configuration] ([IdConfiguration]);
GO
