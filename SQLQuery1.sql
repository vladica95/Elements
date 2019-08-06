PRINT N'Creating Database...';  
GO  
CREATE SCHEMA [NewDatabase]  
    AUTHORIZATION [dbo];  
GO  
PRINT N'Creating NewDatabase.ElemtP...';  
GO  
CREATE TABLE [ElementP] (  
    [IdentifikacioniKod]   VARCHAR           PRIMARY KEY,  
    [RedniBroj]    INT           NOT NULL,  
    [DatumPretrage]     DATETIME           NOT NULL  
);  
GO  
PRINT N'Creating NewDatabase.ElemtC...';  
GO  
CREATE TABLE [ElementC] (  
    [IDKOD] VARCHAR      REFERENCES [ElementP](IdentifikacioniKod),  
    [ID]    INT      IDENTITY (1, 1) PRIMARY KEY,  
    [Grupa]     CHAR (1) NOT NULL,  
    [Vrednost]     INT      NOT NULL  
);  