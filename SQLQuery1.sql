PRINT N'Creating Database...';  
GO  
CREATE SCHEMA [NewDatabase]  
    AUTHORIZATION [dbo];  
GO  
PRINT N'Creating NewDatabase.ElemtP...';  
GO  
CREATE TABLE [NewDatabase].[ElementP] (  
    [IdentifikacioniKod]   INT           PRIMARY KEY,  
    [RedniBroj]    INT           NOT NULL,  
    [DatumPretrage]     DATETIME           NOT NULL  
);  
GO  
PRINT N'Creating NewDatabase.ElemtC...';  
GO  
CREATE TABLE [NewDatabase].[ElementC] (  
    [IDKOD] INT      NOT NULL,  
    [ID]    INT      IDENTITY (1, 1) NOT NULL,  
    [Grupa]     CHAR (1) NOT NULL,  
    [Vrednost]     INT      NOT NULL  
);  