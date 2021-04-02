USE [RageRP]
GO
/****** Object:  Table [dbo].[Characters]    Script Date: 21/04/2019 02:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Characters](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[CharacterName] [varchar](255) NOT NULL,
	[Gender] [int] NOT NULL,
	[CurrentPed] [varchar](255) NOT NULL,
	[Cash] [int] NOT NULL,
	[Bank] [int] NOT NULL,
	[PedString] [varchar](max) NOT NULL,
	[isNewCharacter] [bit] NOT NULL,
	[isPolice] [bit] NOT NULL,
	[PoliceLevel] [int] NOT NULL,
	[isEMS] [bit] NOT NULL,
	[EMSLevel] [int] NOT NULL,
 CONSTRAINT [PK_Characters] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CharacterVehicles]    Script Date: 21/04/2019 02:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CharacterVehicles](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[CharacterID] [bigint] NOT NULL,
	[VehicleID] [bigint] NOT NULL,
 CONSTRAINT [PK_CharacterVehicles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DefaultSpawnLocations]    Script Date: 21/04/2019 02:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DefaultSpawnLocations](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[LocationName] [varchar](max) NOT NULL,
	[posX] [decimal](10, 4) NOT NULL,
	[posY] [decimal](10, 4) NOT NULL,
	[posZ] [decimal](10, 4) NOT NULL,
	[lookX] [decimal](10, 4) NOT NULL,
	[lookY] [decimal](10, 4) NOT NULL,
	[lookZ] [decimal](10, 4) NOT NULL,
	[spawnX] [decimal](10, 4) NULL,
	[spawnY] [decimal](10, 4) NULL,
	[spawnZ] [decimal](10, 4) NULL,
	[heading] [decimal](10, 4) NULL,
 CONSTRAINT [PK_DefaultSpawnLocations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FactionSpawnLocations]    Script Date: 21/04/2019 02:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FactionSpawnLocations](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[LocationName] [varchar](max) NOT NULL,
	[isPolice] [bit] NOT NULL,
	[isEMS] [bit] NOT NULL,
	[posX] [decimal](10, 4) NOT NULL,
	[posY] [decimal](10, 4) NOT NULL,
	[posZ] [decimal](10, 4) NOT NULL,
	[lookX] [decimal](10, 4) NOT NULL,
	[lookY] [decimal](10, 4) NOT NULL,
	[lookZ] [decimal](10, 4) NOT NULL,
	[spawnX] [decimal](10, 4) NULL,
	[spawnY] [decimal](10, 4) NULL,
	[spawnZ] [decimal](10, 4) NULL,
	[heading] [decimal](10, 4) NULL,
 CONSTRAINT [PK_FactionSpawnLocations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerCharacters]    Script Date: 21/04/2019 02:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerCharacters](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[PlayerID] [bigint] NOT NULL,
	[CharacterID] [bigint] NOT NULL,
	[isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_PlayerCharacters] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Players]    Script Date: 21/04/2019 02:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Players](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[PlayerName] [varchar](255) NOT NULL,
	[License] [varchar](9) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[isAdmin] [bit] NOT NULL,
	[AdminLevel] [int] NOT NULL,
	[Password] [varchar](255) NULL,
 CONSTRAINT [PK_Players] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerWhitelist]    Script Date: 21/04/2019 02:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerWhitelist](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[PlayerID] [bigint] NOT NULL,
	[License] [varchar](255) NOT NULL,
	[isWhitelisted] [bit] NOT NULL,
 CONSTRAINT [PK_PlayerWhitelist] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehicles]    Script Date: 21/04/2019 02:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicles](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[carModel] [varchar](255) NOT NULL,
	[colour] [varchar](255) NOT NULL,
	[plate] [varchar](255) NOT NULL,
	[mods] [varchar](max) NOT NULL,
	[inGarage] [bit] NOT NULL,
	[trunk] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Vehicles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Characters] OFF 
GO
SET IDENTITY_INSERT [dbo].[Characters] OFF
GO
SET IDENTITY_INSERT [dbo].[DefaultSpawnLocations] ON 
GO
INSERT [dbo].[DefaultSpawnLocations] ([id], [LocationName], [posX], [posY], [posZ], [lookX], [lookY], [lookZ], [spawnX], [spawnY], [spawnZ], [heading]) VALUES (1, N'Los Santos International', CAST(-1028.1298 AS Decimal(10, 4)), CAST(-2642.2287 AS Decimal(10, 4)), CAST(49.5994 AS Decimal(10, 4)), CAST(-1037.0593 AS Decimal(10, 4)), CAST(-2725.8332 AS Decimal(10, 4)), CAST(26.5260 AS Decimal(10, 4)), CAST(-1038.2340 AS Decimal(10, 4)), CAST(-2738.7620 AS Decimal(10, 4)), CAST(20.1692 AS Decimal(10, 4)), CAST(327.4659 AS Decimal(10, 4)))
GO
INSERT [dbo].[DefaultSpawnLocations] ([id], [LocationName], [posX], [posY], [posZ], [lookX], [lookY], [lookZ], [spawnX], [spawnY], [spawnZ], [heading]) VALUES (2, N'Portola Drive Station', CAST(-891.8143 AS Decimal(10, 4)), CAST(-84.5571 AS Decimal(10, 4)), CAST(64.9400 AS Decimal(10, 4)), CAST(-890.5471 AS Decimal(10, 4)), CAST(-85.1643 AS Decimal(10, 4)), CAST(64.4115 AS Decimal(10, 4)), CAST(-832.6562 AS Decimal(10, 4)), CAST(-101.1316 AS Decimal(10, 4)), CAST(28.1854 AS Decimal(10, 4)), CAST(234.7200 AS Decimal(10, 4)))
GO
SET IDENTITY_INSERT [dbo].[DefaultSpawnLocations] OFF
GO
SET IDENTITY_INSERT [dbo].[FactionSpawnLocations] ON 
GO
INSERT [dbo].[FactionSpawnLocations] ([id], [LocationName], [isPolice], [isEMS], [posX], [posY], [posZ], [lookX], [lookY], [lookZ], [spawnX], [spawnY], [spawnZ], [heading]) VALUES (1, N'Davis Fire Station', 0, 1, CAST(212.6069 AS Decimal(10, 4)), CAST(-1612.6430 AS Decimal(10, 4)), CAST(44.9114 AS Decimal(10, 4)), CAST(208.4230 AS Decimal(10, 4)), CAST(-1639.9932 AS Decimal(10, 4)), CAST(35.3026 AS Decimal(10, 4)), CAST(663.1050 AS Decimal(10, 4)), CAST(-71.8925 AS Decimal(10, 4)), CAST(38.5825 AS Decimal(10, 4)), CAST(26.8565 AS Decimal(10, 4)))
GO
INSERT [dbo].[FactionSpawnLocations] ([id], [LocationName], [isPolice], [isEMS], [posX], [posY], [posZ], [lookX], [lookY], [lookZ], [spawnX], [spawnY], [spawnZ], [heading]) VALUES (2, N'Rockford Hills Fire Station', 0, 1, CAST(-685.7616 AS Decimal(10, 4)), CAST(-50.6085 AS Decimal(10, 4)), CAST(78.8487 AS Decimal(10, 4)), CAST(-651.2114 AS Decimal(10, 4)), CAST(-87.3439 AS Decimal(10, 4)), CAST(47.4825 AS Decimal(10, 4)), CAST(199.7348 AS Decimal(10, 4)), CAST(-1646.9070 AS Decimal(10, 4)), CAST(29.8032 AS Decimal(10, 4)), CAST(241.0663 AS Decimal(10, 4)))
GO
INSERT [dbo].[FactionSpawnLocations] ([id], [LocationName], [isPolice], [isEMS], [posX], [posY], [posZ], [lookX], [lookY], [lookZ], [spawnX], [spawnY], [spawnZ], [heading]) VALUES (3, N'El Burro Heights Fire Station', 0, 1, CAST(1169.0413 AS Decimal(10, 4)), CAST(-1415.9796 AS Decimal(10, 4)), CAST(63.0675 AS Decimal(10, 4)), CAST(1192.2167 AS Decimal(10, 4)), CAST(-1471.5539 AS Decimal(10, 4)), CAST(39.7405 AS Decimal(10, 4)), CAST(1194.2050 AS Decimal(10, 4)), CAST(-1473.9360 AS Decimal(10, 4)), CAST(34.8595 AS Decimal(10, 4)), CAST(275.6947 AS Decimal(10, 4)))
GO
INSERT [dbo].[FactionSpawnLocations] ([id], [LocationName], [isPolice], [isEMS], [posX], [posY], [posZ], [lookX], [lookY], [lookZ], [spawnX], [spawnY], [spawnZ], [heading]) VALUES (4, N'Davis Sheriff''s Station', 1, 0, CAST(428.8711 AS Decimal(10, 4)), CAST(-1604.3663 AS Decimal(10, 4)), CAST(47.2069 AS Decimal(10, 4)), CAST(384.4632 AS Decimal(10, 4)), CAST(-1609.8959 AS Decimal(10, 4)), CAST(29.1691 AS Decimal(10, 4)), CAST(369.8974 AS Decimal(10, 4)), CAST(-1608.5320 AS Decimal(10, 4)), CAST(29.2919 AS Decimal(10, 4)), CAST(238.2178 AS Decimal(10, 4)))
GO
INSERT [dbo].[FactionSpawnLocations] ([id], [LocationName], [isPolice], [isEMS], [posX], [posY], [posZ], [lookX], [lookY], [lookZ], [spawnX], [spawnY], [spawnZ], [heading]) VALUES (5, N'Mission Row Police Station', 1, 0, CAST(401.6901 AS Decimal(10, 4)), CAST(-949.1757 AS Decimal(10, 4)), CAST(54.2695 AS Decimal(10, 4)), CAST(433.5852 AS Decimal(10, 4)), CAST(-984.1002 AS Decimal(10, 4)), CAST(34.6233 AS Decimal(10, 4)), CAST(458.6651 AS Decimal(10, 4)), CAST(-993.3438 AS Decimal(10, 4)), CAST(30.6896 AS Decimal(10, 4)), CAST(41.7385 AS Decimal(10, 4)))
GO
INSERT [dbo].[FactionSpawnLocations] ([id], [LocationName], [isPolice], [isEMS], [posX], [posY], [posZ], [lookX], [lookY], [lookZ], [spawnX], [spawnY], [spawnZ], [heading]) VALUES (6, N'Vespucci Police Station', 1, 0, CAST(-1102.7290 AS Decimal(10, 4)), CAST(-755.0809 AS Decimal(10, 4)), CAST(47.4706 AS Decimal(10, 4)), CAST(-1092.8710 AS Decimal(10, 4)), CAST(-812.6923 AS Decimal(10, 4)), CAST(30.2643 AS Decimal(10, 4)), CAST(-1118.5350 AS Decimal(10, 4)), CAST(-845.7151 AS Decimal(10, 4)), CAST(13.3825 AS Decimal(10, 4)), CAST(123.0584 AS Decimal(10, 4)))
GO
INSERT [dbo].[FactionSpawnLocations] ([id], [LocationName], [isPolice], [isEMS], [posX], [posY], [posZ], [lookX], [lookY], [lookZ], [spawnX], [spawnY], [spawnZ], [heading]) VALUES (7, N'Vinewood Police Station', 1, 0, CAST(693.2681 AS Decimal(10, 4)), CAST(-5.4895 AS Decimal(10, 4)), CAST(106.6995 AS Decimal(10, 4)), CAST(636.3175 AS Decimal(10, 4)), CAST(-4.8682 AS Decimal(10, 4)), CAST(85.2924 AS Decimal(10, 4)), CAST(639.7669 AS Decimal(10, 4)), CAST(0.4571 AS Decimal(10, 4)), CAST(82.7864 AS Decimal(10, 4)), CAST(238.6315 AS Decimal(10, 4)))
GO
SET IDENTITY_INSERT [dbo].[FactionSpawnLocations] OFF
GO
SET IDENTITY_INSERT [dbo].[PlayerCharacters] ON 
GO
INSERT [dbo].[PlayerCharacters] ([id], [PlayerID], [CharacterID], [isDeleted]) VALUES (1, 1, 1, 0)
GO
INSERT [dbo].[PlayerCharacters] ([id], [PlayerID], [CharacterID], [isDeleted]) VALUES (2, 1, 2, 0)
GO
SET IDENTITY_INSERT [dbo].[PlayerCharacters] OFF
GO
SET IDENTITY_INSERT [dbo].[Players] OFF 
GO
SET IDENTITY_INSERT [dbo].[Players] OFF
GO
SET IDENTITY_INSERT [dbo].[PlayerWhitelist] OFF
GO
SET IDENTITY_INSERT [dbo].[PlayerWhitelist] OFF
GO
ALTER TABLE [dbo].[Characters] ADD  CONSTRAINT [DF_Characters_isPolice]  DEFAULT ((0)) FOR [isPolice]
GO
ALTER TABLE [dbo].[Characters] ADD  CONSTRAINT [DF_Characters_PoliceLevel]  DEFAULT ((0)) FOR [PoliceLevel]
GO
ALTER TABLE [dbo].[Characters] ADD  CONSTRAINT [DF_Characters_isEMS]  DEFAULT ((0)) FOR [isEMS]
GO
ALTER TABLE [dbo].[Characters] ADD  CONSTRAINT [DF_Characters_EMSLevel]  DEFAULT ((0)) FOR [EMSLevel]
GO
ALTER TABLE [dbo].[FactionSpawnLocations] ADD  CONSTRAINT [DF_FactionSpawnLocations_isPolice]  DEFAULT ((0)) FOR [isPolice]
GO
ALTER TABLE [dbo].[FactionSpawnLocations] ADD  CONSTRAINT [DF_FactionSpawnLocations_isEMS]  DEFAULT ((0)) FOR [isEMS]
GO
ALTER TABLE [dbo].[Vehicles] ADD  CONSTRAINT [DF_Vehicles_mods]  DEFAULT ('{}') FOR [mods]
GO
ALTER TABLE [dbo].[Vehicles] ADD  CONSTRAINT [DF_Vehicles_Trunk]  DEFAULT ('{}') FOR [trunk]
GO
/****** Object:  StoredProcedure [dbo].[spu_CheckIfWhitelisted]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 26/03/2019 13:04

 * ChangeLog:
 *
 * 26/03/2019 13:04 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_CheckIfWhitelisted]
	@License varchar(255)
AS
BEGIN

	SELECT * FROM PlayerWhitelist WHERE License = @License

END
GO
/****** Object:  StoredProcedure [dbo].[spu_DeleteCharacter]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 07/04/2019 01:24

 * ChangeLog:
 *
 * 07/04/2019 01:24 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_DeleteCharacter]
	@PlayerID bigint,
	@CharacterID bigint
AS
BEGIN
	
	UPDATE PlayerCharacters
	SET isDeleted = 1
	WHERE id = @CharacterID AND PlayerID = @PlayerID

END
GO
/****** Object:  StoredProcedure [dbo].[spu_GetCharacter]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 27/03/2019 20:20

 * ChangeLog:
 *
 * 27/03/2019 20:20 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_GetCharacter]
	@CharacterID bigint,
	@PlayerID bigint
 AS
 BEGIN
	
	SELECT c.* FROM Players p
	INNER JOIN PlayerCharacters pc on pc.PlayerID = p.id
	INNER JOIN Characters c on c.id = pc.CharacterID
	WHERE p.id = @PlayerID AND c.id = @CharacterID

 END
GO
/****** Object:  StoredProcedure [dbo].[spu_GetCharacterVehicles]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 31/03/2019 00:08

 * ChangeLog:
 *
 * 31/03/2019 00:08 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_GetCharacterVehicles]
	@CharacterID bigint,
	@PlayerID bigint
 AS
 BEGIN
	
	SELECT v.* FROM Players p
	INNER JOIN PlayerCharacters pc on pc.PlayerID = p.id
	INNER JOIN Characters c on c.id = pc.CharacterID
	INNER JOIN CharacterVehicles cv on cv.CharacterID = c.id
	INNER JOIN Vehicles v on v.id = cv.VehicleID
	WHERE p.id = @PlayerID AND c.id = @CharacterID

 END
GO
/****** Object:  StoredProcedure [dbo].[spu_GetPlayerBySocialClubName]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 26/03/2019 21:45

 * ChangeLog:
 *
 * 26/03/2019 21:45 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_GetPlayerBySocialClubName]
	@PlayerName varchar(255)
 AS
 BEGIN
	
	SELECT * FROM Players WHERE PlayerName = @PlayerName

 END
GO
/****** Object:  StoredProcedure [dbo].[spu_GetPlayerCharacters]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 07/04/2019 16:18

 * ChangeLog:
 *
 * 07/04/2019 06:18 - Kieran
 * Added isDeleted where clause
 *
 * 26/03/2019 21:46 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_GetPlayerCharacters]
	@PlayerID bigint
 AS
 BEGIN
	
	SELECT c.* FROM Players p
	INNER JOIN PlayerCharacters pc on pc.PlayerID = p.id
	INNER JOIN Characters c on c.id = pc.CharacterID
	WHERE p.id = @PlayerID AND pc.isDeleted = 0

 END
GO
/****** Object:  StoredProcedure [dbo].[spu_GetSpawnLocations]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 21/04/2019 02:50

 * ChangeLog:
 *
 * 21/04/2019 02:50 - Kieran
 * Added Logic to get faction Spawn Locations from CharacterID
 *
 * 20/04/2019 17:16 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_GetSpawnLocations]
	@CharacterID bigint
 AS
 BEGIN

	DECLARE @isPolice bit
	DECLARE @isEMS bit

	SELECT @isPolice = isPolice, @isEMS = isEMS FROM Characters WHERE id = @CharacterID;
	
	with #Default (id, LocationName, posX, posY, posZ, lookX, lookY, lookZ, spawnX, spawnY, spawnZ, heading)
	AS
	(
		SELECT CAST('1' + CAST(id AS varchar) AS bigint) AS id, LocationName, posX, posY, posZ, lookX, lookY, lookZ, spawnX, spawnY, spawnZ, heading FROM DefaultSpawnLocations
	),
	#Faction (id, LocationName, posX, posY, posZ, lookX, lookY, lookZ, spawnX, spawnY, spawnZ, heading)
	AS
	(
		SELECT CAST('2' + CAST(id AS varchar) AS bigint) AS id, LocationName, posX, posY, posZ, lookX, lookY, lookZ, spawnX, spawnY, spawnZ, heading FROM FactionSpawnLocations
		WHERE isPolice = @isPolice OR isEMS = @isEMS
	),
	#Result (id, LocationName, posX, posY, posZ, lookX, lookY, lookZ, spawnX, spawnY, spawnZ, heading)
	AS
	(
		SELECT * FROM #Default
		UNION
		SELECT * FROM #Faction
	)

	SELECT * FROM #Result
 END
GO
/****** Object:  StoredProcedure [dbo].[spu_GetVehicle]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 31/03/2019 00:14

 * ChangeLog:
 *
 * 31/03/2019 00:14 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_GetVehicle]
	@VehicleID bigint
AS
BEGIN
	
	SELECT * FROM Vehicles WHERE id = @VehicleID

END
GO
/****** Object:  StoredProcedure [dbo].[spu_InsertCharacter]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 26/03/2019 13:04

 * ChangeLog:
 *
 * 07/04/2019 21:01 - Kieran
 * Changed ClothingString to PedString
 *
 * 07/04/2019 18:33 - Kieran
 * Added isNewCharacter column to Insert Statement
 *
 * 07/04/2019 01:30 - Kieran
 * Added isDeleted column to Insert Statement
 *
 * 26/03/2019 13:04 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_InsertCharacter]
	@PlayerID bigint,
	@CharacterName varchar(255),
	@Gender int,
	@CurrentPed varchar(255),
	@Cash int,
	@Bank int,
	@PedString varchar(MAX),
	@CharacterID bigint output
AS
BEGIN
	
	INSERT INTO Characters
		(CharacterName
		 ,Gender
		 ,CurrentPed
		 ,Cash
		 ,Bank
		 ,PedString
		 ,isNewCharacter)
	VALUES
		(@CharacterName
		,@Gender
		,@CurrentPed
		,@Cash
		,@Bank
		,@PedString
		,1)
	
	SET @CharacterID = (SELECT SCOPE_IDENTITY())

	INSERT INTO PlayerCharacters
	(PlayerID, CharacterID, isDeleted)
	VALUES
	(@PlayerID, @CharacterID, 0)

END
GO
/****** Object:  StoredProcedure [dbo].[spu_InsertCharacterVehicle]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 31/03/2019 00:13

 * ChangeLog:
 *
 * 31/03/2019 00:13 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_InsertCharacterVehicle]
	@CharacterID bigint,
	@carModel varchar(255),
	@colour varchar(255),
	@plate varchar(255),
	@VehicleID bigint output
AS
BEGIN
	
	INSERT INTO Vehicles
		(carModel
		,colour
		,plate
		,inGarage)
	VALUES
		(@carModel
		,@colour
		,@plate
		,0)
	
	SET @VehicleID = (SELECT SCOPE_IDENTITY())

	INSERT INTO CharacterVehicles
	(VehicleID, CharacterID)
	VALUES
	(@VehicleID, @CharacterID)

END
GO
/****** Object:  StoredProcedure [dbo].[spu_InsertPlayer]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 26/03/2019 21:54

 * ChangeLog:
 *
 * 06/03/2019 01:53 - Kieran
 * @License is now a random 9 digit number
 *
 * 31/03/2019 14:34 - Kieran
 * Added insert into PlayerWhitelist
 *
 * 26/03/2019 21:54 - Kieran
 * Added isAdmin & AdminLevel
 *
 * 26/03/2019 21:30 - Kieran
 * Added @RefID output
 *
 * 26/03/2019 13:55 - Kieran
 * Added DateCreated
 *
 * 25/03/2019 22:40 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_InsertPlayer]
	@PlayerName varchar(255),
	@RefID bigint output
AS
BEGIN

	DECLARE @License varchar(9) = CAST(convert(numeric(9,0),rand() * 899999999) + 100000000 AS varchar(9))

	INSERT INTO Players
	(PlayerName, License, DateCreated, isAdmin, AdminLevel)
	VALUES
	(@PlayerName, @License, getdate(), 0, 0)

	SET @RefID = (SELECT SCOPE_IDENTITY())

	INSERT INTO PlayerWhitelist
	(PlayerID, License, isWhitelisted)
	VALUES
	(@RefID, @License, 0)

END
GO
/****** Object:  StoredProcedure [dbo].[spu_UpdateCharacter]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 20/04/2019 18:27

 * ChangeLog:
 *
 * 20/04/2019 18:27 - Kieran
 * Added logic to update the isNewCharacter flag automatically when a character is updated
 *
 * 20/04/2019 16:48 - Kieran
 * Updated Procedure to use the PedString now
 *
 * 31/03/2019 00:19 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_UpdateCharacter]
	@CharacterID bigint,
	@Gender int,
	@CurrentPed varchar(255),
	@Cash int,
	@Bank int,
	@PedString varchar(max)
AS
BEGIN
	
	UPDATE Characters
	SET Gender = @Gender
	   ,CurrentPed   = @CurrentPed
	   ,Cash    = @Cash
	   ,Bank     = @Bank
	   ,PedString = @PedString
	WHERE id = @CharacterID

	DECLARE @isNewCharacter bit = (SELECT isNewCharacter FROM Characters WHERE id = @CharacterID)
	IF(@isNewCharacter = 1)
	BEGIN
		UPDATE Characters
		SET isNewCharacter = 0
		WHERE id = @CharacterID
	END
END
GO
/****** Object:  StoredProcedure [dbo].[spu_UpdateVehicle]    Script Date: 21/04/2019 02:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Last Edited: Kieran - 31/03/2019 00:16

 * ChangeLog:
 *
 * 31/03/2019 00:16 - Kieran
 * Created Procedure
 */

 CREATE PROCEDURE [dbo].[spu_UpdateVehicle]
	@VehicleID bigint,
	@carModel varchar(255),
	@colour varchar(255),
	@plate varchar(255),
	@mods varchar(MAX),
	@inGarage bit,
	@trunk varchar(MAX)
AS
BEGIN
	
	UPDATE Vehicles
	SET carModel = @carModel
	   ,colour   = @colour
	   ,plate    = @plate
	   ,mods     = @mods
	   ,inGarage = @inGarage
	   ,trunk	 = @trunk
	WHERE id = @VehicleID

END
GO
