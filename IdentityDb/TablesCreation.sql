/*********************[AspNetUsers]****************************************************/
DROP INDEX IF EXISTS [UserNameIndex] ON [AspNetUsers] 

DROP INDEX IF EXISTS [EmailIndex] ON [AspNetUsers] 



DROP INDEX IF EXISTS [RoleNameIndex] ON [AspNetRoles] 

DROP INDEX IF EXISTS [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] 

ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT IF EXISTS [PK_AspNetUserRoles];

ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT IF EXISTS [FK_AspNetUserRoles_AspNetRoles_RoleId];

ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT IF EXISTS [FK_AspNetUserRoles_AspNetUsers_UserId];

DROP INDEX IF EXISTS [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] 

ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT IF EXISTS [PK_AspNetRoleClaims];

ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT IF EXISTS [FK_AspNetRoleClaims_AspNetRoles_RoleId];

DROP INDEX IF EXISTS [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] 

ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT IF EXISTS [PK_AspNetUserLogins];

ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT IF EXISTS [FK_AspNetUserLogins_AspNetUsers_UserId];

ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT IF EXISTS [FK_AspNetUserTokens_AspNetUsers_UserId];

ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT IF EXISTS [PK_AspNetUserTokens];

DROP INDEX IF EXISTS [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] 

ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT IF EXISTS [FK_AspNetUserClaims_AspNetUsers_UserId];

ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT IF EXISTS [PK_AspNetUserClaims];

ALTER TABLE [AspNetRoles] DROP CONSTRAINT IF EXISTS [PK_AspNetRoles];

ALTER TABLE AspNetUserClaims DROP CONSTRAINT IF EXISTS FK_AspNetUserClaims_AspNetUsers_UserId;

ALTER TABLE AspNetUserRoles DROP CONSTRAINT IF EXISTS FK_AspNetUserRoles_AspNetUsers_UserId;

ALTER TABLE AspNetUserTokens DROP CONSTRAINT IF EXISTS FK_AspNetUserTokens_AspNetUsers_UserId;

ALTER TABLE AspNetUserLogins DROP CONSTRAINT IF EXISTS FK_AspNetUserLogins_AspNetUsers_UserId;

ALTER TABLE [AspNetUsers] DROP CONSTRAINT IF EXISTS [PK_AspNetUsers];

DROP TABLE IF EXISTS [dbo].[AspNetRoles] 

DROP TABLE IF EXISTS [dbo].[AspNetUsers] 

DROP TABLE IF EXISTS [dbo].[AspNetUserRoles] 

DROP TABLE IF EXISTS [dbo].[AspNetUserLogins] 

DROP TABLE IF EXISTS [dbo].[AspNetUserTokens] 

DROP TABLE IF EXISTS [dbo].[AspNetUserClaims] 

DROP TABLE IF EXISTS [dbo].[AspNetRoleClaims] 

GO
/****************************************************************************************************/


CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (450)     NOT NULL,
    [UserName]             NVARCHAR (256)     NULL,
    [NormalizedUserName]   NVARCHAR (256)     NULL,
    [Email]                NVARCHAR (256)     NULL,
    [NormalizedEmail]      NVARCHAR (256)     NULL,
    [EmailConfirmed]       BIT                NOT NULL,
    [PasswordHash]         NVARCHAR (MAX)     NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
    [PhoneNumber]          NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed] BIT                NOT NULL,
    [TwoFactorEnabled]     BIT                NOT NULL,
    [LockoutEnd]           DATETIMEOFFSET (7) NULL,
    [LockoutEnabled]       BIT                NOT NULL,
    [AccessFailedCount]    INT                NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO


CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([NormalizedUserName] ASC) WHERE ([NormalizedUserName] IS NOT NULL);


GO

CREATE NONCLUSTERED INDEX [EmailIndex]
    ON [dbo].[AspNetUsers]([NormalizedEmail] ASC);

GO

/***************************[AspNetRoles]**********************************************/



CREATE TABLE [dbo].[AspNetRoles] (
    [Id]               NVARCHAR (450) NOT NULL,
    [Name]             NVARCHAR (256) NULL,
    [NormalizedName]   NVARCHAR (256) NULL,
    [ConcurrencyStamp] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([NormalizedName] ASC) WHERE ([NormalizedName] IS NOT NULL);

GO

/********************************[AspNetUserRoles]*****************************************/



CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (450) NOT NULL,
    [RoleId] NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);
GO

/***********************************[AspNetRoleClaims]**************************************/


CREATE TABLE [dbo].[AspNetRoleClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RoleId]     NVARCHAR (450) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId]
    ON [dbo].[AspNetRoleClaims]([RoleId] ASC);

GO

/*********************************[AspNetUserLogins]****************************************/





	CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider]       NVARCHAR (128) NOT NULL,
    [ProviderKey]         NVARCHAR (128) NOT NULL,
    [ProviderDisplayName] NVARCHAR (MAX) NULL,
    [UserId]              NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);
GO
/************************************[AspNetUserClaims]*************************************/



CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (450) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);
GO


/***********************************[AspNetUserTokens]**************************************/



	CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId]        NVARCHAR (450) NOT NULL,
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [Name]          NVARCHAR (128) NOT NULL,
    [Value]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
